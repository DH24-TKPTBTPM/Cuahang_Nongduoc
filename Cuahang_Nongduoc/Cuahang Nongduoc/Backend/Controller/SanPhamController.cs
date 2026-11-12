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
        // Lỗi (Code smell) : Type Mismatch (Convert.ToInt32) & tiềm ẩn lỗi chia cho 0 (DivideByZeroException)
        // Kỹ thuật         : Introduce Assertion
        // Trước            : long tong_so = Convert.ToInt32(tbl.Rows[0]["SO_LUONG"]);
        //                    tbl.Rows[0]["DON_GIA_NHAP"] = thanh_tien / tong_so; // Chưa chặn tong_so <= 0
        // Sau              : Đổi Convert.ToInt32 sang Convert.ToInt64 đồng bộ kiểu long;
        //                    dùng Assertion và kiểm tra tong_so > 0 chặn triệt để lỗi chia cho 0.
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA1 
        // Ngày chỉnh sửa   : 05/11/2026
        // ===========================================================
        public void CapNhatGiaNhap(String id, long gia_moi ,long so_luong)
        {
            DataTable tbl = factory.LaySanPham(id);
            if (tbl.Rows.Count > 0)
            {
                long tong_so = Convert.ToInt64(tbl.Rows[0]["SO_LUONG"]);
                long tong_gia = Convert.ToInt64(tbl.Rows[0]["DON_GIA_NHAP"]);

                long thanh_tien = gia_moi * so_luong + tong_gia * tong_so;
                tong_so += so_luong;

                if (tong_gia != gia_moi)
                {
                    System.Diagnostics.Debug.Assert(tong_so > 0, "Tổng số lượng sau khi nhập phải lớn hơn 0 để tính đơn giá bình quân.");
                    if (tong_so > 0)
                    {
                        tbl.Rows[0]["DON_GIA_NHAP"] = thanh_tien / tong_so;
                    }
                }
                tbl.Rows[0]["SO_LUONG"] = tong_so;

                factory.Save();
            }

        }
    
        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Code đọc từng cột DataRow để tạo SanPham bị viết thủ công trong Controller
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Lấy từng cột ID, TEN_SAN_PHAM, SO_LUONG, DON_GIA_NHAP... gán tay vào đối tượng SanPham
        // Sau              : Chỉ cần gọi EntityMapper.ToSanPham(tbl.Rows[0]) để chuyển đổi nhanh và đồng nhất
        // Tác giả          : BE2
        // Hỗ trợ           : -
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
        // Hỗ trợ           : -
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
