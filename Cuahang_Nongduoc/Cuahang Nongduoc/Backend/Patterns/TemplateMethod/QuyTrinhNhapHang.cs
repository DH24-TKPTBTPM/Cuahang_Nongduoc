// ==================== [TM-01] REFACTORING ====================
// Kỹ thuật         : Template Method Pattern (GoF)
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Hỗ trợ           : BE2 - Trần Tuấn Anh
// Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
// Ngày chỉnh sửa   : 08/11/2026
// ==============================================================
namespace CuahangNongduoc.Patterns.TemplateMethod
{
    public class QuyTrinhNhapHang : QuyTrinhXuLyPhieuBase
    {
        protected override bool KiemTraHopLe()
        {
            return true;
        }

        protected override bool CapNhatKho()
        {
            return true;
        }

        protected override bool GhiDuLieu()
        {
            return true;
        }
    }
}
