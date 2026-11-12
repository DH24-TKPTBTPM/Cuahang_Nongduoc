using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc.Controller
{
    public class SanPhamController
    {
        SanPhamFactory factory = new SanPhamFactory();

        public void HienthiAutoComboBox(System.Windows.Forms.ComboBox cmb)
        {
            DataTable tbl = factory.DanhsachSanPham();
            cmb.DataSource = tbl;
            cmb.DisplayMember = "TEN_SAN_PHAM";
            cmb.ValueMember = "ID";
            cmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
        }

        public void HienthiDataGridViewComboBoxColumn(System.Windows.Forms.DataGridViewComboBoxColumn cmb)
        {
            cmb.DataSource = factory.DanhsachSanPham();
            cmb.DisplayMember = "TEN_SAN_PHAM";
            cmb.ValueMember = "ID";
            cmb.AutoComplete = true;
        }

        public void TimMaSanPham(String ma)
        {
            factory.TimMaSanPham(ma);
        }

        public void TimTenSanPham(String ten)
        {
            factory.TimTenSanPham(ten);
        }

        public void HienthiDataGridview(System.Windows.Forms.DataGridView dg, System.Windows.Forms.BindingNavigator bn,
            TextBox txtMaSp, TextBox txtTenSp, ComboBox cmbDVT, NumericUpDown numSL, NumericUpDown numDonGiaNhap, NumericUpDown numGiaBanSi, NumericUpDown numGiaBanLe)
        {
            System.Windows.Forms.BindingSource bs = new System.Windows.Forms.BindingSource();
            bs.DataSource = factory.DanhsachSanPham();

            txtMaSp.DataBindings.Clear();
            txtMaSp.DataBindings.Add("Text", bs, "ID");

            txtTenSp.DataBindings.Clear();
            txtTenSp.DataBindings.Add("Text", bs, "TEN_SAN_PHAM");

            cmbDVT.DataBindings.Clear();
            cmbDVT.DataBindings.Add("SelectedValue", bs, "ID_DON_VI_TINH");

            numSL.DataBindings.Clear();
            numSL.DataBindings.Add("Value", bs, "SO_LUONG");

            numDonGiaNhap.DataBindings.Clear();
            numDonGiaNhap.DataBindings.Add("Value", bs, "DON_GIA_NHAP");

            numGiaBanSi.DataBindings.Clear();
            numGiaBanSi.DataBindings.Add("Value", bs, "GIA_BAN_SI");

            numGiaBanLe.DataBindings.Clear();
            numGiaBanLe.DataBindings.Add("Value", bs, "GIA_BAN_LE");
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        // ==================== [R30] REFACTORING ====================
        // Lỗi (Code smell) : CapNhatGiaNhap(): không cộng tồn kho khi giá nhập không đổi
        // Kỹ thuật         : Consolidate Conditional (Consolidate Duplicate Conditional Fragments)
        // Trước            : if (tong_gia != gia_moi)
        //                    {
        //                        long thanh_tien = gia_moi * so_luong + tong_gia * tong_so;
        //                        tong_so += so_luong;
        //                        tbl.Rows[0]["DON_GIA_NHAP"] = thanh_tien / tong_so;
        //                        tbl.Rows[0]["SO_LUONG"] = tong_so;
        //                    }
        // Sau              : Đưa việc cập nhật SO_LUONG ra ngoài điều kiện, luôn cộng dồn tồn kho khi nhập hàng;
        //                    chỉ tính lại đơn giá bình quân khi giá nhập thay đổi.
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA1 
        // Ngày chỉnh sửa   : 05/11/2026
        // ===========================================================

        // ==================== [R22] REFACTORING ====================
        // Lỗi (Code smell) : Primitive Obsession / Integer Overflow & DivideByZero – Dùng Convert.ToInt32 gây tràn số với dữ liệu lớn và thiếu chốt chặn chia cho 0
        // Kỹ thuật         : Introduce Assertion (https://refactoring.guru/introduce-assertion)
        // Trước            : Dùng Convert.ToInt32(tbl.Rows[0]["SO_LUONG"]) và Convert.ToInt32(tbl.Rows[0]["DON_GIA_NHAP"]),
        //                    tính toán trực tiếp không kiểm tra DBNull và không chặn mẫu số tong_so <= 0.
        // Sau              : Thay thế triệt để Convert.ToInt32 bằng Convert.ToInt64 thông qua hàm Đọc/Gán an toàn,
        //                    bổ sung Debug.Assert & Guard Clause chặn chia cho 0 (tong_so > 0) và chống tràn số bằng kiểu decimal/long.
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ===========================================================
        private static long DocInt64AnToan(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return 0L;
            }
            if (value is long)
            {
                return (long)value;
            }
            decimal parsed;
            if (decimal.TryParse(Convert.ToString(value), out parsed))
            {
                if (parsed > long.MaxValue) return long.MaxValue;
                if (parsed < long.MinValue) return long.MinValue;
                return (long)decimal.Truncate(parsed);
            }
            return Convert.ToInt64(value);
        }

        private static int EpKieuInt32KhongTran(long value)
        {
            if (value > int.MaxValue) return int.MaxValue;
            if (value < int.MinValue) return int.MinValue;
            return (int)value;
        }

        private static void GanGiaTriCotSo(DataRow row, string columnName, long value)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
            {
                return;
            }
            Type colType = row.Table.Columns[columnName].DataType;
            if (colType == typeof(int) || colType == typeof(short) || colType == typeof(byte))
            {
                row[columnName] = EpKieuInt32KhongTran(value);
            }
            else
            {
                row[columnName] = value;
            }
        }

        public void CapNhatGiaNhap(String id, long gia_moi, long so_luong)
        {
            // Introduce Assertion: Đảm bảo các điều kiện đầu vào hợp lệ trước khi tính toán
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(id), "Mã sản phẩm không được rỗng.");
            System.Diagnostics.Debug.Assert(gia_moi >= 0, "Giá nhập mới không được âm.");
            System.Diagnostics.Debug.Assert(so_luong >= 0, "Số lượng nhập không được âm.");

            if (string.IsNullOrEmpty(id) || gia_moi < 0 || so_luong < 0)
            {
                return;
            }

            DataTable tbl = factory.LaySanPham(id);
            if (tbl.Rows.Count > 0)
            {
                DataRow row = tbl.Rows[0];
                long tong_so = DocInt64AnToan(row["SO_LUONG"]);
                long tong_gia = DocInt64AnToan(row["DON_GIA_NHAP"]);

                decimal thanh_tien = (decimal)gia_moi * so_luong + (decimal)tong_gia * tong_so;
                tong_so += so_luong;

                if (tong_gia != gia_moi)
                {
                    // Introduce Assertion: Chặn tuyệt đối lỗi chia cho 0 (DivideByZeroException)
                    System.Diagnostics.Debug.Assert(tong_so > 0, "Tổng số lượng phải lớn hơn 0 khi tính đơn giá nhập bình quân.");
                    if (tong_so > 0)
                    {
                        long don_gia_binh_quan = DocInt64AnToan(decimal.Truncate(thanh_tien / tong_so));
                        GanGiaTriCotSo(row, "DON_GIA_NHAP", don_gia_binh_quan);
                    }
                }

                GanGiaTriCotSo(row, "SO_LUONG", tong_so);
                factory.Save();
            }
        }

        // ==================== [R22] REFACTORING ====================
        // Lỗi (Code smell) : Dùng Convert.ToInt32 trực tiếp dễ gây OverflowException và lỗi khi gặp DBNull
        // Kỹ thuật         : Introduce Assertion (https://refactoring.guru/introduce-assertion)
        // Trước            : sp.SoLuong = Convert.ToInt32(tbl.Rows[0]["SO_LUONG"]);
        //                    sp.DonViTinh = ctrlDVT.LayDVT(Convert.ToInt32(tbl.Rows[0]["ID_DON_VI_TINH"]));
        // Sau              : Đọc toàn bộ qua DocInt64AnToan (Convert.ToInt64) và hạ bậc an toàn không quăng lỗi tràn Int32.
        // Tác giả          : BE2 | Hỗ trợ: BE1 | Kiểm tra: QA1 | Ngày chỉnh sửa: 12/11/2026
        // ===========================================================
        public SanPham LaySanPham(String id)
        {
            DataTable tbl = factory.LaySanPham(id);
            SanPham sp = new SanPham();
            DonViTinhController ctrlDVT = new DonViTinhController();
            if (tbl.Rows.Count > 0)
            {
                DataRow row = tbl.Rows[0];
                sp.Id = Convert.ToString(row["ID"]);
                sp.TenSanPham = Convert.ToString(row["TEN_SAN_PHAM"]);
                sp.SoLuong = EpKieuInt32KhongTran(DocInt64AnToan(row["SO_LUONG"]));
                sp.DonGiaNhap = DocInt64AnToan(row["DON_GIA_NHAP"]);
                sp.GiaBanLe = DocInt64AnToan(row["GIA_BAN_LE"]);
                sp.GiaBanSi = DocInt64AnToan(row["GIA_BAN_SI"]);
                sp.DonViTinh = ctrlDVT.LayDVT(EpKieuInt32KhongTran(DocInt64AnToan(row["ID_DON_VI_TINH"])));
            }
            return sp;
        }

        // ==================== [R22] REFACTORING ====================
        // Lỗi (Code smell) : Dùng Convert.ToInt32 trực tiếp cho SO_LUONG và SO_LUONG_TON gây lỗi tràn số và DBNull
        // Kỹ thuật         : Introduce Assertion (https://refactoring.guru/introduce-assertion)
        // Trước            : sp.SoLuong = Convert.ToInt32(row["SO_LUONG"]); ... slt.SoLuong = Convert.ToInt32(row["SO_LUONG_TON"]);
        // Sau              : Chuyển sang đọc kiểu Int64 qua DocInt64AnToan và gán an toàn bằng EpKieuInt32KhongTran.
        // Tác giả          : BE2 | Hỗ trợ: BE1 | Kiểm tra: QA1 | Ngày chỉnh sửa: 12/11/2026
        // ===========================================================
        public static IList<SoLuongTon> LaySoLuongTon()
        {
            SanPhamFactory f = new SanPhamFactory();
            DataTable tbl = f.LaySoLuongTon();

            IList<SoLuongTon> ds = new List<SoLuongTon>();

            DonViTinhController ctrlDVT = new DonViTinhController();
            foreach (DataRow row in tbl.Rows)
            {
                SoLuongTon slt = new SoLuongTon();
                SanPham sp = new SanPham();
                sp.Id = Convert.ToString(row["ID"]);
                sp.TenSanPham = Convert.ToString(row["TEN_SAN_PHAM"]);
                sp.SoLuong = EpKieuInt32KhongTran(DocInt64AnToan(row["SO_LUONG"]));
                sp.DonGiaNhap = DocInt64AnToan(row["DON_GIA_NHAP"]);
                sp.GiaBanLe = DocInt64AnToan(row["GIA_BAN_LE"]);
                sp.GiaBanSi = DocInt64AnToan(row["GIA_BAN_SI"]);
                sp.DonViTinh = ctrlDVT.LayDVT(EpKieuInt32KhongTran(DocInt64AnToan(row["ID_DON_VI_TINH"])));
                slt.SanPham = sp;
                slt.SoLuong = EpKieuInt32KhongTran(DocInt64AnToan(row["SO_LUONG_TON"]));
                ds.Add(slt);
            }
            return ds;
        }

        public DataRow NewRow()
        {
            return factory.NewRow();
        }

        public void Add(DataRow row)
        {
            factory.Add(row);
        }

        public bool Save()
        {
            return factory.Save();
        }
    }
}