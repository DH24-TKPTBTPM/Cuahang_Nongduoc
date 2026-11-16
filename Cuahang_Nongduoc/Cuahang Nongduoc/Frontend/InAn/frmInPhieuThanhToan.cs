using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    //public partial class frmInPhieuThanhToan : Form
    public partial class frmInPhieuThanhToan : BaseReportForm // FE3-[D10] Kế thừa BaseReportForm
    {
        CuahangNongduoc.BusinessObject.PhieuThanhToan m_PhieuThanhToan;
        public frmInPhieuThanhToan(CuahangNongduoc.BusinessObject.PhieuThanhToan ph)
        {
 
            InitializeComponent();
            CauHinhReportViewer(this.reportViewer); // FE3-[D10] Cấu hình ReportViewer dùng chung từ BaseReportForm
            m_PhieuThanhToan = ph;
        }

        private void frmPhieuThanhToan_Load(object sender, EventArgs e)
        {
            // ==================== [D10] DESIGN PATTERN (GoF) ====================
            // Mẫu thiết kế     : Builder + Template Method (GoF)
            // Lỗi (Code smell) : Duplicate Code & Thiếu kiểm tra null trước khi truy cập m_PhieuThanhToan.TongTien
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 15/11/2026
            // ====================================================================
            if (m_PhieuThanhToan == null) // FE3-[D10] Kiểm tra null tránh NullReferenceException
            {
                MessageBox.Show("Không có dữ liệu phiếu thanh toán để in!", "In phiếu thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            HienThiBaoCao(this.reportViewer, this.PhieuThanhToanBindingSource, m_PhieuThanhToan, m_PhieuThanhToan.TongTien); // FE3-[D10] Gọi Template Method
        }
    }
}