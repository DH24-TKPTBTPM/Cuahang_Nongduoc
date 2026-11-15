// ==================== [D06] REFACTORING ====================
// Kỹ thuật         : Strategy Pattern (GoF)
// Chức năng        : Bộ Validator nghiệp vụ
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 08/11/2026
// ==============================================================
namespace CuahangNongduoc.BusinessObject.Validation
{
    public interface IValidationStrategy
    {
        bool Validate(object data, out string errorMessage);
    }
}
