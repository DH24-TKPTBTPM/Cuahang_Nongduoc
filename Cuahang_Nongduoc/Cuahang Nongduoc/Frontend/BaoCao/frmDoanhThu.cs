using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject; // FE3-[FE-BC]
using CuahangNongduoc.Controller; // FE3-[FE-BC]
using Microsoft.Reporting.WinForms; // FE3-[FE-BC]

namespace CuahangNongduoc
{
    public partial class frmDoanhThu : Form
    {
        // FE3-[FE-BC] Khởi tạo Controller và BindingSource cho báo cáo doanh thu
        ChiTietPhieuBanController ctrl = new ChiTietPhieuBanController();
        BindingSource m_BindingSource = new BindingSource();

        public frmDoanhThu()
        {
            InitializeComponent();
            // FE3-[FE-BC] Đồng bộ Font hiển thị và cấu hình ReportViewer, DataSource, sự kiện nút Xem
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);

            m_BindingSource.DataSource = typeof(ChiTietPhieuBan);
            ReportDataSource reportDataSource = new ReportDataSource("CuahangNongduoc_BusinessObject_ChiTietPhieuBan", m_BindingSource);
            this.reportViewer.LocalReport.DataSources.Clear();
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer.LocalReport.ReportEmbeddedResource = "CuahangNongduoc.Report.rptSoLuongBan.rdlc";

            this.button1.Click += new EventHandler(btnXemNgay_Click);
            this.button2.Click += new EventHandler(btnXemThang_Click);
            this.button3.Click += new EventHandler(btnXemNam_Click);
        }

        private void frmDoanhThu_Load(object sender, EventArgs e)
        {
            // ==================== [FE-BC] REFACTORING ====================
            // Lỗi (Code smell) : Năm mặc định bị gắn cứng 2007, chưa gán RDLC, DataSource và tham số báo cáo
            // Kỹ thuật         : Replace Magic Number with Dynamic DateTime & Bind ReportDataSource
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 15/11/2026
            // =============================================================
            comboBox1.SelectedIndex = DateTime.Now.Month - 1;
            numericUpDown1.Value = DateTime.Now.Year;
            dateTimePicker1.Value = DateTime.Now;

            //this.reportViewer.RefreshReport();
            btnXemThang_Click(sender, e); // FE3-[FE-BC] Nạp tham số và dữ liệu tháng hiện tại khi mở form
        }

        // FE3-[FE-BC] Xem doanh thu theo ngày
        private void btnXemNgay_Click(object sender, EventArgs e)
        {
            IList<ReportParameter> param = new List<ReportParameter>();
            param.Add(new ReportParameter("ngay", "Ngày " + dateTimePicker1.Value.Date.ToString("dd/MM/yyyy")));
            this.reportViewer.LocalReport.SetParameters(param);

            m_BindingSource.DataSource = ctrl.ChiTietPhieuBan(dateTimePicker1.Value.Date);
            this.reportViewer.RefreshReport();
        }

        // FE3-[FE-BC] Xem doanh thu theo tháng/năm
        private void btnXemThang_Click(object sender, EventArgs e)
        {
            int thang = comboBox1.SelectedIndex + 1;
            int nam = Convert.ToInt32(numericUpDown1.Value);

            IList<ReportParameter> param = new List<ReportParameter>();
            param.Add(new ReportParameter("ngay", "Tháng " + thang.ToString() + "/" + nam.ToString()));
            this.reportViewer.LocalReport.SetParameters(param);

            m_BindingSource.DataSource = ctrl.ChiTietPhieuBan(thang, nam);
            this.reportViewer.RefreshReport();
        }

        // FE3-[FE-BC] Xem doanh thu theo năm
        private void btnXemNam_Click(object sender, EventArgs e)
        {
            int nam = Convert.ToInt32(numericUpDown1.Value);
            IList<ChiTietPhieuBan> dsNam = new List<ChiTietPhieuBan>();
            for (int thang = 1; thang <= 12; thang++)
            {
                IList<ChiTietPhieuBan> dsThang = ctrl.ChiTietPhieuBan(thang, nam);
                if (dsThang != null)
                {
                    foreach (ChiTietPhieuBan item in dsThang)
                    {
                        dsNam.Add(item);
                    }
                }
            }

            IList<ReportParameter> param = new List<ReportParameter>();
            param.Add(new ReportParameter("ngay", "Năm " + nam.ToString()));
            this.reportViewer.LocalReport.SetParameters(param);

            m_BindingSource.DataSource = dsNam;
            this.reportViewer.RefreshReport();
        }
    }
}

    
