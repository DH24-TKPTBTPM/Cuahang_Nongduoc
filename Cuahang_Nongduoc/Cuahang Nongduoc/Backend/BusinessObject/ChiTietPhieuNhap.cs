using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.BusinessObject
{
    public class ChiTietPhieuNhap
    {
        private PhieuNhap m_PhieuNhap;

        public PhieuNhap PhieuNhap
        {
            get { return m_PhieuNhap; }
            set { m_PhieuNhap = value; }
        }
        private MaSanPham m_MaSP;

        public MaSanPham MaSanPham
        {
            get { return m_MaSP; }
            set { m_MaSP = value; }
        }

        // ==================== [R25] REFACTORING ====================
        // Lỗi (Code smell) : Incomplete Class – Lớp thực thể thiếu các thuộc tính trọng yếu
        // Kỹ thuật         : Add Parameter / Encapsulate Field
        // Trước            : (Chưa có các thuộc tính chi tiết nhập)
        // Sau              : (Bổ sung SoLuong, DonGia, ThanhTien, NSX, HSD)
        // Tác giả          : BE3 – Bùi Lê Tuấn Anh
        // Hỗ trợ           : 
        // Kiểm tra         : 
        // Ngày chỉnh sửa   : 05/11/2026
        // ==============================================================
        private int m_SoLuong;

        public int SoLuong
        {
            get { return m_SoLuong; }
            set { m_SoLuong = value; }
        }

        private long m_DonGia;

        public long DonGia
        {
            get { return m_DonGia; }
            set { m_DonGia = value; }
        }

        private long m_ThanhTien;

        public long ThanhTien
        {
            get { return m_ThanhTien; }
            set { m_ThanhTien = value; }
        }

        private DateTime m_NSX;

        public DateTime NSX
        {
            get { return m_NSX; }
            set { m_NSX = value; }
        }

        private DateTime m_HSD;

        public DateTime HSD
        {
            get { return m_HSD; }
            set { m_HSD = value; }
        }

    }
}
