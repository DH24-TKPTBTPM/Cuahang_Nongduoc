using System;
using System.Collections.Generic;
using System.Data;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer.Mappers;

namespace CuahangNongduoc.DataLayer
{
    /// <summary>
    /// [D04] VIẾT MỚI – Mẫu thiết kế: Repository (PoEAA – Martin Fowler, không thuộc 23 mẫu GoF)
    /// Mục đích : tầng trên (Controller) lấy thẳng đối tượng nghiệp vụ (SanPham...) qua 1 interface chung,
    ///            không phải biết DataTable/DataRow và tên cột của từng DAO
    /// Thay thế : Controller tự gọi Factory lấy DataTable rồi tự map DataRow -> đối tượng
    /// Tác giả  : BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
    /// Ngày tạo : 15/11/2026
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>Toàn bộ đối tượng trong bảng.</summary>
        IList<T> LayTatCa();

        /// <summary>Đối tượng theo mã; null nếu không có.</summary>
        T LayTheoMa(String id);
    }

    /// <summary>
    /// [D04] Repository mẫu cho Sản phẩm – dùng lại SanPhamFactory (truy vấn) và EntityMapper (D05).
    /// BE2/BE3 làm theo mẫu này cho các bảng khác khi cần.
    /// Tác giả  : BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
    /// Ngày tạo : 15/11/2026
    /// </summary>
    public class SanPhamRepository : IRepository<SanPham>
    {
        private readonly SanPhamFactory m_Factory = new SanPhamFactory();

        public IList<SanPham> LayTatCa()
        {
            return EntityMapper.MapList<SanPham>(m_Factory.DanhsachSanPham(), EntityMapper.ToSanPham);
        }

        public SanPham LayTheoMa(String id)
        {
            DataTable tbl = m_Factory.LaySanPham(id);
            return tbl.Rows.Count > 0 ? EntityMapper.ToSanPham(tbl.Rows[0]) : null;
        }
    }
}
