using System;

namespace CuahangNongduoc.Patterns.TemplateMethod
{
    /// <summary>
    /// Template Method Pattern (Behavioral): Định nghĩa khung sườn thuật toán xử lý chứng từ
    /// gồm 5 bước cố định:
    /// 1. Kiểm tra tính hợp lệ (KiemTraHopLe)
    /// 2. Cập nhật tồn kho (CapNhatKho)
    /// 3. Ghi dữ liệu chứng từ (GhiDuLieu)
    /// 4. Cập nhật công nợ (CapNhatCongNo)
    /// 5. In ấn chứng từ (InAn)
    /// </summary>
    public abstract class QuyTrinhXuLyPhieuBase
    {
        /// <summary>
        /// Template Method định nghĩa thứ tự thực thi chuẩn của quy trình xử lý phiếu.
        /// Không cho phép lớp con thay đổi khung sườn này.
        /// </summary>
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
