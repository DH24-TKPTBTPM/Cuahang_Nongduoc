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
    public partial class frmDanhsachPhieuBanSi : Form
    {
        public frmDanhsachPhieuBanSi()
        {
            InitializeComponent();
        }

        PhieuBanController ctrl = new PhieuBanController();
        DaiLyController ctrlDL = new DaiLyController();

        private void frmDanhsachPhieuBanSi_Load(object sender, EventArgs e)
        {
            dataGridView.DataError += dataGridView_DataError;
            ctrlDL.HienthiDaiLyDataGridviewComboBox(colKhachHang);
            ctrl.HienthiPhieuBanSi(bindingNavigator, dataGridView);
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void MoLaiDanhSach()
        {
            ctrl = new PhieuBanController();
            ctrl.HienthiPhieuBanSi(bindingNavigator, dataGridView);
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
            if (maPhieu == null || !XacNhanDongFormBanSi()) return;
            if (MessageBox.Show("Bạn có chắc chắn xóa phiếu bán sỉ này?", "Phiếu bán sỉ",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                new BanLeRepository().XoaPhieu(maPhieu, hoanTon);
                MoLaiDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Phiếu bán sỉ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool XacNhanDongFormBanSi()
        {
            return BanSi == null || BanSi.IsDisposed || BanSi.XacNhanDongFormBanSi();
        }

        frmBanSi BanSi = null;
        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            string maPhieu = MaPhieuDangChon();
            if (BanSi == null || BanSi.IsDisposed)
            {
                BanSi = new frmBanSi(maPhieu);
                BanSi.Show();
            }
            else
            {
                BanSi.Activate();
                if (!string.IsNullOrEmpty(maPhieu))
                {
                    BanSi.ChuyenDenPhieu(maPhieu);
                }
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (!XacNhanDongFormBanSi()) return;
            if (BanSi == null || BanSi.IsDisposed)
            {
                BanSi = new frmBanSi();
                BanSi.Show();
            }
            else
                BanSi.Activate();
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            XoaPhieuDangChon(true);
        }

        private void toolIn_Click(object sender, EventArgs e)
        {
            string maPhieu = MaPhieuDangChon();
            if (!string.IsNullOrEmpty(maPhieu))
            {
                PhieuBanController ctrlPB = new PhieuBanController();
                CuahangNongduoc.BusinessObject.PhieuBan ph = ctrlPB.LayPhieuBan(maPhieu);
                if (ph != null)
                {
                    frmInPhieuBan PhieuBan = new frmInPhieuBan(ph);
                    PhieuBan.Show();
                }
            }
        }

        private void toolTimKiem_Click(object sender, EventArgs e)
        {
            frmTimPhieuBanLe Tim = new frmTimPhieuBanLe(true);
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
    }
}