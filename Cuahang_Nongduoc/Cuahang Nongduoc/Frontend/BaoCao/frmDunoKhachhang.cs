using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CuahangNongduoc.BusinessObject;
using CuahangNongduoc.Controller;
using CuahangNongduoc.DataLayer; // FE3-[FE-CN] Dùng Factory trực tiếp cho Form tự binding


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
        DuNoKhachHangFactory factory = new DuNoKhachHangFactory(); // FE3-[FE-CN] Khởi tạo Factory
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
        // Kỹ thuật         : Move Method (Chuyển logic hiển thị tiến trình và binding lưới về Form giao diện)
        // Trước            : ctrl.Tonghop(toolThang.SelectedIndex + 1, Convert.ToInt32(toolNam.Text), toolProgress, dataGridView, bindingNavigator);
        // Sau              : TongHopDuNo(toolThang.SelectedIndex + 1, Convert.ToInt32(toolNam.Text));
        // Tác giả          : FE3 – Nguyễn Thị Ngân Huệ
        // Hỗ trợ           : BE2 (Task R24)
        // Kiểm tra         : QA1 – Huỳnh Lư Anh Minh
        // Ngày chỉnh sửa   : 08/11/2026
        // =============================================================
        private void TongHopDuNo(int thang, int nam)
        {
            int ThangTruoc = 0, NamTruoc = 0;
            ThamSo.PreMonth(ref ThangTruoc, ref NamTruoc, thang, nam);
            factory.Clear(thang, nam);

            BindingSource bs = new BindingSource();
            bs.DataSource = factory.LayDuNoKhachHang("-1", 0, 0);
            bindingNavigator.BindingSource = bs;
            dataGridView.DataSource = bs;

            KhachHangFactory factoryKH = new KhachHangFactory();
            DataTable tbl = factoryKH.DanhsachKhachHang();
            long dauky = 0, phatsinh = 0, datra = 0, cuoiky = 0;

            toolProgress.Minimum = 0;
            toolProgress.Value = 0;
            toolProgress.Maximum = tbl.Rows.Count;

            foreach (DataRow row in tbl.Rows)
            {
                DataRow r = factory.NewRow();
                String kh = Convert.ToString(row["ID"]);

                dauky = DuNoKhachHangFactory.LayDuNo(kh, ThangTruoc, NamTruoc);
                phatsinh = PhieuBanFactory.LayConNo(kh, thang, nam);
                datra = PhieuThanhToanFactory.LayTongTien(kh, thang, nam);
                cuoiky = (dauky + phatsinh - datra);

                r["ID_KHACH_HANG"] = kh;
                r["THANG"] = thang;
                r["NAM"] = nam;
                r["DAU_KY"] = dauky;
                r["PHAT_SINH"] = phatsinh;
                r["DA_TRA"] = datra;
                r["CUOI_KY"] = cuoiky;

                factory.Add(r);
                toolProgress.Value++;
            }
        }
        private void toolTongHop_Click(object sender, EventArgs e)
        {
            toolProgress.Visible = true;
            DataTable data = ctrl.Tonghop(toolThang.SelectedIndex + 1, Convert.ToInt32(toolNam.Text));
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
            //ctrl.Save();
            factory.Save(); // FE3-[FE-CN] Thay cho ctrl.Save()
        }

        private void toolThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolIn_Click(object sender, EventArgs e)
        {
            // Thêm dòng kiểm tra an toàn này
            if (bindingNavigator.BindingSource == null || bindingNavigator.BindingSource.Current == null)
            {
                MessageBox.Show("Vui lòng tổng hợp dữ liệu trước khi in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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