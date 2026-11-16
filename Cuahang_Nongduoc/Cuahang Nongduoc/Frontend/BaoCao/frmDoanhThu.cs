using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using Microsoft.Reporting.WinForms;

namespace CuahangNongduoc
{
    public partial class frmDoanhThu : Form
    {
        // Ten tai nguyen nhung (Embedded Resource) cua mau in: <RootNamespace>.Report.<ten file rdlc>
        private const string REPORT_RESOURCE = "CuahangNongduoc.Report.rptThongKeChiPhiTonKho.rdlc";

        public frmDoanhThu()
        {
            InitializeComponent();
        }

        private void frmDoanhThu_Load(object sender, EventArgs e)
        {
            // Mac dinh thong ke tu ngay dau thang den hom nay
            DateTime homNay = DateTime.Today;
            this.dtpTuNgay.Value = new DateTime(homNay.Year, homNay.Month, 1);
            this.dtpDenNgay.Value = homNay;

            this.reportViewer.RefreshReport();
        }

        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Thống kê",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            this.Cursor = Cursors.WaitCursor;
            try
            {
                PhieuBanController ctrl = new PhieuBanController();
                ThongKeChiPhiTonKho data = ctrl.LayThongKeChiPhiTonKho(tuNgay, denNgay);
                if (data == null)
                {
                    MessageBox.Show("Không lấy được dữ liệu thống kê.", "Thống kê",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Thong tin cua hang (dong tu ThamSo.LayCuaHang())
                CuaHang ch = ThamSo.LayCuaHang() ?? new CuaHang();

                // Nap tham so
                IList<ReportParameter> param = new List<ReportParameter>();
                param.Add(new ReportParameter("ten_cua_hang", ch.TenCuaHang ?? string.Empty));
                param.Add(new ReportParameter("dia_chi", ch.DiaChi ?? string.Empty));
                param.Add(new ReportParameter("dien_thoai", ch.DienThoai ?? string.Empty));
                param.Add(new ReportParameter("tu_ngay", dtpTuNgay.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
                param.Add(new ReportParameter("den_ngay", dtpDenNgay.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));

                this.reportViewer.LocalReport.ReportEmbeddedResource = REPORT_RESOURCE;
                this.reportViewer.LocalReport.SetParameters(param);

                // Nap nguon du lieu (ten phai khop voi ten DataSet trong rptThongKeChiPhiTonKho.rdlc)
                this.reportViewer.LocalReport.DataSources.Clear();
                this.reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ChiPhiDataSet",
                    new List<ThongKeChiPhiTonKho> { data }));
                this.reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DichVuDataSet",
                    data.ChiTietDichVu ?? new List<DichVuThongKeItem>()));
                this.reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TonKhoDataSet",
                    data.DanhSachTonKho ?? new List<SoLuongTon>()));
                this.reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lập báo cáo thống kê: " + ex.Message, "Thống kê",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
