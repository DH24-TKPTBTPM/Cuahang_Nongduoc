using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmDoanhThu : Form
    {
        public frmDoanhThu()
        {
            InitializeComponent();
            // FE3-[FE-BC] Đồng bộ Font hiển thị và cấu hình AppDomain cho ReportViewer
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
        }

        private void frmDoanhThu_Load(object sender, EventArgs e)
        {
            // ==================== [FE-BC] REFACTORING ====================
            // Lỗi (Code smell) : Năm mặc định bị gán cứng 2007 (dữ liệu Access cũ), chưa chọn tháng hiện tại
            // Kỹ thuật         : Replace Magic Number with Dynamic DateTime
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 12/11/2026
            // =============================================================
            comboBox1.SelectedIndex = DateTime.Now.Month - 1;
            numericUpDown1.Value = DateTime.Now.Year;
            dateTimePicker1.Value = DateTime.Now;
            
            this.reportViewer.RefreshReport();
        }
    }
}