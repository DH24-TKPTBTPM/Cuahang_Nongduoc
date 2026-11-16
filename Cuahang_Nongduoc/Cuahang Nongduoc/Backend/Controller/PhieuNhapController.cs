using System;
using System.Collections.Generic;
using System.Text;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.BusinessObject;
using System.Windows.Forms;
using System.Data;

namespace CuahangNongduoc.Controller
{
    public class PhieuNhapController
    {
        PhieuNhapFactory factory = new PhieuNhapFactory();
        BindingSource bs = new BindingSource();

        public PhieuNhapController()
        {
            bs.DataSource = factory.LayPhieuNhap("-1");
        }

        public DataRow NewRow()
        {
            return factory.NewRow();
        }

        public void Add(DataRow row)
        {
            factory.Add(row);
        }

        public bool Update()
        {
            // Khong MoveNext truoc khi save: thao tac do co the chuyen sang phieu khac.
            return factory.Save();
        }

        public DataService GetDataService()
        {
            return factory.GetDataService();
        }

        public bool Save()
        {
            return factory.Save();
        }

        public int XoaPhieuNhapVaCacLo(String id)
        {
            return factory.XoaPhieuNhapVaCacLo(id);
        }

        public int XoaPhieuNhapVaCacLo(String id, IList<string> idsSanPham)
        {
            return factory.XoaPhieuNhapVaCacLo(id, idsSanPham);
        }

        public PhieuNhap LayPhieuNhap(String id)
        {
            DataTable tbl = factory.LayPhieuNhap(id);
            PhieuNhap ph = null;
            NhaCungCapController ctrlNCC = new NhaCungCapController();

            if (tbl.Rows.Count > 0)
            {
                ph = new PhieuNhap();
                ph.Id = Convert.ToString(tbl.Rows[0]["ID"]);
                ph.NgayNhap = Convert.ToDateTime(tbl.Rows[0]["NGAY_NHAP"]);
                ph.TongTien = Convert.ToInt64(tbl.Rows[0]["TONG_TIEN"]);

                // Đã sửa lại đúng tên cột cho DaTra và ConNo
                ph.DaTra = Convert.ToInt64(tbl.Rows[0]["DA_TRA"]);
                ph.ConNo = Convert.ToInt64(tbl.Rows[0]["CON_NO"]);

                ph.NhaCungCap = ctrlNCC.LayNCC(Convert.ToString(tbl.Rows[0]["ID_NHA_CUNG_CAP"]));
                MaSanPhamController ctrl = new MaSanPhamController();
                ph.ChiTiet = ctrl.ChiTietPhieuNhap(ph.Id);
            }
            return ph;
        }

        public void HienthiPhieuNhap(BindingNavigator bn, DataGridView dg)
        {
            bs.DataSource = factory.DanhsachPhieuNhap();
            bn.BindingSource = bs;
            dg.DataSource = bs;
        }

        public void HienthiPhieuNhap(BindingNavigator bn, TextBox txt, ComboBox cmb, DateTimePicker dt, NumericUpDown numTongTien, NumericUpDown numDaTra, NumericUpDown numConNo)
        {
            // Constructor chỉ nạp bảng rỗng (ID='-1'). Phải nạp danh sách phiếu trước khi bind,
            // nếu không form nhập hàng thấy danh sách rỗng và luôn mở ra một phiếu mới.
            bs.DataSource = factory.DanhsachPhieuNhap();
            bn.BindingSource = bs;

            txt.DataBindings.Clear();
            txt.DataBindings.Add("Text", bs, "ID");

            cmb.DataBindings.Clear();
            cmb.DataBindings.Add("SelectedValue", bs, "ID_NHA_CUNG_CAP");

            dt.DataBindings.Clear();
            dt.DataBindings.Add("Value", bs, "NGAY_NHAP");

            numTongTien.DataBindings.Clear();
            numTongTien.DataBindings.Add("Value", bs, "TONG_TIEN");

            numDaTra.DataBindings.Clear();
            numDaTra.DataBindings.Add("Value", bs, "DA_TRA");

            numConNo.DataBindings.Clear();
            numConNo.DataBindings.Add("Value", bs, "CON_NO");
        }


        public string LayMaPhieuHienTai()
        {
            DataRowView row = bs.Current as DataRowView;
            if (row == null || row.Row == null || row.Row.RowState == DataRowState.Deleted)
                return null;
            if (!row.Row.Table.Columns.Contains("ID") || row["ID"] == DBNull.Value)
                return null;
            return Convert.ToString(row["ID"]);
        }

        public bool ChonPhieuNhap(String id)
        {
            if (bs == null || bs.Count == 0 || String.IsNullOrEmpty(id))
                return false;
            // Duyệt trực tiếp thay vì bs.Find: cột ID là số nguyên nên so khớp với chuỗi qua Find không đáng tin cậy.
            for (int i = 0; i < bs.Count; i++)
            {
                DataRowView drv = bs[i] as DataRowView;
                if (drv != null && String.Equals(Convert.ToString(drv["ID"]), id, StringComparison.OrdinalIgnoreCase))
                {
                    bs.Position = i;
                    return true;
                }
            }
            return false;
        }

        public void TimPhieuNhap(String maNCC, DateTime dt)
        {
            factory.TimPhieuNhap(maNCC, dt);
        }
     
        public void KiemTraHopLe(long soLuong, long donGia)
        {
            // Bắn ngoại lệ (Throw Exception) từ lớp BUS theo đúng báo cáo
            if (soLuong <= 0)
            {
                throw new ArgumentException("Số lượng nhập phải lớn hơn 0!");
            }
            if (donGia <= 0)
            {
                throw new ArgumentException("Đơn giá nhập phải lớn hơn 0!");
            }
        }
    }
}