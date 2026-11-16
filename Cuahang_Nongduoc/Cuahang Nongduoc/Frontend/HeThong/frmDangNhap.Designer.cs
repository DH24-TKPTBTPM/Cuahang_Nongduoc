namespace CuahangNongduoc
{
    partial class frmDangNhap
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDangNhap));
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.cardContainer = new Siticone.Desktop.UI.WinForms.SiticoneShadowPanel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.ctrlClose = new Siticone.Desktop.UI.WinForms.SiticoneControlBox();
            this.ctrlMaximize = new Siticone.Desktop.UI.WinForms.SiticoneControlBox();
            this.ctrlMinimize = new Siticone.Desktop.UI.WinForms.SiticoneControlBox();
            this.lblLoginHeader = new System.Windows.Forms.Label();
            this.lblLoginDesc = new System.Windows.Forms.Label();
            this.lblTenDangNhap = new System.Windows.Forms.Label();
            this.txtTenDangNhap = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new Siticone.Desktop.UI.WinForms.SiticoneTextBox();
            this.chkHienMatKhau = new Siticone.Desktop.UI.WinForms.SiticoneCheckBox();
            this.lblThongBao = new System.Windows.Forms.Label();
            this.btnDangNhap = new Siticone.Desktop.UI.WinForms.SiticoneButton();
            this.btnThoat = new Siticone.Desktop.UI.WinForms.SiticoneButton();
            this.lblHint = new System.Windows.Forms.Label();
            this.pnlLeftGradient = new Siticone.Desktop.UI.WinForms.SiticoneGradientPanel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblBrandTitle = new System.Windows.Forms.Label();
            this.lblBrandSub = new System.Windows.Forms.Label();
            this.lblFeature1 = new System.Windows.Forms.Label();
            this.lblFeature2 = new System.Windows.Forms.Label();
            this.lblFeature3 = new System.Windows.Forms.Label();
            this.lblFeature4 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.siticoneDragControl1 = new Siticone.Desktop.UI.WinForms.SiticoneDragControl(this.components);
            this.pnlBackground.SuspendLayout();
            this.cardContainer.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlLeftGradient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlBackground.Controls.Add(this.cardContainer);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(880, 530);
            this.pnlBackground.TabIndex = 0;
            this.pnlBackground.Resize += new System.EventHandler(this.pnlBackground_Resize);
            // 
            // cardContainer
            // 
            this.cardContainer.BackColor = System.Drawing.Color.Transparent;
            this.cardContainer.Controls.Add(this.pnlRight);
            this.cardContainer.Controls.Add(this.pnlLeftGradient);
            this.cardContainer.FillColor = System.Drawing.Color.White;
            this.cardContainer.Location = new System.Drawing.Point(20, 20);
            this.cardContainer.Name = "cardContainer";
            this.cardContainer.Radius = 10;
            this.cardContainer.ShadowColor = System.Drawing.Color.Black;
            this.cardContainer.ShadowDepth = 60;
            this.cardContainer.ShadowShift = 8;
            this.cardContainer.Size = new System.Drawing.Size(840, 490);
            this.cardContainer.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.White;
            this.pnlRight.Controls.Add(this.ctrlClose);
            this.pnlRight.Controls.Add(this.ctrlMaximize);
            this.pnlRight.Controls.Add(this.ctrlMinimize);
            this.pnlRight.Controls.Add(this.lblLoginHeader);
            this.pnlRight.Controls.Add(this.lblLoginDesc);
            this.pnlRight.Controls.Add(this.lblTenDangNhap);
            this.pnlRight.Controls.Add(this.txtTenDangNhap);
            this.pnlRight.Controls.Add(this.lblMatKhau);
            this.pnlRight.Controls.Add(this.txtMatKhau);
            this.pnlRight.Controls.Add(this.chkHienMatKhau);
            this.pnlRight.Controls.Add(this.lblThongBao);
            this.pnlRight.Controls.Add(this.btnDangNhap);
            this.pnlRight.Controls.Add(this.btnThoat);
            this.pnlRight.Controls.Add(this.lblHint);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(340, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(36, 16, 36, 20);
            this.pnlRight.Size = new System.Drawing.Size(500, 490);
            this.pnlRight.TabIndex = 1;
            // 
            // ctrlClose
            // 
            this.ctrlClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlClose.FillColor = System.Drawing.Color.White;
            this.ctrlClose.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.ctrlClose.HoverState.IconColor = System.Drawing.Color.White;
            this.ctrlClose.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.ctrlClose.Location = new System.Drawing.Point(455, 8);
            this.ctrlClose.Name = "ctrlClose";
            this.ctrlClose.Size = new System.Drawing.Size(36, 28);
            this.ctrlClose.TabIndex = 13;
            // 
            // ctrlMaximize
            // 
            this.ctrlMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlMaximize.ControlBoxType = Siticone.Desktop.UI.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.ctrlMaximize.FillColor = System.Drawing.Color.White;
            this.ctrlMaximize.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(234)))), ((int)(((byte)(246)))));
            this.ctrlMaximize.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.ctrlMaximize.Location = new System.Drawing.Point(415, 8);
            this.ctrlMaximize.Name = "ctrlMaximize";
            this.ctrlMaximize.Size = new System.Drawing.Size(36, 28);
            this.ctrlMaximize.TabIndex = 12;
            // 
            // ctrlMinimize
            // 
            this.ctrlMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlMinimize.ControlBoxType = Siticone.Desktop.UI.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.ctrlMinimize.FillColor = System.Drawing.Color.White;
            this.ctrlMinimize.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(234)))), ((int)(((byte)(246)))));
            this.ctrlMinimize.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.ctrlMinimize.Location = new System.Drawing.Point(375, 8);
            this.ctrlMinimize.Name = "ctrlMinimize";
            this.ctrlMinimize.Size = new System.Drawing.Size(36, 28);
            this.ctrlMinimize.TabIndex = 11;
            // 
            // lblLoginHeader
            // 
            this.lblLoginHeader.AutoSize = true;
            this.lblLoginHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.lblLoginHeader.Location = new System.Drawing.Point(36, 32);
            this.lblLoginHeader.Name = "lblLoginHeader";
            this.lblLoginHeader.Size = new System.Drawing.Size(161, 32);
            this.lblLoginHeader.TabIndex = 0;
            this.lblLoginHeader.Text = "ĐĂNG NHẬP";
            // 
            // lblLoginDesc
            // 
            this.lblLoginDesc.AutoSize = true;
            this.lblLoginDesc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblLoginDesc.Location = new System.Drawing.Point(38, 68);
            this.lblLoginDesc.Name = "lblLoginDesc";
            this.lblLoginDesc.Size = new System.Drawing.Size(288, 15);
            this.lblLoginDesc.TabIndex = 1;
            this.lblLoginDesc.Text = "Vui lòng nhập thông tin tài khoản để truy cập hệ thống";
            // 
            // lblTenDangNhap
            // 
            this.lblTenDangNhap.AutoSize = true;
            this.lblTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenDangNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTenDangNhap.Location = new System.Drawing.Point(38, 104);
            this.lblTenDangNhap.Name = "lblTenDangNhap";
            this.lblTenDangNhap.Size = new System.Drawing.Size(100, 13);
            this.lblTenDangNhap.TabIndex = 2;
            this.lblTenDangNhap.Text = "TÊN ĐĂNG NHẬP";
            // 
            // txtTenDangNhap
            // 
            this.txtTenDangNhap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtTenDangNhap.BorderRadius = 8;
            this.txtTenDangNhap.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenDangNhap.DefaultText = "vodichbatphuong";
            this.txtTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTenDangNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtTenDangNhap.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(165)))), ((int)(((byte)(250)))));
            this.txtTenDangNhap.Location = new System.Drawing.Point(38, 122);
            this.txtTenDangNhap.Margin = new System.Windows.Forms.Padding(4);
            this.txtTenDangNhap.Name = "txtTenDangNhap";
            this.txtTenDangNhap.PlaceholderText = "Tên đăng nhập...";
            this.txtTenDangNhap.SelectedText = "";
            this.txtTenDangNhap.Size = new System.Drawing.Size(420, 38);
            this.txtTenDangNhap.TabIndex = 3;
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.AutoSize = true;
            this.lblMatKhau.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblMatKhau.Location = new System.Drawing.Point(38, 172);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Size = new System.Drawing.Size(65, 13);
            this.lblMatKhau.TabIndex = 4;
            this.lblMatKhau.Text = "MẬT KHẨU";
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.txtMatKhau.BorderRadius = 8;
            this.txtMatKhau.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMatKhau.DefaultText = "";
            this.txtMatKhau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtMatKhau.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(165)))), ((int)(((byte)(250)))));
            this.txtMatKhau.Location = new System.Drawing.Point(38, 190);
            this.txtMatKhau.Margin = new System.Windows.Forms.Padding(4);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.PasswordChar = '●';
            this.txtMatKhau.PlaceholderText = "Mật khẩu...";
            this.txtMatKhau.SelectedText = "";
            this.txtMatKhau.Size = new System.Drawing.Size(420, 38);
            this.txtMatKhau.TabIndex = 5;
            this.txtMatKhau.UseSystemPasswordChar = true;
            // 
            // chkHienMatKhau
            // 
            this.chkHienMatKhau.AutoSize = true;
            this.chkHienMatKhau.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkHienMatKhau.CheckedState.BorderRadius = 3;
            this.chkHienMatKhau.CheckedState.BorderThickness = 0;
            this.chkHienMatKhau.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.chkHienMatKhau.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHienMatKhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.chkHienMatKhau.Location = new System.Drawing.Point(40, 236);
            this.chkHienMatKhau.Name = "chkHienMatKhau";
            this.chkHienMatKhau.Size = new System.Drawing.Size(121, 19);
            this.chkHienMatKhau.TabIndex = 6;
            this.chkHienMatKhau.Text = "Hiển thị mật khẩu";
            this.chkHienMatKhau.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.chkHienMatKhau.UncheckedState.BorderRadius = 3;
            this.chkHienMatKhau.UncheckedState.BorderThickness = 1;
            this.chkHienMatKhau.UncheckedState.FillColor = System.Drawing.Color.White;
            this.chkHienMatKhau.CheckedChanged += new System.EventHandler(this.chkHienMatKhau_CheckedChanged);
            // 
            // lblThongBao
            // 
            this.lblThongBao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThongBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.lblThongBao.Location = new System.Drawing.Point(38, 264);
            this.lblThongBao.Name = "lblThongBao";
            this.lblThongBao.Size = new System.Drawing.Size(420, 32);
            this.lblThongBao.TabIndex = 7;
            this.lblThongBao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblThongBao.Visible = false;
            // 
            // btnDangNhap
            // 
            this.btnDangNhap.BorderRadius = 8;
            this.btnDangNhap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDangNhap.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnDangNhap.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDangNhap.ForeColor = System.Drawing.Color.White;
            this.btnDangNhap.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(216)))));
            this.btnDangNhap.Location = new System.Drawing.Point(38, 306);
            this.btnDangNhap.Name = "btnDangNhap";
            this.btnDangNhap.ShadowDecoration.BorderRadius = 8;
            this.btnDangNhap.ShadowDecoration.Depth = 15;
            this.btnDangNhap.ShadowDecoration.Enabled = true;
            this.btnDangNhap.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.btnDangNhap.Size = new System.Drawing.Size(420, 44);
            this.btnDangNhap.TabIndex = 8;
            this.btnDangNhap.Text = "ĐĂNG NHẬP HỆ THỐNG";
            this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnThoat.BorderRadius = 8;
            this.btnThoat.BorderThickness = 1;
            this.btnThoat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThoat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.btnThoat.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnThoat.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnThoat.Location = new System.Drawing.Point(38, 360);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(420, 36);
            this.btnThoat.TabIndex = 9;
            this.btnThoat.Text = "Thoát ứng dụng";
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // lblHint
            // 
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblHint.Location = new System.Drawing.Point(38, 412);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(420, 48);
            this.lblHint.TabIndex = 10;
            this.lblHint.Text = "Tài khoản quản trị mặc định: vodichbatphuong  |  Mật khẩu: vodichbatphuong123\nNh" +
    "ấn phím Enter để đăng nhập nhanh";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLeftGradient
            // 
            this.pnlLeftGradient.BackColor = System.Drawing.Color.Transparent;
            this.pnlLeftGradient.Controls.Add(this.picLogo);
            this.pnlLeftGradient.Controls.Add(this.lblBrandTitle);
            this.pnlLeftGradient.Controls.Add(this.lblBrandSub);
            this.pnlLeftGradient.Controls.Add(this.lblFeature1);
            this.pnlLeftGradient.Controls.Add(this.lblFeature2);
            this.pnlLeftGradient.Controls.Add(this.lblFeature3);
            this.pnlLeftGradient.Controls.Add(this.lblFeature4);
            this.pnlLeftGradient.Controls.Add(this.lblVersion);
            this.pnlLeftGradient.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftGradient.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlLeftGradient.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.pnlLeftGradient.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.pnlLeftGradient.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftGradient.Name = "pnlLeftGradient";
            this.pnlLeftGradient.Padding = new System.Windows.Forms.Padding(28, 36, 28, 24);
            this.pnlLeftGradient.Size = new System.Drawing.Size(340, 490);
            this.pnlLeftGradient.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.Image = global::CuahangNongduoc.Properties.Resources.info;
            this.picLogo.Location = new System.Drawing.Point(28, 36);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(56, 56);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // lblBrandTitle
            // 
            this.lblBrandTitle.AutoSize = true;
            this.lblBrandTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrandTitle.ForeColor = System.Drawing.Color.White;
            this.lblBrandTitle.Location = new System.Drawing.Point(24, 102);
            this.lblBrandTitle.Name = "lblBrandTitle";
            this.lblBrandTitle.Size = new System.Drawing.Size(262, 28);
            this.lblBrandTitle.TabIndex = 1;
            this.lblBrandTitle.Text = "CỬA HÀNG NÔNG DƯỢC";
            // 
            // lblBrandSub
            // 
            this.lblBrandSub.AutoSize = true;
            this.lblBrandSub.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrandSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(197)))), ((int)(((byte)(253)))));
            this.lblBrandSub.Location = new System.Drawing.Point(26, 134);
            this.lblBrandSub.Name = "lblBrandSub";
            this.lblBrandSub.Size = new System.Drawing.Size(199, 13);
            this.lblBrandSub.TabIndex = 2;
            this.lblBrandSub.Text = "HỆ THỐNG QUẢN LÝ DOANH NGHIỆP";
            // 
            // lblFeature1
            // 
            this.lblFeature1.AutoSize = true;
            this.lblFeature1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeature1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.lblFeature1.Location = new System.Drawing.Point(26, 185);
            this.lblFeature1.Name = "lblFeature1";
            this.lblFeature1.Size = new System.Drawing.Size(217, 17);
            this.lblFeature1.TabIndex = 3;
            this.lblFeature1.Text = "✔  Quản lý xuất - nhập kho vật tư";
            // 
            // lblFeature2
            // 
            this.lblFeature2.AutoSize = true;
            this.lblFeature2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeature2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.lblFeature2.Location = new System.Drawing.Point(26, 222);
            this.lblFeature2.Name = "lblFeature2";
            this.lblFeature2.Size = new System.Drawing.Size(225, 17);
            this.lblFeature2.TabIndex = 4;
            this.lblFeature2.Text = "✔  Theo dõi lô & hạn sử dụng (FEFO)";
            // 
            // lblFeature3
            // 
            this.lblFeature3.AutoSize = true;
            this.lblFeature3.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeature3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.lblFeature3.Location = new System.Drawing.Point(26, 259);
            this.lblFeature3.Name = "lblFeature3";
            this.lblFeature3.Size = new System.Drawing.Size(209, 17);
            this.lblFeature3.TabIndex = 5;
            this.lblFeature3.Text = "✔  Bán sỉ, bán lẻ & sổ nợ khách hàng";
            // 
            // lblFeature4
            // 
            this.lblFeature4.AutoSize = true;
            this.lblFeature4.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFeature4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.lblFeature4.Location = new System.Drawing.Point(26, 296);
            this.lblFeature4.Name = "lblFeature4";
            this.lblFeature4.Size = new System.Drawing.Size(224, 17);
            this.lblFeature4.TabIndex = 6;
            this.lblFeature4.Text = "✔  Báo cáo doanh thu & phân quyền";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(197)))), ((int)(((byte)(253)))));
            this.lblVersion.Location = new System.Drawing.Point(26, 448);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(161, 13);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Phiên bản 2.0 • Siticone Blue UI";
            // 
            // siticoneDragControl1
            // 
            this.siticoneDragControl1.TargetControl = this.pnlLeftGradient;
            // 
            // frmDangNhap
            // 
            this.AcceptButton = this.btnDangNhap;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(880, 530);
            this.Controls.Add(this.pnlBackground);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.MinimumSize = new System.Drawing.Size(880, 530);
            this.Name = "frmDangNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập hệ thống - Quản lý Cửa hàng Nông Dược";
            this.Load += new System.EventHandler(this.frmDangNhap_Load);
            this.pnlBackground.ResumeLayout(false);
            this.cardContainer.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.pnlLeftGradient.ResumeLayout(false);
            this.pnlLeftGradient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private Siticone.Desktop.UI.WinForms.SiticoneShadowPanel cardContainer;
        private Siticone.Desktop.UI.WinForms.SiticoneGradientPanel pnlLeftGradient;
        private System.Windows.Forms.Panel pnlRight;
        private Siticone.Desktop.UI.WinForms.SiticoneControlBox ctrlClose;
        private Siticone.Desktop.UI.WinForms.SiticoneControlBox ctrlMaximize;
        private Siticone.Desktop.UI.WinForms.SiticoneControlBox ctrlMinimize;
        private System.Windows.Forms.Label lblLoginHeader;
        private System.Windows.Forms.Label lblLoginDesc;
        private System.Windows.Forms.Label lblTenDangNhap;
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtTenDangNhap;
        private System.Windows.Forms.Label lblMatKhau;
        private Siticone.Desktop.UI.WinForms.SiticoneTextBox txtMatKhau;
        private Siticone.Desktop.UI.WinForms.SiticoneCheckBox chkHienMatKhau;
        private System.Windows.Forms.Label lblThongBao;
        private Siticone.Desktop.UI.WinForms.SiticoneButton btnDangNhap;
        private Siticone.Desktop.UI.WinForms.SiticoneButton btnThoat;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblBrandTitle;
        private System.Windows.Forms.Label lblBrandSub;
        private System.Windows.Forms.Label lblFeature1;
        private System.Windows.Forms.Label lblFeature2;
        private System.Windows.Forms.Label lblFeature3;
        private System.Windows.Forms.Label lblFeature4;
        private System.Windows.Forms.Label lblVersion;
        private Siticone.Desktop.UI.WinForms.SiticoneDragControl siticoneDragControl1;
    }
}
