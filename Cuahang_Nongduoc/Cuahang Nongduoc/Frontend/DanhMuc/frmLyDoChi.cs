using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.DataLayer;
// ĐÃ THAY ĐỔI: thêm dịch vụ phân quyền cho màn hình Lý do chi.
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmLyDoChi : Form
    {
        CuahangNongduoc.Controller.LyDoChiController ctrl = new CuahangNongduoc.Controller.LyDoChiController();
        public frmLyDoChi()
        {
            InitializeComponent();
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // ĐÃ THAY ĐỔI: chặn thao tác xóa trên lưới nếu người dùng không có quyền sửa lý do chi.
            if (!AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaLyDoChi))
            {
                e.Cancel = true;
                return;
            }

            // Get the ID value of the row being deleted (colID is hidden but present)
            object val = e.Row.Cells["colID"].Value;
            long idLyDo = 0;
            if (val != null && long.TryParse(val.ToString(), out idLyDo))
            {
                if (IsLyDoInUse(idLyDo))
                {
                    MessageBox.Show("Không thể xóa Lý do này vì đang được tham chiếu trong Phiếu chi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }

            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Ly Do Chi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            var drv = bindingNavigator.BindingSource?.Current as DataRowView;
            long idLyDo = 0;
            if (drv != null && drv.Row.Table.Columns.Contains("ID"))
            {
                var obj = drv["ID"];
                if (obj != null && long.TryParse(obj.ToString(), out idLyDo))
                {
                    if (IsLyDoInUse(idLyDo))
                    {
                        MessageBox.Show("Không thể xóa Lý do này vì đang được tham chiếu trong Phiếu chi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Ly Do Chi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();

                // Persist change immediately and refresh
                if (!ctrl.Save())
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
            }
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
            // ĐÃ THAY ĐỔI: áp dụng quyền hiện tại lên các nút và trạng thái chỉnh sửa của lưới.
            ApDungPhanQuyen();
        }

        // ĐÃ THAY ĐỔI: bật/tắt các thao tác chỉnh sửa theo quyền sửa lý do chi.
        private void ApDungPhanQuyen()
        {
            bool coQuyenSua = AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaLyDoChi);
            bindingNavigatorAddNewItem.Enabled = coQuyenSua;
            bindingNavigatorDeleteItem.Enabled = coQuyenSua;
            toolLuu.Enabled = coQuyenSua;
            dataGridView.ReadOnly = !coQuyenSua;
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            // ĐÃ THAY ĐỔI: yêu cầu quyền sửa trước khi thêm lý do chi.
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaLyDoChi, "thêm lý do chi")) return;
            dataGridView.EndEdit();
            bindingNavigator.BindingSource.EndEdit();

            for (int i = 0; i < bindingNavigator.BindingSource.Count; i++)
            {
                var rowView = bindingNavigator.BindingSource[i] as DataRowView;
                if (rowView == null ||
                    (rowView.Row.RowState != DataRowState.Added &&
                     rowView.Row.RowState != DataRowState.Modified))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(Convert.ToString(rowView["LY_DO"])))
                {
                    bindingNavigator.BindingSource.Position = i;
                    dataGridView.CurrentCell = dataGridView.Rows[i].Cells["colLyDoChi"];
                    dataGridView.BeginEdit(true);
                    MessageBox.Show("Vui lòng nhập lý do chi trước khi thêm lý do mới.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            bindingNavigator.BindingSource.AddNew();
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            // ĐÃ THAY ĐỔI: yêu cầu quyền sửa trước khi lưu lý do chi.
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaLyDoChi, "sửa lý do chi")) return;
            dataGridView.EndEdit();
            bindingNavigator.BindingSource.EndEdit();

            for (int i = 0; i < bindingNavigator.BindingSource.Count; i++)
            {
                var rowView = bindingNavigator.BindingSource[i] as DataRowView;
                if (rowView == null ||
                    (rowView.Row.RowState != DataRowState.Added &&
                     rowView.Row.RowState != DataRowState.Modified))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(Convert.ToString(rowView["LY_DO"])))
                {
                    bindingNavigator.BindingSource.Position = i;
                    dataGridView.CurrentCell = dataGridView.Rows[i].Cells["colLyDoChi"];
                    dataGridView.BeginEdit(true);
                    MessageBox.Show("Vui lòng nhập lý do chi trước khi lưu.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            ctrl.Save();
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator);
            bindingNavigator.BindingSource.MoveLast();
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Helper: returns true if any PHIEU_CHI references this LyDoChi id
        private bool IsLyDoInUse(long idLyDo)
        {
            try
            {
                PhieuChiFactory phFactory = new PhieuChiFactory();
                DataTable tbl = phFactory.DanhsachPhieuChi();
                if (tbl == null || tbl.Rows.Count == 0) return false;

                // Use Select to find any rows referencing this LyDoChi
                DataRow[] rows = tbl.Select("ID_LY_DO_CHI = " + idLyDo);
                return rows != null && rows.Length > 0;
            }
            catch
            {
                // If something goes wrong, be conservative and treat as 'in use' to avoid accidental deletion
                return true;
            }
        }
    }
}