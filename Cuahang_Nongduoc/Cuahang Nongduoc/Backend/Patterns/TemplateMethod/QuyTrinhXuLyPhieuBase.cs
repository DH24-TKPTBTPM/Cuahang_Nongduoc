using System;

namespace CuahangNongduoc.Patterns.TemplateMethod
{
    // ==================== [TM-01] REFACTORING ====================
    // Kỹ thuật         : Template Method Pattern (GoF)
    // Tác giả          : BE3 - Bùi Lê Tuấn Anh
    // Hỗ trợ           : BE2 - Trần Tuấn Anh
    // Kiểm duyệt       : QA1 - Huỳnh Lư Anh Minh
    // Ngày chỉnh sửa   : 08/11/2026
    // ==============================================================
    public abstract class QuyTrinhXuLyPhieuBase
    {
        public bool XuLyPhieu()
        {
            if (!KiemTraHopLe())
            {
                return false;
            }

            if (!CapNhatKho())
            {
                return false;
            }

            if (!GhiDuLieu())
            {
                return false;
            }

            CapNhatCongNo();

            if (CanInAn())
            {
                InAn();
            }

            return true;
        }

        protected abstract bool KiemTraHopLe();
        protected abstract bool CapNhatKho();
        protected abstract bool GhiDuLieu();
        protected virtual void CapNhatCongNo() { }
        protected virtual bool CanInAn() { return true; }
        protected virtual void InAn() { }
    }
}
