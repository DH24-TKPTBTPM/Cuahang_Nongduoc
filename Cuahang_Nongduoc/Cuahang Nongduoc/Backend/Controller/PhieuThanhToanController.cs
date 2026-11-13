using System;
using System.Collections.Generic;
using System.Text;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.BusinessObject;
using System.Data;

namespace CuahangNongduoc.Controller
{
    // ==================== [R26] REFACTORING ====================
    // Lỗi (Code smell) : Phương thức nhận quá nhiều tham số (Long Parameter List)
    // Kỹ thuật         : Introduce Parameter Object
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
    // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
    // Ngày chỉnh sửa   : 08/11/2026
    // ==============================================================
    public class PhieuThanhToanSearchParam
    {
        public String IdKhachHang { get; set; }
        public DateTime Ngay { get; set; }
    }

    
    public class PhieuThanhToanController
    {
        PhieuThanhToanFactory factory = new PhieuThanhToanFactory();


        public DataRow NewRow()
        {
            return factory.NewRow();
        }
        public void Add(DataRow row)
        {
            factory.Add(row);
        }
        // ==================== [R33] REFACTORING ====================
        // Lỗi (Code smell) : Controller trả về kiểu dữ liệu không nhất quán
        // Kỹ thuật         : Standardize API Contract
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : BE2 - Trần Tuấn Anh
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public bool Save()
        {
            return factory.Save();
        }

        public PhieuThanhToan LayPhieuThanhToan(String id)
        {
            PhieuThanhToan ph = null;
            DataTable tbl = factory.LayPhieuThanhToan(id);
            if (tbl.Rows.Count > 0 )
            {
                ph = new PhieuThanhToan();
                ph.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                KhachHangController ctrlKH = new KhachHangController();
                ph.KhachHang = ctrlKH.LayKhachHang(Convert.ToString(tbl.Rows[0]["ID_KHACH_HANG"]));
                ph.NgayThanhToan = Convert.ToDateTime(tbl.Rows[0]["NGAY_THANH_TOAN"]);
                ph.TongTien = Convert.ToInt64(tbl.Rows[0]["TONG_TIEN"]);
                ph.GhiChu = Convert.ToString(tbl.Rows[0]["GHI_CHU"]);
            }
            return ph;
        }
        
        // ==================== [R26] REFACTORING ====================
        // Lỗi (Code smell) : Phương thức nhận quá nhiều tham số (Long Parameter List)
        // Kỹ thuật         : Introduce Parameter Object
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public DataTable HienthiPhieuThanhToan()
        {
            return factory.DanhsachPhieuThanhToan();
        }

        // ==================== [R26] REFACTORING ====================
        // Lỗi (Code smell) : Phương thức nhận quá nhiều tham số (Long Parameter List)
        // Kỹ thuật         : Introduce Parameter Object
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public DataTable TimPhieuThanhToan(PhieuThanhToanSearchParam param)
        {
            return factory.TimPhieuThanhToan(param.IdKhachHang, param.Ngay);
        }

        
    }
}
