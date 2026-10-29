using System;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.Patterns.Singleton
{
    /// <summary>
    /// Singleton Pattern (Creational): Quản lý tập trung các tham số toàn cục và sinh mã chứng từ.
    /// </summary>
    public sealed class ThamSoManager
    {
        private static volatile ThamSoManager _instance = null;
        private static readonly object _lock = new object();

        private ThamSoManager()
        {
        }

        public static ThamSoManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ThamSoManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public long LayMaPhieuBan()
        {
            return ThamSo.LayMaPhieuBan();
        }

        public void CapNhatMaPhieuBan(long maPhieuMoi)
        {
            ThamSo.GanMaPhieuBan(maPhieuMoi);
        }

        public long LayMaPhieuNhap()
        {
            return ThamSo.LayMaPhieuNhap();
        }

        public void CapNhatMaPhieuNhap(long maPhieuMoi)
        {
            ThamSo.GanMaPhieuNhap(maPhieuMoi);
        }
    }
}
