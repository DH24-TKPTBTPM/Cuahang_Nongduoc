using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.Frontend.BanHang;
using CuahangNongduoc.Patterns.Facade;
using CuahangNongduoc.Patterns.Factory;
using CuahangNongduoc.Patterns.Strategy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmDanhsachPhieuBan : Form
    {
        private LoaiPhieuBan _loaiPhieu;
        public frmDanhsachPhieuBan(LoaiPhieuBan loaiPhieu)
        {
            InitializeComponent();
            _loaiPhieu = loaiPhieu;
            if (_loaiPhieu == LoaiPhieuBan.BanLe)
            {
                this.Text = "Danh Sách Phiếu Bán Lẻ";
            }
            else
            {
                this.Text = "Danh Sách Phiếu Bán Sỉ";
            }
        }

        PhieuBanController ctrl = new PhieuBanController();
        KhachHangController ctrlKH = new KhachHangController();
        private readonly BanHangFacade _facade = new BanHangFacade();
        private void frmDanhsachPhieuNhap_Load(object sender, EventArgs e)
        {
            // [Sửa build 15/11/2026] Controller (R23) trả DataTable, Form tự gắn vào cột ComboBox;
            // Bán sỉ dùng danh sách Đại lý như bản gốc frmDanhsachPhieuBanSi (trước đây gộp nhầm thành Khách hàng).
            // Sửa: BE1 - Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA1 - <Họ tên>
            bool laBanLe = (_loaiPhieu == LoaiPhieuBan.BanLe);
            colKhachhang.DataSource = laBanLe ? ctrlKH.HienthiKhachHangDataGridviewComboBox() : ctrlKH.HienthiDaiLyDataGridviewComboBox();
            colKhachhang.DisplayMember = "HO_TEN";
            colKhachhang.ValueMember = "ID";
            colKhachhang.DataPropertyName = "ID_KHACH_HANG";
            colKhachhang.HeaderText = laBanLe ? "Khách hàng" : "Đại lý";

            if (laBanLe)
                ctrl.HienthiPhieuBanLe(bindingNavigator, dataGridView);
            else
                ctrl.HienthiPhieuBanSi(bindingNavigator, dataGridView);
        }
        frmBanHang BanLe = null;
        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (BanLe == null || BanLe.IsDisposed)
            {
                if (_loaiPhieu == LoaiPhieuBan.BanLe)
                {
                    BanLe = new frmBanHang(ctrl, new BanLeStrategy());
                }
                else
                {
                    BanLe = new frmBanHang(ctrl, new BanSiStrategy());
                }
                BanLe.Show();
            }
            else
            {
                BanLe.Activate();
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (BanLe == null || BanLe.IsDisposed)
            {
                if (_loaiPhieu == LoaiPhieuBan.BanLe)
                {
                    BanLe = new frmBanHang(new BanLeStrategy());
                }
                else
                {
                    BanLe = new frmBanHang(new BanSiStrategy());
                }
                BanLe.Show();
            }
            else
            {
                BanLe.Activate();
            }
        }


        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                DataRowView view = (DataRowView)bindingNavigator.BindingSource.Current;
                if (view != null)
                {
                    bool ketQua = _facade.XoaPhieuBan(view["ID"].ToString());
                    if (!ketQua)
                    {
                        MessageBox.Show(_facade.LastErrorMessage); 
                        e.Cancel = true; 
                    }
                    
                }
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            DataRowView view = (DataRowView)bindingNavigator.BindingSource.Current;
            if (view != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool ketQua = _facade.XoaPhieuBan(view["ID"].ToString());
                    if (ketQua)
                    {
                        bindingNavigator.BindingSource.RemoveCurrent();
                    }
                    else
                    {
                        MessageBox.Show(_facade.LastErrorMessage); 
                    }
                }
            }
        }

        private void toolPrint_Click(object sender, EventArgs e)
        {
            DataRowView row = (DataRowView)bindingNavigator.BindingSource.Current;
            if (row != null)
            {
                PhieuBanController ctrlPB = new PhieuBanController();
                String ma_phieu = row["ID"].ToString();
                CuahangNongduoc.BusinessObject.PhieuBan ph = ctrlPB.LayPhieuBan(ma_phieu);
                frmInPhieuBan frmIn = new frmInPhieuBan(ph);
                frmIn.Show();
            }
        }

        private void toolTimKiem_Click(object sender, EventArgs e)
        {
            frmTimPhieuBanLe Tim = new frmTimPhieuBanLe(_loaiPhieu == LoaiPhieuBan.BanSi);
            Point p = PointToScreen(toolTimKiem.Bounds.Location);
            p.X += toolTimKiem.Width;
            p.Y += toolTimKiem.Height;
            Tim.Location = p;
            Tim.ShowDialog();
            if (Tim.DialogResult == DialogResult.OK)
            {
                ctrl.TimPhieuBan(Tim.cmbNCC.SelectedValue.ToString(), Tim.dtNgayNhap.Value.Date);
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}