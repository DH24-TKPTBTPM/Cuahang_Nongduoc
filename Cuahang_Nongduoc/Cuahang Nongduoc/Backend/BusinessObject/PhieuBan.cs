using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.BusinessObject
{
    public class PhieuBan
    {
        private String m_Id;

        public String Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        private KhachHang m_KH;

        public KhachHang KhachHang
        {
            get { return m_KH; }
            set { m_KH = value; }
        }

        private DateTime m_NgayBan;

        public DateTime NgayBan
        {
            get { return m_NgayBan; }
            set { m_NgayBan = value; }
        }
        private long m_TongTien;

        public long TongTien
        {
            get { return m_TongTien; }
            set { m_TongTien = value; }
        }
        private long m_DaTra;

        public long DaTra
        {
            get { return m_DaTra; }
            set { m_DaTra = value; }
        }
        private long m_ConNo;

        public long ConNo
        {
            get { return m_ConNo; }
            set { m_ConNo = value; }
        }

        public long TienHang { get; set; }
        public long TongDichVu { get; set; }
        public bool GiaoHang { get; set; }
        public decimal KhoangCachKm { get; set; }
        public long PhiVanChuyen { get; set; }
        public string LoaiGiamGia { get; set; }
        public decimal TyLeGiamGia { get; set; }
        public long TienGiamGia { get; set; }
        public IList<DichVuPhieuBan> DichVu { get; set; }

        private IList<ChiTietPhieuBan> m_ChiTiet;

        public IList<ChiTietPhieuBan> ChiTiet
        {
            get { return m_ChiTiet; }
            set { m_ChiTiet = value; }
        }

        public string TenKhachHang
        {
            get { return m_KH != null ? m_KH.HoTen : "Khách bán lẻ"; }
        }

        public string DiaChiKhachHang
        {
            get { return m_KH != null ? m_KH.DiaChi : "-"; }
        }

        public string DienThoaiKhachHang
        {
            get { return m_KH != null ? m_KH.DienThoai : "-"; }
        }
    }
}
