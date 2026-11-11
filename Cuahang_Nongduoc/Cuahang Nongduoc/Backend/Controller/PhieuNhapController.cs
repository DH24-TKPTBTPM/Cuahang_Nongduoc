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

        public void Update()
        {
            bs.MoveNext();
            factory.Save();
        }
        public void Save()
        {
            factory.Save();
        }

    
        public PhieuNhap LayPhieuNhap(String id)
        {
            DataTable tbl = factory.LayPhieuNhap(id);
            PhieuNhap ph = null;
            NhaCungCapController ctrlNCC = new NhaCungCapController();
            if (tbl.Rows.Count > 0)
            {

                ph = new PhieuNhap();
                ph.Id =Convert.ToString( tbl.Rows[0]["ID"]);
                ph.NgayNhap = Convert.ToDateTime(tbl.Rows[0]["NGAY_NHAP"]);
                ph.TongTien = Convert.ToInt64(tbl.Rows[0]["TONG_TIEN"]);
                // ==================== [R12] REFACTORING ====================
                // Lỗi (Code smell) : Wrong Field Assignment – DaTra, ConNo đọc nhầm cột TONG_TIEN
                // Kỹ thuật         : Fix Bug
                // Trước            : ph.DaTra = Convert.ToInt64(tbl.Rows[0]["TONG_TIEN"]);
                // Sau              : ph.DaTra = Convert.ToInt64(tbl.Rows[0]["DA_TRA"]);
                // Tác giả          : BE3 – Bùi Lê Tuấn Anh
                // Hỗ trợ           : 
                // Kiểm tra         : 
                // Ngày chỉnh sửa   : 05/11/2026
                // ==============================================================
                ph.DaTra = Convert.ToInt64(tbl.Rows[0]["DA_TRA"]);
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

        public void HienthiPhieuNhap(BindingNavigator bn,TextBox txt,ComboBox cmb, DateTimePicker dt, NumericUpDown numTongTien, NumericUpDown numDaTra, NumericUpDown numConNo)
        {

            bn.BindingSource = bs;

            txt.DataBindings.Clear();
            txt.DataBindings.Add("Text", bs,"ID");

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

        public void TimPhieuNhap(String maNCC, DateTime dt)
        {
            factory.TimPhieuNhap(maNCC, dt);
        }
   
    }
}
