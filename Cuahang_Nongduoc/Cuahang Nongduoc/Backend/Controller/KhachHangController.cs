using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;


namespace CuahangNongduoc.Controller
{
    public class KhachHangController
    {
        KhachHangFactory factory = new KhachHangFactory();

// ==================== [R23] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng Magic Number (số cứng) khi truy xuất cột
// Kỹ thuật         : Replace Magic Number
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE2
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public DataTable HienthiAutoComboBox(bool loai)
        {
            return factory.DanhsachKhachHang(loai);
        }

// ==================== [R23] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng Magic Number (số cứng) khi truy xuất cột
// Kỹ thuật         : Replace Magic Number
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE2
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public DataTable HienthiChungAutoComboBox()
        {
            return factory.DanhsachKhachHang();
        }

// ==================== [R23] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng Magic Number (số cứng) khi truy xuất cột
// Kỹ thuật         : Replace Magic Number
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE2
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public DataTable HienthiKhachHangDataGridview(bool loai)
        {
            DataTable tbl = factory.DanhsachKhachHang(loai);
            tbl.Columns["LOAI_KH"].DefaultValue = loai;
            return tbl;
        }

// ==================== [R23] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng Magic Number (số cứng) khi truy xuất cột
// Kỹ thuật         : Replace Magic Number
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE2
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public DataTable HienthiKhachHangChungDataGridviewComboBox()
        {
            return factory.DanhsachKhachHang();
        }

// ==================== [R23] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng Magic Number (số cứng) khi truy xuất cột
// Kỹ thuật         : Replace Magic Number
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE2
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public DataTable HienthiKhachHangDataGridviewComboBox()
        {
            return factory.DanhsachKhachHang(false);
        }

// ==================== [R23] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng Magic Number (số cứng) khi truy xuất cột
// Kỹ thuật         : Replace Magic Number
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE2
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public DataTable HienthiDaiLyDataGridviewComboBox()
        {
            return factory.DanhsachKhachHang(true);
        }

// ==================== [R23] REFACTORING ====================
// Lỗi (Code smell) : Sử dụng Magic Number (số cứng) khi truy xuất cột
// Kỹ thuật         : Replace Magic Number
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : FE2
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public DataTable HienthiDaiLyDataGridview(bool loai)
        {
            DataTable tbl = factory.DanhsachKhachHang(loai);
            tbl.Columns["LOAI_KH"].DefaultValue = loai;
            return tbl;
        }

        public void TimHoTen(String hoten, bool loai)
        {
            factory.TimHoTen(hoten, loai);
        }
        public void TimDiaChi(String diachi, bool loai)
        {
            factory.TimDiaChi(diachi, loai);
        }
        
        public KhachHang LayKhachHang(String id)
        {
            DataTable tbl = factory.LayKhachHang(id);
            KhachHang kh = new KhachHang();
            if (tbl.Rows.Count > 0)
            {
                kh.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                kh.HoTen = Convert.ToString(tbl.Rows[0]["HO_TEN"]);
                kh.DienThoai = Convert.ToString(tbl.Rows[0]["DIEN_THOAI"]);
                kh.DiaChi = Convert.ToString(tbl.Rows[0]["DIA_CHI"]);
                kh.LoaiKH = Convert.ToBoolean(tbl.Rows[0]["LOAI_KH"]);
            }
            return kh;
        }

        public IList<KhachHang> LayDanhSachKhachHang()
        {
            DataTable tbl = factory.DanhsachKhachHang();
            IList<KhachHang> ds = new List<KhachHang>();

            foreach (DataRow row in tbl.Rows)
            {
                KhachHang kh = new KhachHang();
                kh.Id = Convert.ToString(row["ID"]);
                kh.HoTen = Convert.ToString(row["HO_TEN"]);
                kh.DienThoai = Convert.ToString(row["DIEN_THOAI"]);
                kh.DiaChi = Convert.ToString(row["DIA_CHI"]);
                kh.LoaiKH = Convert.ToBoolean(row["LOAI_KH"]);
                ds.Add(kh);
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
