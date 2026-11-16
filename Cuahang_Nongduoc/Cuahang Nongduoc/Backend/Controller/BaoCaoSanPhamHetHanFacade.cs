using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using Microsoft.Reporting.WinForms;

namespace CuahangNongduoc.Controller
{
    /// <summary>
    /// Facade cho luong bao cao het han: lay du lieu, gan BindingSource,
    /// thiet lap tham so RDLC va hien thi ReportViewer.
    /// Form chi can goi HienThi(ngayTinh).
    /// </summary>
    public sealed class BaoCaoSanPhamHetHanFacade
    {
        private readonly BindingSource bindingSource;
        private readonly ReportViewer reportViewer;

        public BaoCaoSanPhamHetHanFacade(BindingSource bindingSource, ReportViewer reportViewer)
        {
            if (bindingSource == null) throw new ArgumentNullException("bindingSource");
            if (reportViewer == null) throw new ArgumentNullException("reportViewer");

            this.bindingSource = bindingSource;
            this.reportViewer = reportViewer;
        }

        public void HienThi(DateTime ngayTinh)
        {
            IList<MaSanPham> data = MaSanPhamController.LayMaSanPhamHetHan(ngayTinh.Date);
            IList<ReportParameter> param = new List<ReportParameter>();
            param.Add(new ReportParameter(
                "ngay_tinh", "Tính đến ngày " + ngayTinh.Date.ToString("dd/MM/yyyy")));

            bindingSource.DataSource = data;
            reportViewer.LocalReport.SetParameters(param);
            reportViewer.RefreshReport();
        }
    }
}
