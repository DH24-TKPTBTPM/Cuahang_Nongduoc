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
        public void Save()
        {
            factory.Save();
        }


        // ==================== [D05] REFACTORING ====================
        // Lỗi (Code smell) : Cả 3 hàm lấy chi tiết phiếu bán đều copy y hệt nhau đoạn đọc DataRow
        // Kỹ thuật         : Mapper Pattern (PoEAA)
        // Trước            : Viết lặp lại 3 vòng foreach để đọc DON_GIA, SO_LUONG, THANH_TIEN, ID_MA_SAN_PHAM cho 3 kiểu lọc khác nhau
        // Sau              : Mỗi hàm chỉ cần gọi EntityMapper.MapList(..., EntityMapper.ToChiTietPhieuBan) rất ngắn gọn
        // Tác giả          : BE2
        // Hỗ trợ           : -
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
