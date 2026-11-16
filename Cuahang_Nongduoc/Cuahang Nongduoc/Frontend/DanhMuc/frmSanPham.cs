using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CuahangNongduoc.Controller;
using CuahangNongduoc.Auth;

namespace CuahangNongduoc
{
    /// <summary>
    /// Màn hình quản lý danh mục sản phẩm, bao gồm tồn kho tổng hợp, giá nhập và giá bán.
    /// Data Binding liên kết các ô nhập với DataTable do Controller tải từ SQL Server.
    /// </summary>
    public partial class frmSanPham : Form
    {
        private FormEditState editState = FormEditState.View;
        private readonly SanPhamController ctrl = new SanPhamController();
        private readonly DonViTinhController ctrlDVT = new DonViTinhController();

        /// <summary>
        /// Ngăn luồng tìm kiếm bị thực thi hai lần khi phím Enter đồng thời kích hoạt sự kiện Leave.
        /// </summary>
        private bool _dangTimKiem = false;

        public frmSanPham()
        {
            InitializeComponent();
        }

        /// Nạp danh mục đơn vị tính và sản phẩm trước khi thiết lập UI State mặc định là View.
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            NapDanhSachDonViTinh();
            NapCotDonViTinhChoGrid();
            NapDanhSachSanPham();
            ApDungPhanQuyen();
            SetState(FormEditState.View);
        }

        /// <summary>
        /// Đồng bộ UI State với quyền chỉnh sửa dữ liệu; View phải bảo toàn dữ liệu đã lưu,
        /// còn Add/Edit cho phép BindingSource nhận thay đổi để Controller ghi xuống SQL Server.
        /// </summary>
        /// <param name="state">Trạng thái giao diện cần áp dụng.</param>
        private void SetState(FormEditState state)
        {
            editState = state;
            bool editing = state != FormEditState.View;
            bindingNavigatorAddNewItem.Enabled = !editing;
            bindingNavigatorDeleteItem.Enabled = !editing;
            toolLuu.Enabled = editing;
            dataGridView.ReadOnly = !editing;
            txtTenSanPham.ReadOnly = !editing;
            cmbDVT.Enabled = editing;
            numSoLuong.Enabled = editing;
            numDonGiaNhap.Enabled = editing;
            numGiaBanSi.Enabled = editing;
            numGiaBanLe.Enabled = editing;
        }

        private void ApDungPhanQuyen()
        {
            bool coQuyenSua = AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaSanPham);
            bindingNavigatorAddNewItem.Enabled = coQuyenSua;
            bindingNavigatorDeleteItem.Enabled = coQuyenSua;
            toolLuu.Enabled = coQuyenSua;
            dataGridView.ReadOnly = !coQuyenSua;
            txtTenSanPham.ReadOnly = !coQuyenSua;
            cmbDVT.Enabled = coQuyenSua;
            numSoLuong.Enabled = coQuyenSua;
            numDonGiaNhap.Enabled = coQuyenSua;
            numGiaBanSi.Enabled = coQuyenSua;
            numGiaBanLe.Enabled = coQuyenSua;
            btnThemDVT.Enabled = AuthorizationService.CoQuyen(AuthorizationService.Permission.SuaDonViTinh);
        }

        /// <summary>
        /// Tải danh mục đơn vị tính để ComboBox hiển thị tên nhưng lưu khóa ID.
        /// </summary>
        private void NapDanhSachDonViTinh()
        {
            ctrlDVT.HienthiAutoComboBox(cmbDVT);
        }

        /// <summary>
        /// Bổ sung cột chọn đơn vị tính một lần; tránh tạo cột trùng sau mỗi lần tải lại dữ liệu.
        /// </summary>
        private void NapCotDonViTinhChoGrid()
        {
            if (!dataGridView.Columns.Contains("colDVT"))
            {
                var colDVT = ctrlDVT.HienthiDataGridViewComboBoxColumn();
                colDVT.Name = "colDVT";
                dataGridView.Columns.Add(colDVT);
            }
        }

        /// <summary>
        /// Tải lại danh sách sản phẩm và thiết lập Data Binding cho các control nhập liệu.
        /// </summary>
        private void NapDanhSachSanPham()
        {
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator,
                txtMaSanPham, txtTenSanPham, cmbDVT,
                numSoLuong, numDonGiaNhap, numGiaBanSi, numGiaBanLe);
        }

        /// <summary>
        /// Điều phối tìm kiếm theo mã hoặc tên, giữ kết quả trong tầng Data Access thông qua Controller.
        /// </summary>
        private void TimSanPham()
        {
            if (toolTimMaSanPham.Checked)
            {
                ctrl.TimMaSanPham(toolTimSanPham.Text.Trim());
            }
            else
            {
                ctrl.TimTenSanPham(toolTimSanPham.Text.Trim());
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu tối thiểu trước khi cho phép ghi sản phẩm vào cơ sở dữ liệu.
        /// </summary>
        private bool TenSanPhamHopLe()
        {
            if (string.IsNullOrWhiteSpace(txtTenSanPham.Text))
            {
                MessageBox.Show("Vui long nhap ten san pham.", "Thong bao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanPham.Focus();
                return false;
            }
            return true;
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaSanPham, "s?a s?n ph?m")) return;
            if (!TenSanPhamHopLe()) return;

            // Kết thúc Data Binding trước khi Save để mọi giá trị đang nằm ở control được đẩy vào DataRow.
            dataGridView.EndEdit();
            bindingNavigator.BindingSource.EndEdit();

            bool luuThanhCong = ctrl.Save();
            if (luuThanhCong)
            {
                MessageBox.Show("Luu thanh cong.", "Thong bao",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Khong co thay doi nao duoc luu.", "Thong bao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaSanPham, "th�m s?n ph?m")) return;
            SetState(FormEditState.Add);
            DataRow row = ctrl.NewRow();

            long maSanPhamMoi = ThamSo.SanPham;

            row["ID"] = maSanPhamMoi;
            row["TEN_SAN_PHAM"] = string.Empty;
            row["SO_LUONG"] = 0;
            row["DON_GIA_NHAP"] = 0;
            row["GIA_BAN_SI"] = 0;
            row["GIA_BAN_LE"] = 0;

            if (cmbDVT.SelectedValue != null)
            {
                row["ID_DON_VI_TINH"] = cmbDVT.SelectedValue;
            }

            ctrl.Add(row);

            ThamSo.SanPham = maSanPhamMoi + 1;

            bindingNavigator.BindingSource.MoveLast();
            txtTenSanPham.Focus();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaSanPham, "x�a s?n ph?m")) return;
            if (bindingNavigator.BindingSource == null ||
                bindingNavigator.BindingSource.Count == 0)
                return;

            // Kiểm tra Foreign Key trước khi xóa để tránh SQL Server phát sinh lỗi và bảo toàn lịch sử bán hàng.
            DataRowView current = bindingNavigator.BindingSource.Current as DataRowView;
            if (current != null && current.Row.RowState != DataRowState.Added)
            {
                using (SqlConnection connection = new SqlConnection(DataService.m_ConnectString))
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM CHI_TIET_PHIEU_BAN CT INNER JOIN MA_SAN_PHAM MSP ON CT.ID_MA_SAN_PHAM = MSP.ID WHERE MSP.ID_SAN_PHAM = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", Convert.ToString(current["ID"]));
                    connection.Open();
                    if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Không thể xóa Sản phẩm này vì đã phát sinh trong Phiếu Bán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            var xacNhan = MessageBox.Show(
                "Ban co chac chan muon xoa san pham nay khong?",
                "Xac nhan xoa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes) return;

            bindingNavigator.BindingSource.RemoveCurrent();
            if (ctrl.Save())
            {
                NapDanhSachSanPham();
                SetState(FormEditState.View);
            }
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnThemDVT_Click(object sender, EventArgs e)
        {
            frmDonViTinh dvtForm = new frmDonViTinh();
            dvtForm.ShowDialog();
            dvtForm.Dispose();

            NapDanhSachDonViTinh();

            if (dataGridView.Columns.Contains("colDVT"))
            {
                dataGridView.Columns.Remove("colDVT");
            }
            NapCotDonViTinhChoGrid();
        }

        private void toolTimMaSanPham_Click(object sender, EventArgs e)
        {
            toolTimMaSanPham.Checked = true;
            toolTimTenSanPham.Checked = false;
            toolTimSanPham.Text = string.Empty;
        }

        private void mnuTimTenSanPham_Click(object sender, EventArgs e)
        {
            toolTimMaSanPham.Checked = false;
            toolTimTenSanPham.Checked = true;
            toolTimSanPham.Text = string.Empty;
        }

        private void toolTimSanPham_Enter(object sender, EventArgs e)
        {
            toolTimSanPham.Text = string.Empty;
            toolTimSanPham.ForeColor = Color.Black;
            _dangTimKiem = false;
        }

        private void toolTimSanPham_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _dangTimKiem = true;
                TimSanPham();
            }
        }

        private void toolTimSanPham_Leave(object sender, EventArgs e)
        {
            if (!_dangTimKiem)
            {
                TimSanPham();
            }
            _dangTimKiem = false;
        }
    }
}
