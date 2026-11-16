using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer;

namespace CuahangNongduoc
{
    public partial class frmDanhsachPhieuBanLe : Form
    {
        public frmDanhsachPhieuBanLe()
        {
            InitializeComponent();
        }

        PhieuBanController ctrl = new PhieuBanController();
        KhachHangController ctrlKH = new KhachHangController();
        private void frmDanhsachPhieuNhap_Load(object sender, EventArgs e)
        {
            dataGridView.DataError += dataGridView_DataError;
            ctrlKH.HienthiKhachHangDataGridviewComboBox(colKhachhang);
            ctrl.HienthiPhieuBanLe(bindingNavigator, dataGridView);
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void MoLaiDanhSach()
        {
            ctrl = new PhieuBanController();
            ctrl.HienthiPhieuBanLe(bindingNavigator, dataGridView);
        }

        private void ToolXemLai_Click(object sender, EventArgs e)
        {
            MoLaiDanhSach();
        }

        private string MaPhieuDangChon()
        {
            if (dataGridView.CurrentRow != null && dataGridView.CurrentRow.DataBoundItem != null)
            {
                DataRowView drv = dataGridView.CurrentRow.DataBoundItem as DataRowView;
                if (drv != null && drv.Row.Table.Columns.Contains("ID") && drv["ID"] != DBNull.Value)
                    return Convert.ToString(drv["ID"]);
            }
            if (bindingNavigator.BindingSource == null || bindingNavigator.BindingSource.Current == null)
                return null;
            DataRowView row = bindingNavigator.BindingSource.Current as DataRowView;
            return row == null ? null : Convert.ToString(row["ID"]);
        }

        private void XoaPhieuDangChon(bool hoanTon)
        {
            string maPhieu = MaPhieuDangChon();
            if (maPhieu == null) return;
            if (MessageBox.Show("Bạn có chắc chắn xóa phiếu bán này?", "Phiếu bán lẻ",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                new BanLeRepository().XoaPhieu(maPhieu, hoanTon);
                MoLaiDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Phiếu bán lẻ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        frmBanLe BanLe = null;
        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            string maPhieu = MaPhieuDangChon();
            if (BanLe == null || BanLe.IsDisposed)
            {
                BanLe = new frmBanLe(maPhieu);
                BanLe.Show();
            }
            else
            {
                BanLe.Activate();
                if (!string.IsNullOrEmpty(maPhieu))
                {
                    BanLe.ChuyenDenPhieu(maPhieu);
                }
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (BanLe == null || BanLe.IsDisposed)
            {
                BanLe = new frmBanLe();
                BanLe.Show();
            }
            else
                BanLe.Activate();
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            XoaPhieuDangChon(true);
        }

        private void toolPrint_Click(object sender, EventArgs e)
        {
            if (bindingNavigator.BindingSource == null || bindingNavigator.BindingSource.Current == null) return;
            DataRowView row = bindingNavigator.BindingSource.Current as DataRowView;
            if (row != null)
            {
                PhieuBanController ctrlPB = new PhieuBanController();
                String ma_phieu = row["ID"].ToString();
                CuahangNongduoc.BusinessObject.PhieuBan ph = ctrlPB.LayPhieuBan(ma_phieu);
                frmInPhieuBan PhieuBan = new frmInPhieuBan(ph);
                PhieuBan.Show();
            }
        }

        private void toolTimKiem_Click(object sender, EventArgs e)
        {
            frmTimPhieuBanLe Tim = new frmTimPhieuBanLe(false);
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