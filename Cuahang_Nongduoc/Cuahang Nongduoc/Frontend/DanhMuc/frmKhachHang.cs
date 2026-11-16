using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
    /// <summary>Màn hình quản lý khách hàng cá nhân dùng trong nghiệp vụ bán hàng và công nợ.</summary>
    public partial class frmKhachHang : Form
    {
        private FormEditState editState = FormEditState.View;
        private readonly CuahangNongduoc.Controller.KhachHangController ctrl = new CuahangNongduoc.Controller.KhachHangController();

        public frmKhachHang()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            try
            {
                // ID là khóa tham chiếu trong PHIEU_BAN nên chỉ được cấp tự động, không cho sửa trực tiếp.
                txtMa.ReadOnly = true;
                txtMa.BackColor = Color.WhiteSmoke;

                // Nạp DataTable một lần để lưới, BindingNavigator và các ô chi tiết dùng cùng nguồn dữ liệu.
                ctrl.HienthiKhachHangDataGridview(dataGridView, bindingNavigator);

                // Data Binding giúp thay đổi trên form được phản ánh vào DataRow trước khi Controller Save.
                BindControl(txtMa, "ID");
                BindControl(txtHoTen, "HO_TEN");
                BindControl(txtDiaChi, "DIA_CHI");
                BindControl(txtDienThoai, "DIEN_THOAI");
                SetState(FormEditState.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Điều khiển UI State để dữ liệu khách hàng chỉ được sửa trong Add/Edit.</summary>
        /// <param name="state">Trạng thái View, Add hoặc Edit.</param>
        private void SetState(FormEditState state)
        {
            editState = state;
            bool editing = state != FormEditState.View;
            bindingNavigatorAddNewItem.Enabled = !editing;
            bindingNavigatorDeleteItem.Enabled = !editing;
            toolLuu.Enabled = editing;
            toolHuy.Enabled = editing;
            txtHoTen.ReadOnly = !editing;
            txtDiaChi.ReadOnly = !editing;
            txtDienThoai.ReadOnly = !editing;
            dataGridView.ReadOnly = !editing;
        }

        /// <summary>Liên kết thuộc tính hiển thị của control với cột tương ứng trong BindingSource.</summary>
        /// <param name="control">Control nhận dữ liệu.</param><param name="propertyName">Tên cột DataTable.</param>
        private void BindControl(Control control, string propertyName)
        {
            if (control != null && bindingNavigator.BindingSource != null)
            {
                control.DataBindings.Clear();
                control.DataBindings.Add("Text", bindingNavigator.BindingSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Chặn dữ liệu thiếu trước khi Data Binding đẩy bản ghi không hợp lệ xuống Data Access Layer.
                string hoTen = txtHoTen.Text.Trim();
                if (string.IsNullOrWhiteSpace(hoTen))
                {
                    MessageBox.Show("Họ tên khách hàng không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return;
                }

                // Chuẩn hóa số điện thoại giúp các nghiệp vụ liên hệ và kiểm tra trùng hoạt động ổn định.
                string sdt = txtDienThoai.Text.Trim();
                if (string.IsNullOrWhiteSpace(sdt))
                {
                    MessageBox.Show("Số điện thoại khách hàng là bắt buộc!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDienThoai.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(sdt))
                {
                    // Bao gồm các đầu số di động hiện hành sau chuyển đổi 11 số về 10 số.
                    bool sdtHopLe = Regex.IsMatch(sdt, @"^(03[2-9]|05[2-9]|07[0-9]|08[1-9]|09[0-9])[0-9]{7}$");
                    if (!sdtHopLe)
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập số di động Việt Nam gồm 10 chữ số, bắt đầu bằng đầu số hợp lệ (03, 05, 07, 08 hoặc 09).", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDienThoai.Focus();
                        return;
                    }

                    // Kiểm tra trùng ngay trên bộ nhớ để phản hồi nhanh trước khi phát sinh lệnh ghi SQL Server.
                    if (bindingNavigator.BindingSource != null && bindingNavigator.BindingSource.DataSource is DataTable dt)
                    {
                        var currentRow = ((DataRowView)bindingNavigator.BindingSource.Current)?.Row;
                        foreach (DataRow row in dt.Rows)
                        {
                            // Bỏ qua dòng hiện tại đang sửa
                            if (row == currentRow) continue;

                            // Nếu trạng thái dòng bị xóa thì bỏ qua
                            if (row.RowState == DataRowState.Deleted) continue;

                            if (row["DIEN_THOAI"] != DBNull.Value && row["DIEN_THOAI"].ToString().Trim() == sdt)
                            {
                                MessageBox.Show($"Số điện thoại '{sdt}' đã tồn tại cho khách hàng khác ({row["HO_TEN"]}). Vui lòng nhập số khác!", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtDienThoai.Focus();
                                return;
                            }
                        }
                    }
                }

                // Chuẩn hóa dữ liệu trình bày để tìm kiếm và đối soát khách hàng không phụ thuộc khoảng trắng thừa.
                txtHoTen.Text = Regex.Replace(hoTen, @"\s+", " ");
                if (!string.IsNullOrEmpty(txtDiaChi.Text))
                {
                    txtDiaChi.Text = Regex.Replace(txtDiaChi.Text.Trim(), @"\s+", " ");
                }

                // Kết thúc Data Binding trước Save để Controller nhận đầy đủ thay đổi của bản ghi hiện tại.
                if (bindingNavigator.BindingSource != null)
                {
                    bindingNavigator.BindingSource.EndEdit();
                }

                bindingNavigatorPositionItem.Focus();

                BindingSource source = bindingNavigator.BindingSource;
                string savedId = (source.Current as DataRowView)?["ID"]?.ToString();
                // Controller chuyển bản ghi đã xác thực xuống Data Access Layer và SQL Server.
                ctrl.Save();

                ctrl.TaiLaiKhachHang(true, source);
                if (!string.IsNullOrEmpty(savedId))
                {
                    for (int i = 0; i < source.Count; i++)
                    {
                        if (source[i] is DataRowView row && Convert.ToString(row["ID"]) == savedId)
                        {
                            source.Position = i;
                            break;
                        }
                    }
                }

                MessageBox.Show("Đã lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetState(FormEditState.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            SetState(FormEditState.Add);
            if (bindingNavigator.BindingSource != null)
            {
                long maso = ThamSo.KhachHang;
                DataRowView row = (DataRowView)bindingNavigator.BindingSource.AddNew();
                row["ID"] = maso;
                row["HO_TEN"] = "";
                row["DIA_CHI"] = "";
                row["DIEN_THOAI"] = "";

                // Tăng tham số tự động sau khi cấp mã thành công
                ThamSo.KhachHang = maso + 1;
            }

            txtHoTen.Focus();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (bindingNavigator.BindingSource?.Current != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BindingSource source = bindingNavigator.BindingSource;
                    try
                    {
                        // Đồng bộ Data Binding trước khi kiểm tra/xóa để ID dùng kiểm tra Foreign Key là giá trị mới nhất.
                        source.EndEdit();
                        if (source.Current is DataRowView currentRow)
                        {
                            string id = Convert.ToString(currentRow["ID"]);
                            if (String.IsNullOrWhiteSpace(id))
                            {
                                source.RemoveCurrent();
                                return;
                            }
                            // Kiểm tra Foreign Key với PHIEU_BAN để không xóa khách hàng đã có lịch sử mua hàng.
                            using (SqlConnection connection = new SqlConnection(DataService.m_ConnectString))
                            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM PHIEU_BAN WHERE ID_KHACH_HANG = @id", connection))
                            {
                                command.Parameters.AddWithValue("@id", id);
                                connection.Open();
                                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                                {
                                    MessageBox.Show("Không thể xóa Khách hàng này vì đã có lịch sử Mua hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        source.RemoveCurrent();
                        if (!ctrl.Save())
                        {
                            ctrl.TaiLaiKhachHang(true, source);
                            MessageBox.Show("Không có dữ liệu nào được xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            ctrl.TaiLaiKhachHang(true, source);
                            SetState(FormEditState.View);
                        }
                    }
                    catch (Exception ex)
                    {
                        ctrl.TaiLaiKhachHang(true, source);
                        MessageBox.Show("Không thể xóa khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void toolTimHoTen_Click(object sender, EventArgs e)
        {
            toolTimDiaChi.Checked = false;
            toolTimHoTen.Checked = true;
            toolTimKhachHang.Text = "Tìm theo Họ tên";
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
                toolTimKhachHang.Text = toolTimHoTen.Checked ? "Tìm theo Họ tên" : "Tìm theo Địa chỉ";
                toolTimKhachHang.ForeColor = Color.Gray;
            }
        }

        private void toolTimKhachHang_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(toolTimKhachHang.Text) && bindingNavigator.BindingSource != null)
            {
                try
                {
                    ctrl.TaiLaiKhachHang(true, bindingNavigator.BindingSource);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
                            ctrl.TimHoTen(keyword, true, bindingNavigator.BindingSource);
                        else
                            ctrl.TimDiaChi(keyword, true, bindingNavigator.BindingSource);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            // Xử lý hoàn tác (RejectChanges) nếu có dữ liệu chưa lưu trước khi thoát
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

        private void txtDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép các phím điều khiển hệ thống (Backspace, Delete, v.v.)
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Chặn tuyệt đối không cho nhập chữ cái hoặc ký tự đặc biệt, chỉ nhận số (0-9)
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Khống chế nghiêm ngặt tối đa không quá 10 ký tự số
            if (txtDienThoai.Text.Length >= 10 && txtDienThoai.SelectionLength == 0)
            {
                e.Handled = true;
            }
        }

        private void toolHuy_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingNavigator.BindingSource != null)
                {
                    BindingSource source = bindingNavigator.BindingSource;
                    source.CancelEdit();

                    // Bỏ chọn bản ghi để làm trống các ô nhập mà không thay đổi danh sách.
                    source.Position = -1;
                    dataGridView.ClearSelection();
                }

                MessageBox.Show("Đã hủy bỏ thao tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy thao tác: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
