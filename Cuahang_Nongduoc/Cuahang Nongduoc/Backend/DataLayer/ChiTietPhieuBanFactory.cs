using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    // ==================== [R28-BH] REFACTORING ====================
    // Lỗi (Code smell) : Tight Coupling với OleDb/Access cũ, không tương thích với SQL Server
    // Kỹ thuật         : Migrate Data Provider
    // Trước            : Dùng System.Data.OleDb (OleDbCommand, OleDbType), DataService.ExecuteNoneQuery()
    // Sau              : Chuyển sang System.Data.SqlClient (SqlCommand, SqlDbType), DataService.UpdateDataTable()
    // Tác giả          : BE2
    // Hỗ trợ           : BE1
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 06/11/2026
    // ==============================================================
    public class ChiTietPhieuBanFactory
    {
        DataService m_Ds = new DataService();

        public DataTable LayChiTietPhieuBan(String idPhieuBan)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM CHI_TIET_PHIEU_BAN WHERE ID_PHIEU_BAN = @id");
            cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idPhieuBan;
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        // ==================== [R32-BH] REFACTORING ====================
        // Lỗi (Code smell) : Lọc ngày bằng CONVERT(date, ...) và MONTH(...)/YEAR(...) khiến SQL Server không dùng được Index trên NGAY_BAN
        // Kỹ thuật         : Substitute Algorithm
        // Trước            : Dùng CONVERT(date, PB.NGAY_BAN) = CONVERT(date, @ngayban) và MONTH(PB.NGAY_BAN) = @thang AND YEAR(PB.NGAY_BAN) = @nam
        // Sau              : Đổi cả 2 hàm sang lọc theo khoảng (PB.NGAY_BAN >= @tu AND PB.NGAY_BAN < @den)
        // Tác giả          : BE2
        // Hỗ trợ           : -
        // Kiểm tra         : QA2
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public DataTable LayChiTietPhieuBan(DateTime dtNgayBan)
        {
            DateTime tu = dtNgayBan.Date;
            SqlCommand cmd = new SqlCommand("SELECT CT.* FROM CHI_TIET_PHIEU_BAN CT INNER JOIN PHIEU_BAN PB ON CT.ID_PHIEU_BAN = PB.ID " +
                    " WHERE PB.NGAY_BAN >= @tu AND PB.NGAY_BAN < @den");
            cmd.Parameters.Add("@tu", SqlDbType.DateTime).Value = tu;
            cmd.Parameters.Add("@den", SqlDbType.DateTime).Value = tu.AddDays(1);
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataTable LayChiTietPhieuBan(int thang, int nam)
        {
            DateTime tu = new DateTime(nam, thang, 1);
            SqlCommand cmd = new SqlCommand("SELECT CT.* FROM CHI_TIET_PHIEU_BAN CT INNER JOIN PHIEU_BAN PB ON CT.ID_PHIEU_BAN = PB.ID " +
                    " WHERE PB.NGAY_BAN >= @tu AND PB.NGAY_BAN < @den");
            cmd.Parameters.Add("@tu", SqlDbType.DateTime).Value = tu;
            cmd.Parameters.Add("@den", SqlDbType.DateTime).Value = tu.AddMonths(1);
            m_Ds.Load(cmd);
            return m_Ds.ResultTable;
        }

        public DataRow NewRow()
        {
            return m_Ds.NewRow();
        }

        public void Add(DataRow row)
        {
            m_Ds.Rows.Add(row);
        }

        // ==================== [R31] REFACTORING ====================
        // Lỗi (Code smell) : Incomplete State Handling – Tồn kho chỉ cập nhật khi thêm mới, bỏ quên khi sửa hoặc xóa chi tiết phiếu
        // Kỹ thuật         : Complete State Handling (Decompose Conditional)
        // Trước            : Chỉ xử lý DataRowState.Added (trừ tồn kho), bỏ qua DataRowState.Modified và DataRowState.Deleted
        // Sau              : Xử lý đủ 3 trạng thái của DataRow:
        //                    - Added: Trừ tồn kho (-soLuong)
        //                    - Modified: Bù trừ chênh lệch (-delta) hoặc đổi mã sản phẩm
        //                    - Deleted: Hoàn trả lại số lượng tồn kho (+soLuong gốc qua DataRowVersion.Original)
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public bool Save()
        {
            foreach (DataRow row in m_Ds.Rows)
            {
                DongBoTonKhoTheoTrangThai(row);
            }
            return m_Ds.UpdateDataTable() > 0;
        }

        private void DongBoTonKhoTheoTrangThai(DataRow row)
        {
            if (row.RowState == DataRowState.Added)
            {
                if (row["ID_MA_SAN_PHAM"] != DBNull.Value && row["SO_LUONG"] != DBNull.Value)
                {
                    string masp = Convert.ToString(row["ID_MA_SAN_PHAM"]);
                    int soLuong = Convert.ToInt32(row["SO_LUONG"]);
                    MaSanPhanFactory.CapNhatSoLuong(masp, -soLuong);
                }
            }
            else if (row.RowState == DataRowState.Deleted)
            {
                // Khi dòng bị xóa, dùng DataRowVersion.Original để đọc dữ liệu gốc và hoàn trả lại kho
                if (row["ID_MA_SAN_PHAM", DataRowVersion.Original] != DBNull.Value &&
                    row["SO_LUONG", DataRowVersion.Original] != DBNull.Value)
                {
                    string masp = Convert.ToString(row["ID_MA_SAN_PHAM", DataRowVersion.Original]);
                    int soLuong = Convert.ToInt32(row["SO_LUONG", DataRowVersion.Original]);
                    MaSanPhanFactory.CapNhatSoLuong(masp, soLuong);
                }
            }
            else if (row.RowState == DataRowState.Modified)
            {
                string maspMoi = row["ID_MA_SAN_PHAM", DataRowVersion.Current] != DBNull.Value ? Convert.ToString(row["ID_MA_SAN_PHAM", DataRowVersion.Current]) : "";
                string maspCu = row["ID_MA_SAN_PHAM", DataRowVersion.Original] != DBNull.Value ? Convert.ToString(row["ID_MA_SAN_PHAM", DataRowVersion.Original]) : "";
                int slMoi = row["SO_LUONG", DataRowVersion.Current] != DBNull.Value ? Convert.ToInt32(row["SO_LUONG", DataRowVersion.Current]) : 0;
                int slCu = row["SO_LUONG", DataRowVersion.Original] != DBNull.Value ? Convert.ToInt32(row["SO_LUONG", DataRowVersion.Original]) : 0;

                if (maspMoi == maspCu)
                {
                    int delta = slMoi - slCu;
                    if (delta != 0 && !string.IsNullOrEmpty(maspMoi))
                    {
                        // delta > 0: bán tăng -> trừ thêm tồn kho; delta < 0: giảm bớt -> hoàn trả lại kho
                        MaSanPhanFactory.CapNhatSoLuong(maspMoi, -delta);
                    }
                }
                else
                {
                    // Nếu đổi sang mã sản phẩm khác: hoàn trả kho mã cũ, trừ kho mã mới
                    if (!string.IsNullOrEmpty(maspCu))
                    {
                        MaSanPhanFactory.CapNhatSoLuong(maspCu, slCu);
                    }
                    if (!string.IsNullOrEmpty(maspMoi))
                    {
                        MaSanPhanFactory.CapNhatSoLuong(maspMoi, -slMoi);
                    }
                }
            }
        }
    }
}
