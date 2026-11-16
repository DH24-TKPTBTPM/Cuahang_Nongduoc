using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmInDunoKhachHang : Form
    {
        CuahangNongduoc.BusinessObject.DuNoKhachHang m_DuNo;
        public frmInDunoKhachHang(CuahangNongduoc.BusinessObject.DuNoKhachHang  dn)
        {
            InitializeComponent();
            m_DuNo = dn;
        }

        // Đọc thuộc tính chuỗi của đối tượng theo tên (thử lần lượt các tên), không có hoặc rỗng thì trả về "".
        private static string LayChuoi(object obj, params string[] tenThuocTinh)
        {
            if (obj == null)
                return String.Empty;
            foreach (string ten in tenThuocTinh)
            {
                System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(ten);
                if (pi != null)
                {
                    object v = pi.GetValue(obj, null);
                    return v == null ? String.Empty : Convert.ToString(v);
                }
            }
            return String.Empty;
        }

        private void frmInDunoKhachHang_Load(object sender, EventArgs e)
        {
            Num2Str num = new Num2Str();
            IList<Microsoft.Reporting.WinForms.ReportParameter> param = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            CuahangNongduoc.BusinessObject.CuaHang ch = ThamSo.LayCuaHang();
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("ten_cua_hang", ch.TenCuaHang));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dia_chi", ch.DiaChi));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dien_thoai", ch.DienThoai));

            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("bang_chu", num.NumberToString(m_DuNo.CuoiKy.ToString())));

            // Thông tin khách hàng truyền qua tham số (trường rỗng thì in trống) thay vì để report tự đọc đối tượng KhachHang.
            object kh = m_DuNo.KhachHang;
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("kh_ho_ten", LayChuoi(kh, "HoTen", "TenKhachHang", "Ten")));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("kh_dia_chi", LayChuoi(kh, "DiaChi")));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("kh_dien_thoai", LayChuoi(kh, "DienThoai", "SoDienThoai")));

            this.reportViewer.LocalReport.SetParameters(param);
            this.DuNoKhachHangBindingSource.DataSource = m_DuNo;
            this.reportViewer.RefreshReport();
        }
    }
}