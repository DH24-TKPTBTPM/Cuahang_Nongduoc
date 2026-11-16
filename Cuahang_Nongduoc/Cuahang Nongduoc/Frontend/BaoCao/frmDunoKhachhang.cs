using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;


namespace CuahangNongduoc
{
    public partial class frmDunoKhachhang : Form
    {
        public frmDunoKhachhang()
        {
            InitializeComponent();
        }
        DuNoKhachHangController ctrl = new DuNoKhachHangController();
        KhachHangController ctrlKH = new KhachHangController();
        private void frmDunoKhachhang_Load(object sender, EventArgs e)
        {

            this.toolThang.SelectedIndex = DateTime.Now.Month - 1;
            this.toolNam.Text = DateTime.Now.Year.ToString();
            ctrlKH.HienthiKhachHangChungDataGridviewComboBox(colKhachHang);

        }

        private void btnTongHop_Click(object sender, EventArgs e)
        {
            
            
        }

        private void toolNam_Validating(object sender, CancelEventArgs e)
        {
            bool ok = true;
            try
            {
                long nam = Convert.ToInt32(toolNam.Text);
                if (nam < 2000 || nam > 9999)
                {
                    ok = false;
                }
            }
            catch
            {
                ok = false;
            }
            if (!ok)
            {
                MessageBox.Show("Thông tin năm không hợp lệ!", "Tong Hop Du No", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void toolTongHop_Click(object sender, EventArgs e)
        {
            toolProgress.Visible = true;
            ctrl.Tonghop(toolThang.SelectedIndex + 1, Convert.ToInt32(toolNam.Text), toolProgress, dataGridView, bindingNavigator);
            toolProgress.Visible = false;
        }

        // Ô Khách hàng (combobox) gặp mã không có trong danh sách thì bỏ hộp thoại lỗi mặc định của DataGridView.
        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static long LaySoNguyen(object v)
        {
            return (v == null || v == DBNull.Value) ? 0 : Convert.ToInt64(v);
        }

        private void toolIn_Click(object sender, EventArgs e)
        {
            BindingSource bs = bindingNavigator.BindingSource;
            DataRowView row = bs == null ? null : bs.Current as DataRowView;
            if (row == null || row.Row == null || row.Row.RowState == DataRowState.Deleted)
            {
                MessageBox.Show("Vui lòng bấm Tổng hợp và chọn một dòng dư nợ cần in.", "In dư nợ khách hàng",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DuNoKhachHang dn = new DuNoKhachHang();
                dn.Thang = (int)LaySoNguyen(row["THANG"]);
                dn.Nam = (int)LaySoNguyen(row["NAM"]);
                dn.DauKy = LaySoNguyen(row["DAU_KY"]);
                dn.PhatSinh = LaySoNguyen(row["PHAT_SINH"]);
                dn.DaTra = LaySoNguyen(row["DA_TRA"]);
                dn.CuoiKy = LaySoNguyen(row["CUOI_KY"]);
                dn.KhachHang = ctrlKH.LayKhachHang(Convert.ToString(row["ID_KHACH_HANG"]));
                if (dn.KhachHang == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng của dòng đang chọn.", "In dư nợ khách hàng",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                frmInDunoKhachHang InDuNo = new frmInDunoKhachHang(dn);
                InDuNo.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở trang in dư nợ khách hàng: " + ex.Message, "Lỗi in dư nợ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
