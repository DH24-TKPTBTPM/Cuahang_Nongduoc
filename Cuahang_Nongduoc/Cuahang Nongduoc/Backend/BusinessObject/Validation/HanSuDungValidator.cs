// ==================== [D06] REFACTORING ====================
// Kỹ thuật         : Strategy Pattern (GoF)
// Chức năng        : Bộ Validator nghiệp vụ
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 08/11/2026
// ==============================================================
using System;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.BusinessObject.Validation
{
    public class HanSuDungValidator : IValidationStrategy
    {
        public bool Validate(object data, out string errorMessage)
        {
            errorMessage = string.Empty;
            DateTime ngaySanXuat;
            DateTime hanSuDung;
            ChiTietPhieuNhap chiTietNhap = data as ChiTietPhieuNhap;
            MaSanPham maSanPham = data as MaSanPham;

            if (chiTietNhap != null)
            {
                ngaySanXuat = chiTietNhap.NSX;
                hanSuDung = chiTietNhap.HSD;
            }
            else if (maSanPham != null)
            {
                ngaySanXuat = maSanPham.NgaySanXuat;
                hanSuDung = maSanPham.NgayHetHan;
            }
            else
            {
                errorMessage = "Dữ liệu kiểm tra hạn sử dụng không hợp lệ.";
                return false;
            }

            if (hanSuDung <= ngaySanXuat)
            {
                errorMessage = "Hạn sử dụng phải lớn hơn ngày sản xuất.";
                return false;
            }

            return true;
        }
    }
}
