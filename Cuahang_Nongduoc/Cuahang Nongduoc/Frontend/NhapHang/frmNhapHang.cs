using CuahangNongduoc.Backend.Patterns.State;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.Frontend.Common1;
using CuahangNongduoc.Patterns.Factory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmNhapHang : Form
    {
        SanPhamController ctrlSanPham = new SanPhamController();
        PhieuNhapController ctrl = new PhieuNhapController();
        MaSanPhamController ctrlMaSP = new MaSanPhamController();
        NhaCungCapController ctrlNCC = new NhaCungCapController();
        PhieuNhap m_PhieuNhap = null;


        // ==================== [FE-FE2] REFACTORING ====================
        // Lỗi (Code smell) : Primitive Obsession / Hardcoded State – Quản lý trạng thái giao diện bằng enum 'status' gây phân nhánh rải rác
        // Kỹ thuật         : Replace State/Strategy with State Pattern & Encapsulate Field
        // Trước            : Controll status = Controll.Normal; (dùng if/else kiểm tra status ở các nút bấm & hàm Allow để private)
        // Sau              : private IFrmNhapHangState currentState; (chuyển giao quản lý cho các lớp State & đổi Allow() thành public)
        // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Hỗ trợ           : <Không>
        // Kiểm tra         : QA – <Họ_tên>
        // Ngày chỉnh sửa   : 9/11/2026
        // ===========================================================
        private IFrmNhapHangState currentState;

        public frmNhapHang()
        {
            InitializeComponent();
            SetState(new ThemState());
        }

        public frmNhapHang(PhieuNhapController ctrlPN)
          : this()
        {
            this.ctrl = ctrlPN;
            SetState(new XemState());
        }

        // Phương thức thay đổi State hiện tại
        public void SetState(IFrmNhapHangState state)
        {
            this.currentState = state;
            this.currentState.Handle(this);
        }

        void BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (currentState is XemState)
                ctrlMaSP.HienThiChiTietPhieuNhap(txtMaPhieu.Text, dataGridView);
        }

        private void frmNhapHang_Load(object sender, EventArgs e)
        {

            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
            ctrlSanPham.HienthiDataGridViewComboBoxColumn(colSanPham);
            ctrlNCC.HienthiAutoComboBox(cmbNhaCungCap);



            ctrl.HienthiPhieuNhap(bindingNavigator, txtMaPhieu, cmbNhaCungCap, dtNgayNhap, numTongTien, numDaTra, numConNo);
            bindingNavigator.BindingSource.CurrentChanged -= new EventHandler(BindingSource_CurrentChanged);
            bindingNavigator.BindingSource.CurrentChanged += new EventHandler(BindingSource_CurrentChanged);

            ctrlMaSP.HienThiChiTietPhieuNhap(txtMaPhieu.Text, dataGridView);

            if (currentState is ThemState)
            {
                // Khởi tạo phiếu nhập bằng PhieuFactory
                m_PhieuNhap = PhieuFactory.TaoPhieuNhap();
                txtMaPhieu.Text = m_PhieuNhap.Id;
            }


        }



        private void btnAdd_Click(object sender, EventArgs e)
        {


            MaSanPhamController ctrl = new MaSanPhamController();
            MaSanPham masp = ctrl.LayMaSanPham(txtMaSo.Text.Trim());
            if (masp == null)
            {
                foreach (DataGridViewRow view in dataGridView.Rows)
                {
                    if (txtMaSo.Text.Trim().Equals(view.Cells["colMaSanPham"].Value))
                    {
                        MessageBox.Show("Mã sản phẩm này đã tồn tại trong danh sách! Vui lòng nhập lại !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }

                if (txtMaSo.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Vui lòng nhập Mã sản phẩm !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (numGiaNhap.Value <= 0)
                {
                    MessageBox.Show("Vui lòng nhập Đơn giá !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (numSoLuong.Value <= 0)
                {
                    MessageBox.Show("Vui lòng nhập Số lượng !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (dtNgaySanXuat.Value >= dtNgayHetHan.Value)
                {
                    MessageBox.Show("Ngày hết hạn phải lớn hơn ngày sản xuất!", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    numTongTien.Value += numThanhTien.Value;
                    DataRow row = ctrlMaSP.NewRow();
                    row["ID_SAN_PHAM"] = cmbSanPham.SelectedValue;
                    row["ID_PHIEU_NHAP"] = txtMaPhieu.Text;
                    row["ID"] = txtMaSo.Text;
                    row["DON_GIA_NHAP"] = numGiaNhap.Value;
                    row["SO_LUONG"] = numSoLuong.Value;

                    // ==================== [FE-08b] REFACTORING ====================
                    // Lỗi (Code smell) : Wrong Field Assignment – NGAY_NHAP gán nhầm bằng dtNgaySanXuat
                    // Kỹ thuật         : Fix Bug
                    // Trước            : row["NGAY_NHAP"] = dtNgaySanXuat.Value.Date;
                    // Sau              : row["NGAY_NHAP"] = dtNgayNhap.Value.Date;
                    // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
                    // Hỗ trợ           : <Không>
                    // Kiểm tra         : QA1 – <Họ_tên>
                    // Ngày chỉnh sửa   : 04/11/2026
                    // ===========================================================
                    row["NGAY_NHAP"] = dtNgayNhap.Value.Date;

                    row["NGAY_SAN_XUAT"] = dtNgaySanXuat.Value.Date;
                    row["NGAY_HET_HAN"] = dtNgayHetHan.Value.Date;
                    ctrlMaSP.Add(row);

                }
            }
            else
            {
                MessageBox.Show("Mã sản phẩm này đã tồn tại! Vui lòng nhập lại !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void numGiaNhap_ValueChanged(object sender, EventArgs e)
        {
            numThanhTien.Value = numGiaNhap.Value * numSoLuong.Value;
        }



        private void toolLuuThoat_Click(object sender, EventArgs e)
        {
            bindingNavigatorPositionItem.Focus();
            this.Luu();
            SetState(new XemState());
        }

        void Luu()
        {
            if (currentState is ThemState)
            {
                ThemMoi();

            }
            else
            {
                CapNhat();
            }
        }
        void CapNhat()
        {
            ctrlMaSP.Save();

            ctrl.Update();
        }
        // ==================== [R16] REFACTORING ====================
        // Lỗi (Code smell) : Long Method / Missing Validation Guard –  Kiểm tra mã phiếu trước khi thêm dòng; tách hàm nhỏ
        // Kỹ thuật         : Decompose Conditional + Extract Method
        // Trước            : Thêm row vào ctrl trước, kiểm tra mã trùng sau, gom chung logic vào 1 hàm ThemMoi()
        // Sau              : Kiểm tra mã trùng trước (Guard Clause), tách KiemTraHopLePhieuNhap(), CapNhatMaPhieuTuDong(), CapNhatGiaVaTonKho()
        // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Hỗ trợ           : <Không>
        // Kiểm tra         : QA1 – <Họ_tên>
        // Ngày chỉnh sửa   : 04/11/2026
        // ===========================================================
        void ThemMoi()
        {
            // Bước 1: Kiểm tra hợp lệ trước khi làm bất kỳ thao tác nào
            if (!KiemTraHopLePhieuNhap()) return;

            // Bước 2: Khởi tạo dữ liệu phiếu nhập
            DataRow row = ctrl.NewRow();
            row["ID"] = txtMaPhieu.Text.Trim();
            row["NGAY_NHAP"] = dtNgayNhap.Value.Date;
            row["TONG_TIEN"] = numTongTien.Value;
            row["ID_NHA_CUNG_CAP"] = cmbNhaCungCap.SelectedValue;
            row["DA_TRA"] = numDaTra.Value;
            row["CON_NO"] = numConNo.Value;
            ctrl.Add(row);

            // Bước 3: Tăng mã tự động & Lưu dữ liệu
            CapNhatMaPhieuTuDong();
            ctrl.Save();
            ctrlMaSP.Save();

            // Bước 4: Cập nhật tồn kho & giá nhập
            CapNhatGiaVaTonKho();
        }

        private bool KiemTraHopLePhieuNhap()
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            if (string.IsNullOrEmpty(maPhieu))
            {
                MessageBox.Show("Mã phiếu nhập không được để trống!", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaPhieu.Focus();
                return false;
            }

            PhieuNhapController ctrlPN = new PhieuNhapController();
            if (ctrlPN.LayPhieuNhap(maPhieu) != null)
            {
                MessageBox.Show("Mã Phiếu nhập này đã tồn tại trong hệ thống!", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaPhieu.Focus();
                return false;
            }

            return true;
        }

        private void CapNhatMaPhieuTuDong()
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            if (ThamSo.LaSoNguyen(maPhieu))
            {
                long so = Convert.ToInt64(maPhieu);
                if (so >= ThamSo.LayMaPhieuNhap())
                {
                    ThamSo.GanMaPhieuNhap(so + 1);
                }
            }
        }

        private void CapNhatGiaVaTonKho()
        {
            SanPhamController ctrlSP = new SanPhamController();
            foreach (DataGridViewRow view in dataGridView.Rows)
            {
                if (view.IsNewRow || view.Cells["colSanPham"].Value == null) continue;

                string idSanPham = Convert.ToString(view.Cells["colSanPham"].Value);
                long donGia = Convert.ToInt64(view.Cells["colDonGiaNhap"].Value ?? 0);
                long soLuong = Convert.ToInt64(view.Cells["colSoLuong"].Value ?? 0);

                ctrlSP.CapNhatGiaNhap(idSanPham, donGia, soLuong);
            }
        }

        private void toolLuuThem_Click(object sender, EventArgs e)
        {
            ctrl = new PhieuNhapController();

            // Chuyển State sang Thêm
            SetState(new ThemState());

            // Tạo đối tượng qua PhieuFactory
            m_PhieuNhap = PhieuFactory.TaoPhieuNhap();

            txtMaPhieu.Text = m_PhieuNhap.Id;
            dtNgayNhap.Value = m_PhieuNhap.NgayNhap;
            numTongTien.Value = Convert.ToDecimal(m_PhieuNhap.TongTien);
            numDaTra.Value = Convert.ToDecimal(m_PhieuNhap.DaTra);
            numConNo.Value = Convert.ToDecimal(m_PhieuNhap.ConNo);

            ctrlMaSP.HienThiChiTietPhieuNhap(txtMaPhieu.Text, dataGridView);
        }

        private void toolSavePrint_Click(object sender, EventArgs e)
        {
            if (!(currentState is XemState))
            {
                MessageBox.Show("Vui lòng lưu lại Phiếu nhập hiện tại!", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                String ma_phieu = txtMaPhieu.Text;
                PhieuNhapController ctrlPN = new PhieuNhapController();
                CuahangNongduoc.BusinessObject.PhieuNhap ph = ctrlPN.LayPhieuNhap(ma_phieu);
                frmInPhieuNhap PhieuNhap = new frmInPhieuNhap(ph);
                PhieuNhap.Show();
            }
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            if (!(currentState is XemState))
            {
                if (MessageBox.Show("Bạn có muốn lưu lại Phiếu nhập này không?", "Phieu Nhap Hang", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Luu();
                }
            }

            this.Close();
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            frmSanPham SanPham = new frmSanPham();
            SanPham.ShowDialog();
            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
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

         public void Allow(bool val)
        {
            txtMaPhieu.Enabled = val;
            cmbNhaCungCap.Enabled = val;
            dtNgayNhap.Enabled = val;
            numTongTien.Enabled = val;
            numDaTra.Enabled = val;
            numConNo.Enabled = val;
            btnAdd.Enabled = val;
            btnRemove.Enabled = val;
            dataGridView.Enabled = val;
        }

        private void toolChinhsua_Click(object sender, EventArgs e)
        {
            SetState(new SuaState());
        }

        // ==================== [FE-07] REFACTORING ====================
        // Lỗi (Code smell) : Wrong BindingSource – btnRemove ép kiểu sai làm crash & không xóa dòng
        // Kỹ thuật         : Fix Bug
        // Trước            : DataRowView row = (DataRowView)bindingNavigator.BindingSource.Current;
        // Sau              : Lấy CurrentRow từ dataGridView, trừ tiền và gọi dataGridView.Rows.Remove()
        // Tác giả          : FE2 – Nguyễn Huỳnh Ngọc Anh
        // Hỗ trợ           : <Không>
        // Kiểm tra         : QA1 – <Họ_tên>
        // Ngày chỉnh sửa   : 04/11/2026
        // ===========================================================
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null || dataGridView.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa khỏi danh sách nhập!", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn xóa sản phẩm này khỏi danh sách nhập?", "Phieu Nhap", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataGridViewRow currentRow = dataGridView.CurrentRow;

                long donGia = Convert.ToInt64(currentRow.Cells["colDonGiaNhap"].Value ?? 0);
                long soLuong = Convert.ToInt64(currentRow.Cells["colSoLuong"].Value ?? 0);

                numTongTien.Value = Math.Max(0, numTongTien.Value - (donGia * soLuong));

                dataGridView.Rows.Remove(currentRow);
            }
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa Phiếu Nhập này không?", "Phieu Nhap", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                numTongTien.Value -= Convert.ToInt64(e.Row.Cells["colDonGiaNhap"].Value) * Convert.ToInt64(e.Row.Cells["colSoLuong"].Value);
            }
        }

        private void toolXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Phieu Nhap", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();
                ctrl.Save();
            }
        }

        private void numDaTra_ValueChanged(object sender, EventArgs e)
        {
            numConNo.Value = numTongTien.Value - numDaTra.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmNhaCungCap NCC = new frmNhaCungCap();
            NCC.ShowDialog();
            ctrlNCC.HienthiAutoComboBox(cmbNhaCungCap);
        }



    }
}
