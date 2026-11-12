using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmSanphamHethan : Form
    {
        public frmSanphamHethan()
        {
            InitializeComponent();
            // FE3-[FE-BC] Đồng bộ Font hiển thị và cấu hình AppDomain cho ReportViewer
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);//fe3
            reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);//fe3
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            IList<CuahangNongduoc.BusinessObject.MaSanPham> data = CuahangNongduoc.Controller.MaSanPhamController.LayMaSanPhamHetHan(dt.Value.Date);
            IList<Microsoft.Reporting.WinForms.ReportParameter> param = new List<Microsoft.Reporting.WinForms.ReportParameter>();

            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("ngay_tinh", dt.Value.Date.ToString("dd/MM/yyyy")));
            this.MaSanPhamBindingSource.DataSource = data;
            this.reportViewer.LocalReport.SetParameters(param);
            this.reportViewer.RefreshReport();
        }

        private void frmSanphamHethan_Load(object sender, EventArgs e)
        {
            // ==================== [FE-BC] REFACTORING ====================
            // Lỗi (Code smell) : Sự kiện Load bỏ trống, chưa khởi tạo ngày mặc định cho bộ lọc SQL Server
            // Kỹ thuật         : Initialize Default State on Form Load
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 12/11/2026
            // =============================================================
            dt.Value = DateTime.Now;
        }
    }
}