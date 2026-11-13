using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmLyDoChi : Form
    {
        CuahangNongduoc.Controller.LyDoChiController ctrl = new CuahangNongduoc.Controller.LyDoChiController();
        public frmLyDoChi()
        {
            InitializeComponent();
        }

        // ==================== [FE-DM] REFACTORING ====================
        // Lỗi (Code smell) : Tight Coupling – Quản lý DataBinding UI tại Controller
        // Kỹ thuật         : Move Method (Tách hàm hiển thị riêng tại Form và quản lý Binding)
        // Trước            : ctrl.HienthiDataGridview(dataGridView, bindingNavigator) gọi trực tiếp trong Event Load
        // Sau              : Tách riêng phương thức HienThiDanhSach(), Form tự chủ động quản lý Binding
        // Tác giả          : FE2 Nguyễn Huỳnh Ngọc Anh
        // Hỗ Trợ           : Không
        // Kiểm tra         : QA
        // Ngày chỉnh sửa   : 11/11/2026
        // =============================================================
        private void frmLyDoChi_Load(object sender, EventArgs e)
        {
            HienThiDanhSach();
        }

        private void HienThiDanhSach()
        {
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Ly Do Chi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Ly Do Chi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();
            }
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
        }

      

        private void toolLuu_Click(object sender, EventArgs e)
        {
            bindingNavigatorPositionItem.Focus();
            ctrl.Save();
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
            bindingNavigator.BindingSource.MoveLast();
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}