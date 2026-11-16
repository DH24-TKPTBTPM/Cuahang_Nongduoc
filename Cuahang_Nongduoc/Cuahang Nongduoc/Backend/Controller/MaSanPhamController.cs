using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc.Controller
{
    public class MaSanPhamController
    {
        MaSanPhanFactory factory = new MaSanPhanFactory();

        public DataRow NewRow()
        {
            return factory.NewRow();
        }
        public void Add(DataRow row)
        {
            factory.Add(row);
        }
        public bool Save()
        {
            return factory.Save();
        }

        public SanPham LaySanPham(String idMaSanPham)
        {
            MaSanPhanFactory f = new MaSanPhanFactory();
            DataTable tbl = f.LaySanPham(idMaSanPham);
            SanPham sp = null;
            DonViTinhController ctrlDVT = new DonViTinhController();
            if (tbl.Rows.Count > 0)
            {
                sp =  new  SanPham();
                sp.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                sp.TenSanPham = Convert.ToString(tbl.Rows[0]["TEN_SAN_PHAM"]);
                sp.SoLuong = Convert.ToInt32(tbl.Rows[0]["SO_LUONG"]);
                sp.DonGiaNhap = Convert.ToInt64(tbl.Rows[0]["DON_GIA_NHAP"]);
                sp.GiaBanLe = Convert.ToInt64(tbl.Rows[0]["GIA_BAN_LE"]);
                sp.GiaBanSi = Convert.ToInt64(tbl.Rows[0]["GIA_BAN_SI"]);
                sp.DonViTinh = ctrlDVT.LayDVT(Convert.ToInt32(tbl.Rows[0]["ID_DON_VI_TINH"]));
            }
            return sp;

        }

        //Nhập hàng 
         public DataTable LayCacLoMoi()
        {
            return factory.LayCacLoMoi();
        }
        public DataService GetDataService()
        {
            return factory.GetDataService();
        }
        public bool CoLoDaBan(String idMaLo)
        {
            return factory.CoLoDaBan(idMaLo);
        }
        public bool CoLoDaBanTrongPhieuNhap(String idPhieuNhap)
        {
            return factory.CoLoDaBanTrongPhieuNhap(idPhieuNhap);
        }
         public DataTable DanhSachSanPhamTheoPhieuNhap(String idPhieuNhap)
        {
            return factory.DanhSachSanPhamTheoPhieuNhap(idPhieuNhap);
        }
        public DataTable LayCacLoDaXoa()
        {
            return factory.LayCacLoDaXoa();
        }





        public MaSanPham LayMaSanPham(String idMaSanPham)
        {
            MaSanPhanFactory f = new MaSanPhanFactory();
            DataTable tbl = f.LayMaSanPham(idMaSanPham);
            MaSanPham sp = null;
            SanPhamController ctrlSanPham = new SanPhamController();
            if (tbl.Rows.Count > 0)
            {
                sp = new MaSanPham();
                sp.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                sp.SoLuong = Convert.ToInt32(tbl.Rows[0]["SO_LUONG"]);
                sp.GiaNhap = Convert.ToInt64(tbl.Rows[0]["DON_GIA_NHAP"]);
                sp.NgayNhap = Convert.ToDateTime(tbl.Rows[0]["NGAY_NHAP"]);
                sp.NgaySanXuat = Convert.ToDateTime(tbl.Rows[0]["NGAY_SAN_XUAT"]);
                sp.NgayHetHan = Convert.ToDateTime(tbl.Rows[0]["NGAY_HET_HAN"]);
                sp.SanPham = ctrlSanPham.LaySanPham(tbl.Rows[0]["ID_SAN_PHAM"].ToString());
                
            }
            return sp;

        }

        public static IList<MaSanPham> LayMaSanPhamHetHan(DateTime dt)
        {
            IList<MaSanPham> ds = new List<MaSanPham>();
            MaSanPhanFactory f = new MaSanPhanFactory();
            DataTable tbl = f.DanhsachMaSanPhamHetHan(dt);

            MaSanPham sp = null;
            string tenSanPhamTruoc = null;
            foreach  ( DataRow row in tbl.Rows)
            {
                sp = new MaSanPham();
                sp.Id = Convert.ToString(row["ID"]);
                sp.SoLuong = Convert.ToInt32(row["SO_LUONG"]);
                sp.GiaNhap = Convert.ToInt64(row["DON_GIA_NHAP"]);
                sp.NgayNhap = Convert.ToDateTime(row["NGAY_NHAP"]);
                sp.NgaySanXuat = Convert.ToDateTime(row["NGAY_SAN_XUAT"]);
                sp.NgayHetHan = Convert.ToDateTime(row["NGAY_HET_HAN"]);
                sp.SanPham = new SanPham();
                sp.SanPham.Id = Convert.ToString(row["ID_SAN_PHAM"]);
                sp.SanPham.TenSanPham = Convert.ToString(row["TEN_SAN_PHAM"]);
                sp.TenSanPhamHienThi = String.Equals(tenSanPhamTruoc, sp.TenSanPham,
                    StringComparison.Ordinal) ? String.Empty : sp.TenSanPham;
                tenSanPhamTruoc = sp.TenSanPham;
                ds.Add(sp);
            }

            for (int i = 0; i < ds.Count; i++)
            {
                ds[i].LaDongCuoiNhom = i == ds.Count - 1 ||
                    !String.Equals(ds[i].TenSanPham, ds[i + 1].TenSanPham, StringComparison.Ordinal);
            }
            return ds;

        }

        public void HienThiAutoComboBox2ThamSo(String sp, ComboBox cmb)
        {
            HienThiAutoComboBox(sp, cmb, false);
        }

        public void HienThiAutoComboBox(String sp, ComboBox cmb, bool baoGomHetHan)
        {
            cmb.DataSource = factory.DanhsachMaSanPham(sp, baoGomHetHan);
            cmb.DisplayMember = "ID";
            cmb.ValueMember = "ID";
        }

        public void HienThiDataGridViewComboBox(DataGridViewComboBoxColumn cmb)
        {
            cmb.DataSource = factory.DanhsachMaSanPham();
            cmb.DisplayMember = "ID";
            cmb.ValueMember = "ID";
            cmb.DataPropertyName = "ID_MA_SAN_PHAM";
            cmb.HeaderText = "Mã sản phẩm";
        }

        public void HienThiChiTietPhieuNhap(String id, DataGridView dg)
        {
            
            dg.DataSource = factory.DanhsachChiTiet(id);
        }
        public IList<MaSanPham> ChiTietPhieuNhap(String id)
        {
            SanPhamController ctrlSanPham = new SanPhamController();
            IList<MaSanPham> ds = new List<MaSanPham>();
            DataTable tbl = factory.DanhsachChiTiet(id);
            foreach (DataRow row in tbl.Rows)
            {
                MaSanPham sp = new MaSanPham();
                sp = new MaSanPham();
                sp.Id = Convert.ToString(row["ID"]);
                // SO_LUONG của lô là số CÒN LẠI sau khi bán. Phiếu nhập phải in số lượng đã nhập
                // (còn lại + đã bán), nếu không Thành tiền các dòng không cộng ra Tổng tiền phiếu.
                sp.SoLuong = checked(Convert.ToInt32(row["SO_LUONG"]) + Convert.ToInt32(factory.LaySoLuongDaBan(sp.Id)));
                sp.GiaNhap = Convert.ToInt64(row["DON_GIA_NHAP"]);
                sp.ThanhTien = (long)sp.SoLuong * sp.GiaNhap;
                sp.NgayNhap = Convert.ToDateTime(row["NGAY_NHAP"]);
                sp.NgaySanXuat = Convert.ToDateTime(row["NGAY_SAN_XUAT"]);
                sp.NgayHetHan = Convert.ToDateTime(row["NGAY_HET_HAN"]);
                sp.SanPham = ctrlSanPham.LaySanPham(row["ID_SAN_PHAM"].ToString());
                ds.Add(sp);
            }
            return ds;
        }

    }
}
