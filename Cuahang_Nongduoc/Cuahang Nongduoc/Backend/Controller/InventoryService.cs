using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CuahangNongduoc.BusinessObject.Events;

namespace CuahangNongduoc.Controller
{
    // ==================== [D07] REFACTORING ====================
    // Lỗi (Code smell) : Tight Coupling – Tầng dữ liệu không có Subject trung tâm để thông báo thay đổi tồn kho tới các bộ xử lý liên quan
    // Kỹ thuật         : Observer Pattern (GoF)
    // Trước            : Chưa có lớp quản lý danh sách Observer và phát sự kiện tồn kho
    // Sau              : Viết mới InventoryService (Subject) quản lý Attach/Detach/Notify và truy vấn tồn kho/HSD để phát sự kiện
    // Tác giả          : BE2
    // Hỗ trợ           : BE3
    // Kiểm tra         : QA1
    // Ngày chỉnh sửa   : 12/11/2026
    // ==============================================================
    public class InventoryService
    {
        private static readonly InventoryService _instance = new InventoryService();
        private readonly List<IInventoryObserver> _observers = new List<IInventoryObserver>();

        public event EventHandler<InventoryChangedEventArgs> InventoryChanged;
        public static InventoryService Instance { get { return _instance; } }

        public InventoryService()
        {
            Attach(new CapNhatKhoObserver());
            Attach(new CanhBaoTonKhoObserver());
        }

        public void Attach(IInventoryObserver observer)
        {
            if (observer != null && !_observers.Contains(observer)) _observers.Add(observer);
        }

        public void Detach(IInventoryObserver observer)
        {
            if (observer != null) _observers.Remove(observer);
        }

        public void Notify(InventoryChangedEventArgs e)
        {
            foreach (IInventoryObserver observer in _observers) observer.Update(e);
            if (InventoryChanged != null) InventoryChanged(this, e);
        }

        public void NotifyInventoryChanged(string maSanPham, int soLuongThayDoi)
        {
            if (string.IsNullOrEmpty(maSanPham)) return;
            try
            {
                DataService ds = new DataService();
                SqlCommand cmd = new SqlCommand("SELECT SO_LUONG, NGAY_HET_HAN FROM MA_SAN_PHAM WHERE ID = @id");
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = maSanPham;
                ds.Load(cmd);
                if (ds.Rows.Count == 0) return;

                DataRow r = ds.Rows[0];
                int ton = r["SO_LUONG"] == DBNull.Value ? 0 : Convert.ToInt32(r["SO_LUONG"]);
                DateTime hsd = r["NGAY_HET_HAN"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(r["NGAY_HET_HAN"]);
                Notify(new InventoryChangedEventArgs(maSanPham, soLuongThayDoi, ton, hsd));
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Error("InventoryService.NotifyInventoryChanged lỗi", ex);
            }
        }
    }
}
