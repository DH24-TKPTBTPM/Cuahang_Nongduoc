using System;

namespace CuahangNongduoc.BusinessObject.Events
{
    // ==================== [D07] REFACTORING ====================
    // Lỗi (Code smell) : Khi kho tăng/giảm số lượng, hệ thống không có sự kiện báo cho các chức năng theo dõi (ghi log kho, cảnh báo hết hàng/hết hạn)
    // Kỹ thuật         : Observer Pattern (GoF)
    // Trước            : Chưa có lớp sự kiện và bộ lắng nghe mỗi khi tồn kho thay đổi
    // Sau              : Tạo gói sự kiện InventoryChangedEventArgs, giao diện IInventoryObserver cùng 2 bộ lắng nghe CapNhatKhoObserver (ghi log kho) và CanhBaoTonKhoObserver (báo hết hàng/hết hạn)
    // Tác giả          : BE2
    // Hỗ trợ           : BE3
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 12/11/2026
    // ==============================================================
    public class InventoryChangedEventArgs : EventArgs
    {
        public string MaSanPham { get; private set; }
        public int SoLuongThayDoi { get; private set; }
        public int SoLuongTon { get; private set; }
        public DateTime NgayHetHan { get; private set; }

        public InventoryChangedEventArgs(string maSanPham, int soLuongThayDoi, int soLuongTon, DateTime ngayHetHan)
        {
            MaSanPham = maSanPham;
            SoLuongThayDoi = soLuongThayDoi;
            SoLuongTon = soLuongTon;
            NgayHetHan = ngayHetHan;
        }
    }

    public interface IInventoryObserver
    {
        void Update(InventoryChangedEventArgs e);
    }

    public class CapNhatKhoObserver : IInventoryObserver
    {
        public void Update(InventoryChangedEventArgs e)
        {
            if (e == null) return;
            AppLogger.Instance.Info(string.Format("[Kho] Mã SP {0}: thay đổi {1:+#;-#;0}, tồn hiện tại = {2}", e.MaSanPham, e.SoLuongThayDoi, e.SoLuongTon));
        }
    }

    public class CanhBaoTonKhoObserver : IInventoryObserver
    {
        public const int NguongSapHet = 5;
        public string LastWarning { get; private set; }

        public void Update(InventoryChangedEventArgs e)
        {
            if (e == null) return;
            string msg = null;
            if (e.SoLuongTon <= 0)
                msg = string.Format("Cảnh báo hết hàng: Mã {0} (tồn = {1})", e.MaSanPham, e.SoLuongTon);
            else if (e.SoLuongTon <= NguongSapHet)
                msg = string.Format("Cảnh báo sắp hết hàng: Mã {0} (tồn = {1})", e.MaSanPham, e.SoLuongTon);

            if (e.NgayHetHan != DateTime.MinValue && e.NgayHetHan.Date <= DateTime.Today)
            {
                string expMsg = string.Format("Cảnh báo hết hạn: Mã {0} (HSD {1:dd/MM/yyyy})", e.MaSanPham, e.NgayHetHan);
                msg = string.IsNullOrEmpty(msg) ? expMsg : msg + " | " + expMsg;
            }

            if (!string.IsNullOrEmpty(msg))
            {
                LastWarning = msg;
                AppLogger.Instance.Info("[Cảnh báo kho] " + msg);
            }
        }
    }
}
