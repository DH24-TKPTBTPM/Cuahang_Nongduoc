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
            this.txtMatKhauMoi = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.lblXacNhan = new System.Windows.Forms.Label();
            this.txtXacNhan = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.chkHienThi = new Siticone.Desktop.UI.WinForms.SiticoneCheckBox();
            this.btnDatLai = new Siticone.Desktop.UI.WinForms.SiticoneButton();
            this.btnHuy = new Siticone.Desktop.UI.WinForms.SiticoneButton();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelTop.Controls.Add(this.lblTitleHead);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(430, 56);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitleHead
            // 
            this.lblTitleHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitleHead.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitleHead.ForeColor = System.Drawing.Color.White;
            this.lblTitleHead.Location = new System.Drawing.Point(0, 0);
            this.lblTitleHead.Name = "lblTitleHead";
            this.lblTitleHead.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.lblTitleHead.Size = new System.Drawing.Size(430, 56);
            this.lblTitleHead.TabIndex = 0;
            this.lblTitleHead.Text = "ĐẶT LẠI MẬT KHẨU";
            this.lblTitleHead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTaiKhoan
            // 
            this.lblTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTaiKhoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblTaiKhoan.Location = new System.Drawing.Point(24, 70);
            this.lblTaiKhoan.Name = "lblTaiKhoan";
            this.lblTaiKhoan.Size = new System.Drawing.Size(382, 24);
            this.lblTaiKhoan.TabIndex = 1;
            this.lblTaiKhoan.Text = "Tài khoản: ";
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblNote.Location = new System.Drawing.Point(24, 98);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(382, 32);
            this.lblNote.TabIndex = 2;
            this.lblNote.Text = "Nhập mật khẩu mới cho tài khoản. Người dùng sẽ được yêu cầu đổi mật khẩu ở lần đă" +
    "ng nhập tiếp theo.";
            // 
            // lblMatKhauMoi
            // 
            this.lblMatKhauMoi.AutoSize = true;
            this.lblMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblMatKhauMoi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblMatKhauMoi.Location = new System.Drawing.Point(24, 136);
            this.lblMatKhauMoi.Name = "lblMatKhauMoi";
            this.lblMatKhauMoi.Size = new System.Drawing.Size(92, 15);
            this.lblMatKhauMoi.TabIndex = 3;
            this.lblMatKhauMoi.Text = "Mật khẩu mới *";
            // 
            // txtMatKhauMoi
            // 
            this.txtMatKhauMoi.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtMatKhauMoi.BorderRadius = 8;
            this.txtMatKhauMoi.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMatKhauMoi.DefaultText = "";
            this.txtMatKhauMoi.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtMatKhauMoi.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.txtMatKhauMoi.Location = new System.Drawing.Point(24, 154);
            this.txtMatKhauMoi.Margin = new System.Windows.Forms.Padding(4);
            this.txtMatKhauMoi.Name = "txtMatKhauMoi";
            this.txtMatKhauMoi.PasswordChar = '●';
            this.txtMatKhauMoi.PlaceholderText = "Tối thiểu 6 ký tự...";
            this.txtMatKhauMoi.SelectedText = "";
            this.txtMatKhauMoi.Size = new System.Drawing.Size(382, 36);
            this.txtMatKhauMoi.TabIndex = 4;
            this.txtMatKhauMoi.UseSystemPasswordChar = true;
            // 
            // lblXacNhan
            // 
            this.lblXacNhan.AutoSize = true;
            this.lblXacNhan.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblXacNhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblXacNhan.Location = new System.Drawing.Point(24, 198);
            this.lblXacNhan.Name = "lblXacNhan";
            this.lblXacNhan.Size = new System.Drawing.Size(123, 15);
            this.lblXacNhan.TabIndex = 5;
            this.lblXacNhan.Text = "Xác nhận mật khẩu *";
            // 
            // txtXacNhan
            // 
            this.txtXacNhan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtXacNhan.BorderRadius = 8;
            this.txtXacNhan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtXacNhan.DefaultText = "";
            this.txtXacNhan.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtXacNhan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.txtXacNhan.Location = new System.Drawing.Point(24, 216);
            this.txtXacNhan.Margin = new System.Windows.Forms.Padding(4);
            this.txtXacNhan.Name = "txtXacNhan";
            this.txtXacNhan.PasswordChar = '●';
            this.txtXacNhan.PlaceholderText = "Nhập lại mật khẩu giống bên trên...";
            this.txtXacNhan.SelectedText = "";
            this.txtXacNhan.Size = new System.Drawing.Size(382, 36);
            this.txtXacNhan.TabIndex = 6;
            this.txtXacNhan.UseSystemPasswordChar = true;
            // 
            // chkHienThi
            // 
            this.chkHienThi.AutoSize = true;
            this.chkHienThi.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkHienThi.CheckedState.BorderRadius = 3;
            this.chkHienThi.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkHienThi.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkHienThi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.chkHienThi.Location = new System.Drawing.Point(26, 260);
            this.chkHienThi.Name = "chkHienThi";
            this.chkHienThi.Size = new System.Drawing.Size(121, 19);
            this.chkHienThi.TabIndex = 7;
            this.chkHienThi.Text = "Hiển thị mật khẩu";
            this.chkHienThi.CheckedChanged += new System.EventHandler(this.chkHienThi_CheckedChanged);
            // 
            // btnDatLai
            // 
            this.btnDatLai.BorderRadius = 8;
            this.btnDatLai.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDatLai.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnDatLai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnDatLai.ForeColor = System.Drawing.Color.White;
            this.btnDatLai.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
            this.btnDatLai.Location = new System.Drawing.Point(176, 296);
            this.btnDatLai.Name = "btnDatLai";
            this.btnDatLai.Size = new System.Drawing.Size(130, 38);
            this.btnDatLai.TabIndex = 8;
            this.btnDatLai.Text = "Đặt lại mật khẩu";
            this.btnDatLai.Click += new System.EventHandler(this.btnDatLai_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnHuy.BorderRadius = 8;
            this.btnHuy.BorderThickness = 1;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FillColor = System.Drawing.Color.White;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnHuy.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnHuy.Location = new System.Drawing.Point(312, 296);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(95, 38);
            this.btnHuy.TabIndex = 9;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // frmDatLaiMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(430, 350);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnDatLai);
            this.Controls.Add(this.chkHienThi);
            this.Controls.Add(this.txtXacNhan);
            this.Controls.Add(this.lblXacNhan);
            this.Controls.Add(this.txtMatKhauMoi);
            this.Controls.Add(this.lblMatKhauMoi);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblTaiKhoan);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatLaiMatKhau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đặt lại mật khẩu tài khoản - Siticone UI";
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
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtMatKhauMoi;
        private System.Windows.Forms.Label lblXacNhan;
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtXacNhan;
        private Siticone.Desktop.UI.WinForms.SiticoneCheckBox chkHienThi;
        private Siticone.Desktop.UI.WinForms.SiticoneButton btnDatLai;
        private Siticone.Desktop.UI.WinForms.SiticoneButton btnHuy;
    }
}
