using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    public partial class frmDaiLy : Form
    {
        private readonly CuahangNongduoc.Controller.DaiLyController ctrl = new CuahangNongduoc.Controller.DaiLyController();

        public frmDaiLy()
        {
            InitializeComponent();
            CauHinhChiDocDanhSachDaiLy();

            // Đảm bảo Form hiển thị chính giữa màn hình
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmDaiLy_Load(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị dữ liệu lên DataGridView và BindingNavigator
                ctrl.HienthiDaiLyDataGridview(dataGridView, bindingNavigator);

                // Binding dữ liệu cho các TextBox nhập liệu (cột DAI_LY thay vì HO_TEN)
                BindControl(txtMa, "ID");
                BindControl(txtHoTen, "DAI_LY");
                BindControl(txtDiaChi, "DIA_CHI");
                BindControl(txtDienThoai, "DIEN_THOAI");

                txtHoTen.MaxLength = 255;
                txtDiaChi.MaxLength = 255;
                txtDienThoai.MaxLength = 50;
                ApDungPhanQuyen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApDungPhanQuyen()
        {
            bool coQuyenSua = AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaDaiLy);

            bindingNavigatorAddNewItem.Enabled = coQuyenSua;
            bindingNavigatorDeleteItem.Enabled = coQuyenSua;
            toolLuu.Enabled = coQuyenSua;
            txtHoTen.ReadOnly = !coQuyenSua;
            txtDiaChi.ReadOnly = !coQuyenSua;
            txtDienThoai.ReadOnly = !coQuyenSua;
        }

        // Refactoring: Extract Method - tách quy tắc chỉ đọc khỏi phân quyền nhập liệu.
        private void CauHinhChiDocDanhSachDaiLy()
        {
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView.CellBeginEdit += dataGridView_CellBeginEdit;

            txtMa.ReadOnly = true;
            txtMa.BackColor = Color.WhiteSmoke;
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Chặn cả BeginEdit nếu cấu hình bảng bị mở khóa ở nơi khác.
            e.Cancel = true;
        }

        /// <summary>
        /// Gắn DataBindings cho 1 TextBox vào BindingSource thông qua BindingNavigator.
        /// </summary>
        private void BindControl(Control control, string propertyName)
        {
            if (control != null && bindingNavigator.BindingSource != null)
            {
                control.DataBindings.Clear();
                control.DataBindings.Add("Text", bindingNavigator.BindingSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        // ============================================================
        // 1. NÚT THÊM
        // ============================================================
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(
                AuthorizationService.Permission.SuaDaiLy, "thêm đại lý")) return;

            if (bindingNavigator.BindingSource != null)
            {
                long maso = ThamSo.KhachHang;
                DataRowView row = (DataRowView)bindingNavigator.BindingSource.AddNew();
                row["ID"] = maso;
                row["DAI_LY"] = "";
                row["DIA_CHI"] = "";
                row["DIEN_THOAI"] = "";

                // Tăng tham số tự động sau khi cấp mã thành công
                ThamSo.KhachHang = maso + 1;
            }
            txtHoTen.Focus();
        }

        // ============================================================
        // 2. NÚT XÓA
        // ============================================================
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(
                AuthorizationService.Permission.SuaDaiLy, "xóa đại lý")) return;

            if (bindingNavigator.BindingSource?.Current != null)
            {
                try
                {
                    DataRowView view = (DataRowView)bindingNavigator.BindingSource.Current;

                    // Kiểm tra ràng buộc – bỏ qua nếu dòng mới chưa lưu
                    if (view.Row.RowState != DataRowState.Added)
                    {
                        string idDaiLy = Convert.ToString(view["ID"]);
                        if (!ctrl.KiemTraRangBuocXoa(idDaiLy))
                        {
                            MessageBox.Show("Không thể xóa đại lý này vì đã có phát sinh Phiếu bán hàng hoặc Dư nợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa đại lý này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bindingNavigator.BindingSource.RemoveCurrent();
                        // Lưu ngay xuống DB để xóa khỏi cơ sở dữ liệu
                        ctrl.Save();
                        MessageBox.Show("Đã xóa đại lý thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Bấm phím Delete trên bàn phím (DataGridView)
        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaDaiLy))
            {
                e.Cancel = true;
                return;
            }

            try
            {
                // Kiểm tra ràng buộc trước khi xóa
                if (e.Row.Cells["colID"].Value != null)
                {
                    string idDaiLy = e.Row.Cells["colID"].Value.ToString();
                    DataRowView drv = e.Row.DataBoundItem as DataRowView;

                    // Chỉ kiểm tra ràng buộc nếu dòng đã tồn tại trong DB
                    if (drv != null && drv.Row.RowState != DataRowState.Added)
                    {
                        if (!ctrl.KiemTraRangBuocXoa(idDaiLy))
                        {
                            MessageBox.Show("Không thể xóa đại lý này vì đã có phát sinh Phiếu bán hàng hoặc Dư nợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                            return;
                        }
                    }
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa đại lý này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    // Sau khi xóa xong sẽ tự động lưu xuống DB
                    this.BeginInvoke((Action)(() => {
                        try { ctrl.Save(); } catch { }
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        // ============================================================
        // 3. NÚT LƯU
        // ============================================================
        private void toolLuu_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(
                AuthorizationService.Permission.SuaDaiLy, "lưu đại lý")) return;

            try
            {
                // 1. Kiểm tra trường Tên đại lý không được để trống
                string tenDaiLy = txtHoTen.Text.Trim();
                if (string.IsNullOrWhiteSpace(tenDaiLy))
                {
                    MessageBox.Show("Tên đại lý không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return;
                }

                // 2. Kiểm tra số điện thoại bắt buộc, đúng định dạng và không trùng.
                string sdt = txtDienThoai.Text.Trim();
                string loiDienThoai = LayLoiDienThoai(sdt);
                if (loiDienThoai != null)
                {
                    MessageBox.Show(loiDienThoai, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDienThoai.Focus();
                    return;
                }

                // 4. Tự động chuẩn hóa khoảng trắng
                txtHoTen.Text = Regex.Replace(tenDaiLy, @"\s+", " ");
                if (!string.IsNullOrEmpty(txtDiaChi.Text))
                {
                    txtDiaChi.Text = Regex.Replace(txtDiaChi.Text.Trim(), @"\s+", " ");
                }

                // 5. Buộc BindingSource kết thúc quá trình chỉnh sửa
                if (bindingNavigator.BindingSource != null)
                {
                    bindingNavigator.BindingSource.EndEdit();
                }

                bindingNavigatorPositionItem.Focus();

                // 6. Gọi lệnh lưu xuống CSDL bảng DAI_LY
                ctrl.Save();

                MessageBox.Show("Đã lưu dữ liệu đại lý thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refactoring: Extract Method - tách kiểm tra SĐT khỏi sự kiện Lưu.
        private string LayLoiDienThoai(string sdt)
        {
            // Refactoring: Replace Nested Conditional with Guard Clauses.
            if (string.IsNullOrWhiteSpace(sdt))
                return "Số điện thoại không được để trống!";

            if (!Regex.IsMatch(sdt, @"^(03[2-9]|05[2-9]|07[0-9]|08[1-9]|09[0-9])[0-9]{7}$"))
                return "Số điện thoại không hợp lệ! Vui lòng nhập số di động Việt Nam gồm 10 chữ số, bắt đầu bằng đầu số hợp lệ (03, 05, 07, 08 hoặc 09).";

            if (bindingNavigator.BindingSource != null && bindingNavigator.BindingSource.DataSource is DataTable dt)
            {
                var currentRow = ((DataRowView)bindingNavigator.BindingSource.Current)?.Row;
                foreach (DataRow row in dt.Rows)
                {
                    if (row == currentRow || row.RowState == DataRowState.Deleted) continue;

                    if (row["DIEN_THOAI"] != DBNull.Value && row["DIEN_THOAI"].ToString().Trim() == sdt)
                        return $"Số điện thoại '{sdt}' đã tồn tại cho đại lý khác ({row["DAI_LY"]}). Vui lòng nhập số khác!";
                }
            }

            return null;
        }

        // ============================================================
        // 4. NÚT HỦY THAO TÁC
        // ============================================================
        private void toolHuy_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingNavigator.BindingSource != null)
                {
                    BindingSource source = bindingNavigator.BindingSource;
                    source.CancelEdit();

                    // Nạp lại toàn bộ danh sách đại lý (hủy bỏ mọi thay đổi chưa lưu)
                    ctrl.TaiLaiDaiLy(source);
                    source.MoveFirst();
                }

                MessageBox.Show("Đã hủy bỏ thao tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy thao tác: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // NÚT THOÁT
        // ============================================================
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

        // ============================================================
        // CHẶN NHẬP CHỮ VÀO Ô ĐIỆN THOẠI
        // ============================================================
        private void txtDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (txtDienThoai.Text.Length >= 10 && txtDienThoai.SelectionLength == 0)
            {
                e.Handled = true;
            }
        }

        // ============================================================
        // TÌM KIẾM
        // ============================================================
        private void toolTimHoTen_Click(object sender, EventArgs e)
        {
            toolTimDiaChi.Checked = false;
            toolTimHoTen.Checked = true;
            toolTimKhachHang.Text = "Tìm theo Tên đại lý";
            toolTimKhachHang.ForeColor = Color.Gray;
            bindingNavigator.Focus();
        }

        private void toolTimDiaChi_Click(object sender, EventArgs e)
        {
            toolTimHoTen.Checked = false;
            toolTimDiaChi.Checked = true;
            toolTimKhachHang.Text = "Tìm theo Địa chỉ";
            toolTimKhachHang.ForeColor = Color.Gray;
            bindingNavigator.Focus();
        }

        private void toolTimKhachHang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                string keyword = toolTimKhachHang.Text.Trim();
                if (!string.IsNullOrEmpty(keyword) && !keyword.StartsWith("Tìm theo"))
                {
                    try
                    {
                        if (toolTimHoTen.Checked)
                            ctrl.TimHoTen(keyword, bindingNavigator.BindingSource);
                        else
                            ctrl.TimDiaChi(keyword, bindingNavigator.BindingSource);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void toolTimKhachHang_Enter(object sender, EventArgs e)
        {
            if (toolTimKhachHang.Text.StartsWith("Tìm theo"))
            {
                toolTimKhachHang.Text = "";
                toolTimKhachHang.ForeColor = Color.Black;
            }
        }

        private void toolTimKhachHang_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(toolTimKhachHang.Text))
            {
                toolTimKhachHang.Text = toolTimHoTen.Checked ? "Tìm theo Tên đại lý" : "Tìm theo Địa chỉ";
                toolTimKhachHang.ForeColor = Color.Gray;
            }
        }
    }
}
