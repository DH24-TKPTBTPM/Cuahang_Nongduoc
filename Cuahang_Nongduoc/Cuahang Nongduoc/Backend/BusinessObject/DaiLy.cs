using System;
using System.Collections.Generic;
using System.Text;

namespace CuahangNongduoc.BusinessObject
{
    public class DaiLy
    {
        public DaiLy() { }

        public DaiLy(String id)
        {
            m_Id = id;
        }

        public DaiLy(String id, String tenDaiLy, String diaChi, String dienThoai)
        {
            m_Id = id;
            m_TenDaiLy = tenDaiLy;
            m_DiaChi = diaChi;
            m_DienThoai = dienThoai;
        }

        private String m_Id;
        public String Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        private String m_TenDaiLy;
        public String TenDaiLy
        {
            get { return m_TenDaiLy; }
            set { m_TenDaiLy = value; }
        }

        private String m_DiaChi;
        public String DiaChi
        {
            get { return m_DiaChi; }
            set { m_DiaChi = value; }
        }

        private String m_DienThoai;
        public String DienThoai
        {
            get { return m_DienThoai; }
            set { m_DienThoai = value; }
        }
    }
}