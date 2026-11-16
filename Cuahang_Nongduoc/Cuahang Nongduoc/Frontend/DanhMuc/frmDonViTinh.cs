using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.Controller;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    /// <summary>
    /// Form quan ly don vi tinh - cho phep xem, them, sua, xoa don vi tinh.
    /// Refactored: them day du xu ly su kien Them/Xoa, validation, xac nhan xoa.
    /// </summary>
    public partial class frmDonViTinh : Form
    {
        // =====================================================================
        // FIELDS
        // =====================================================================
        private readonly DonViTinhController ctrl = new DonViTinhController();
        private bool dangXoa;

        // =====================================================================
        // CONSTRUCTOR
        // =====================================================================
        public frmDonViTinh()
        {
            InitializeComponent();
            CauHinhBindingNavigator();
            dataGridView.UserDeletingRow += dataGridView_UserDeletingRow;
        }

        // =====================================================================
        // LOAD FORM
        // =====================================================================
        private void frmDonViTinh_Load(object sender, EventArgs e)
        {
            try { NapDanhSachDonViTinh(); ApDungPhanQuyen(); }
            catch (Exception ex) { HienThiLoi(ex); }
        }

        private void ApDungPhanQuyen()
        {
            bool coQuyenSua = AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaDonViTinh);
            bindingNavigatorAddNewItem.Enabled = coQuyenSua;
            bindingNavigatorDeleteItem.Enabled = coQuyenSua;
            toolLuu.Enabled = coQuyenSua;
            dataGridView.ReadOnly = !coQuyenSua;
        }

        // =====================================================================
        // PRIVATE METHODS - Extract Method
        // =====================================================================

        /// <summary>
        /// Nap danh sach don vi tinh len DataGridView.
        /// </summary>
        private void NapDanhSachDonViTinh()
        {
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
        }

        /// <summary>
        /// Cau hinh BindingNavigator: gan su kien Them va Xoa tuy chinh
        /// thay vi de mac dinh (mac dinh khong co Save va khong co xac nhan).
        /// </summary>
        private void CauHinhBindingNavigator()
        {
            // Gỡ handler mặc định trước khi gắn handler tùy chỉnh.
            bindingNavigator.AddNewItem = null;
            bindingNavigator.DeleteItem = null;
            bindingNavigatorAddNewItem.Enabled = true;
            bindingNavigatorDeleteItem.Enabled = true;
            bindingNavigatorAddNewItem.Click += new EventHandler(bindingNavigatorAddNewItem_Click);
            bindingNavigatorDeleteItem.Click += new EventHandler(bindingNavigatorDeleteItem_Click);
        }

        // =====================================================================
        // SU KIEN TOOLBAR - LUU
        // =====================================================================
        private void toolLuu_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaDonViTinh, "sửa đơn vị tính")) return;
            try
            {
                if (bindingNavigator.BindingSource == null || !dataGridView.EndEdit()) return;
                bindingNavigator.BindingSource.EndEdit();
                // Factory kiểm tra tất cả dòng thay đổi, kể cả khi không còn Current.
                bool saved = ctrl.Save();
                MessageBox.Show(saved ? "Lưu đơn vị tính thành công." : "Không có thay đổi nào được lưu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { HienThiLoi(ex); }
        }


        // =====================================================================
        // SU KIEN BINDINGNAVIGATOR - THEM MOI
        // =====================================================================
        /// <summary>
        /// Xu ly them dong moi vao BindingSource (giao dien).
        /// Nguoi dung nhap ten roi nhan Luu de luu xuong DB.
        /// </summary>
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaDonViTinh, "thêm đơn vị tính")) return;
            try
            {
                if (bindingNavigator.BindingSource == null || !dataGridView.EndEdit()) return;
                bindingNavigator.BindingSource.EndEdit();
                bindingNavigator.BindingSource.AddNew();
                bindingNavigator.BindingSource.MoveLast();
            }
            catch (Exception ex) { HienThiLoi(ex); }
        }

        // =====================================================================
        // SU KIEN BINDINGNAVIGATOR - XOA
        // =====================================================================
        /// <summary>
        /// Xoa don vi tinh hien tai sau khi xac nhan.
        /// LUI CU: BindingNavigator mac dinh xoa khong hoi xac nhan va khong Save.
        /// </summary>
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaDonViTinh, "xóa đơn vị tính")) return;
            DataRowView view = bindingNavigator.BindingSource == null ? null :
                bindingNavigator.BindingSource.Current as DataRowView;
            XoaDonViTinh(view);
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // Hoãn xử lý đến khi grid kết thúc sự kiện; không sửa Rows ngay trong UserDeletingRow.
            e.Cancel = true;
            DataRowView view = e.Row.DataBoundItem as DataRowView;
            if (view == null || dangXoa) return;
            BeginInvoke(new MethodInvoker(delegate { XoaDonViTinh(view); }));
        }

        private void XoaDonViTinh(DataRowView view)
        {
            if (dangXoa || view == null || (!view.IsNew && view.Row.RowState == DataRowState.Detached) ||
                view.Row.RowState == DataRowState.Deleted) return;
            dangXoa = true;
            try
            {
                int id;
                if (ctrl.LayIdDaLuu(view.Row, out id) && ctrl.DangDuocSuDung(id))
                {
                    MessageBox.Show(CuahangNongduoc.DataLayer.DonViTinhFactory.DonViDangDuocSuDung,
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa đơn vị tính này không?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                // Chỉ xóa dòng đã xác định, không Save/RejectChanges toàn bộ danh sách.
                if (view.IsNew) view.CancelEdit();
                else ctrl.Xoa(view.Row);
            }
            catch (Exception ex) { HienThiLoi(ex); }
            finally { dangXoa = false; }
        }

        private void HienThiLoi(Exception ex)
        {
            MessageBox.Show("Không thể thực hiện thao tác.\n" + ex.Message,
                "Lỗi đơn vị tính", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
