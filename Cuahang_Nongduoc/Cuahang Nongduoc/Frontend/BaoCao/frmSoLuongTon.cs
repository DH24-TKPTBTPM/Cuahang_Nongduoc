using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmSoLuongTon : Form
    {
        public frmSoLuongTon()
        {
            InitializeComponent();
            // ==================== [FE-BC] REFACTORING ====================
            // Lỗi (Code smell) : Thiếu cấu hình AppDomain cho ReportViewer và đồng bộ Font hiển thị
            // Kỹ thuật         : Clean UI & ReportViewer Configuration
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 12/11/2026
            // =============================================================
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
        }

        private void frmSoLuongTon_Load(object sender, EventArgs e)
        {
            IList<CuahangNongduoc.BusinessObject.SoLuongTon> data = CuahangNongduoc.Controller.SanPhamController.LaySoLuongTon();
            this.SoLuongTonBindingSource.DataSource = data;
            this.reportViewer.RefreshReport();
        }
    }
}