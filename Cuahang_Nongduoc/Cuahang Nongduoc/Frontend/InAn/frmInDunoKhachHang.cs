using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    
    public partial class frmInDunoKhachHang : BaseReportForm // FE3-[D10] Kế thừa BaseReportForm
    //public partial class frmInDunoKhachHang : Form
    {
        CuahangNongduoc.BusinessObject.DuNoKhachHang m_DuNo;
        public frmInDunoKhachHang(CuahangNongduoc.BusinessObject.DuNoKhachHang  dn)
        {
            InitializeComponent();
            CauHinhReportViewer(this.reportViewer); // FE3-[D10] Cấu hình ReportViewer dùng chung từ BaseReportForm
            m_DuNo = dn;
        }

        private void frmInDunoKhachHang_Load(object sender, EventArgs e)
        {
            // ==================== [D10] DESIGN PATTERN (GoF) ====================
            // Mẫu thiết kế     : Builder + Template Method (GoF)
            // Lỗi (Code smell) : Duplicate Code & Thiếu kiểm tra null trước khi truy cập m_DuNo.CuoiKy
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 15/11/2026
            // ====================================================================
            if (m_DuNo == null) // FE3-[D10] Kiểm tra null tránh NullReferenceException
            {
                MessageBox.Show("Không có dữ liệu dư nợ khách hàng để in!", "In dư nợ khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            HienThiBaoCao(this.reportViewer, this.DuNoKhachHangBindingSource, m_DuNo, m_DuNo.CuoiKy); // FE3-[D10] Gọi Template Method
        }
    }
}