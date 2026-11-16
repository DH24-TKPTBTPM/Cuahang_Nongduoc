using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmInPhieuBan : Form
    {
        CuahangNongduoc.BusinessObject.PhieuBan m_PhieuBan;
        public frmInPhieuBan(CuahangNongduoc.BusinessObject.PhieuBan ph)
        {
            InitializeComponent();
            m_PhieuBan = ph;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(700, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            ChuanHoaDuLieu();
            using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CuahangNongduoc.Report.rptChiTietPhieuBan.rdlc"))
            {
                if (stream == null)
                    throw new InvalidOperationException("Không tìm thấy mẫu chi tiết phiếu bán.");

                this.reportViewer.LocalReport.LoadSubreportDefinition("rptChiTietPhieuBan", stream);
            }
            this.reportViewer.LocalReport.SubreportProcessing += new Microsoft.Reporting.WinForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
        }

        private void ChuanHoaDuLieu()
        {
            if (m_PhieuBan == null) return;
            if (m_PhieuBan.KhachHang == null)
            {
                m_PhieuBan.KhachHang = new CuahangNongduoc.BusinessObject.KhachHang();
                m_PhieuBan.KhachHang.HoTen = "Khách bán lẻ";
                m_PhieuBan.KhachHang.DiaChi = "-";
                m_PhieuBan.KhachHang.DienThoai = "-";
            }
            else
            {
                if (string.IsNullOrEmpty(m_PhieuBan.KhachHang.HoTen))
                    m_PhieuBan.KhachHang.HoTen = "Khách bán lẻ";
                if (m_PhieuBan.KhachHang.DiaChi == null)
                    m_PhieuBan.KhachHang.DiaChi = "-";
                if (m_PhieuBan.KhachHang.DienThoai == null)
                    m_PhieuBan.KhachHang.DienThoai = "-";
            }
        }

        void LocalReport_SubreportProcessing(object sender, Microsoft.Reporting.WinForms.SubreportProcessingEventArgs e)
        {
            e.DataSources.Clear();
            IList<CuahangNongduoc.BusinessObject.ChiTietPhieuBan> dsChiTiet = null;
            if (m_PhieuBan != null && m_PhieuBan.ChiTiet != null)
            {
                dsChiTiet = m_PhieuBan.ChiTiet;
            }
            else
            {
                dsChiTiet = new List<CuahangNongduoc.BusinessObject.ChiTietPhieuBan>();
            }
            e.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("CuahangNongduoc_BusinessObject_ChiTietPhieuBan", dsChiTiet));
        }

        private string GiaTriThamSo(string giaTri)
        {
            return giaTri ?? string.Empty;
        }

        private void frmInPhieuBan_Load(object sender, EventArgs e)
        {
            ChuanHoaDuLieu();
            Num2Str num = new Num2Str();
            IList<Microsoft.Reporting.WinForms.ReportParameter> param = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            CuahangNongduoc.BusinessObject.CuaHang ch = ThamSo.LayCuaHang();
            if (ch == null)
                ch = new CuahangNongduoc.BusinessObject.CuaHang();

            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("ten_cua_hang", GiaTriThamSo(ch.TenCuaHang)));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dia_chi", GiaTriThamSo(ch.DiaChi)));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("dien_thoai", GiaTriThamSo(ch.DienThoai)));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("bang_chu", GiaTriThamSo(num.NumberToString(m_PhieuBan != null ? m_PhieuBan.TongTien.ToString() : "0"))));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("tien_hang", m_PhieuBan != null ? m_PhieuBan.TienHang.ToString("#,##0") : "0"));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("tong_dich_vu", m_PhieuBan != null ? m_PhieuBan.TongDichVu.ToString("#,##0") : "0"));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("phi_van_chuyen", m_PhieuBan != null ? m_PhieuBan.PhiVanChuyen.ToString("#,##0") : "0"));
            param.Add(new Microsoft.Reporting.WinForms.ReportParameter("tien_giam", m_PhieuBan != null ? m_PhieuBan.TienGiamGia.ToString("#,##0") : "0"));

            this.reportViewer.LocalReport.SetParameters(param);
            this.PhieuBanBindingSource.DataSource = m_PhieuBan;
            this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource(
                "DichVuDataSet", m_PhieuBan != null && m_PhieuBan.DichVu != null
                    ? m_PhieuBan.DichVu
                    : new List<CuahangNongduoc.BusinessObject.DichVuPhieuBan>()));
            this.reportViewer.RefreshReport();
        }
    }
}