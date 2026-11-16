using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmTimPhieuThu : Form
    {
        // Phát ra khi bấm nút Đồng ý
        public event EventHandler TimKiem;

        public frmTimPhieuThu()
        {
            InitializeComponent();
        }

        private void frmTimPhieuThu_Load(object sender, EventArgs e)
        {
            Controller.KhachHangController ctrl = new CuahangNongduoc.Controller.KhachHangController();
            ctrl.HienthiKhachHangVaDaiLyAutoComboBox(cmbKhachHang);

            GanSuKienNutDongY(this);
        }

        // Tìm mọi nút có DialogResult = OK, bỏ việc đóng form, thay bằng sự kiện TimKiem
        private void GanSuKienNutDongY(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                Button btn = c as Button;
                if (btn != null && btn.DialogResult == DialogResult.OK)
                {
                    btn.DialogResult = DialogResult.None;
                    btn.Click += NutDongY_Click;
                }
                if (c.HasChildren)
                {
                    GanSuKienNutDongY(c);
                }
            }
        }

        private void NutDongY_Click(object sender, EventArgs e)
        {
            if (TimKiem != null)
            {
                TimKiem(this, EventArgs.Empty);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
