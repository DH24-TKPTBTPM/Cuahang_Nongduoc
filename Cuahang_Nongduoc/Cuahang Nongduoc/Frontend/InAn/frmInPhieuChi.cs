using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{

    //public partial class frmInPhieuChi : Form
    public partial class frmInPhieuChi : BaseReportForm // FE3-[D10] Kế thừa BaseReportForm
    {
        CuahangNongduoc.BusinessObject.PhieuChi m_PhieuChi;
        public frmInPhieuChi(CuahangNongduoc.BusinessObject.PhieuChi ph)
        {
            InitializeComponent();
            CauHinhReportViewer(this.reportViewer); // FE3-[D10] Cấu hình ReportViewer dùng chung từ BaseReportForm
            m_PhieuChi = ph;
        }

        private void frmInPhieuChi_Load(object sender, EventArgs e)
        {
            // ==================== [D10] DESIGN PATTERN (GoF) ====================
            // Mẫu thiết kế     : Builder + Template Method (GoF)
            // Lỗi (Code smell) : Duplicate Code & Thiếu kiểm tra null trước khi truy cập m_PhieuChi.TongTien
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 15/11/2026
            // ====================================================================
            if (m_PhieuChi == null) // FE3-[D10] Kiểm tra null tránh NullReferenceException
            {
                MessageBox.Show("Không có dữ liệu phiếu chi để in!", "In phiếu chi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            HienThiBaoCao(this.reportViewer, this.PhieuChiBindingSource, m_PhieuChi, m_PhieuChi.TongTien); // FE3-[D10] Gọi Template Method
        }
    }
}