# Sinh script SQL Server tu access_dump.json (do export_access.ps1 xuat ra).
# Chay: python gen_sql.py   -> ghi 01..04_*.sql vao thu muc Database/
import json, os, io
HERE = os.path.dirname(os.path.abspath(__file__))
OUT = os.path.dirname(HERE)
DB = "CuahangNongduoc"
d = json.load(io.open(os.path.join(HERE, "access_dump.json"), encoding="utf-8-sig"))
tables = sorted(t["TABLE_NAME"] for t in d["tables"])
cols = {}
for c in d["columns"]:
    if c["TABLE_NAME"] in tables:
        cols.setdefault(c["TABLE_NAME"], []).append(c)
for t in cols: cols[t].sort(key=lambda c: c["ORDINAL_POSITION"])
pks = {}
for p in d["pks"]:
    if p["TABLE_NAME"] in tables:
        pks.setdefault(p["TABLE_NAME"], []).append((p["ORDINAL"], p["COLUMN_NAME"]))
for t in pks: pks[t] = [c for _, c in sorted(pks[t])]
IDENTITY = {("DON_VI_TINH", "ID"), ("LY_DO_CHI", "ID")}   # AutoNumber trong Access
# Khoa ngoai co san trong Access (giu ON UPDATE/DELETE CASCADE nhu ban goc)
fk_access = [(f["FK_TABLE_NAME"], f["FK_COLUMN_NAME"], f["PK_TABLE_NAME"], f["PK_COLUMN_NAME"])
             for f in d["fks"] if f["FK_TABLE_NAME"] in tables]
# Khoa ngoai Access KHONG khai bao nhung du lieu va code co dung (da kiem tra: 0 dong mo coi)
fk_them = [("PHIEU_BAN", "ID_KHACH_HANG", "KHACH_HANG", "ID"),
           ("CHI_TIET_PHIEU_BAN", "ID_MA_SAN_PHAM", "MA_SAN_PHAM", "ID"),
           ("PHIEU_NHAP", "ID_NHA_CUNG_CAP", "NHA_CUNG_CAP", "ID"),
           ("SAN_PHAM", "ID_DON_VI_TINH", "DON_VI_TINH", "ID"),
           ("PHIEU_CHI", "ID_LY_DO_CHI", "LY_DO_CHI", "ID")]
# Thu tu tao bang / nap du lieu: bang cha truoc
ORDER = ["THAM_SO", "DON_VI_TINH", "LY_DO_CHI", "KHACH_HANG", "DAI_LY", "NHA_CUNG_CAP", "SAN_PHAM",
         "PHIEU_NHAP", "MA_SAN_PHAM", "PHIEU_BAN", "CHI_TIET_PHIEU_BAN", "PHIEU_CHI", "PHIEU_THANH_TOAN", "DU_NO_KH"]
assert sorted(ORDER) == tables, (sorted(ORDER), tables)

def sqltype(c):
    t = c["DATA_TYPE"]
    if t == 130: return "NVARCHAR(%d)" % c["CHARACTER_MAXIMUM_LENGTH"]
    if t == 3: return "INT"
    if t == 2: return "SMALLINT"
    if t == 7: return "DATETIME"
    if t == 11: return "BIT"
    raise Exception("Chua ho tro kieu %s (%s.%s)" % (t, c["TABLE_NAME"], c["COLUMN_NAME"]))

HDR = (u"-- =============================================================\n"
       u"-- %s\n"
       u"-- Nguồn  : cuahang.dll (Access) -> SQL Server. Sinh bởi Database/tools/gen_sql.py\n"
       u"-- Việc   : %s\n"
       u"-- Tác giả: tuananh (Team Lead, làm thay BE1) | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>\n"
       u"-- Ngày   : 02/11/2026\n"
       u"-- =============================================================\n")

def write(name, text):
    with io.open(os.path.join(OUT, name), "w", encoding="utf-8-sig", newline="\r\n") as f:
        f.write(text)

# ---------- 01
write("01_CreateDatabase.sql", HDR % ("01 - Tạo CSDL", "SQL-01") + u"""
USE master;
GO
IF DB_ID(N'%(db)s') IS NOT NULL
BEGIN
    ALTER DATABASE [%(db)s] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [%(db)s];
END
GO
-- Vietnamese_CI_AS: sắp xếp/so sánh tiếng Việt đúng, không phân biệt hoa thường
CREATE DATABASE [%(db)s] COLLATE Vietnamese_CI_AS;
GO
""" % {"db": DB})

# ---------- 02
s = HDR % ("02 - Tạo bảng, khóa chính, khóa ngoại, chỉ mục", "SQL-01") + u"\nUSE [%s];\nGO\n\n" % DB
for t in reversed(ORDER):
    s += u"IF OBJECT_ID(N'dbo.%s', N'U') IS NOT NULL DROP TABLE dbo.%s;\n" % (t, t)
s += u"GO\n\n"
for t in ORDER:
    lines = []
    for c in cols[t]:
        n = c["COLUMN_NAME"]
        typ = sqltype(c)
        isk = n in pks.get(t, [])
        parts = [u"    [%s] %s" % (n, typ)]
        if (t, n) in IDENTITY: parts.append(u"IDENTITY(1,1)")
        if c["DATA_TYPE"] == 11:
            parts.append(u"NOT NULL CONSTRAINT DF_%s_%s DEFAULT (0)" % (t, n))
        else:
            parts.append(u"NOT NULL" if (isk or not c["IS_NULLABLE"]) else u"NULL")
            if c.get("COLUMN_HASDEFAULT") and c.get("COLUMN_DEFAULT") is not None:
                parts.append(u"CONSTRAINT DF_%s_%s DEFAULT (%s)" % (t, n, c["COLUMN_DEFAULT"]))
        lines.append(u" ".join(parts))
    if t in pks:
        lines.append(u"    CONSTRAINT PK_%s PRIMARY KEY (%s)" % (t, u", ".join(u"[%s]" % k for k in pks[t])))
    s += u"CREATE TABLE dbo.%s (\n%s\n);\nGO\n\n" % (t, u",\n".join(lines))
    if t not in pks:
        s += u"-- %s: bảng tham số 1 dòng, Access không có khóa chính (giữ nguyên để code cũ chạy).\n\n" % t
s += u"-- Khóa ngoại có sẵn trong Access (giữ CASCADE như bản gốc)\n"
for a, c, b, k in fk_access:
    s += (u"ALTER TABLE dbo.%s ADD CONSTRAINT FK_%s_%s FOREIGN KEY ([%s]) REFERENCES dbo.%s([%s]) "
          u"ON UPDATE CASCADE ON DELETE CASCADE;\n" % (a, a, b, c, b, k))
s += u"GO\n\n-- Khóa ngoại BỔ SUNG (Access thiếu, dữ liệu đã kiểm tra không có dòng mồ côi). NO ACTION: chặn xóa bản ghi cha đang được dùng.\n"
for a, c, b, k in fk_them:
    s += u"ALTER TABLE dbo.%s ADD CONSTRAINT FK_%s_%s FOREIGN KEY ([%s]) REFERENCES dbo.%s([%s]);\n" % (a, a, b, c, b, k)
s += u"GO\n\n-- Chỉ mục cho các cột khóa ngoại (Access có sẵn các index này)\n"
for a, c, b, k in fk_access + fk_them:
    if pks.get(a, [None])[0] == c: continue
    s += u"CREATE INDEX IX_%s_%s ON dbo.%s([%s]);\n" % (a, c, a, c)
s += u"GO\n"
write("02_CreateTables.sql", s)

# ---------- 03
def lit(v, c):
    if v is None: return u"NULL"
    t = c["DATA_TYPE"]
    if t == 11: return u"1" if v else u"0"
    if t in (2, 3): return u"%d" % v
    if t == 7: return u"'%s'" % v          # ISO 8601 yyyy-MM-ddTHH:mm:ss, khong phu thuoc ngon ngu
    return u"N'%s'" % unicode_s(v).replace(u"'", u"''")
def unicode_s(v):
    try: return unicode(v)
    except NameError: return str(v)
s = HDR % ("03 - Nạp dữ liệu từ Access", "SQL-02") + u"\nUSE [%s];\nGO\nSET NOCOUNT ON;\nSET XACT_ABORT ON;\nBEGIN TRANSACTION;\n\n" % DB
for t in ORDER:
    rows = d["data"][t]
    s += u"-- %s: %d dòng\n" % (t, len(rows))
    if not rows: s += u"\n"; continue
    names = [c["COLUMN_NAME"] for c in cols[t]]
    ident = any((t, n) in IDENTITY for n in names)
    if ident: s += u"SET IDENTITY_INSERT dbo.%s ON;\n" % t
    for r in rows:
        s += u"INSERT INTO dbo.%s (%s) VALUES (%s);\n" % (t, u", ".join(u"[%s]" % n for n in names),
             u", ".join(lit(r.get(n), c) for n, c in zip(names, cols[t])))
    if ident: s += u"SET IDENTITY_INSERT dbo.%s OFF;\n" % t
    s += u"\n"
s += u"COMMIT TRANSACTION;\nGO\n"
write("03_SeedData.sql", s)

# ---------- 04
s = HDR % ("04 - Đối chiếu số dòng Access <-> SQL Server", "SQL-02 (QA2 chạy để kiểm tra)") + u"\nUSE [%s];\nGO\n" % DB
s += u"SELECT t.Bang, t.SoDongAccess, t.SoDongSqlServer,\n       CASE WHEN t.SoDongAccess = t.SoDongSqlServer THEN N'Khớp' ELSE N'LỆCH' END AS KetQua\nFROM (\n"
s += u"\n    UNION ALL\n".join(u"    SELECT N'%s' AS Bang, %d AS SoDongAccess, (SELECT COUNT(*) FROM dbo.%s) AS SoDongSqlServer" % (t, len(d["data"][t]), t) for t in ORDER)
s += u"\n) t\nORDER BY t.Bang;\nGO\n\n-- Đối chiếu tổng tiền (phải bằng số trong Access)\n"
def tot(t, c): return sum((r.get(c) or 0) for r in d["data"][t])
checks = [("PHIEU_BAN", "TONG_TIEN"), ("PHIEU_NHAP", "TONG_TIEN"), ("PHIEU_CHI", "TONG_TIEN"),
          ("PHIEU_THANH_TOAN", "TONG_TIEN"), ("CHI_TIET_PHIEU_BAN", "THANH_TIEN"), ("SAN_PHAM", "SO_LUONG"), ("MA_SAN_PHAM", "SO_LUONG")]
s += u"\nUNION ALL\n".join(u"SELECT N'%s.%s' AS Cot, %d AS TongAccess, (SELECT ISNULL(SUM(CAST([%s] AS BIGINT)),0) FROM dbo.%s) AS TongSqlServer" % (t, c, tot(t, c), c, t) for t, c in checks)
s += u";\nGO\n"
write("04_KiemTraDuLieu.sql", s)
print("OK:", [n for n in os.listdir(OUT) if n.endswith(".sql")])
