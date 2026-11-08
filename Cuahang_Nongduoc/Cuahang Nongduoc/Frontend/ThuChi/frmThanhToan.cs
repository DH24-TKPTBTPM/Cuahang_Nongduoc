using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.Controller;

namespace CuahangNongduoc
{
    public partial class frmThanhToan : Form
    {
        KhachHangController ctrlKH = new KhachHangController();
        PhieuThanhToanController ctrl = new PhieuThanhToanController();
        public frmThanhToan()
        {
            InitializeComponent();
        }

        private void frmThanhToan_Load(object sender, EventArgs e)
        {
            ctrlKH.HienthiChungAutoComboBox(cmbKhachHang);
            ctrlKH.HienthiKhachHangChungDataGridviewComboBox(colKhachHang);
            HienThiPhieuThanhToan(ctrl.HienthiPhieuThanhToan());
            bindingNavigator.BindingSource.AddingNew += new AddingNewEventHandler(BindingSource_AddingNew);
        }

        // ==================== [R26] REFACTORING ====================
        // Lỗi (Code smell) : Phương thức nhận quá nhiều tham số (Long Parameter List)
        // Kỹ thuật         : Introduce Parameter Object
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        private void HienThiPhieuThanhToan(DataTable data)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = data;
            bindingNavigator.BindingSource = bs;
            dataGridView.DataSource = bs;

            txtMaPhieu.DataBindings.Clear();
            txtMaPhieu.DataBindings.Add("Text", bs, "ID");

            cmbKhachHang.DataBindings.Clear();
            cmbKhachHang.DataBindings.Add("SelectedValue", bs, "ID_KHACH_HANG");

            dtNgayThanhToan.DataBindings.Clear();
            dtNgayThanhToan.DataBindings.Add("Value", bs, "NGAY_THANH_TOAN");

            numTongTien.DataBindings.Clear();
            numTongTien.DataBindings.Add("Value", bs, "TONG_TIEN");

            txtGhiChu.DataBindings.Clear();
            txtGhiChu.DataBindings.Add("Text", bs, "GHI_CHU");
        }

        void BindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            
        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            long maphieu = ThamSo.LayMaPhieuThanhToan();;
            ThamSo.GanMaPhieuThanhToan(maphieu+1);

            DataRow row = ctrl.NewRow();
            row["ID"] = maphieu;
            row["NGAY_THANH_TOAN"] = DateTime.Now.Date;
            row["TONG_TIEN"] = numTongTien.Value;
            ctrl.Add(row);
            bindingNavigator.BindingSource.MoveLast();
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa phiếu thanh toán này không?", "Phieu Thanh Toan",   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void toolDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa phiếu thanh toán này không?", "Phieu Thanh Toan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();
                ctrl.Save();
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            txtMaPhieu.Focus();
            bindingNavigator.BindingSource.MoveNext();
            ctrl.Save();
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void toolIn_Click(object sender, EventArgs e)
        {
            DataRowView row = (DataRowView)bindingNavigator.BindingSource.Current;
            if (row != null)
            {
                PhieuThanhToanController ctrlTT = new PhieuThanhToanController();
                String ma_phieu = row["ID"].ToString();
                CuahangNongduoc.BusinessObject.PhieuThanhToan ph = ctrlTT.LayPhieuThanhToan(ma_phieu);
                frmInPhieuThanhToan PhieuThanhToan = new frmInPhieuThanhToan(ph);
                PhieuThanhToan.Show();
            }
        }

        private void toolTimKiem_Click(object sender, EventArgs e)
        {
            frmTimPhieuThu Tim = new frmTimPhieuThu();
            Point p = PointToScreen(toolTimKiem.Bounds.Location);
            p.X += toolTimKiem.Width;
            p.Y += toolTimKiem.Height;
            Tim.Location = p;
            Tim.ShowDialog();
            if (Tim.DialogResult == DialogResult.OK)
            {
                HienThiPhieuThanhToan(ctrl.TimPhieuThanhToan(new PhieuThanhToanSearchParam
                {
                    IdKhachHang = Tim.cmbKhachHang.SelectedValue.ToString(),
                    Ngay = Tim.dtNgayThu.Value.Date
                }));
            }
        }

    }
}