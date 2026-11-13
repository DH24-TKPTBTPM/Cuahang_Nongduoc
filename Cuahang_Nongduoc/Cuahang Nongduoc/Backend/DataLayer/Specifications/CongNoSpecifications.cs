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
    public class CongNoTheoKhoangThoiGianSpec : Specification<DuNoKhachHang>
    {
        private readonly int m_KyBatDau;
        private readonly int m_KyKetThuc;

        public CongNoTheoKhoangThoiGianSpec(int thangBatDau, int namBatDau,
            int thangKetThuc, int namKetThuc)
        {
            m_KyBatDau = namBatDau * 12 + thangBatDau;
            m_KyKetThuc = namKetThuc * 12 + thangKetThuc;
        }

        public override bool IsSatisfiedBy(DuNoKhachHang congNo)
        {
            if (congNo == null)
                return false;

            int kyHienTai = congNo.Nam * 12 + congNo.Thang;
            return kyHienTai >= m_KyBatDau && kyHienTai <= m_KyKetThuc;
        }
    }

    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public class CongNoTheoSoDuSpec : Specification<DuNoKhachHang>
    {
        private readonly long m_SoDuTu;
        private readonly long m_SoDuDen;

        public CongNoTheoSoDuSpec(long soDuTu, long soDuDen)
        {
            m_SoDuTu = soDuTu;
            m_SoDuDen = soDuDen;
        }

        public override bool IsSatisfiedBy(DuNoKhachHang congNo)
        {
            return congNo != null && congNo.CuoiKy >= m_SoDuTu
                && congNo.CuoiKy <= m_SoDuDen;
        }
    }
}
