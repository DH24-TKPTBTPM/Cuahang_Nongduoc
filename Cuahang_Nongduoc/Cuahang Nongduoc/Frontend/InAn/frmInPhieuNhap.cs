using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    //public partial class frmInPhieuNhap : Form
    public partial class frmInPhieuNhap : BaseReportForm // FE3-[D10] Kế thừa BaseReportForm
    {
        CuahangNongduoc.BusinessObject.PhieuNhap m_PhieuNhap = null;
        public frmInPhieuNhap(CuahangNongduoc.BusinessObject.PhieuNhap ph)
        {
            m_PhieuNhap = ph;
            InitializeComponent();
            CauHinhReportViewer(this.reportViewer); // FE3-[D10] Cấu hình ReportViewer dùng chung từ BaseReportForm

            //reportViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
            this.reportViewer.LocalReport.SubreportProcessing += new Microsoft.Reporting.WinForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
        }

        void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WinForms.SubreportProcessingEventArgs e)
        {
            if (m_PhieuNhap == null) return; // FE3-[D10] Kiểm tra null tránh NullReferenceException
            e.DataSources.Clear();
            e.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("CuahangNongduoc_BusinessObject_MaSanPham", m_PhieuNhap.ChiTiet));
        }

        private void frmInPhieuNhap_Load(object sender, EventArgs e)
        {
            // ==================== [D10] DESIGN PATTERN (GoF) ====================
            // Mẫu thiết kế     : Builder + Template Method (GoF)
            // Lỗi (Code smell) : Duplicate Code & Thiếu kiểm tra null trước khi truy cập m_PhieuNhap.TongTien
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 15/11/2026
            // ====================================================================
            if (m_PhieuNhap == null) // FE3-[D10] Kiểm tra null tránh NullReferenceException
            {
                MessageBox.Show("Không có dữ liệu phiếu nhập để in!", "In phiếu nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            HienThiBaoCao(this.reportViewer, this.PhieuNhapBindingSource, m_PhieuNhap, m_PhieuNhap.TongTien); // FE3-[D10] Gọi Template Method
        }
    }
}