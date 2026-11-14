using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.DataLayer.Mappers;

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
    
        // ==================== [R10] REFACTORING ====================
        // Lỗi (Code smell) : Lặp lại đoạn code gán 7 trường của SanPham từ DataRow ở LaySanPham() và LaySoLuongTon()
        // Kỹ thuật         : Extract Method (kết hợp Mapper [D05])
        // Trước            : Cả 2 hàm đều tự đọc từng cột ID, TEN_SAN_PHAM, SO_LUONG, DON_GIA_NHAP, GIA_BAN_LE, GIA_BAN_SI, ID_DON_VI_TINH để gán vào SanPham
        // Sau              : Tách toàn bộ phần gán 7 trường sang dùng chung EntityMapper.ToSanPham / EntityMapper.ToSoLuongTon
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Code đọc từng cột DataRow để tạo SanPham bị viết thủ công trong Controller
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Lấy từng cột ID, TEN_SAN_PHAM, SO_LUONG, DON_GIA_NHAP... gán tay vào đối tượng SanPham
        // Sau              : Chỉ cần gọi EntityMapper.ToSanPham(tbl.Rows[0]) để chuyển đổi nhanh và đồng nhất
        // Tác giả          : BE2
        // Hỗ trợ           : BE3
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public SanPham LaySanPham(String id)
        {
            DataTable tbl = factory.LaySanPham(id);
            return tbl.Rows.Count > 0 ? EntityMapper.ToSanPham(tbl.Rows[0]) : new SanPham();
        }

        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Lặp lại toàn bộ đoạn code tạo SanPham ngay trong vòng lặp lấy tồn kho
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Dùng vòng lặp foreach tự đọc từng cột tạo SanPham rồi mới gán vào SoLuongTon
        // Sau              : Gọi EntityMapper.MapList<SoLuongTon>(..., EntityMapper.ToSoLuongTon) chỉ trong 1 dòng
        // Tác giả          : BE2
        // Hỗ trợ           : BE3
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public static IList<SoLuongTon> LaySoLuongTon()
        {
            return EntityMapper.MapList<SoLuongTon>(new SanPhamFactory().LaySoLuongTon(), EntityMapper.ToSoLuongTon);
        }

        // ==================== FE-DM  REFACTORING ====================
        // Lỗi (Code smell) : Tight Coupling – Controller nhận 9 tham số WinForms Controls
        // Kỹ thuật         : Remove Parameter / Move DataBinding Responsibility
        // Trước            : HienthiDataGridview nhận DataGridView, BindingNavigator và 7 Controls nhập liệu
        // Sau              : HienthiDataGridview chỉ nhận DataGridView và BindingNavigator, gán BindingSource
        // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Ngày chỉnh sửa   : 11/11/2026
        // ===================================================================
        public void HienthiDataGridview(System.Windows.Forms.DataGridView dg, System.Windows.Forms.BindingNavigator bn)
        {
            System.Windows.Forms.BindingSource bs = new System.Windows.Forms.BindingSource();
            bs.DataSource = factory.DanhsachSanPham();

            bn.BindingSource = bs;
            dg.DataSource = bs;
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