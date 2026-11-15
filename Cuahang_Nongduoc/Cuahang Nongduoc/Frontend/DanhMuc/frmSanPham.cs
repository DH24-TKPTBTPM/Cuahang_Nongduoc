using CuahangNongduoc.Controller;
using CuahangNongduoc.Frontend.Common1; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmSanPham : Form
    {
        SanPhamController ctrl = new SanPhamController();
        DonViTinhController ctrlDVT = new DonViTinhController();

        public frmSanPham()
        {
            InitializeComponent();
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            // Gọi 2 hàm đã được tách riêng để xử lý hiển thị giao diện tại Frontend
            HienThiDonViTinh();
            HienThiSanPham();
        }

        // ==================== [FE-DM] REFACTORING ====================
        // Lỗi (Code smell) : Tight Coupling – DonViTinhController nhận tham số UI Control (ComboBox)
        // Kỹ thuật         : Move Method (Tách việc hiển thị/binding ComboBox về Form)
        // Trước            : ctrlDVT.HienthiAutoComboBox(cmbDVT);
        //                    dataGridView.Columns.Add(ctrlDVT.HienthiDataGridViewComboBoxColumn());
        // Sau              : Form tự nạp và dựng DataGridViewComboBoxColumn từ Controller
        // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Hỗ Trợ           : Không
        // Kiểm tra         : QA
        // Ngày chỉnh sửa   : 11/11/2026
        // =============================================================
        private void HienThiDonViTinh()
        {
            ctrlDVT.HienthiAutoComboBox(cmbDVT);

            if (!dataGridView.Columns.Contains("ID_DON_VI_TINH"))
            {
                DataGridViewComboBoxColumn colDVT = ctrlDVT.HienthiDataGridViewComboBoxColumn();
                dataGridView.Columns.Add(colDVT);
            }
        }

        // ==================== [FE-DM] REFACTORING ====================
        // Lỗi (Code smell) : Tight Coupling – SanPhamController nhận 9 WinForms Controls làm tham số
        // Kỹ thuật         : Move Method (Chuyển toàn bộ logic DataBinding các ô nhập liệu từ Controller về Form)
        // Trước            : ctrl.HienthiDataGridview(dataGridView, bindingNavigator, txtMaSanPham, txtTenSanPham, cmbDVT, numSoLuong, numDonGiaNhap, numGiaBanSi, numGiaBanLe);
        // Sau              : Controller chỉ nạp Lưới/Navigator, Form lấy BindingSource tự DataBindings vào từng Control
        // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Hỗ Trợ           : Không
        // Kiểm tra         : QA
        // Ngày chỉnh sửa   : 11/11/2026
        // =============================================================
        private void HienThiSanPham()
        {
            // 1. Controller chỉ gán DataGridView & BindingNavigator (loại bỏ 7 tham số Control nhập liệu)
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);

            // 2. Form tự chủ động thực hiện DataBinding cho các ô nhập liệu
            BindingSource bs = bindingNavigator.BindingSource;
            if (bs != null)
            {
                txtMaSanPham.DataBindings.Clear();
                txtTenSanPham.DataBindings.Clear();
                cmbDVT.DataBindings.Clear();
                numSoLuong.DataBindings.Clear();
                numDonGiaNhap.DataBindings.Clear();
                numGiaBanSi.DataBindings.Clear();
                numGiaBanLe.DataBindings.Clear();

                txtMaSanPham.DataBindings.Add("Text", bs, "ID", true, DataSourceUpdateMode.OnPropertyChanged);
                txtTenSanPham.DataBindings.Add("Text", bs, "TEN_SAN_PHAM", true, DataSourceUpdateMode.OnPropertyChanged);
                cmbDVT.DataBindings.Add("SelectedValue", bs, "ID_DON_VI_TINH", true, DataSourceUpdateMode.OnPropertyChanged);
                numSoLuong.DataBindings.Add("Value", bs, "SO_LUONG", true, DataSourceUpdateMode.OnPropertyChanged, 0);
                numDonGiaNhap.DataBindings.Add("Value", bs, "DON_GIA_NHAP", true, DataSourceUpdateMode.OnPropertyChanged, 0);
                numGiaBanSi.DataBindings.Add("Value", bs, "GIA_BAN_SI", true, DataSourceUpdateMode.OnPropertyChanged, 0);
                numGiaBanLe.DataBindings.Add("Value", bs, "GIA_BAN_LE", true, DataSourceUpdateMode.OnPropertyChanged, 0);
            }
        }


        private void toolLuu_Click(object sender, EventArgs e)
        {
            bindingNavigatorPositionItem.Focus();
            ctrl.Save();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            DataRow row = ctrl.NewRow();
            long maso = ThamSo.SanPham;
            ThamSo.SanPham = maso+1;
            row["ID"] = maso;
            row["TEN_SAN_PHAM"] = "";
            row["SO_LUONG"] = 0;
            row["DON_GIA_NHAP"] = 0;
            row["GIA_BAN_SI"] = 0;
            row["GIA_BAN_LE"] = 0;
            ctrl.Add(row);
            bindingNavigator.BindingSource.MoveLast();
            
        }

      
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "San Pham", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();
            }
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ==================== [FE-09] REFACTORING ====================
        // Lỗi (Code smell) : Duplicate Code – Xử lý DataError thủ công chỉ dùng e.Cancel
        // Kỹ thuật         : Extract Method (Gọi hàm dùng chung từ GridErrorHelper)
        // Trước            : e.Cancel = true;
        // Sau              : GridErrorHelper.HandleDataError(sender, e);
        // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Hỗ trợ           : Không
        // Kiểm tra         : QA – <Điền họ tên> (QA điền khi test đạt)
        // Ngày chỉnh sửa   : 10/11/2026
        // =============================================================
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            GridErrorHelper.HandleDataError(sender, e);
        }

        private void btnThemDVT_Click(object sender, EventArgs e)
        {
            frmDonViTinh DVT = new frmDonViTinh();
            DVT.ShowDialog();
            ctrlDVT.HienthiAutoComboBox(cmbDVT);
        }


        private void toolTimMaSanPham_Click(object sender, EventArgs e)
        {
            toolTimMaSanPham.Checked = true;
            toolTimTenSanPham.Checked = false;
            toolTimSanPham.Text = "";

        }

        private void mnuTimTenSanPham_Click(object sender, EventArgs e)
        {
            toolTimMaSanPham.Checked = false;
            toolTimTenSanPham.Checked = true;
            toolTimSanPham.Text = "";
        }

        private void toolTimSanPham_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimSanPham();
            }
        }

        private void toolTimSanPham_Leave(object sender, EventArgs e)
        {
            TimSanPham();
        }

        void TimSanPham()
        {
            if (toolTimMaSanPham.Checked == true)
            {
                ctrl.TimMaSanPham(toolTimSanPham.Text);
            }
            else
            {
                ctrl.TimTenSanPham(toolTimSanPham.Text);
            }
        }

        private void toolTimSanPham_Enter(object sender, EventArgs e)
        {
            toolTimSanPham.Text = "";
            toolTimSanPham.ForeColor = Color.Black;
        }
      


    }
}