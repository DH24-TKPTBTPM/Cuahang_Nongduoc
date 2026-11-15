// ==================== [D06] REFACTORING ====================
// Kỹ thuật         : Strategy Pattern (GoF)
// Chức năng        : Bộ Validator nghiệp vụ
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 08/11/2026
// ==============================================================
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.BusinessObject.Validation
{
    public class TonKhoValidator : IValidationStrategy
    {
        public bool Validate(object data, out string errorMessage)
        {
            errorMessage = string.Empty;
            ChiTietPhieuBan chiTiet = data as ChiTietPhieuBan;

            if (chiTiet == null || chiTiet.MaSanPham == null)
            {
                errorMessage = "Dữ liệu kiểm tra tồn kho không hợp lệ.";
                return false;
            }

            if (chiTiet.SoLuong < 0 || chiTiet.SoLuong > chiTiet.MaSanPham.SoLuong)
            {
                errorMessage = "Số lượng xuất vượt quá số lượng tồn kho.";
                return false;
            }

            return true;
        }
    }
}
