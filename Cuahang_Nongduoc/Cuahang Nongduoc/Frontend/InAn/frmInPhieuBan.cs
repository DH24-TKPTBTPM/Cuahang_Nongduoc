using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmInPhieuBan : BaseReportForm // FE3-[D10] Kế thừa BaseReportForm
    //public partial class frmInPhieuBan : Form
    {
        CuahangNongduoc.BusinessObject.PhieuBan m_PhieuBan;
        public frmInPhieuBan(CuahangNongduoc.BusinessObject.PhieuBan ph)
        {
            InitializeComponent();
            
            //reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
            CauHinhReportViewer(this.reportViewer); // FE3-[D10] Cấu hình ReportViewer dùng chung từ BaseReportForm
            this.reportViewer.LocalReport.SubreportProcessing += new Microsoft.Reporting.WinForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            m_PhieuBan = ph;
        }

        void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WinForms.SubreportProcessingEventArgs e)
        {
            if (m_PhieuBan == null) return; // FE3-[D10] Kiểm tra null tránh NullReferenceException
            e.DataSources.Clear();
            e.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("CuahangNongduoc_BusinessObject_ChiTietPhieuBan", m_PhieuBan.ChiTiet));
        }

        private void frmInPhieuBan_Load(object sender, EventArgs e)
        {
            // ==================== [D10] DESIGN PATTERN (GoF) ====================
            // Mẫu thiết kế     : Builder + Template Method (GoF)
            // Lỗi (Code smell) : Duplicate Code & Thiếu kiểm tra null trước khi truy cập m_PhieuBan.TongTien
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 15/11/2026
            // ====================================================================
            if (m_PhieuBan == null) // FE3-[D10] Kiểm tra null tránh NullReferenceException
            {
                MessageBox.Show("Không có dữ liệu phiếu bán để in!", "In phiếu bán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            HienThiBaoCao(this.reportViewer, this.PhieuBanBindingSource, m_PhieuBan, m_PhieuBan.TongTien); // FE3-[D10] Gọi Template Method
        }
    }
    
}