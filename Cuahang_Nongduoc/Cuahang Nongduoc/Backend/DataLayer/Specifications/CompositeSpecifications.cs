namespace CuahangNongduoc.DataLayer.Specifications
{
    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public class AndSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> m_Left;
        private readonly ISpecification<T> m_Right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            m_Left = left;
            m_Right = right;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return m_Left.IsSatisfiedBy(candidate) && m_Right.IsSatisfiedBy(candidate);
        }
    }

    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public class OrSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> m_Left;
        private readonly ISpecification<T> m_Right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            m_Left = left;
            m_Right = right;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return m_Left.IsSatisfiedBy(candidate) || m_Right.IsSatisfiedBy(candidate);
        }
    }

    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public class NotSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> m_Specification;

        public NotSpecification(ISpecification<T> specification)
        {
            m_Specification = specification;
        }

        public override bool IsSatisfiedBy(T candidate)
        {
            return !m_Specification.IsSatisfiedBy(candidate);
        }
    }
}
