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
    /// Form quan ly san pham - cho phep xem, them, sua, xoa va tim kiem san pham.
    /// Refactored: Extract Method, Guard Clauses, Remove Dead Code, Fix logic loi.
    /// </summary>
    public partial class frmSanPham : Form
    {
        // =====================================================================
        // FIELDS
        // =====================================================================
        private readonly SanPhamController ctrl = new SanPhamController();
        private readonly DonViTinhController ctrlDVT = new DonViTinhController();

        /// <summary>
        /// Flag tranh goi TimSanPham() 2 lan khi nhan Enter (Enter se kich hoat Leave).
        /// </summary>
        private bool _dangTimKiem = false;

        // =====================================================================
        // CONSTRUCTOR
        // =====================================================================
        public frmSanPham()
        {
            InitializeComponent();
        }

        // =====================================================================
        // LOAD FORM
        // =====================================================================
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            NapDanhSachDonViTinh();
            NapCotDonViTinhChoGrid();
            NapDanhSachSanPham();
            ApDungPhanQuyen();
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

        // =====================================================================
        // PRIVATE METHODS - Extract Method
        // =====================================================================

        /// <summary>
        /// Nap danh sach don vi tinh vao ComboBox cmbDVT.
        /// </summary>
        private void NapDanhSachDonViTinh()
        {
            ctrlDVT.HienthiAutoComboBox(cmbDVT);
        }

        /// <summary>
        /// Them cot ComboBox "Don vi tinh" vao DataGridView neu chua ton tai.
        /// LOI CU: goi Columns.Add moi lan Load -> cot bi trung lap.
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
        /// Nap (hoac lam moi) danh sach san pham len DataGridView va cac control input.
        /// </summary>
        private void NapDanhSachSanPham()
        {
            ctrl.HienthiDataGridview(dataGridView, bindingNavigator,
                txtMaSanPham, txtTenSanPham, cmbDVT,
                numSoLuong, numDonGiaNhap, numGiaBanSi, numGiaBanLe);
        }

        /// <summary>
        /// Thuc hien tim kiem san pham theo tieu chi dang duoc chon.
        /// Extract Method - gom logic tim kiem tu nhieu event handler.
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
        /// Kiem tra ten san pham nhap vao co hop le khong.
        /// Guard Clause - tach validation ra ham rieng.
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

        // =====================================================================
        // SU KIEN TOOLBAR - LUU
        // =====================================================================
        private void toolLuu_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaSanPham, "s?a s?n ph?m")) return;
            if (!TenSanPhamHopLe()) return;

            // FIX: Commit pending edits truoc khi luu
            // - dataGridView.EndEdit(): commit neu nguoi dung dang sua truc tiep tren grid
            // - BindingSource.EndEdit(): dam bao tat ca gia tri tu controls (txt, num, cmb)
            //   duoc push vao DataRow truoc khi m_DataAdapter.Update() duoc goi
            // Khong dung bindingNavigatorPositionItem.Focus() vi ToolStrip items
            // khong tham gia focus chain thong thuong, khong dam bao trigger Leave event
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


        // =====================================================================
        // SU KIEN TOOLBAR - THEM MOI
        // =====================================================================
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaSanPham, "th�m s?n ph?m")) return;
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

        // =====================================================================
        // SU KIEN TOOLBAR - XOA
        // =====================================================================
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (!AuthorizationService.KiemTraVaThongBao(AuthorizationService.Permission.SuaSanPham, "x�a s?n ph?m")) return;
            if (bindingNavigator.BindingSource == null ||
                bindingNavigator.BindingSource.Count == 0)
                return;

            var xacNhan = MessageBox.Show(
                "Ban co chac chan muon xoa san pham nay khong?",
                "Xac nhan xoa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes) return;

            bindingNavigator.BindingSource.RemoveCurrent();

            //bool xoaThanhCong = ctrl.Save();
            //if (!xoaThanhCong)
            //{
            //    MessageBox.Show("Xoa that bai. Co the san pham dang duoc su dung trong phieu nhap/ban.",
            //        "Loi xoa", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //    // FIX: Goi RejectChanges() thay vi NapDanhSachSanPham()
            //    // - RejectChanges() hoan nguyen row tu trang thai Deleted ve Unchanged trong m_Ds
            //    // - BindingSource tu cap nhat lai giao dien (row hien ra lai day du)
            //    // - Tranh loi lech binding khi NapDanhSachSanPham() tao moi BindingSource
            //    ctrl.RejectChanges();
            //}
        }


        // =====================================================================
        // SU KIEN TOOLBAR - THOAT
        // =====================================================================
        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // =====================================================================
        // SU KIEN DATAGRIDVIEW - XU LY LOI DU LIEU
        // =====================================================================
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        // =====================================================================
        // SU KIEN NUT THEM DON VI TINH
        // =====================================================================
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

        // =====================================================================
        // SU KIEN TIM KIEM - CHON TIEU CHI
        // =====================================================================
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

        // =====================================================================
        // SU KIEN TIM KIEM - NHAP TU KHOA
        // =====================================================================
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
