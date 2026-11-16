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
    public partial class frmThanhToan : Form
    {
        KhachHangController ctrlKH = new KhachHangController();
        PhieuThanhToanController ctrl = new PhieuThanhToanController();
        frmTimPhieuThu m_Tim = null;
        bool m_DangDong = false;

        public frmThanhToan()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            m_DangDong = true;
            base.OnFormClosing(e);
        }

        private void frmThanhToan_Load(object sender, EventArgs e)
        {
            ctrlKH.HienthiKhachHangVaDaiLyAutoComboBox(cmbKhachHang);
            ctrlKH.HienthiKhachHangVaDaiLyDataGridviewComboBox(colKhachHang);
            ctrl.HienthiPhieuThanhToan(bindingNavigator, dataGridView, cmbKhachHang, txtMaPhieu, dtNgayThanhToan, numTongTien, txtGhiChu);
            bindingNavigator.BindingSource.AddingNew += new AddingNewEventHandler(BindingSource_AddingNew);
        }

        void BindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {

        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            long maphieu = ThamSo.LayMaPhieuThanhToan();
            ThamSo.GanMaPhieuThanhToan(maphieu + 1);

            DataRow row = ctrl.NewRow();
            row["ID"] = maphieu;
            row["NGAY_THANH_TOAN"] = DateTime.Now.Date;
            row["TONG_TIEN"] = numTongTien.Value;
            ctrl.Add(row);
            bindingNavigator.BindingSource.MoveLast();
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa phiếu thanh toán này không?", "Phieu Thanh Toan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void toolDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn xóa phiếu thanh toán này không?", "Phieu Thanh Toan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingNavigator.BindingSource.RemoveCurrent();
                ctrl.Save();
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            txtMaPhieu.Focus();
            bindingNavigator.BindingSource.MoveNext();
            ctrl.Save();
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void toolIn_Click(object sender, EventArgs e)
        {
            // Lưu trước để phiếu mới có trong database
            try
            {
                bindingNavigator.BindingSource.EndEdit();
                ctrl.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không lưu được phiếu thanh toán");
                return;
            }

            if (bindingNavigator.BindingSource.Current == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu thanh toán cần in!");
                return;
            }

            DataRowView row = bindingNavigator.BindingSource.Current as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Không lấy được dữ liệu phiếu thanh toán!");
                return;
            }

            string ma_phieu = row["ID"].ToString();

            PhieuThanhToanController ctrlTT = new PhieuThanhToanController();
            CuahangNongduoc.BusinessObject.PhieuThanhToan ph = ctrlTT.LayPhieuThanhToan(ma_phieu);

            if (ph == null)
            {
                MessageBox.Show("Không tìm thấy phiếu thanh toán!");
                return;
            }

            frmInPhieuThanhToan formIn = new frmInPhieuThanhToan(ph);
            formIn.ShowDialog();
        }

        // Bấm nút Tìm kiếm trên thanh công cụ: mở form tìm (không modal)
        private void toolTimKiem_Click(object sender, EventArgs e)
        {
            // Đang mở rồi thì chỉ đưa lên trước, không mở thêm cái mới
            if (m_Tim != null && !m_Tim.IsDisposed)
            {
                m_Tim.Activate();
                return;
            }

            m_Tim = new frmTimPhieuThu();

            Point p = PointToScreen(toolTimKiem.Bounds.Location);
            p.X += toolTimKiem.Width;
            p.Y += toolTimKiem.Height;
            m_Tim.StartPosition = FormStartPosition.Manual;
            m_Tim.Location = p;

            m_Tim.TimKiem += Tim_TimKiem;
            m_Tim.FormClosed += Tim_FormClosed;

            m_Tim.Show(this);
        }

        // Bấm Đồng ý trong form tìm: lọc dữ liệu, form tìm vẫn mở
        private void Tim_TimKiem(object sender, EventArgs e)
        {
            frmTimPhieuThu tim = (frmTimPhieuThu)sender;

            if (tim.cmbKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần tìm!");
                return;
            }

            ctrl.TimPhieuThanhToan(bindingNavigator, dataGridView, cmbKhachHang, txtMaPhieu,
                dtNgayThanhToan, numTongTien, txtGhiChu,
                tim.cmbKhachHang.SelectedValue.ToString(), tim.dtNgayThu.Value.Date);
        }

        // Tắt form tìm kiếm: trở lại bảng bình thường
        private void Tim_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Tim = null;

            // Form thanh toán đang đóng thì không nạp lại nữa
            if (m_DangDong) return;

            try
            {
                bindingNavigator.BindingSource.EndEdit();
                ctrl.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi lưu phiếu thanh toán");
            }

            ctrl.HienthiPhieuThanhToan(bindingNavigator, dataGridView, cmbKhachHang, txtMaPhieu,
                dtNgayThanhToan, numTongTien, txtGhiChu);
        }
    }
}
