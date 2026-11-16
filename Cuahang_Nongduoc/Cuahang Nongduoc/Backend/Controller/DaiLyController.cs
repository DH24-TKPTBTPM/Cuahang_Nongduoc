using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc.Controller
{
    public class DaiLyController
    {
        DaiLyFactory factory = new DaiLyFactory();

        /// <summary>
        /// Đổ danh sách đại lý vào ComboBox (AutoComplete).
        /// </summary>
        public void HienthiAutoComboBox(ComboBox cmb)
        {
            DataTable tbl = factory.DanhsachDaiLy();
            cmb.DataSource = tbl;
            cmb.DisplayMember = "DAI_LY";
            cmb.ValueMember = "ID";
        }

        public void HienthiAutoComboBoxBanSi(ComboBox cmb)
        {
            cmb.DataSource = factory.DanhsachDaiLyBanSi();
            cmb.DisplayMember = "DAI_LY";
            cmb.ValueMember = "ID";
        }

        /// <summary>
        /// Hiển thị dữ liệu đại lý lên DataGridView + BindingNavigator.
        /// </summary>
        public void HienthiDaiLyDataGridview(DataGridView dg, BindingNavigator bn)
        {
            BindingSource bs = new BindingSource();
            DataTable tbl = factory.DanhsachDaiLy();
            bs.DataSource = tbl;
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        /// <summary>
        /// Đổ danh sách đại lý vào DataGridViewComboBoxColumn.
        /// </summary>
        public void HienthiDaiLyDataGridviewComboBox(DataGridViewComboBoxColumn cmb)
        {
            cmb.DataSource = factory.DanhsachDaiLy();
            cmb.DisplayMember = "DAI_LY";
            cmb.ValueMember = "ID";
            cmb.DataPropertyName = "ID_KHACH_HANG";
            cmb.HeaderText = "Đại lý";
        }

        public void HienthiDaiLyBanSiDataGridviewComboBox(DataGridViewComboBoxColumn cmb)
        {
            cmb.DataSource = factory.DanhsachDaiLyBanSi();
            cmb.DisplayMember = "DAI_LY";
            cmb.ValueMember = "ID";
            cmb.DataPropertyName = "ID_KHACH_HANG";
            cmb.HeaderText = "Đại lý";
        }

        /// <summary>
        /// Tìm đại lý theo tên – gán kết quả trực tiếp vào BindingSource.
        /// </summary>
        public void TimHoTen(String hoten, BindingSource bindingSource)
        {
            bindingSource.DataSource = factory.TimHoTenMoi(hoten);
        }

        /// <summary>
        /// Tìm đại lý theo địa chỉ – gán kết quả trực tiếp vào BindingSource.
        /// </summary>
        public void TimDiaChi(String diachi, BindingSource bindingSource)
        {
            bindingSource.DataSource = factory.TimDiaChiMoi(diachi);
        }

        /// <summary>
        /// Tải lại toàn bộ danh sách đại lý vào BindingSource (dùng khi Hủy thao tác).
        /// </summary>
        public void TaiLaiDaiLy(BindingSource bindingSource)
        {
            bindingSource.DataSource = factory.DanhsachDaiLy();
        }

        /// <summary>
        /// Kiểm tra có ràng buộc ngăn cản xóa hay không.
        /// Trả về true nếu an toàn để xóa.
        /// </summary>
        public bool KiemTraRangBuocXoa(String idDaiLy)
        {
            return factory.KiemTraRangBuocXoa(idDaiLy);
        }

        /// <summary>
        /// Lấy thông tin 1 đại lý theo ID.
        /// </summary>
        public DaiLy LayDaiLy(String id)
        {
            DataTable tbl = factory.LayDaiLy(id);
            DaiLy dl = new DaiLy();
            if (tbl.Rows.Count > 0)
            {
                dl.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                dl.TenDaiLy = Convert.ToString(tbl.Rows[0]["DAI_LY"]);
                dl.DiaChi = Convert.ToString(tbl.Rows[0]["DIA_CHI"]);
                dl.DienThoai = Convert.ToString(tbl.Rows[0]["DIEN_THOAI"]);
            }
            return dl;
        }

        /// <summary>
        /// Lấy danh sách tất cả đại lý.
        /// </summary>
        public IList<DaiLy> LayDanhSachDaiLy()
        {
            DataTable tbl = factory.DanhsachDaiLy();
            IList<DaiLy> ds = new List<DaiLy>();

            foreach (DataRow row in tbl.Rows)
            {
                DaiLy dl = new DaiLy();
                dl.Id = Convert.ToString(row["ID"]);
                dl.TenDaiLy = Convert.ToString(row["DAI_LY"]);
                dl.DiaChi = Convert.ToString(row["DIA_CHI"]);
                dl.DienThoai = Convert.ToString(row["DIEN_THOAI"]);
                ds.Add(dl);
            }
            return ds;
        }

        /// <summary>
        /// Hủy thao tác: RejectChanges.
        /// </summary>
        public void HuyThaoTac()
        {
            factory.RejectChanges();
        }

        public DataRow NewRow()
        {
            return factory.NewRow();
        }

        public void Add(DataRow row)
        {
            factory.Add(row);
        }

        /// <summary>
        /// Lưu dữ liệu xuống CSDL bảng DAI_LY.
        /// </summary>
        public bool Save()
        {
            return factory.Save();
        }
    }
}
