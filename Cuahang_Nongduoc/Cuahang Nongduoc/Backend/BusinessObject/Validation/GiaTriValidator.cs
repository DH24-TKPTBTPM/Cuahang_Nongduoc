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
    public class GiaTriValidator : IValidationStrategy
    {
        public bool Validate(object data, out string errorMessage)
        {
            errorMessage = string.Empty;
            long donGia;
            ChiTietPhieuBan chiTietBan = data as ChiTietPhieuBan;
            ChiTietPhieuNhap chiTietNhap = data as ChiTietPhieuNhap;

            if (chiTietBan != null)
            {
                donGia = chiTietBan.DonGia;
            }
            else if (chiTietNhap != null)
            {
                donGia = chiTietNhap.DonGia;
            }
            else
            {
                errorMessage = "Dữ liệu kiểm tra giá trị không hợp lệ.";
                return false;
            }

            if (donGia <= 0)
            {
                errorMessage = "Đơn giá phải lớn hơn 0.";
                return false;
            }

            return true;
        }
    }
}
