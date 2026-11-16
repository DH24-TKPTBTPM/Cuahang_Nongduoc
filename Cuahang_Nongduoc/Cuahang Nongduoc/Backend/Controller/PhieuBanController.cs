using System;
using System.Collections.Generic;
using System.Text;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.BusinessObject;
using System.Windows.Forms;
using System.Data;

namespace CuahangNongduoc.Controller
{
    
    public class PhieuBanController
    {
        PhieuBanFactory factory = new PhieuBanFactory();

        BindingSource bs = new BindingSource();

        private readonly PhieuBanFactory _factory = new PhieuBanFactory();

        public PhieuBanController()
        {
            bs.DataSource = factory.LayPhieuBan("-1");
        }
        public DataRow NewRow()
        {
            return factory.NewRow();
        }
        public void Add(DataRow row)
        {
            factory.Add(row);
        }
        public void Update()
        {
            bs.MoveNext();
            factory.Save();
        }
        public void Save()
        {
            int n = PhieuBanFactory.LaySoPhieu();
            if (n >= 50)
            {
                MessageBox.Show("Đây là bản dùng thử! Chỉ lưu được 50 phiếu bán!", "Phieu Ban", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Đây là bản dùng thử! Chỉ lưu được thêm " + Convert.ToString(50-n) + " phiếu bán!", "Phieu Ban", MessageBoxButtons.OK, MessageBoxIcon.Information);
                factory.Save();
            }
            
        }
        public void HienthiPhieuBanLe(BindingNavigator bn, DataGridView dg)
        {

            bs.DataSource = factory.DanhsachPhieuBanLe();
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        public void HienthiPhieuBanSi(BindingNavigator bn, DataGridView dg)
        {

            bs.DataSource = factory.DanhsachPhieuBanSi();
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        public void HienthiPhieuBan(BindingNavigator bn,ComboBox cmb, TextBox txt, DateTimePicker dt, NumericUpDown numTongTien, NumericUpDown numDatra, NumericUpDown numConNo)
        {
            HienthiPhieuBan(bn, cmb, txt, dt, numTongTien, numDatra, numConNo, false);
        }

        public void HienthiPhieuBanLe(BindingNavigator bn, ComboBox cmb, TextBox txt, DateTimePicker dt,
            NumericUpDown numTongTien, NumericUpDown numDatra, NumericUpDown numConNo)
        {
            HienthiPhieuBan(bn, cmb, txt, dt, numTongTien, numDatra, numConNo, true);
        }

        private void HienthiPhieuBan(BindingNavigator bn, ComboBox cmb, TextBox txt, DateTimePicker dt,
            NumericUpDown numTongTien, NumericUpDown numDatra, NumericUpDown numConNo, bool banLe)
        {
            bs.DataSource = banLe ? factory.DanhsachPhieuBanLe() : factory.DanhsachPhieuBanSi();
            bn.BindingSource = bs;

            txt.DataBindings.Clear();
            txt.DataBindings.Add("Text", bs, "ID");

            cmb.DataBindings.Clear();
            cmb.DataBindings.Add("SelectedValue", bs, "ID_KHACH_HANG");

            dt.DataBindings.Clear();
            dt.DataBindings.Add("Value", bs, "NGAY_BAN");

            numTongTien.DataBindings.Clear();
            numTongTien.DataBindings.Add("Value", bs, "TONG_TIEN");

            numDatra.DataBindings.Clear();
            numDatra.DataBindings.Add("Value", bs, "DA_TRA");

            numConNo.DataBindings.Clear();
            numConNo.DataBindings.Add("Value", bs, "CON_NO");


        }

        public PhieuBan LayPhieuBan(String id)
        {
            DataTable tbl = factory.LayPhieuBan(id);
            PhieuBan ph = null;
            if (tbl.Rows.Count > 0)
            {

                ph = new PhieuBan();
                ph.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                
                ph.NgayBan = Convert.ToDateTime(tbl.Rows[0]["NGAY_BAN"]);
                ph.TongTien = LayLong(tbl.Rows[0], "TONG_TIEN", 0);
                ph.DaTra = LayLong(tbl.Rows[0], "DA_TRA", 0);
                ph.ConNo = LayLong(tbl.Rows[0], "CON_NO", ph.TongTien - ph.DaTra);
                BanLeRepository repository = new BanLeRepository();
                ThongTinChiPhiPhieuBan chiPhi = repository.LayChiPhiPhieu(ph.Id);
                ph.TienHang = chiPhi.TienHang;
                ph.TongDichVu = chiPhi.TongDichVu;
                ph.GiaoHang = chiPhi.GiaoHang;
                ph.KhoangCachKm = chiPhi.KhoangCachKm;
                ph.PhiVanChuyen = chiPhi.PhiVanChuyen;
                ph.LoaiGiamGia = chiPhi.LoaiGiamGia;
                ph.TyLeGiamGia = chiPhi.TyLeGiamGia;
                ph.TienGiamGia = chiPhi.TienGiamGia;
                ph.DichVu = chiPhi.DichVu;
                KhachHangController ctrlKH = new KhachHangController();
                ph.KhachHang = ctrlKH.LayKhachHang(Convert.ToString(tbl.Rows[0]["ID_KHACH_HANG"]));
                if (ph.KhachHang == null)
                {
                    ph.KhachHang = new KhachHang();
                    ph.KhachHang.HoTen = "Khách bán lẻ";
                    ph.KhachHang.DiaChi = "";
                    ph.KhachHang.DienThoai = "";
                }
                ChiTietPhieuBanController ctrl = new ChiTietPhieuBanController();
                ph.ChiTiet = ctrl.ChiTietPhieuBan(ph.Id);
            }
            return ph;
        }

        private long LayLong(DataRow row, string column, long defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? Convert.ToInt64(row[column]) : defaultValue;
        }

        private decimal LayDecimal(DataRow row, string column, decimal defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? Convert.ToDecimal(row[column]) : defaultValue;
        }

        private bool LayBool(DataRow row, string column, bool defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? Convert.ToBoolean(row[column]) : defaultValue;
        }

        private string LayString(DataRow row, string column, string defaultValue)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? Convert.ToString(row[column]) : defaultValue;
        }

        public void TimPhieuBan(String maKH, DateTime dt)
        {
            factory.TimPhieuBan(maKH, dt);

        }
        public ThongKeChiPhiTonKho LayThongKeChiPhiTonKho(DateTime tuNgay, DateTime denNgay)
        {
            DateTime tu = tuNgay.Date;
            DateTime den = denNgay.Date.AddDays(1).AddSeconds(-1);

            // Lấy dữ liệu tổng hợp chi phí bán hàng
            DataTable dtTongHop = _factory.ThongKeTongHopChiPhiBanHang(tu, den);

            // Lấy dữ liệu chi tiết dịch vụ phụ
            DataTable dtDichVu = _factory.ThongKeChiTietDichVuPhu(tu, den);

            // Lấy danh sách tồn kho hiện tại
            IList<SoLuongTon> danhSachTonKho = SanPhamController.LaySoLuongTon();

            // Khởi tạo đối tượng kết quả
            var thongKe = new ThongKeChiPhiTonKho
            {
                TuNgay = tuNgay,
                DenNgay = denNgay,
                ChiTietDichVu = new List<DichVuThongKeItem>(),
                DanhSachTonKho = danhSachTonKho,
            };

            // Nạp dữ liệu tổng hợp (nếu có)
            if (dtTongHop.Rows.Count > 0)
            {
                DataRow row = dtTongHop.Rows[0];
                thongKe.SoHoaDon = row["SO_HOA_DON"] != DBNull.Value ? Convert.ToInt32(row["SO_HOA_DON"]) : 0;
                thongKe.TongTienHang = row["TONG_TIEN_HANG"] != DBNull.Value ? Convert.ToInt64(row["TONG_TIEN_HANG"]) : 0;
                thongKe.TongPhiVanChuyen = row["TONG_PHI_VAN_CHUYEN"] != DBNull.Value ? Convert.ToInt64(row["TONG_PHI_VAN_CHUYEN"]) : 0;
                thongKe.TongPhiDichVu = row["TONG_DICH_VU"] != DBNull.Value ? Convert.ToInt64(row["TONG_DICH_VU"]) : 0;
                thongKe.TongTienGiamGia = row["TONG_TIEN_GIAM"] != DBNull.Value ? Convert.ToInt64(row["TONG_TIEN_GIAM"]) : 0;
                thongKe.TongDoanhThu = row["TONG_DOANH_THU"] != DBNull.Value ? Convert.ToInt64(row["TONG_DOANH_THU"]) : 0;
            }

            // Nạp chi tiết dịch vụ phụ
            foreach (DataRow row in dtDichVu.Rows)
            {
                var item = new DichVuThongKeItem
                {
                    TenDichVu = row["TEN_DICH_VU"].ToString(),
                    SoLan = row["SO_LAN"] != DBNull.Value ? Convert.ToInt32(row["SO_LAN"]) : 0,
                    TongTien = row["TONG_TIEN"] != DBNull.Value ? Convert.ToInt64(row["TONG_TIEN"]) : 0
                };
                thongKe.ChiTietDichVu.Add(item);
            }

            return thongKe;
        }
    }
}
