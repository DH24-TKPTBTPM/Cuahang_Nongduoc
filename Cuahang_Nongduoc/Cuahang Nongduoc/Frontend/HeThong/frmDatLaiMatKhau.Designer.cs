namespace CuahangNongduoc
{
    partial class frmDatLaiMatKhau
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitleHead = new System.Windows.Forms.Label();
            this.lblTaiKhoan = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblMatKhauMoi = new System.Windows.Forms.Label();
            this.txtMatKhauMoi = new System.Windows.Forms.TextBox();
            this.lblXacNhan = new System.Windows.Forms.Label();
            this.txtXacNhan = new System.Windows.Forms.TextBox();
            this.chkHienThi = new System.Windows.Forms.CheckBox();
            this.btnDatLai = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(180, 130, 0);
            this.panelTop.Controls.Add(this.lblTitleHead);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 55;
            this.panelTop.Name = "panelTop";

            // lblTitleHead
            this.lblTitleHead.AutoSize = false;
            this.lblTitleHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitleHead.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitleHead.ForeColor = System.Drawing.Color.White;
            this.lblTitleHead.Name = "lblTitleHead";
            this.lblTitleHead.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblTitleHead.Text = "🔑  ĐẶT LẠI MẬT KHẨU";
            this.lblTitleHead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // lblTaiKhoan
            this.lblTaiKhoan.AutoSize = false;
            this.lblTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTaiKhoan.ForeColor = System.Drawing.Color.FromArgb(30, 80, 50);
            this.lblTaiKhoan.Location = new System.Drawing.Point(20, 70);
            this.lblTaiKhoan.Name = "lblTaiKhoan";
            this.lblTaiKhoan.Size = new System.Drawing.Size(360, 25);
            this.lblTaiKhoan.Text = "Tài khoản: ...";

            // lblNote
            this.lblNote.AutoSize = false;
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblNote.ForeColor = System.Drawing.Color.FromArgb(120, 80, 0);
            this.lblNote.Location = new System.Drawing.Point(20, 95);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(360, 35);
            this.lblNote.Text = "Người dùng sẽ bị yêu cầu đổi mật khẩu khi đăng nhập tiếp theo.";

            // lblMatKhauMoi
            this.lblMatKhauMoi.AutoSize = true;
            this.lblMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMatKhauMoi.Location = new System.Drawing.Point(20, 140);
            this.lblMatKhauMoi.Name = "lblMatKhauMoi";
            this.lblMatKhauMoi.Text = "Mật khẩu mới (ít nhất 6 ký tự)";

            // txtMatKhauMoi
            this.txtMatKhauMoi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMatKhauMoi.Location = new System.Drawing.Point(20, 160);
            this.txtMatKhauMoi.MaxLength = 200;
            this.txtMatKhauMoi.Name = "txtMatKhauMoi";
            this.txtMatKhauMoi.Size = new System.Drawing.Size(360, 28);
            this.txtMatKhauMoi.UseSystemPasswordChar = true;

            // lblXacNhan
            this.lblXacNhan.AutoSize = true;
            this.lblXacNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblXacNhan.Location = new System.Drawing.Point(20, 200);
            this.lblXacNhan.Name = "lblXacNhan";
            this.lblXacNhan.Text = "Xác nhận mật khẩu mới";

            // txtXacNhan
            this.txtXacNhan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXacNhan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtXacNhan.Location = new System.Drawing.Point(20, 220);
            this.txtXacNhan.MaxLength = 200;
            this.txtXacNhan.Name = "txtXacNhan";
            this.txtXacNhan.Size = new System.Drawing.Size(360, 28);
            this.txtXacNhan.UseSystemPasswordChar = true;

            // chkHienThi
            this.chkHienThi.AutoSize = true;
            this.chkHienThi.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkHienThi.Location = new System.Drawing.Point(20, 258);
            this.chkHienThi.Name = "chkHienThi";
            this.chkHienThi.Text = "Hiển thị mật khẩu";
            this.chkHienThi.CheckedChanged += new System.EventHandler(this.chkHienThi_CheckedChanged);

            // btnDatLai
            this.btnDatLai.BackColor = System.Drawing.Color.FromArgb(180, 130, 0);
            this.btnDatLai.FlatAppearance.BorderSize = 0;
            this.btnDatLai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatLai.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDatLai.ForeColor = System.Drawing.Color.White;
            this.btnDatLai.Location = new System.Drawing.Point(20, 295);
            this.btnDatLai.Name = "btnDatLai";
            this.btnDatLai.Size = new System.Drawing.Size(170, 38);
            this.btnDatLai.Text = "Đặt lại mật khẩu";
            this.btnDatLai.Click += new System.EventHandler(this.btnDatLai_Click);

            // btnHuy
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnHuy.Location = new System.Drawing.Point(205, 295);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(175, 38);
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            // frmDatLaiMatKhau
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblTaiKhoan);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblMatKhauMoi);
            this.Controls.Add(this.txtMatKhauMoi);
            this.Controls.Add(this.lblXacNhan);
            this.Controls.Add(this.txtXacNhan);
            this.Controls.Add(this.chkHienThi);
            this.Controls.Add(this.btnDatLai);
            this.Controls.Add(this.btnHuy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatLaiMatKhau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đặt lại mật khẩu";
            this.Load += new System.EventHandler(this.frmDatLaiMatKhau_Load);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitleHead;
        private System.Windows.Forms.Label lblTaiKhoan;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblMatKhauMoi;
        private System.Windows.Forms.TextBox txtMatKhauMoi;
        private System.Windows.Forms.Label lblXacNhan;
        private System.Windows.Forms.TextBox txtXacNhan;
        private System.Windows.Forms.CheckBox chkHienThi;
        private System.Windows.Forms.Button btnDatLai;
        private System.Windows.Forms.Button btnHuy;
    }
}
