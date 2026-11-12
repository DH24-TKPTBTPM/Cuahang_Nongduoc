using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.DataLayer.Mappers;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Controller
{
    public class MaSanPhamController
    {
        MaSanPhanFactory factory = new MaSanPhanFactory();

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

        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Đoạn code chuyển DataRow sang SanPham/MaSanPham bị copy lặp lại ở 3 hàm liên tiếp
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Cả 3 hàm LaySanPham, LayMaSanPham, LayMaSanPhamHetHan đều tự đọc từng cột rồi gán tay vào đối tượng
        // Sau              : Dùng chung EntityMapper.ToSanPham, EntityMapper.ToMaSanPham và EntityMapper.MapList cho gọn và dễ sửa
        // Tác giả          : BE2
        // Hỗ trợ           : -
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public SanPham LaySanPham(String idMaSanPham)
        {
            DataTable tbl = new MaSanPhanFactory().LaySanPham(idMaSanPham);
            return tbl.Rows.Count > 0 ? EntityMapper.ToSanPham(tbl.Rows[0]) : null;
        }

        public MaSanPham LayMaSanPham(String idMaSanPham)
        {
            DataTable tbl = new MaSanPhanFactory().LayMaSanPham(idMaSanPham);
            return tbl.Rows.Count > 0 ? EntityMapper.ToMaSanPham(tbl.Rows[0]) : null;
        }

        public static IList<MaSanPham> LayMaSanPhamHetHan(DateTime dt)
        {
            return EntityMapper.MapList<MaSanPham>(new MaSanPhanFactory().DanhsachMaSanPhamHetHan(dt), EntityMapper.ToMaSanPham);
        }

        public void HienThiAutoComboBox(String sp, ComboBox cmb)
        {
            cmb.DataSource = factory.DanhsachMaSanPham(sp);
            cmb.DisplayMember = "ID";
            cmb.ValueMember = "ID";
        }

        public void HienThiDataGridViewComboBox(DataGridViewComboBoxColumn cmb)
        {
            cmb.DataSource = factory.DanhsachMaSanPham();
            cmb.DisplayMember = "ID";
            cmb.ValueMember = "ID";
            cmb.DataPropertyName = "ID_MA_SAN_PHAM";
            cmb.HeaderText = "Mã sản phẩm";
        }

        public void HienThiChiTietPhieuNhap(String id, DataGridView dg)
        {
            
            dg.DataSource = factory.DanhsachChiTiet(id);
        }
        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Lặp lại vòng lặp chuyển DataRow sang danh sách MaSanPham
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Tự duyệt DataTable, gán tay từng thuộc tính và tự nhân SO_LUONG * DON_GIA_NHAP cho ThanhTien
        // Sau              : Gọi EntityMapper.MapList<MaSanPham>(..., EntityMapper.ToMaSanPham) để xử lý trọn gói
        // Tác giả          : BE2
        // Hỗ trợ           : -
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public IList<MaSanPham> ChiTietPhieuNhap(String id)
        {
            return EntityMapper.MapList<MaSanPham>(factory.DanhsachChiTiet(id), EntityMapper.ToMaSanPham);
        }

    }
}
