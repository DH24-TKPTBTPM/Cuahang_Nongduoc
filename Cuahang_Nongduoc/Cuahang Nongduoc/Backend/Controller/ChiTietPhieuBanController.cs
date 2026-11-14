using System;
using System.Collections.Generic;
using System.Text;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.DataLayer.Mappers;
using CuahangNongduoc.BusinessObject;
using System.Windows.Forms;
using System.Data;

namespace CuahangNongduoc.Controller
{

    public class ChiTietPhieuBanController
    {
        ChiTietPhieuBanFactory factory = new ChiTietPhieuBanFactory();



        public void HienThiChiTiet(DataGridView dgv, String idPhieuBan)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = factory.LayChiTietPhieuBan(idPhieuBan);
            dgv.DataSource = bs;
        }
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


        // ==================== [R15] REFACTORING ====================
        // Lỗi (Code smell) : 3 hàm overload ChiTietPhieuBan(...) có thân hàm trùng lặp hoàn toàn
        // Kỹ thuật         : Extract Method (kết hợp Mapper [D05])
        // Trước            : Cả 3 overload (theo idPhieuBan, theo ngày bán, theo tháng/năm) đều viết lặp vòng lặp foreach đọc từng dòng DataRow
        // Sau              : Rút gọn cả 3 hàm về 1 dòng gọi dùng chung EntityMapper.MapList(..., EntityMapper.ToChiTietPhieuBan)
        // Tác giả          : BE2
        // Hỗ trợ           : BE1
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Cả 3 hàm lấy chi tiết phiếu bán đều copy y hệt nhau đoạn đọc DataRow
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Viết lặp lại 3 vòng foreach để đọc DON_GIA, SO_LUONG, THANH_TIEN, ID_MA_SAN_PHAM cho 3 kiểu lọc khác nhau
        // Sau              : Mỗi hàm chỉ cần gọi EntityMapper.MapList(..., EntityMapper.ToChiTietPhieuBan) rất ngắn gọn
        // Tác giả          : BE2
        // Hỗ trợ           : BE3
        // Kiểm tra         : QA1
        // Ngày chỉnh sửa   : 12/11/2026
        // ==============================================================
        public IList<ChiTietPhieuBan> ChiTietPhieuBan(String idPhieuBan)
        {
            return EntityMapper.MapList<ChiTietPhieuBan>(factory.LayChiTietPhieuBan(idPhieuBan), EntityMapper.ToChiTietPhieuBan);
        }

        public IList<ChiTietPhieuBan> ChiTietPhieuBan(DateTime dtNgayBan)
        {
            return EntityMapper.MapList<ChiTietPhieuBan>(factory.LayChiTietPhieuBan(dtNgayBan), EntityMapper.ToChiTietPhieuBan);
        }
        public IList<ChiTietPhieuBan> ChiTietPhieuBan(int thang, int nam)
        {
            return EntityMapper.MapList<ChiTietPhieuBan>(factory.LayChiTietPhieuBan(thang, nam), EntityMapper.ToChiTietPhieuBan);
        }

    }
}
