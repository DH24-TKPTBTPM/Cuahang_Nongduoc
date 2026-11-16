using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmInPhieuThanhToan : Form
    {
        CuahangNongduoc.BusinessObject.PhieuThanhToan m_PhieuThanhToan;

        public frmInPhieuThanhToan(CuahangNongduoc.BusinessObject.PhieuThanhToan ph)
        {
            InitializeComponent();
            m_PhieuThanhToan = ph;
        }

        private void frmPhieuThanhToan_Load(object sender, EventArgs e)
        {
            try
            {
                CuahangNongduoc.BusinessObject.CuaHang ch = ThamSo.LayCuaHang();
                if (ch == null)
                {
                    MessageBox.Show("Chưa có thông tin cửa hàng, vui lòng nhập trước khi in.");
                    return;
                }

                Num2Str num = new Num2Str();
                string bangChu = m_PhieuThanhToan.TongTien > 0
                    ? num.NumberToString(m_PhieuThanhToan.TongTien.ToString())
                    : "Không đồng";

                IList<Microsoft.Reporting.WinForms.ReportParameter> param =
                    new List<Microsoft.Reporting.WinForms.ReportParameter>();
                param.Add(new Microsoft.Reporting.WinForms.ReportParameter("ten_cua_hang", ch.TenCuaHang));
                param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dia_chi", ch.DiaChi));
                param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dien_thoai", ch.DienThoai));
                param.Add(new Microsoft.Reporting.WinForms.ReportParameter("bang_chu", bangChu));

                this.reportViewer.LocalReport.SetParameters(param);

                this.PhieuThanhToanBindingSource.DataSource =
                    new List<CuahangNongduoc.BusinessObject.PhieuThanhToan> { m_PhieuThanhToan };

                this.reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi in phiếu thanh toán");
            }
        }
    }
}
