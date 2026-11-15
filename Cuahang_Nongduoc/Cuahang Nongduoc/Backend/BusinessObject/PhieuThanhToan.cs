using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.BusinessObject
{
    public class PhieuThanhToan
    {
        public PhieuThanhToan() { }
        public PhieuThanhToan(String id)
        {
            m_Id = id;
        }
        private String m_Id;

        public String Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
// ==================== [R34] REFACTORING ====================
// Lỗi (Code smell) : Thuộc tính dẫn xuất chưa tự tính, tên biến sai ngữ nghĩa
// Kỹ thuật         : Encapsulate Field + Rename Field
// Tác giả          : BE3 - Bùi Lê Tuấn Anh
// Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
// Ngày chỉnh sửa   : 11/11/2026
// ==============================================================
        private DateTime m_NgayThanhToan;

        public DateTime NgayThanhToan
        {
            get { return m_NgayThanhToan; }
            set { m_NgayThanhToan = value; }
        }
        private long m_TongTien;

        public long TongTien
        {
            get { return m_TongTien; }
            set { m_TongTien = value; }
        }

        private KhachHang m_KH;

        public KhachHang KhachHang
        {
            get { return m_KH; }
            set { m_KH = value; }
        }

        private String m_GhiChu;

        public String GhiChu
        {
            get { return m_GhiChu; }
            set { m_GhiChu = value; }
        }
	

	
    }
}
