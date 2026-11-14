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
            // Lỗi (Code smell) : Duplicate Code – Lặp lại 11 dòng khởi tạo ReportParameter và gán DataSource
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 12/11/2026
            // ====================================================================
            HienThiBaoCao(this.reportViewer, this.DuNoKhachHangBindingSource, m_DuNo, m_DuNo.CuoiKy); // FE3-[D10] Gọi Template Method
            //Num2Str num = new Num2Str();
            //IList<Microsoft.Reporting.WinForms.ReportParameter> param = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            //CuahangNongduoc.BusinessObject.CuaHang ch = ThamSo.LayCuaHang();
            //param.Add(new Microsoft.Reporting.WinForms.ReportParameter("ten_cua_hang", ch.TenCuaHang));
            //param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dia_chi", ch.DiaChi));
            //param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dien_thoai", ch.DienThoai));

            //param.Add(new Microsoft.Reporting.WinForms.ReportParameter("bang_chu", num.NumberToString(m_DuNo.CuoiKy.ToString())));

            //this.reportViewer.LocalReport.SetParameters(param);
            //this.DuNoKhachHangBindingSource.DataSource = m_DuNo;
            //this.reportViewer.RefreshReport();
        }
    }
}