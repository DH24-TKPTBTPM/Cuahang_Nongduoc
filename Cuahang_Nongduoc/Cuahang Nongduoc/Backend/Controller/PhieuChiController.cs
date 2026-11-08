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
    public class PhieuChiSearchParam
    {
        public int LyDo { get; set; }
        public DateTime Ngay { get; set; }
    }

    
    public class PhieuChiController
    {
        PhieuChiFactory factory = new PhieuChiFactory();


        public DataRow NewRow()
        {
            return factory.NewRow();
        }
        public void Add(DataRow row)
        {
            factory.Add(row);
        }
        public void Save()
        {
            factory.Save();
        }

        public PhieuChi LayPhieuChi(String id)
        {
            PhieuChi ph = null;
            DataTable tbl = factory.LayPhieuChi(id);
            if (tbl.Rows.Count > 0 )
            {
                ph = new PhieuChi();
                ph.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                LyDoChiController ctrlLyDo = new LyDoChiController();
                ph.LyDoChi = ctrlLyDo.LayLyDoChi(Convert.ToInt64(tbl.Rows[0]["ID_LY_DO_CHI"]));
                ph.NgayChi = Convert.ToDateTime(tbl.Rows[0]["NGAY_CHI"]);
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
        public DataTable HienthiPhieuChi()
        {
            return factory.DanhsachPhieuChi();
        }

        // ==================== [R26] REFACTORING ====================
        // Lỗi (Code smell) : Phương thức nhận quá nhiều tham số (Long Parameter List)
        // Kỹ thuật         : Introduce Parameter Object
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        public DataTable TimPhieuChi(PhieuChiSearchParam param)
        {
            return factory.TimPhieuChi(param.LyDo, param.Ngay);
        }
    }
}
