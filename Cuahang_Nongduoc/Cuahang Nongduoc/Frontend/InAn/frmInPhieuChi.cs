using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmInPhieuChi : Form
    {
        CuahangNongduoc.BusinessObject.PhieuChi m_PhieuChi;
        public frmInPhieuChi(CuahangNongduoc.BusinessObject.PhieuChi ph)
        {
            InitializeComponent();
            m_PhieuChi = ph;
        }

        private void frmInPhieuChi_Load(object sender, EventArgs e)
        {
            try
            {
                // Safeguard object and nested properties
                if (m_PhieuChi == null)
                {
                    m_PhieuChi = new CuahangNongduoc.BusinessObject.PhieuChi("0");
                    m_PhieuChi.TongTien = 0;
                    m_PhieuChi.NgayChi = DateTime.Now;
                    m_PhieuChi.GhiChu = string.Empty;
                }
                if (m_PhieuChi.LyDoChi == null)
                {
                    m_PhieuChi.LyDoChi = new CuahangNongduoc.BusinessObject.LyDoChi { Id = 0, LyDo = string.Empty };
                }
                if (m_PhieuChi.NgayChi == default(DateTime))
                {
                    m_PhieuChi.NgayChi = DateTime.Now;
                }

                // Safe compute "bang_chu"
                string bangChu;
                try
                {
                    Num2Str num = new Num2Str();
                    bangChu = num.NumberToString(m_PhieuChi.TongTien.ToString());
                    if (bangChu == null) bangChu = string.Empty;
                }
                catch
                {
                    bangChu = string.Empty;
                }

                // Build parameter list (use non-null values)
                var ch = ThamSo.LayCuaHang();
                var paramList = new List<Microsoft.Reporting.WinForms.ReportParameter>
                {
                    new Microsoft.Reporting.WinForms.ReportParameter("ten_cua_hang", ch?.TenCuaHang ?? string.Empty),
                    new Microsoft.Reporting.WinForms.ReportParameter("dia_chi", ch?.DiaChi ?? string.Empty),
                    new Microsoft.Reporting.WinForms.ReportParameter("dien_thoai", ch?.DienThoai ?? string.Empty),
                    new Microsoft.Reporting.WinForms.ReportParameter("bang_chu", bangChu)
                };

                // Create DataTable with primitive fields matching rptPhieuChi.rdlc dataset (LyDo string)
                DataTable dt = new DataTable();
                dt.Columns.Add("GhiChu", typeof(string));
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("LyDo", typeof(string));
                dt.Columns.Add("NgayChi", typeof(DateTime));
                dt.Columns.Add("TongTien", typeof(long));

                var row = dt.NewRow();
                row["GhiChu"] = m_PhieuChi.GhiChu ?? string.Empty;
                row["Id"] = (m_PhieuChi.Id ?? string.Empty).ToString();
                row["LyDo"] = m_PhieuChi.LyDoChi?.LyDo ?? string.Empty;
                row["NgayChi"] = m_PhieuChi.NgayChi;
                row["TongTien"] = m_PhieuChi.TongTien;
                dt.Rows.Add(row);

                // Prepare ReportViewer (local)
                this.reportViewer.LocalReport.DataSources.Clear();

                // DataSource name must match the one defined in rptPhieuChi.rdlc
                var rds = new Microsoft.Reporting.WinForms.ReportDataSource("CuahangNongduoc_BusinessObject_PhieuChi", dt);
                this.reportViewer.LocalReport.DataSources.Add(rds);

                // Keep BindingSource consistent for designer
                this.PhieuChiBindingSource.DataSource = dt;

                // Set parameters and refresh in guarded block to capture inner exceptions
                try
                {
                    this.reportViewer.LocalReport.SetParameters(paramList);
                    this.reportViewer.RefreshReport();
                }
                catch (Exception rptEx)
                {
                    // Show detailed error to help debugging (inner exception included)
                    MessageBox.Show("Lỗi khi khởi tạo báo cáo:\n" + rptEx.ToString(), "Lỗi In phiếu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.Print(rptEx.ToString());
                }
            }
            catch (Exception ex)
            {
                // Fail-safe: show full exception so bạn biết nguyên nhân thay vì crash
                MessageBox.Show("Lỗi khi load báo cáo: " + ex.ToString(), "Lỗi In phiếu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.Print(ex.ToString());
            }
        }
    }
}