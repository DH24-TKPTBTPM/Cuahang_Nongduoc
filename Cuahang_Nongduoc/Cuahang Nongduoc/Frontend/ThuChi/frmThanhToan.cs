using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer; // FE3-[FE-TC] Dùng PhieuThanhToanFactory cho Form tự binding

namespace CuahangNongduoc
{
    public partial class frmThanhToan : Form
    {
        KhachHangController ctrlKH = new KhachHangController();
        PhieuThanhToanController ctrl = new PhieuThanhToanController();
        PhieuThanhToanFactory factory = new PhieuThanhToanFactory(); // FE3-[FE-TC] Khởi tạo Factory
        public frmThanhToan()
        {
            InitializeComponent();
        }

        // ==================== [FE-TC] REFACTORING ====================
        // Lỗi (Code smell) : Controller nhận trực tiếp các control UI (BindingNavigator, DataGridView, TextBox...) để binding
        // Kỹ thuật         : Move Method (Chuyển logic binding dữ liệu từ Controller về Form giao diện)
        // Trước            : ctrl.HienthiPhieuThanhToan(bindingNavigator, dataGridView, cmbKhachHang, txtMaPhieu, dtNgayThanhToan, numTongTien, txtGhiChu);
        // Sau              : HienthiPhieuThanhToan(factory.DanhsachPhieuThanhToan());
        // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
        // Hỗ trợ           : BE2 (Task R26)
        // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa   : 08/11/2026
        // =============================================================
        private void HienthiPhieuThanhToan(DataTable dt)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dt;
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
        private void frmThanhToan_Load(object sender, EventArgs e)
        {
            ctrlKH.HienthiChungAutoComboBox(cmbKhachHang);
            ctrlKH.HienthiKhachHangChungDataGridviewComboBox(colKhachHang);
            //ctrl.HienthiPhieuThanhToan(bindingNavigator, dataGridView, cmbKhachHang, txtMaPhieu, dtNgayThanhToan, numTongTien, txtGhiChu);
            HienthiPhieuThanhToan(factory.DanhsachPhieuThanhToan()); // FE3-[FE-TC] Form tự binding dữ liệu
            bindingNavigator.BindingSource.AddingNew += new AddingNewEventHandler(BindingSource_AddingNew);
        }

        void BindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            
        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            long maphieu = ThamSo.LayMaPhieuThanhToan();;
            ThamSo.GanMaPhieuThanhToan(maphieu+1);

            //DataRow row = ctrl.NewRow();
            DataRow row = factory.NewRow(); // FE3-[FE-TC] Thay cho ctrl.NewRow()
            row["ID"] = maphieu;
            row["NGAY_THANH_TOAN"] = DateTime.Now.Date;
            row["TONG_TIEN"] = numTongTien.Value;
            //ctrl.Add(row);
            factory.Add(row); // FE3-[FE-TC] Thay cho ctrl.Add(row)
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
                // ctrl.Save();
                factory.Save(); // FE3-[FE-TC] Thay cho ctrl.Save()
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            txtMaPhieu.Focus();
            bindingNavigator.BindingSource.MoveNext();
            // ctrl.Save();
            factory.Save(); // FE3-[FE-TC] Thay cho ctrl.Save()
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
                // ctrl.TimPhieuThanhToan(bindingNavigator, dataGridView, cmbKhachHang, txtMaPhieu, dtNgayThanhToan, numTongTien, txtGhiChu,
                //Tim.cmbKhachHang.SelectedValue.ToString(), Tim.dtNgayThu.Value.Date);
                HienthiPhieuThanhToan(factory.TimPhieuThanhToan(Tim.cmbKhachHang.SelectedValue.ToString(), Tim.dtNgayThu.Value.Date)); // FE3-[FE-TC] Tự binding kết quả tìm kiếm
            }
        }

    }
}