using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.Controller;
using CuahangNongduoc.BusinessObject;

namespace CuahangNongduoc
{
    public partial class frmBanLe: Form
    {
        SanPhamController ctrlSanPham = new SanPhamController();
        KhachHangController ctrlKhachHang = new KhachHangController();
        MaSanPhamController ctrlMaSanPham = new MaSanPhamController();
        PhieuBanController ctrlPhieuBan = new PhieuBanController();
        ChiTietPhieuBanController ctrlChiTiet = new ChiTietPhieuBanController();
        IList<MaSanPham> deleted = new List<MaSanPham>();
        Controll status = Controll.Normal;

        public frmBanLe()
        {
            InitializeComponent();
            dgvChitiet.CellValueChanged += new DataGridViewCellEventHandler(dgvChitiet_CellValueChanged);
            dgvChitiet.CellEndEdit += new DataGridViewCellEventHandler(dgvChitiet_CellEndEdit);
            status = Controll.AddNew;
            // ==================== [R18] REFACTORING ====================
            // Lỗi (Code smell) : Không làm sạch danh sách deleted khi chuyển sang trạng thái Thêm mới
            // Kỹ thuật         : Separate Query from Modifier
            // Trước            : Không có deleted.Clear() khi Thêm mới
            // Sau              : Gọi deleted.Clear() khi bắt đầu Thêm mới phiếu bán
            // Tác giả          : FE1 – Dương Thị Kim Anh
            // Hỗ trợ           : BE2 – Trần Tuấn Anh
            // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
            // Ngày chỉnh sửa   : 04/11/2026
            // ===========================================================
            deleted.Clear();
        }


     
        public frmBanLe(PhieuBanController ctrlPB)
            : this()
        {
            this.ctrlPhieuBan = ctrlPB;
            status = Controll.Normal;
        }

        private void frmNhapHang_Load(object sender, EventArgs e)
        {

            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
            ctrlMaSanPham.HienThiDataGridViewComboBox(colMaSanPham);

            cmbSanPham.SelectedIndexChanged += new EventHandler(cmbSanPham_SelectedIndexChanged);

            ctrlKhachHang.HienthiAutoComboBox(cmbKhachHang, false);

            ctrlPhieuBan.HienthiPhieuBan(bindingNavigator,cmbKhachHang, txtMaPhieu, dtNgayLapPhieu, numTongTien, numDaTra, numConNo);

            bindingNavigator.BindingSource.CurrentChanged -= new EventHandler(BindingSource_CurrentChanged);
            bindingNavigator.BindingSource.CurrentChanged += new EventHandler(BindingSource_CurrentChanged);
            
            ctrlChiTiet.HienThiChiTiet(dgvDanhsachSP, txtMaPhieu.Text);
            // ==================== [R18] REFACTORING ====================
            // Lỗi (Code smell) : Uncleared Temporary Data – Không làm sạch DataTable chi tiết khi tạo phiếu mới
            // Kỹ thuật         : Separate Query from Modifier
            // Trước            : Chỉ gọi ctrlChiTiet.HienThiChiTiet(...)
            // Sau              : Gọi Clear() trên DataTable của BindingSource khi ở trạng thái ThemMoi
            // Tác giả          : FE1 – Dương Thị Kim Anh
            // Hỗ trợ           : BE2 – Trần Tuấn Anh
            // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
            // Ngày chỉnh sửa   : 04/11/2026
            // ===========================================================
            if (bindingNavigatorChitiet.BindingSource.DataSource is DataTable dtChiTiet)
            {
                dtChiTiet.Clear();
            }

            if (status == Controll.AddNew)
            {
                txtMaPhieu.Text = ThamSo.LayMaPhieuBan().ToString();
            }
            else
            {
                this.Allow(false);
            }


        }

        void BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (status == Controll.Normal)
            {
                ctrlChiTiet.HienThiChiTiet(dgvDanhsachSP, txtMaPhieu.Text);
            }
        }


        void cmbSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedValue != null)
            {
                MaSanPhamController ctrlMSP = new MaSanPhamController();

                cmbMaSanPham.SelectedIndexChanged -= new EventHandler(cmbMaSanPham_SelectedIndexChanged);
                ctrlMSP.HienThiAutoComboBox(cmbSanPham.SelectedValue.ToString(), cmbMaSanPham);
                cmbMaSanPham.SelectedIndexChanged += new EventHandler(cmbMaSanPham_SelectedIndexChanged);
            }
        }

        void cmbMaSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaSanPhamController ctrl = new MaSanPhamController();
            MaSanPham masp = ctrl.LayMaSanPham(cmbMaSanPham.SelectedValue.ToString());
            numDonGia.Value = masp.SanPham.GiaBanLe;
            txtGiaNhap.Text = masp.GiaNhap.ToString("#,###0");
            txtGiaBanSi.Text = masp.SanPham.GiaBanSi.ToString("#,###0");
            txtGiaBanLe.Text = masp.SanPham.GiaBanLe.ToString("#,###0");
            txtGiaBQGQ.Text = masp.SanPham.DonGiaNhap.ToString("#,###0");

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            if (cmbMaSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Mã sản phẩm !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (numSoLuong.Value <= 0)
            {
                MessageBox.Show("Vui lòng nhập Số lượng !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (numDonGia.Value * numSoLuong.Value != numThanhTien.Value)
            {
                MessageBox.Show("Thành tiền sai!", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                numTongTien.Value += numThanhTien.Value;
                DataRow row = ctrlChiTiet.NewRow();
                row["ID_MA_SAN_PHAM"] = cmbMaSanPham.SelectedValue;
                row["ID_PHIEU_BAN"] = txtMaPhieu.Text;
                row["DON_GIA"] = numDonGia.Value;
                row["SO_LUONG"] = numSoLuong.Value;
                row["THANH_TIEN"] = numThanhTien.Value;
                ctrlChiTiet.Add(row);

            }

        }

        private void numDonGia_ValueChanged(object sender, EventArgs e)
        {
            numThanhTien.Value = numDonGia.Value * numSoLuong.Value;
        }

        private void numTongTien_ValueChanged(object sender, EventArgs e)
        {
            numConNo.Value = numTongTien.Value - numDaTra.Value;
        }

        private void toolLuu_Click(object sender, EventArgs e)
        {
            bindingNavigatorPositionItem.Focus();
            this.Luu();
            status = Controll.Normal;
            this.Allow(false);
        }

        void Luu()
        {
            if (status == Controll.AddNew)
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
            foreach (MaSanPham masp in deleted)
            {
                CuahangNongduoc.DataLayer.MaSanPhanFactory.CapNhatSoLuong(masp.Id, masp.SoLuong);
            }
            deleted.Clear();

            ctrlChiTiet.Save();

            ctrlPhieuBan.Update();

        }
        void ThemMoi()
        {
            deleted.Clear();
            DataRow row = ctrlPhieuBan.NewRow();
            row["ID"] = txtMaPhieu.Text;
            row["ID_KHACH_HANG"] = cmbKhachHang.SelectedValue;
            row["NGAY_BAN"] = dtNgayLapPhieu.Value.Date;
            row["TONG_TIEN"] = numTongTien.Value;
            row["DA_TRA"] = numDaTra.Value;
            row["CON_NO"] = numConNo.Value;
            ctrlPhieuBan.Add(row);

            PhieuBanController ctrl = new PhieuBanController();

            if (ctrl.LayPhieuBan(txtMaPhieu.Text) != null)
            {
                MessageBox.Show("Mã Phiếu bán này đã tồn tại !", "Phieu Nhap", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ThamSo.LaSoNguyen(txtMaPhieu.Text))
            {
                long so = Convert.ToInt64(txtMaPhieu.Text);
                if (so >= ThamSo.LayMaPhieuBan())
                {
                    ThamSo.GanMaPhieuBan(so + 1);
                }
            }

            ctrlPhieuBan.Save();

            ctrlChiTiet.Save();

        }

        private void toolLuu_Them_Click(object sender, EventArgs e)
        {
            ctrlPhieuBan = new PhieuBanController();
            status = Controll.AddNew;
            deleted.Clear();
            txtMaPhieu.Text = ThamSo.LayMaPhieuBan().ToString();
            numTongTien.Value = 0;
            ctrlChiTiet.HienThiChiTiet(dgvDanhsachSP, txtMaPhieu.Text);
            this.Allow(true);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (bindingNavigatorChitiet.BindingSource.Current == null)
                return;

            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Bán Lẻ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ==================== [R18] REFACTORING ====================
                // Lỗi (Code smell) : Side Effect on Uncommitted Data – Xóa dòng khi đang Thêm mới vẫn ghi nhận xóa/hoàn kho như đang Sửa
                // Kỹ thuật         : Separate Query from Modifier
                // Trước            : BindingSource bs = bindingNavigatorChitiet.BindingSource;
                //                    DataRowView row = (DataRowView)bs.Current;
                //                    numTongTien.Value -= Convert.ToInt64(row["THANH_TIEN"]);
                //                    bs.RemoveCurrent();
                // Sau              : Phân biệt rõ: khi ThemMoi (AddNew) chỉ xóa hẳn dòng tạm khỏi bảng (AcceptChanges); khi Edit mới giữ trạng thái Deleted để cập nhật kho lúc Lưu
                // Tác giả          : FE1 – Dương Thị Kim Anh
                // Hỗ trợ           : BE2 – Trần Tuấn Anh
                // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
                // Ngày chỉnh sửa   : 04/11/2026
                // ===========================================================
                BindingSource bs = bindingNavigatorChitiet.BindingSource;
                DataRowView row = (DataRowView)bs.Current;
                long thanhTienXoa = Convert.ToInt64(row["THANH_TIEN"]);

                if (status == Controll.AddNew)
                {
                    // Đang Thêm mới: Phiếu chưa lưu xuống DB -> xóa dòng tạm và hủy trạng thái Deleted để không bị hoàn kho nhầm
                    DataRow dataRow = row.Row;
                    bs.RemoveCurrent();
                    if (dataRow.RowState == DataRowState.Deleted)
                    {
                        dataRow.AcceptChanges();
                    }
                }
                else if (status == Controll.Edit)
                {
                    // Đang Chỉnh sửa phiếu cũ:
                    // Nếu dòng vừa mới bấm "+" thêm vào trong lúc sửa (Added) thì xóa hẳn;
                    // Nếu dòng cũ đã có trong DB (Unchanged/Modified) thì giữ RowState = Deleted để hoàn kho khi Lưu
                    DataRow dataRow = row.Row;
                    bool isNewlyAdded = (dataRow.RowState == DataRowState.Added);
                    bs.RemoveCurrent();
                    if (isNewlyAdded && dataRow.RowState == DataRowState.Deleted)
                    {
                        dataRow.AcceptChanges();
                    }
                }

                numTongTien.Value = Math.Max(0, numTongTien.Value - thanhTienXoa);
            }
        }

        private void dgvDanhsachSP_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Bán Lẻ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BindingSource bs = bindingNavigatorChitiet.BindingSource;
                DataRowView row = (DataRowView)bs.Current;
                numTongTien.Value -= Convert.ToInt64(row["THANH_TIEN"]);

                // ==================== [R18] REFACTORING ====================
                // Lỗi (Code smell) : Xóa dòng khi đang Thêm mới vẫn đưa vào danh sách deleted để hoàn kho
                // Kỹ thuật         : Separate Query from Modifier
                // Trước            : deleted.Add(new MaSanPham(Convert.ToString(row["ID_MA_SAN_PHAM"]), Convert.ToInt32(row["SO_LUONG"])));
                // Sau              : if (status == Controll.Edit) { deleted.Add(...); }
                // Tác giả          : FE1 – Dương Thị Kim Anh
                // Hỗ trợ           : BE2 – Trần Tuấn Anh
                // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
                // Ngày chỉnh sửa   : 04/11/2026
                // ===========================================================
                if (status == Controll.Edit)
                {
                    deleted.Add(new MaSanPham(Convert.ToString(row["ID_MA_SAN_PHAM"]), Convert.ToInt32(row["SO_LUONG"])));
                }

                bs.RemoveCurrent();
            }
        }

        private void toolLuuIn_Click(object sender, EventArgs e)
        {
            if (status != Controll.Normal)
            {
                MessageBox.Show("Vui lòng lưu lại Phiếu bán hiện tại!", "Phieu Ban Le", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                String ma_phieu = txtMaPhieu.Text;

                PhieuBanController ctrlPB = new PhieuBanController();

                CuahangNongduoc.BusinessObject.PhieuBan ph = ctrlPB.LayPhieuBan(ma_phieu);

                frmInPhieuBan InPhieuBan = new frmInPhieuBan(ph);

                InPhieuBan.Show();

            }
        }

        private void dgvDanhsachSP_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }

        private void toolChinhSua_Click(object sender, EventArgs e)
        {
            status = Controll.Edit;
            this.Allow(true);
        }

        void Allow(bool val)
        {
            txtMaPhieu.Enabled = val;
            dtNgayLapPhieu.Enabled = val;
            numTongTien.Enabled = val;
            btnAdd.Enabled = val;
            btnRemove.Enabled = val;
            dgvDanhsachSP.Enabled = val;
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            if (status != Controll.Normal)
            {
                if (MessageBox.Show("Bạn có muốn lưu lại Phiếu bán này không?", "Phieu Ban Le", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Luu();
                }
            }
            this.Close();
        }

        private void toolXoa_Click(object sender, EventArgs e)
        {
            DataRowView view =  (DataRowView)bindingNavigator.BindingSource.Current;
            if (view != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn xóa không?", "Phieu Ban Le", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ChiTietPhieuBanController ctrl = new ChiTietPhieuBanController();
                    IList<ChiTietPhieuBan> ds = ctrl.ChiTietPhieuBan(view["ID"].ToString());
                    foreach (ChiTietPhieuBan ct in ds)
                    {
                        CuahangNongduoc.DataLayer.MaSanPhanFactory.CapNhatSoLuong(ct.MaSanPham.Id, ct.SoLuong);
                    }
                    bindingNavigator.BindingSource.RemoveCurrent();
                    ctrlPhieuBan.Save();
                }
            }
        }

        private void btnThemDaiLy_Click(object sender, EventArgs e)
        {
            frmKhachHang KhachHang = new frmKhachHang();
            KhachHang.ShowDialog();
            ctrlKhachHang.HienthiAutoComboBox(cmbKhachHang, false);
        }

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            frmSanPham SanPham = new frmSanPham();
            SanPham.ShowDialog();
            ctrlSanPham.HienthiAutoComboBox(cmbSanPham);
        }
        // ==================== [FE-08a] REFACTORING ====================
        // Lỗi (Code smell) : Missing Real-time Calculation – Thay đổi số lượng trên lưới không tự động cập nhật Thành tiền và Tổng tiền
        // Kỹ thuật         : Fix Bug / Event-driven Update
        // Before           : Người dùng đổi số lượng nhưng Thành tiền và Tổng tiền không tự động thay đổi
        // After            : Bắt sự kiện CellValueChanged trên DataGridView để tự tính lại Thành tiền = SoLuong * DonGia và cập nhật Tổng tiền
        // Tác giả          : FE1 – Dương Thị Kim Anh
        // Hỗ trợ           : BE2 – Trần Tuấn Anh
        // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa   : 04/11/2026
        // =============================================================
        private void dgvChitiet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu đổi giá trị ở cột "Số lượng" (DataPropertyName là "SO_LUONG")
            if (e.RowIndex >= 0 && dgvChitiet.Columns[e.ColumnIndex].DataPropertyName == "SO_LUONG")
            {
                DataGridViewRow row = dgvChitiet.Rows[e.RowIndex];

                int soLuong = 0;
                long donGiarows = 0;

                if (row.Cells["SO_LUONG"].Value != null && row.Cells["SO_LUONG"].Value != DBNull.Value)
                {
                    int.TryParse(row.Cells["SO_LUONG"].Value.ToString(), out soLuong);
                }

                if (row.Cells["DON_GIA"].Value != null && row.Cells["DON_GIA"].Value != DBNull.Value)
                {
                    long.TryParse(row.Cells["DON_GIA"].Value.ToString(), out donGiarows);
                }

                // Tính lại Thành tiền mới cho dòng đó
                long thanhTienMoi = (long)soLuong * donGiarows;
                row.Cells["THANH_TIEN"].Value = thanhTienMoi;

                // Gọi hàm tính lại tổng tiền hóa đơn của form (tên hàm có sẵn trong đồ án là TinhTongTien())
                if (this.GetType().GetMethod("TinhTongTien", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) != null)
                
            }
        }

        private void dgvChitiet_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvChitiet.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

    }
}
