using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.BusinessObject
{
    public class SoLuongTon
    {
        private SanPham  m_SanPham;

        public SanPham  SanPham
        {
            get { return m_SanPham; }
            set { m_SanPham = value; }
        }
        private int m_SoLuongTon;

        public int SoLuong
        {
            get { return m_SoLuongTon; }
            set { m_SoLuongTon = value; }
        }

        // Các thuộc tính phẳng dành cho RDLC. ReportViewer có thể trả #Error
        // khi biểu thức truy cập thuộc tính lồng nhau của object tùy chỉnh.
        public string MaSanPhamBaoCao
        {
            get { return SanPham == null ? string.Empty : (SanPham.Id ?? string.Empty); }
        }

        public string TenSanPhamBaoCao
        {
            get { return SanPham == null ? string.Empty : (SanPham.TenSanPham ?? string.Empty); }
        }

        public long DonGiaNhapBaoCao
        {
            get { return SanPham == null ? 0L : SanPham.DonGiaNhap; }
        }

        public long GiaBanLeBaoCao
        {
            get { return SanPham == null ? 0L : SanPham.GiaBanLe; }
        }

        public long GiaBanSiBaoCao
        {
            get { return SanPham == null ? 0L : SanPham.GiaBanSi; }
        }

        public string DonViTinhBaoCao
        {
            get
            {
                return SanPham == null || SanPham.DonViTinh == null
                    ? string.Empty
                    : (SanPham.DonViTinh.Ten ?? string.Empty);
            }
        }

	
    }
}
