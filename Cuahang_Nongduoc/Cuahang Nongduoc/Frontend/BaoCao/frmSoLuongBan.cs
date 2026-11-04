using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;

namespace CuahangNongduoc
{
    public partial class frmSoLuongBan : Form
    {
        public frmSoLuongBan()
        {
            InitializeComponent();

            // ==================== [FE-11] REFACTORING ====================
            // L?i (Code smell) : L?i hi?n th? font ti?ng Vi?t trên form báo cáo
            // Kythuat         : Clean UI Property (Set Font Programmatically)
            // Tác giar          : FE3 - Nguy?n Th? Ngân Hu?
            // H? tr?          : Không
            // Ki?m tra        : QA2 - D? Th? Trúc Hoà
            // Ngày ch?nh s?a  : 03/11/2026
            // =============================================================
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);

            reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
        }

        private void frmSoLuongBan_Load(object sender, EventArgs e)
        {
            cmbThang.SelectedIndex = DateTime.Now.Month - 1;
            numNam.Value = DateTime.Now.Year;
        }

        ChiTietPhieuBanController ctrl = new ChiTietPhieuBanController();

        
        private void btnXemNgay_Click(object sender, EventArgs e)
        {
            IList<Microsoft.Reporting.WinForms.ReportParameter> param = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("ngay", "Ngày " + dtNgay.Value.Date.ToString("dd/MM/yyyy")));

            this.reportViewer.LocalReport.SetParameters(param);

            this.ChiTietPhieuBanBindingSource.DataSource = ctrl.ChiTietPhieuBan(dtNgay.Value.Date);
            this.reportViewer.RefreshReport();
        }

        private void btnXemThang_Click(object sender, EventArgs e)
        {
            IList<Microsoft.Reporting.WinForms.ReportParameter> param = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("ngay",
                "Tháng " + Convert.ToString(cmbThang.SelectedIndex + 1) + "/" + numNam.Value.ToString()));

            this.reportViewer.LocalReport.SetParameters(param);


            this.ChiTietPhieuBanBindingSource.DataSource = ctrl.ChiTietPhieuBan(cmbThang.SelectedIndex + 1, Convert.ToInt32(numNam.Value));
            this.reportViewer.RefreshReport();
        }

      
    }
}