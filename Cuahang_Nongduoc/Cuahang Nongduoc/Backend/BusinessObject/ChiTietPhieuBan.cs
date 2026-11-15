using System;
using System.Collections.Generic;
using System.Text;
//thang cuong xe co
namespace CuahangNongduoc.BusinessObject
{
    public class ChiTietPhieuBan
    {
        private PhieuBan m_PhieuBan;

        public PhieuBan PhieuBan
        {
            get { return m_PhieuBan; }
            set { m_PhieuBan = value; }
        }
        private MaSanPham m_MaSP;

        public MaSanPham MaSanPham
        {
            get { return m_MaSP; }
            set { m_MaSP = value; }
        }
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
// ==================== [R34] REFACTORING ====================
// Lỗi (Code smell) : Thuộc tính dẫn xuất chưa tự tính, tên biến sai ngữ nghĩa
// Kỹ thuật         : Encapsulate Field + Rename Field
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        public long ThanhTien
        {
            get { return m_SoLuong * m_DonGia; }
        }

    }
}
