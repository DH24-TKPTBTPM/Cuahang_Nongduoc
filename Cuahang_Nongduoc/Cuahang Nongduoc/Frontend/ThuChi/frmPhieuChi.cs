using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer;//FE3-[FE-TC] Dùng PhieuChiFactory cho Form tự binding

namespace CuahangNongduoc
{
    public partial class frmPhieuChi : Form
    {
        LyDoChiController ctrlLyDo = new LyDoChiController();
        PhieuChiController ctrl = new PhieuChiController();
        PhieuChiFactory factory = new PhieuChiFactory();//FE3-[FE-TC] Khởi tạo Factory
        public frmPhieuChi()
        {
            InitializeComponent();
        }

        // ==================== [FE-TC] REFACTORING ====================
        // Lỗi (Code smell) : Controller nhận trực tiếp các control UI (BindingNavigator, DataGridView, TextBox...) để binding
        // Kỹ thuật         : Move Method (Chuyển logic binding dữ liệu từ Controller về Form giao diện)
        // Trước            : ctrl.HienthiPhieuChi(bindingNavigator, dataGridView, cmbLyDoChi, txtMaPhieu, dtNgayChi, numTongTien, txtGhiChu);
        // Sau              : HienthiPhieuChi(factory.DanhsachPhieuChi());
        // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
        // Hỗ trợ           : BE2 (Task R26)
        // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa   : 08/11/2026
        // =============================================================

        private void HienthiPhieuChi(DataTable dt)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dt;
            bindingNavigator.BindingSource = bs;
            dataGridView.DataSource = bs;

            txtMaPhieu.DataBindings.Clear();
            txtMaPhieu.DataBindings.Add("Text", bs, "ID");

            cmbLyDoChi.DataBindings.Clear();
            cmbLyDoChi.DataBindings.Add("SelectedValue", bs, "ID_LY_DO_CHI");

            dtNgayChi.DataBindings.Clear();
            dtNgayChi.DataBindings.Add("Value", bs, "NGAY_CHI");

            numTongTien.DataBindings.Clear();
            numTongTien.DataBindings.Add("Value", bs, "TONG_TIEN");

            txtGhiChu.DataBindings.Clear();
            txtGhiChu.DataBindings.Add("Text", bs, "GHI_CHU");
        } 
        
        // ==================== [R26] REFACTORING ====================
        // Lỗi (Code smell) : Phương thức nhận quá nhiều tham số (Long Parameter List)
        // Kỹ thuật         : Introduce Parameter Object
        // Tác giả          : BE3 - Bùi Lê Tuấn Anh
        // Hỗ trợ           : FE3 - Nguyễn Thị Ngân Huệ
        // Kiểm duyệt       : QA2 - Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 08/11/2026
        // ==============================================================
        private void frmThanhToan_Load(object sender, EventArgs e)
        { 
            ctrlLyDo.HienthiAutoComboBox(cmbLyDoChi);
            ctrlLyDo.HienthiDataGridviewComboBox(colLyDoChi);
            HienthiPhieuChi(factory.DanhsachPhieuChi());
            //ctrl.HienthiPhieuChi(bindingNavigator, dataGridView, cmbLyDoChi, txtMaPhieu, dtNgayChi, numTongTien, txtGhiChu);
        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            long maphieu = ThamSo.PhieuChi;
            ThamSo.PhieuChi=maphieu+1;
            // FE3-[FE-TC] Thay cho ctrl.NewRow()
            DataRow row = factory.NewRow();//DataRow row = ctrl.NewRow();
            row["ID"] = maphieu;
            row["NGAY_CHI"] = dtNgayChi.Value.Date;
            row["TONG_TIEN"] = numTongTien.Value;
            // FE3-[FE-TC] Thay cho ctrl.Add(row)
            factory.Add(row);//ctrl.Add(row);
            bindingNavigator.BindingSource.MoveLast();
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa phiếu chi này không?", "Phieu Chi",   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void toolDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa phiếu chi này không?", "Phieu Chi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();
                // FE3-[FE-TC] Thay cho ctrl.Save()
                factory.Save();//ctrl.Save();
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            txtMaPhieu.Focus();
            bindingNavigator.BindingSource.MoveNext();
            // FE3-[FE-TC] Thay cho ctrl.Save()
            factory.Save();//ctrl.Save();
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
                PhieuChiController ctrlChi = new PhieuChiController();
                String ma_phieu = row["ID"].ToString();
                CuahangNongduoc.BusinessObject.PhieuChi ph = ctrlChi.LayPhieuChi(ma_phieu);
                frmInPhieuChi InPhieu = new frmInPhieuChi(ph);
                InPhieu.Show();
            }
        }

        private void btnThemLyDoChi_Click(object sender, EventArgs e)
        {
            frmLyDoChi Chi = new frmLyDoChi();
            Chi.ShowDialog();
            ctrlLyDo.HienthiAutoComboBox(cmbLyDoChi);
        }

        private void toolTimKiem_Click(object sender, EventArgs e)
        {
            frmTimPhieuChi Tim = new frmTimPhieuChi();
            Point p = PointToScreen(toolTimKiem.Bounds.Location);
            p.X += toolTimKiem.Width;
            p.Y += toolTimKiem.Height;
            Tim.Location = p;
            Tim.ShowDialog();
            if (Tim.DialogResult == DialogResult.OK)
            {
                HienthiPhieuChi(factory.TimPhieuChi(new PhieuChiSearchParam
                {
                    LyDo = Convert.ToInt32(Tim.cmbLyDo.SelectedValue),
                    Ngay = Tim.dtNgayChi.Value.Date
                }));
            }
        }
    }
}