using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.Controller;

namespace CuahangNongduoc
{
    public partial class frmDanhsachPhieuNhap : Form
    {
        public frmDanhsachPhieuNhap()
        {
            InitializeComponent();
        }

        PhieuNhapController ctrl = new PhieuNhapController();
        NhaCungCapController ctrlNCC = new NhaCungCapController();

        private void frmDanhsachPhieuNhap_Load(object sender, EventArgs e)
        {
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            ctrlNCC.HienthiDataGridviewComboBox(colNhaCungCap);
            ctrl.HienthiPhieuNhap(bindingNavigator, dataGridView);
        }

        frmNhapHang NhapHang = null;

        // Tải lại danh sách từ CSDL và chọn lại phiếu cũ (nếu còn).
        private void TaiLaiDanhSach(string maPhieuCanChon)
        {
            ctrl.HienthiPhieuNhap(bindingNavigator, dataGridView);
            if (!String.IsNullOrEmpty(maPhieuCanChon))
                ctrl.ChonPhieuNhap(maPhieuCanChon);
        }

        // Form nhập hàng đóng lại (đã lưu/xóa/sửa) thì làm mới danh sách để thấy dữ liệu mới.
        private void NhapHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            TaiLaiDanhSach(ctrl.LayMaPhieuHienTai());
        }

        // them = true: phiếu mới; chinhSua = true: mở phiếu đang chọn và vào chế độ chỉnh sửa ngay.
        private void MoFormNhapHang(bool them, bool chinhSua)
        {
            if (NhapHang != null && !NhapHang.IsDisposed)
            {
                NhapHang.Activate();
                return;
            }

            if (them)
            {
                NhapHang = new frmNhapHang();
            }
            else
            {
                string maPhieu = ctrl.LayMaPhieuHienTai();
                if (String.IsNullOrEmpty(maPhieu))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập trong danh sách.", "Phiếu nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                NhapHang = new frmNhapHang(ctrl, chinhSua);
            }

            NhapHang.FormClosed += new FormClosedEventHandler(NhapHang_FormClosed);
            NhapHang.Show();
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            // Chỉ mở khi nhấp đúp vào một dòng (không mở khi nhấp đúp vào tiêu đề cột, thanh cuộn, vùng trống).
            Point p = dataGridView.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hit = dataGridView.HitTest(p.X, p.Y);
            if (hit.Type != DataGridViewHitTestType.Cell && hit.Type != DataGridViewHitTestType.RowHeader)
                return;
            MoFormNhapHang(false, false);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            MoFormNhapHang(true, false);
        }

        // Nút Chỉnh sửa ngay trên danh sách: mở phiếu đang chọn ở chế độ chỉnh sửa.
        private void toolChinhSua_Click(object sender, EventArgs e)
        {
            MoFormNhapHang(false, true);
        }

        private void toolIn_Click(object sender, EventArgs e)
        {
            DataRowView row = bindingNavigator.BindingSource == null
                ? null : bindingNavigator.BindingSource.Current as DataRowView;
            if (row == null || row.Row == null || row.Row.RowState == DataRowState.Deleted ||
                row["ID"] == DBNull.Value || String.IsNullOrEmpty(Convert.ToString(row["ID"]).Trim()))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần in.", "In phiếu nhập",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string maPhieu = Convert.ToString(row["ID"]);
                PhieuNhapController ctrlPN = new PhieuNhapController();
                CuahangNongduoc.BusinessObject.PhieuNhap ph = ctrlPN.LayPhieuNhap(maPhieu);
                if (ph == null)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu phiếu nhập để in.", "In phiếu nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                frmInPhieuNhap formIn = new frmInPhieuNhap(ph);
                formIn.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở trang in phiếu nhập: " + ex.Message, "Lỗi in phiếu nhập",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            BindingSource bs = bindingNavigator.BindingSource;
            DataRowView row = bs == null ? null : bs.Current as DataRowView;
            if (row == null || row.Row == null || row.Row.RowState == DataRowState.Deleted ||
                row["ID"] == DBNull.Value)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa.", "Xóa phiếu nhập",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string idPhieu = Convert.ToString(row["ID"]);
            int viTriCu = bs.Position;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu nhập " + idPhieu + "?",
                "Xóa phiếu nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                MaSanPhamController ctrlLo = new MaSanPhamController();
                if (ctrlLo.CoLoDaBanTrongPhieuNhap(idPhieu))
                {
                    MessageBox.Show("Không thể xóa phiếu " + idPhieu + " vì có lô đã được sử dụng trong hóa đơn bán.",
                        "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataTable bangSanPham = ctrlLo.DanhSachSanPhamTheoPhieuNhap(idPhieu);
                List<string> idsSanPham = new List<string>();
                foreach (DataRow sp in bangSanPham.Rows)
                {
                    string idSanPham = Convert.ToString(sp["ID_SAN_PHAM"]);
                    if (!String.IsNullOrEmpty(idSanPham) && !idsSanPham.Contains(idSanPham))
                        idsSanPham.Add(idSanPham);
                }

                // Xóa các lô + phiếu và tính lại tồn kho trong cùng một transaction.
                int affected = ctrl.XoaPhieuNhapVaCacLo(idPhieu, idsSanPham);
                if (affected <= 0)
                    throw new InvalidOperationException("Không có dữ liệu phiếu nhập nào được xóa.");

                TaiLaiDanhSach(null);
                ctrlNCC.HienthiDataGridviewComboBox(colNhaCungCap);
                BindingSource bsMoi = bindingNavigator.BindingSource;
                if (bsMoi != null && bsMoi.Count > 0)
                    bsMoi.Position = Math.Min(viTriCu, bsMoi.Count - 1);

                MessageBox.Show("Đã xóa phiếu nhập " + idPhieu + " và đồng bộ tồn kho.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa phiếu nhập " + idPhieu + ": " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolTimKiem_Click(object sender, EventArgs e)
        {
            frmTimPhieuNhap TimPhieu = new frmTimPhieuNhap();
            Point p = PointToScreen(toolTimKiem.Bounds.Location);
            p.X += toolTimKiem.Width;
            p.Y += toolTimKiem.Height;
            TimPhieu.Location = p;
            TimPhieu.ShowDialog();
            if (TimPhieu.DialogResult == DialogResult.OK)
            {
                if (TimPhieu.cmbNCC.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp để tìm kiếm.", "Tìm kiếm",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ctrl.TimPhieuNhap(TimPhieu.cmbNCC.SelectedValue.ToString(), TimPhieu.dtNgayNhap.Value.Date);
            }
        }
    }
}
