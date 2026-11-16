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
            colKhachHang.DataSource = ctrlKH.HienthiKhachHangChungDataGridviewComboBox();
            colKhachHang.DisplayMember = "HO_TEN";
            colKhachHang.ValueMember = "ID";
            colKhachHang.DataPropertyName = "ID_KHACH_HANG";
            colKhachHang.HeaderText = "Khách hàng";

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
                // ==================== [FE-11] REFACTORING ====================
                // Lỗi (Code smell) : Sai tiêu đề thông báo MessageBox (không dấu)
                // Kỹ thuật         : Fix Bug
                // Trước            : MessageBox.Show("Thông tin năm không hợp lệ!", "Tong Hop Du No", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Sau              : MessageBox.Show("Thông tin năm không hợp lệ!", "Tổng hợp dư nợ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
                // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
                // Ngày chỉnh sửa   : 03/11/2026
                // Ngày kiểm thử    : 07/11/2026
                // ============================================================
                MessageBox.Show("Thông tin năm không hợp lệ!", "Tổng hợp dư nợ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        // ==================== [FE-CN] REFACTORING ====================
        // Lỗi (Code smell) : Controller nhận trực tiếp các control UI (ToolStripProgressBar, DataGridView, BindingNavigator)
        // Kỹ thuật         : Move Method (Chuyển logic hiển thị tiến trình và binding lưới về Form giao diện, gọi qua DuNoKhachHangController)
        // Trước            : ctrl.Tonghop(toolThang.SelectedIndex + 1, Convert.ToInt32(toolNam.Text), toolProgress, dataGridView, bindingNavigator);
        // Sau              : DataTable data = ctrl.Tonghop(toolThang.SelectedIndex + 1, Convert.ToInt32(toolNam.Text));
        // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
        // Hỗ trợ           : BE3 (Task R24)
        // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa   : 15/11/2026
        // =============================================================
        
        private void toolTongHop_Click(object sender, EventArgs e)
        {
            toolProgress.Visible = true;
            DataTable data = ctrl.Tonghop(toolThang.SelectedIndex + 1, Convert.ToInt32(toolNam.Text)); // FE3-[FE-CN] Gọi tổng hợp qua DuNoKhachHangController
            BindingSource bs = new BindingSource();
            bs.DataSource = data;
            bindingNavigator.BindingSource = bs;
            dataGridView.DataSource = bs;
            toolProgress.Maximum = data.Rows.Count;
            toolProgress.Value = data.Rows.Count;
            toolProgress.Visible = false;
        }
        private void toolLuu_Click(object sender, EventArgs e)
        {
            toolThang.Focus();
            ctrl.Save();

        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolIn_Click(object sender, EventArgs e)
        {
            // ==================== [FE-BC] REFACTORING ====================
            // Lỗi (Code smell) : Bấm nút In khi chưa tổng hợp dữ liệu gây lỗi NullReferenceException
            // Kỹ thuật         : Introduce Guard Clause (Kiểm tra điều kiện biên trước khi in)
            // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
            // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
            // Ngày chỉnh sửa   : 12/11/2026
            // =============================================================
            if (bindingNavigator.BindingSource == null || bindingNavigator.BindingSource.Current == null)
            {
                MessageBox.Show("Vui lòng tổng hợp dư nợ trước khi in!", "Tổng hợp dư nợ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRowView row = (DataRowView)bindingNavigator.BindingSource.Current;
            KhachHangController ctrlKH = new KhachHangController();
            DuNoKhachHang dn = new DuNoKhachHang();

            dn.Thang = Convert.ToInt32(row["THANG"]);
            dn.Nam = Convert.ToInt32(row["NAM"]);
            dn.DauKy = Convert.ToInt64(row["DAU_KY"]);
            dn.PhatSinh = Convert.ToInt64(row["PHAT_SINH"]);
            dn.DaTra = Convert.ToInt64(row["DA_TRA"]);
            dn.CuoiKy = Convert.ToInt64(row["CUOI_KY"]);
            dn.KhachHang = ctrlKH.LayKhachHang(Convert.ToString(row["ID_KHACH_HANG"]));

            frmInDunoKhachHang InDuNo = new frmInDunoKhachHang(dn);
            InDuNo.Show();
        }
    }
}