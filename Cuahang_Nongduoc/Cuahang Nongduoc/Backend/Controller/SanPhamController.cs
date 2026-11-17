using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;


namespace CuahangNongduoc.Controller
{
    /// <summary>Điều phối nghiệp vụ sản phẩm giữa giao diện, Data Binding và Data Access Layer.</summary>
    public class SanPhamController
    {
        SanPhamFactory factory = new SanPhamFactory();

        /// <summary>Tải danh sách sản phẩm vào ComboBox để các nghiệp vụ khác chọn theo khóa ID.</summary>
        /// <param name="cmb">ComboBox nhận nguồn dữ liệu sản phẩm.</param>
        public void HienthiAutoComboBox(System.Windows.Forms.ComboBox cmb)
        {
            DataTable tbl = factory.DanhsachSanPham(); 
            cmb.DataSource = tbl;
            cmb.DisplayMember = "TEN_SAN_PHAM";
            cmb.ValueMember = "ID";
            cmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
        }
        /// <summary>Thiết lập cột ComboBox sản phẩm trong lưới chi tiết phiếu nhập.</summary>
        /// <param name="cmb">Cột lưới cần liên kết dữ liệu.</param>
        public void HienthiDataGridViewComboBoxColumn(System.Windows.Forms.DataGridViewComboBoxColumn cmb)
        {
            cmb.DataSource = factory.DanhsachSanPham();
            cmb.DisplayMember = "TEN_SAN_PHAM";
            cmb.ValueMember = "ID";
            cmb.AutoComplete = true;
        }
        public void TimMaSanPham(String ma)
        {
            factory.TimMaSanPham(ma);
        }
        public void TimTenSanPham(String ten)
        {
            factory.TimTenSanPham(ten);
        }

        /// <summary>Nạp dữ liệu sản phẩm từ SQL Server và liên kết đồng nhất các control với BindingSource.</summary>
        /// <param name="dg">Lưới hiển thị danh sách sản phẩm.</param>
        /// <param name="bn">BindingNavigator điều hướng bản ghi.</param>
        public void HienthiDataGridview(System.Windows.Forms.DataGridView dg, System.Windows.Forms.BindingNavigator bn,
            TextBox txtMaSp, TextBox txtTenSp, ComboBox cmbDVT, NumericUpDown numSL, NumericUpDown numDonGiaNhap, NumericUpDown numGiaBanSi, NumericUpDown numGiaBanLe)
        {
            System.Windows.Forms.BindingSource bs = new System.Windows.Forms.BindingSource();
            bs.DataSource = factory.DanhsachSanPham();
            
            txtMaSp.DataBindings.Clear();
            txtMaSp.DataBindings.Add("Text", bs, "ID");

            txtTenSp.DataBindings.Clear();
            txtTenSp.DataBindings.Add("Text", bs, "TEN_SAN_PHAM");

            cmbDVT.DataBindings.Clear();
            cmbDVT.DataBindings.Add("SelectedValue", bs, "ID_DON_VI_TINH");

            numSL.DataBindings.Clear();
            numSL.DataBindings.Add("Value", bs, "SO_LUONG");

            numDonGiaNhap.DataBindings.Clear();
            numDonGiaNhap.DataBindings.Add("Value", bs, "DON_GIA_NHAP");

            numGiaBanSi.DataBindings.Clear();
            numGiaBanSi.DataBindings.Add("Value", bs, "GIA_BAN_SI");

            numGiaBanLe.DataBindings.Clear();
            numGiaBanLe.DataBindings.Add("Value", bs, "GIA_BAN_LE");
            bn.BindingSource = bs;
            dg.DataSource = bs;

            
        }
        /// <summary>Cập nhật giá nhập bình quân và số lượng tổng khi phát sinh lô nhập mới.</summary>
        /// <param name="id">Mã sản phẩm cần cập nhật.</param><param name="gia_moi">Đơn giá lô mới.</param><param name="so_luong">Số lượng lô mới.</param>
        public void CapNhatGiaNhap(String id, long gia_moi ,long so_luong)
        {
            DataTable tbl = factory.LaySanPham(id);
            if (tbl.Rows.Count > 0)
            {
                long tong_so = Convert.ToInt64(tbl.Rows[0]["SO_LUONG"]);
                long tong_gia = Convert.ToInt64(tbl.Rows[0]["DON_GIA_NHAP"]);
                if (tong_gia != gia_moi)
                {
                    long thanh_tien = gia_moi * so_luong + tong_gia * tong_so;
                    tong_so += so_luong;
                    tbl.Rows[0]["DON_GIA_NHAP"] = thanh_tien / tong_so;
                    tbl.Rows[0]["SO_LUONG"] = tong_so;
                }
                factory.Save();
            }

        }
    
        /// <summary>Đọc một sản phẩm và đơn vị tính tương ứng từ Data Access Layer.</summary>
        /// <param name="id">Mã sản phẩm.</param><returns>Đối tượng sản phẩm; đối tượng rỗng nếu không tìm thấy.</returns>
        public SanPham LaySanPham(String id)
        {
            DataTable tbl = factory.LaySanPham(id);
            SanPham sp = new SanPham();
            DonViTinhController ctrlDVT = new DonViTinhController();
            if (tbl.Rows.Count > 0)
            {
                sp.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                sp.TenSanPham =  Convert.ToString(tbl.Rows[0]["TEN_SAN_PHAM"]);
                sp.SoLuong = Convert.ToInt32(tbl.Rows[0]["SO_LUONG"]);
                sp.DonGiaNhap = Convert.ToInt64(tbl.Rows[0]["DON_GIA_NHAP"]);
                sp.GiaBanLe = Convert.ToInt64(tbl.Rows[0]["GIA_BAN_LE"]);
                sp.GiaBanSi = Convert.ToInt64(tbl.Rows[0]["GIA_BAN_SI"]);
                sp.DonViTinh = ctrlDVT.LayDVT(Convert.ToInt32(tbl.Rows[0]["ID_DON_VI_TINH"]));
            }
            return sp;

        }

        public static IList<SoLuongTon> LaySoLuongTon()
        {
            SanPhamFactory f = new SanPhamFactory();
            DataTable tbl = f.LaySoLuongTon();

            IList<SoLuongTon> ds = new List<SoLuongTon>();
            

            DonViTinhController ctrlDVT = new DonViTinhController();
            foreach(DataRow row in tbl.Rows)
            {
                SoLuongTon slt = new SoLuongTon();
                SanPham sp = new SanPham();
                sp.Id = Convert.ToString(row["ID"]);
                sp.TenSanPham = Convert.ToString(row["TEN_SAN_PHAM"]);
                sp.SoLuong = Convert.ToInt32(row["SO_LUONG"]);
                sp.DonGiaNhap = Convert.ToInt64(row["DON_GIA_NHAP"]);
                sp.GiaBanLe = Convert.ToInt64(row["GIA_BAN_LE"]);
                sp.GiaBanSi = Convert.ToInt64(row["GIA_BAN_SI"]);
                sp.DonViTinh = ctrlDVT.LayDVT(Convert.ToInt32(row["ID_DON_VI_TINH"]));
                slt.SanPham = sp;
                slt.SoLuong = Convert.ToInt32(row["SO_LUONG_TON"]);
                ds.Add(slt);
            }
            return ds;

        }

        public DataRow NewRow()
        {
            return factory.NewRow();
        }
        public void Add(DataRow row)
        {
            factory.Add(row);
        }
        /// <summary>Ghi các thay đổi Add/Edit/Delete đang nằm trong DataTable xuống SQL Server.</summary>
        /// <returns>True khi Data Access Layer ghi nhận thành công.</returns>
        public bool Save()
        {
            return factory.Save();
        }

        /// <summary>
        /// Hoàn nguyên toàn bộ thay đổi chưa commit để UI trở về trạng thái nhất quán sau lỗi ghi dữ liệu.
        /// </summary>
        public void RejectChanges()
        {
            factory.RejectChanges();
        }
    }
}
