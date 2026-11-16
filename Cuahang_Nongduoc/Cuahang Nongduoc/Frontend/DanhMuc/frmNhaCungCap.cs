using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmNhaCungCap : Form
    {
        private readonly CuahangNongduoc.Controller.NhaCungCapController ctrl = new CuahangNongduoc.Controller.NhaCungCapController();

        public frmNhaCungCap()
        {
            InitializeComponent();
            bindingNavigator.AddNewItem = null;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.CellValidating += dataGridView_CellValidating;
            txtSDT.Leave += txtSDT_Leave;
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            // Khóa mã số không cho sửa thủ công
            txtMaNhaCungCap.ReadOnly = true;
            txtMaNhaCungCap.BackColor = Color.WhiteSmoke;

            // Mở khóa địa chỉ cho phép người dùng nhập
            txtDiaChi.ReadOnly = false;

            // Giới hạn số điện thoại tối đa 10 ký tự
            txtSDT.MaxLength = 10;

            // Nạp dữ liệu và liên kết DataBindings với TextBoxes và DataGridView
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator, txtMaNhaCungCap, txtNhaCungCap, txtDiaChi, txtSDT);

            // Đồng bộ mã nhà cung cấp tiếp theo
            CapNhatThamSoMa();
            ApDungPhanQuyen();
        }

        private void ApDungPhanQuyen()
        {
            bool coQuyenSua = AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaNhaCungCap);
            bindingNavigatorAddNewItem.Enabled = coQuyenSua;
            bindingNavigatorDeleteItem.Enabled = coQuyenSua;
            toolLuu.Enabled = coQuyenSua;
            dataGridView.ReadOnly = !coQuyenSua;
            txtNhaCungCap.ReadOnly = !coQuyenSua;
            txtDiaChi.ReadOnly = !coQuyenSua;
            txtSDT.ReadOnly = !coQuyenSua;
        }

        private void CapNhatThamSoMa()
        {
            long maxId = 0;
            DataTable tbl = bindingNavigator.BindingSource?.DataSource as DataTable;

            if (tbl != null)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                        continue;

                    long id;
                    if (long.TryParse(Convert.ToString(row["ID"]), out id))
                    {
                        if (id > maxId)
                            maxId = id;
                    }
                }
            }

            ThamSo.NhaCungCap = Math.Max(maxId + 1, ThamSo.NhaCungCap);
        }

        private long LayMaNhaCungCapMoi()
        {
            long maxId = 0;
            DataTable tbl = bindingNavigator.BindingSource?.DataSource as DataTable;

            if (tbl != null)
            {
                foreach (DataRow r in tbl.Rows)
                {
                    if (r.RowState == DataRowState.Deleted)
                        continue;

                    long id;
                    if (long.TryParse(Convert.ToString(r["ID"]), out id))
                    {
                        if (id > maxId)
                            maxId = id;
                    }
                }
            }

            long paramId = ThamSo.NhaCungCap;
            long newId = Math.Max(maxId + 1, paramId);

            if (tbl != null)
            {
                bool trungMa;
                do
                {
                    trungMa = false;
                    foreach (DataRow r in tbl.Rows)
                    {
                        if (r.RowState == DataRowState.Deleted)
                            continue;

                        if (Convert.ToString(r["ID"]) == newId.ToString())
                        {
                            trungMa = true;
                            newId++;
                            break;
                        }
                    }
                }
                while (trungMa);
            }

            return newId;
        }

        /// <summary>
        /// Kiểm tra số điện thoại có đúng định dạng số di động Việt Nam hay không:
        /// Gồm đúng 10 chữ số, bắt đầu bằng các đầu số nhà mạng VN: 03, 05, 07, 08, 09.
        /// </summary>
        private bool KiemTraSoDienThoaiVN(string sdt)
        {
            if (string.IsNullOrWhiteSpace(sdt))
                return false;

            return Regex.IsMatch(sdt.Trim(), @"^(03|05|07|08|09)[0-9]{8}$");
        }

        /// <summary>
        /// Kiểm tra số điện thoại có bị trùng với nhà cung cấp khác hay không.
        /// </summary>
        /// <param name="sdt">Số điện thoại cần kiểm tra</param>
        /// <param name="currentId">Mã nhà cung cấp hiện tại (bỏ qua khi sửa)</param>
        /// <param name="tenTrung">Tên nhà cung cấp bị trùng (nếu có)</param>
        /// <param name="idTrung">Mã nhà cung cấp bị trùng (nếu có)</param>
        /// <returns>True nếu trùng, False nếu không trùng</returns>
        private bool KiemTraTrungSoDienThoai(string sdt, string currentId, out string tenTrung, out string idTrung)
        {
            DataTable tbl = bindingNavigator.BindingSource?.DataSource as DataTable;
            if (tbl == null && bindingNavigator.BindingSource?.DataSource is DataView dv)
            {
                tbl = dv.Table;
            }
            return ctrl.KiemTraTrungSDT(sdt, currentId, tbl, out tenTrung, out idTrung);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (bindingNavigator.BindingSource == null)
                return;

            // Kiểm tra: nếu dòng hiện tại chưa hoàn tất thông tin thì yêu cầu nhập đầy đủ trước khi thêm
            if (bindingNavigator.BindingSource.Current is DataRowView cur && cur.Row.RowState != DataRowState.Deleted)
            {
                string curTen = Convert.ToString(cur["HO_TEN"]).Trim();
                string curDiaChi = Convert.ToString(cur["DIA_CHI"]).Trim();
                string curSDT = Convert.ToString(cur["DIEN_THOAI"]).Trim();
                string curId = Convert.ToString(cur["ID"]).Trim();

                if (string.IsNullOrEmpty(curTen) || string.IsNullOrEmpty(curDiaChi) || string.IsNullOrEmpty(curSDT))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin cho nhà cung cấp hiện tại trước khi thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (string.IsNullOrEmpty(curTen)) txtNhaCungCap.Focus();
                    else if (string.IsNullOrEmpty(curDiaChi)) txtDiaChi.Focus();
                    else txtSDT.Focus();
                    return;
                }

                if (!KiemTraSoDienThoaiVN(curSDT))
                {
                    MessageBox.Show("Số điện thoại của nhà cung cấp hiện tại không hợp lệ!\nSố điện thoại Việt Nam phải gồm đúng 10 chữ số và bắt đầu bằng các đầu số: 03, 05, 07, 08, 09.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }

                if (KiemTraTrungSoDienThoai(curSDT, curId, out string tenTrung, out string idTrung))
                {
                    string thongBao = string.IsNullOrEmpty(tenTrung)
                        ? $"Số điện thoại '{curSDT}' của nhà cung cấp hiện tại đã tồn tại cho nhà cung cấp có mã '{idTrung}'!"
                        : $"Số điện thoại '{curSDT}' của nhà cung cấp hiện tại đã được sử dụng bởi nhà cung cấp '{tenTrung}' (Mã: {idTrung})!";
                    MessageBox.Show(thongBao + "\nVui lòng chỉnh sửa số điện thoại trước khi thêm mới.", "Trùng số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    txtSDT.SelectAll();
                    return;
                }
            }

            long newId = LayMaNhaCungCapMoi();

            DataRowView newRow = (DataRowView)bindingNavigator.BindingSource.AddNew();
            newRow["ID"] = newId.ToString();
            newRow["HO_TEN"] = "";
            newRow["DIA_CHI"] = "";
            newRow["DIEN_THOAI"] = "";

            ThamSo.NhaCungCap = newId + 1;

            txtNhaCungCap.Focus();
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingNavigator.BindingSource == null || bindingNavigator.BindingSource.Count == 0)
                {
                    return;
                }

                // 1. Bắt buộc nhập tên nhà cung cấp
                string hoTen = txtNhaCungCap.Text.Trim();
                if (string.IsNullOrWhiteSpace(hoTen))
                {
                    MessageBox.Show("Vui lòng nhập tên nhà cung cấp (không được để trống)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNhaCungCap.Focus();
                    return;
                }

                // 2. Bắt buộc nhập địa chỉ
                string diaChi = txtDiaChi.Text.Trim();
                if (string.IsNullOrWhiteSpace(diaChi))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ nhà cung cấp (không được để trống)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiaChi.Focus();
                    return;
                }

                // 3. Bắt buộc nhập số điện thoại
                string sdt = txtSDT.Text.Trim();
                if (string.IsNullOrWhiteSpace(sdt))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại nhà cung cấp (không được để trống)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }

                // 4. Kiểm tra số điện thoại Việt Nam hợp lệ (đúng 10 số, bắt đầu bằng 03, 05, 07, 08, 09)
                if (!KiemTraSoDienThoaiVN(sdt))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!\nSố điện thoại Việt Nam phải gồm đúng 10 chữ số và bắt đầu bằng các đầu số: 03, 05, 07, 08, 09.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }

                // 5. Kiểm tra trùng số điện thoại của nhà cung cấp đang nhập
                string curId = txtMaNhaCungCap.Text.Trim();
                if (KiemTraTrungSoDienThoai(sdt, curId, out string tenTrung, out string idTrung))
                {
                    string thongBao = string.IsNullOrEmpty(tenTrung)
                        ? $"Số điện thoại '{sdt}' đã tồn tại cho nhà cung cấp có mã '{idTrung}'!"
                        : $"Số điện thoại '{sdt}' đã được sử dụng bởi nhà cung cấp '{tenTrung}' (Mã: {idTrung})!";
                    MessageBox.Show(thongBao + "\nVui lòng nhập số điện thoại khác.", "Trùng số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    txtSDT.SelectAll();
                    return;
                }

                // 6. Kiểm tra các dòng đang thêm mới hoặc chỉnh sửa khác trong DataTable
                DataTable dt = bindingNavigator.BindingSource.DataSource as DataTable;
                if (dt == null && bindingNavigator.BindingSource.DataSource is DataView dvSource)
                {
                    dt = dvSource.Table;
                }
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Unchanged)
                            continue;

                        // Bỏ qua dòng hiện tại đang chọn vì đã kiểm tra qua TextBoxes ở trên
                        if (bindingNavigator.BindingSource.Current is DataRowView cur && cur.Row == row)
                            continue;

                        string rTen = Convert.ToString(row["HO_TEN"]).Trim();
                        string rDiaChi = Convert.ToString(row["DIA_CHI"]).Trim();
                        string rSDT = Convert.ToString(row["DIEN_THOAI"]).Trim();
                        string rId = Convert.ToString(row["ID"]).Trim();

                        if (string.IsNullOrWhiteSpace(rTen))
                        {
                            MessageBox.Show($"Nhà cung cấp mã {rId} chưa nhập tên!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(rDiaChi))
                        {
                            MessageBox.Show($"Nhà cung cấp mã {rId} chưa nhập địa chỉ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(rSDT))
                        {
                            MessageBox.Show($"Nhà cung cấp mã {rId} chưa nhập số điện thoại!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (!KiemTraSoDienThoaiVN(rSDT))
                        {
                            MessageBox.Show($"Nhà cung cấp mã {rId} có số điện thoại không hợp lệ!\nSố điện thoại Việt Nam phải gồm đúng 10 chữ số và bắt đầu bằng: 03, 05, 07, 08, 09.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (KiemTraTrungSoDienThoai(rSDT, rId, out string rowTenTrung, out string rowIdTrung))
                        {
                            string thongBao = string.IsNullOrEmpty(rowTenTrung)
                                ? $"Nhà cung cấp mã '{rId}' có số điện thoại '{rSDT}' bị trùng với nhà cung cấp mã '{rowIdTrung}'!"
                                : $"Nhà cung cấp mã '{rId}' có số điện thoại '{rSDT}' bị trùng với nhà cung cấp '{rowTenTrung}' (Mã: {rowIdTrung})!";
                            MessageBox.Show(thongBao + "\nVui lòng kiểm tra lại trước khi lưu.", "Trùng số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                // 7. Chuẩn hóa khoảng trắng
                txtNhaCungCap.Text = Regex.Replace(hoTen, @"\s+", " ");
                txtDiaChi.Text = Regex.Replace(diaChi, @"\s+", " ");
                txtSDT.Text = sdt;

                // 8. Đồng bộ dữ liệu vào BindingSource
                bindingNavigator.BindingSource.EndEdit();
                bindingNavigatorPositionItem.Focus();

                // 9. Lưu vào cơ sở dữ liệu
                ctrl.Save();

                MessageBox.Show("Đã lưu thông tin nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (bindingNavigator.BindingSource?.Current != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bindingNavigator.BindingSource.RemoveCurrent();
                }
            }
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Nhà Cung Cấp", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingNavigator.BindingSource?.DataSource is DataTable dt)
                {
                    dt.RejectChanges();
                }
            }
            catch { }

            this.Close();
        }

        private void txtNhaCungCap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtDiaChi.Focus();
            }
        }

        private void txtDiaChi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtSDT.Focus();
            }
        }

        private void txtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                toolLuu_Click(sender, e);
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép các phím điều khiển (Backspace, Delete, ...)
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Chỉ cho phép nhập chữ số 0-9
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Số điện thoại Việt Nam bắt buộc chữ số đầu tiên phải là số 0
            if ((txtSDT.SelectionStart == 0 && txtSDT.SelectionLength == txtSDT.Text.Length) ||
                (txtSDT.Text.Length == 0 && txtSDT.SelectionStart == 0))
            {
                if (e.KeyChar != '0')
                {
                    e.Handled = true;
                    return;
                }
            }

            // Khống chế tối đa chỉ 10 chữ số, không cho nhập nhiều hơn
            if (txtSDT.Text.Length >= 10 && txtSDT.SelectionLength == 0)
            {
                e.Handled = true;
            }
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            // Ngăn chặn trường hợp paste văn bản chứa chữ hoặc vượt quá 10 số
            string digitsOnly = Regex.Replace(txtSDT.Text, @"[^\d]", "");
            if (digitsOnly.Length > 10)
            {
                digitsOnly = digitsOnly.Substring(0, 10);
            }

            if (txtSDT.Text != digitsOnly)
            {
                int selStart = txtSDT.SelectionStart;
                txtSDT.Text = digitsOnly;
                txtSDT.SelectionStart = Math.Min(selStart, txtSDT.Text.Length);
            }
        }

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
                return;

            // Nếu người dùng đã nhập đủ 10 số hợp lệ thì kiểm tra ngay xem có bị trùng không
            if (sdt.Length == 10 && KiemTraSoDienThoaiVN(sdt))
            {
                string curId = txtMaNhaCungCap.Text.Trim();
                if (KiemTraTrungSoDienThoai(sdt, curId, out string tenTrung, out string idTrung))
                {
                    string thongBao = string.IsNullOrEmpty(tenTrung)
                        ? $"Số điện thoại '{sdt}' đã tồn tại cho nhà cung cấp có mã '{idTrung}'!"
                        : $"Số điện thoại '{sdt}' đã được sử dụng bởi nhà cung cấp '{tenTrung}' (Mã: {idTrung})!";
                    MessageBox.Show(thongBao + "\nVui lòng nhập số điện thoại khác.", "Trùng số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    txtSDT.SelectAll();
                }
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.KeyPress -= DienThoai_KeyPress;

                if (dataGridView.CurrentCell != null)
                {
                    string columnName =
                        dataGridView.Columns[
                            dataGridView.CurrentCell.ColumnIndex
                        ].DataPropertyName;

                    if (columnName == "DIEN_THOAI")
                    {
                        tb.MaxLength = 10;
                        tb.KeyPress += DienThoai_KeyPress;
                    }
                }
            }
        }

        private void DienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (sender is TextBox tb)
            {
                // Số điện thoại Việt Nam bắt buộc chữ số đầu tiên phải là số 0
                if ((tb.SelectionStart == 0 && tb.SelectionLength == tb.Text.Length) ||
                    (tb.Text.Length == 0 && tb.SelectionStart == 0))
                {
                    if (e.KeyChar != '0')
                    {
                        e.Handled = true;
                        return;
                    }
                }

                if (tb.Text.Length >= 10 && tb.SelectionLength == 0)
                {
                    e.Handled = true;
                }
            }
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string columnName =
                dataGridView.Columns[e.ColumnIndex].DataPropertyName;

            if (columnName != "DIEN_THOAI")
                return;

            string phone = Convert.ToString(e.FormattedValue).Trim();

            if (!string.IsNullOrEmpty(phone))
            {
                if (!KiemTraSoDienThoaiVN(phone))
                {
                    MessageBox.Show(
                        "Số điện thoại không hợp lệ!\nSố điện thoại Việt Nam phải gồm đúng 10 chữ số và bắt đầu bằng các đầu số: 03, 05, 07, 08, 09.",
                        "Dữ liệu không hợp lệ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    e.Cancel = true;
                    return;
                }

                // Lấy ID của dòng đang chỉnh sửa trên DataGridView
                string rowId = "";
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
                {
                    var row = dataGridView.Rows[e.RowIndex];
                    if (row.DataBoundItem is DataRowView drv)
                    {
                        rowId = Convert.ToString(drv["ID"]).Trim();
                    }
                    else if (row.Cells["colID"] != null && row.Cells["colID"].Value != null)
                    {
                        rowId = Convert.ToString(row.Cells["colID"].Value).Trim();
                    }
                }

                if (KiemTraTrungSoDienThoai(phone, rowId, out string tenTrung, out string idTrung))
                {
                    string thongBao = string.IsNullOrEmpty(tenTrung)
                        ? $"Số điện thoại '{phone}' đã được sử dụng bởi nhà cung cấp có mã '{idTrung}'!"
                        : $"Số điện thoại '{phone}' đã được sử dụng bởi nhà cung cấp '{tenTrung}' (Mã: {idTrung})!";
                    MessageBox.Show(
                        thongBao + "\nVui lòng nhập số điện thoại khác.",
                        "Trùng số điện thoại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    e.Cancel = true;
                    return;
                }
            }
        }

        private void btnDS_Click(object sender, EventArgs e)
        {
            if (bindingNavigator.BindingSource != null)
            {
                ctrl.TaiLaiNhaCungCap(bindingNavigator.BindingSource);
            }
            else
            {
                ctrl.HienthiDataGridview(dataGridView, bindingNavigator, txtMaNhaCungCap, txtNhaCungCap, txtDiaChi, txtSDT);
            }
        }

        private void toolTimHoTen_Click(object sender, EventArgs e)
        {
            toolTimDiaChi.Checked = false;
            toolTimHoTen.Checked = true;
            toolTimNhaCungCap.Text = "Tìm theo Nhà cung cấp";
            toolTimNhaCungCap.ForeColor = Color.FromArgb(224, 224, 224);
            bindingNavigator.Focus();
        }

        private void toolTimDiaChi_Click(object sender, EventArgs e)
        {
            toolTimHoTen.Checked = false;
            toolTimDiaChi.Checked = true;
            toolTimNhaCungCap.Text = "Tìm theo Địa chỉ";
            toolTimNhaCungCap.ForeColor = Color.FromArgb(224, 224, 224);
            bindingNavigator.Focus();
        }

        private void toolTimNhaCungCap_Enter(object sender, EventArgs e)
        {
            if (toolTimNhaCungCap.Text.StartsWith("Tìm theo"))
            {
                toolTimNhaCungCap.Text = "";
                toolTimNhaCungCap.ForeColor = Color.Black;
            }
        }

        private void toolTimNhaCungCap_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(toolTimNhaCungCap.Text))
            {
                toolTimNhaCungCap.Text = toolTimHoTen.Checked ? "Tìm theo Nhà cung cấp" : "Tìm theo Địa chỉ";
                toolTimNhaCungCap.ForeColor = Color.FromArgb(224, 224, 224);
            }
        }

        private void toolTimNhaCungCap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 || e.KeyChar == (char)Keys.Enter)
            {
                string keyword = toolTimNhaCungCap.Text.Trim();
                if (!string.IsNullOrEmpty(keyword) && !keyword.StartsWith("Tìm theo"))
                {
                    if (toolTimHoTen.Checked)
                    {
                        ctrl.TimHoTen(keyword, bindingNavigator.BindingSource);
                    }
                    else
                    {
                        ctrl.TimDiaChi(keyword, bindingNavigator.BindingSource);
                    }
                }
            }
        }

        private void txtMaNhaCungCap_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtNhaCungCap_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {
        }
    }
}