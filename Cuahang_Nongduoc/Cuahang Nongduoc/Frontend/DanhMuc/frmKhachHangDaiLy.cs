using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmKhachHangDaiLy : Form
    {
        CuahangNongduoc.Controller.KhachHangController ctrl = new CuahangNongduoc.Controller.KhachHangController();

        // Biến cờ để phân biệt đang mở form cho Đại lý hay Khách hàng
        private bool _isDaiLy;

        // ==================== [R06] REFACTORING ====================
        // Lỗi (Code smell) : Duplicate Code - frmKhachHang và frmDaiLy có giao diện và code giống hệt nhau
        // Kỹ thuật : Parameterize (Tham số hóa) / Gộp Form
        // Trước : Dùng 2 form riêng biệt không có tham số (new frmKhachHang() và new frmDaiLy())
        // Sau : Gộp thành 1 form chung, truyền biến cờ bool isDaiLy qua constructor để phân biệt
        // Tác giả : FE2 - Nguyễn Huỳnh Ngọc Anh
        // Hỗ trợ : <Không>
        // Kiểm tra : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa : 06/11/2026
        // ===========================================================
        public frmKhachHangDaiLy(bool isDaiLy)
        {
            InitializeComponent();
            _isDaiLy = isDaiLy;

            // Tự động đổi tên tiêu đề Form cho phù hợp
            this.Text = _isDaiLy ? "Quản lý Đại Lý" : "Quản lý Khách Hàng";
        }

        // ==================== [FE-DM] REFACTORING ====================
        // Lỗi (Code smell) : Tight Coupling – Quản lý DataBinding UI tại Controller
        // Kỹ thuật         : Move Method (Form gọi Controller nạp Grid/Navigator và tự làm chủ DataBinding)
        // Trước            : Gọi trực tiếp Controller trong Form_Load mà không tách phương thức hiển thị riêng
        // Sau              : Tách riêng HienThiDanhSach(), Form chịu trách nhiệm bind UI Control nếu có
        // Tác giả          : FE2 - Nguyễn Huỳnh Ngọc Anh
        // Hỗ Trợ           : Không
        // Kiểm tra         : QA
        // Ngày chỉnh sửa   : 11/11/2026
        // =============================================================
        private void frmKhachHangDaiLy_Load(object sender, EventArgs e)
        {
            HienThiDanhSach();
        }

        private void HienThiDanhSach()
        {
            // Kiểm tra biến cờ để load đúng dữ liệu
            if (_isDaiLy)
            {
                DataTable tbl = ctrl.HienthiDaiLyDataGridview(true);
                BindingSource bs = new BindingSource();
                bs.DataSource = tbl;
                bindingNavigator.BindingSource = bs;
                dataGridView.DataSource = bs;
            }
            else
            {
                DataTable tbl = ctrl.HienthiKhachHangDataGridview(false);
                BindingSource bs = new BindingSource();
                bs.DataSource = tbl;
                bindingNavigator.BindingSource = bs;
                dataGridView.DataSource = bs;
            }
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            bindingNavigatorPositionItem.Focus();
            ctrl.Save();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            long maso = ThamSo.KhachHang;
            ThamSo.KhachHang = maso + 1;

            DataRowView row = (DataRowView)bindingNavigator.BindingSource.AddNew();
            row["ID"] = maso;
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            string title = _isDaiLy ? "Đại Lý" : "Khách Hàng";
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();
            }
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            string title = _isDaiLy ? "Đại Lý" : "Khách Hàng";
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void toolTimHoTen_Click(object sender, EventArgs e)
        {
            toolTimDiaChi.Checked = !toolTimDiaChi.Checked;
            toolTimHoTen.Checked = !toolTimDiaChi.Checked;
            toolTimKhachHang.Text = "Tìm theo Họ tên";
            bindingNavigator.Focus();
        }

        private void toolTimDiaChi_Click(object sender, EventArgs e)
        {
            toolTimHoTen.Checked = !toolTimHoTen.Checked;
            toolTimDiaChi.Checked = !toolTimHoTen.Checked;
            toolTimKhachHang.Text = "Tìm theo Địa chỉ";
            bindingNavigator.Focus();
        }

        private void toolTimKhachHang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                // Truyền trực tiếp biến _isDaiLy vào hàm tìm kiếm
                if (toolTimHoTen.Checked)
                    ctrl.TimHoTen(toolTimKhachHang.Text, _isDaiLy);
                else
                    ctrl.TimDiaChi(toolTimKhachHang.Text, _isDaiLy);
            }
        }

        private void toolTimKhachHang_Leave(object sender, EventArgs e)
        {
            if (toolTimHoTen.Checked == true)
                toolTimKhachHang.Text = "Tìm theo Họ tên";
            else
                toolTimKhachHang.Text = "Tìm theo Địa chỉ";

            toolTimKhachHang.ForeColor = Color.FromArgb(224, 224, 224);
        }

        private void toolTimKhachHang_Enter(object sender, EventArgs e)
        {
            toolTimKhachHang.Text = "";
            toolTimKhachHang.ForeColor = Color.Black;
        }
    }
}