namespace CuahangNongduoc.DataLayer.Specifications
{
    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
    }

    // ==================== [D12] DESIGN PATTERN ====================
    // Vấn đề           : Lọc sản phẩm/công nợ với nhiều tiêu chí phức tạp, hardcode
    // Kỹ thuật (Pattern): Specification (PoEAA)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày tạo         : 11/11/2026
    // ==============================================================
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);

        public Specification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public Specification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
