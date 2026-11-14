using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc
{
    // ==================== [D10] DESIGN PATTERN (GoF) ====================
    // Mẫu thiết kế     : Builder + Template Method (GoF)
    // Vấn đề (Smell)   : Duplicate Code – 5 form in ấn (frmInDunoKhachHang, frmInPhieuBan,
    //                    frmInPhieuChi, frmInPhieuNhap, frmInPhieuThanhToan) lặp lại toàn bộ
    //                    quy trình lấy thông tin cửa hàng, đổi số tiền thành chữ, tạo danh sách
    //                    ReportParameter, gán DataSource và RefreshReport.
    // Giải pháp        :
    //   1. ReportBuilder (Builder Pattern): Xây dựng danh sách ReportParameter theo từng bước.
    //   2. BaseReportForm (Template Method Pattern): Định nghĩa khung phương thức HienThiBaoCao()
    //      chuẩn hóa các bước nạp báo cáo cho cả 5 form in.
    // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
    // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
    // Ngày chỉnh sửa   : 12/11/2026
    // ====================================================================

    /// <summary>
    /// Builder Pattern (GoF): Hỗ trợ khởi tạo danh sách ReportParameter từng bước cho ReportViewer.
    /// </summary>
    public class ReportBuilder
    {
        private readonly IList<ReportParameter> m_Params = new List<ReportParameter>();

        public ReportBuilder WithThongTinCuaHang()
        {
            CuaHang ch = ThamSo.LayCuaHang();
            m_Params.Add(new ReportParameter("ten_cua_hang", ch.TenCuaHang));
            m_Params.Add(new ReportParameter("dia_chi", ch.DiaChi));
            m_Params.Add(new ReportParameter("dien_thoai", ch.DienThoai));
            return this;
        }

        public ReportBuilder WithTienBangChu(long soTien)
        {
            Num2Str num = new Num2Str();
            m_Params.Add(new ReportParameter("bang_chu", num.NumberToString(soTien.ToString())));
            return this;
        }

        public ReportBuilder WithParameter(string tenThamSo, string giaTri)
        {
            m_Params.Add(new ReportParameter(tenThamSo, giaTri));
            return this;
        }

        public IList<ReportParameter> Build()
        {
            return m_Params;
        }
    }

    /// <summary>
    /// Template Method Pattern (GoF): Lớp cơ sở định nghĩa khung quy trình hiển thị báo cáo cho 5 form in.
    /// </summary>
    public class BaseReportForm : Form
    {
        protected void CauHinhReportViewer(ReportViewer viewer)
        {
            this.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            if (viewer != null)
            {
                viewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
            }
        }

        // Template Method: Khung chuẩn 4 bước để hiển thị phiếu in lên ReportViewer
        protected void HienThiBaoCao(ReportViewer viewer, BindingSource bindingSource, object dataSource, long soTien)
        {
            ReportBuilder builder = new ReportBuilder()
                .WithThongTinCuaHang()
                .WithTienBangChu(soTien);

            TuyChinhThamSo(builder);

            viewer.LocalReport.SetParameters(builder.Build());
            bindingSource.DataSource = dataSource;
            viewer.RefreshReport();
        }

        // Hook method cho phép form con bổ sung tham số riêng nếu cần
        protected virtual void TuyChinhThamSo(ReportBuilder builder)
        {
        }
    }
}