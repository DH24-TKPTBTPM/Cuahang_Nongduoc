using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.DataLayer.Specifications
{
    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public class SanPhamTheoTenSpec : Specification<SanPham>
    {
        private readonly string m_TuKhoa;

        public SanPhamTheoTenSpec(string tuKhoa)
        {
            m_TuKhoa = tuKhoa == null ? string.Empty : tuKhoa;
        }

        public override bool IsSatisfiedBy(SanPham sanPham)
        {
            return sanPham != null && sanPham.TenSanPham != null
                && sanPham.TenSanPham.IndexOf(m_TuKhoa, System.StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public class SanPhamTheoKhoangGiaSpec : Specification<SanPham>
    {
        private readonly long m_GiaTu;
        private readonly long m_GiaDen;

        public SanPhamTheoKhoangGiaSpec(long giaTu, long giaDen)
        {
            m_GiaTu = giaTu;
            m_GiaDen = giaDen;
        }

        public override bool IsSatisfiedBy(SanPham sanPham)
        {
            return sanPham != null && sanPham.GiaBanLe >= m_GiaTu
                && sanPham.GiaBanLe <= m_GiaDen;
        }
    }

    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public class SanPhamTheoSoLuongSpec : Specification<SanPham>
    {
        private readonly int m_SoLuongToiThieu;

        public SanPhamTheoSoLuongSpec(int soLuongToiThieu)
        {
            m_SoLuongToiThieu = soLuongToiThieu;
        }

        public override bool IsSatisfiedBy(SanPham sanPham)
        {
            return sanPham != null && sanPham.SoLuong >= m_SoLuongToiThieu;
        }
    }
}
