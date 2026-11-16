namespace CuahangNongduoc
{
    partial class frmHuongDanSuDung
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.lstChuDe = new System.Windows.Forms.ListBox();
            this.lblMucLuc = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.wbNoiDung = new System.Windows.Forms.WebBrowser();
            this.panelTop.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(44)))), ((int)(((byte)(34)))));
            this.panelTop.Controls.Add(this.lblSubTitle);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(24, 12, 24, 12);
            this.panelTop.Size = new System.Drawing.Size(1084, 75);
            this.panelTop.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(243)))), ((int)(((byte)(208)))));
            this.lblSubTitle.Location = new System.Drawing.Point(24, 43);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(1036, 20);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "TÃ i liá»‡u quy trÃ¬nh nghiá»‡p vá»¥ & váº­n hÃ nh Há»‡ thá»‘ng Quáº£n lÃ½ Cá»­a hÃ ng NÃ´ng DÆ°á»£c";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(24, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1036, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HÆ¯á»šNG DáºªN Sá»¬ Dá»¤NG Há»† THá» NG";
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.panelLeft.Controls.Add(this.lstChuDe);
            this.panelLeft.Controls.Add(this.lblMucLuc);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 75);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(12);
            this.panelLeft.Size = new System.Drawing.Size(300, 636);
            this.panelLeft.TabIndex = 1;
            // 
            // lstChuDe
            // 
            this.lstChuDe.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstChuDe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstChuDe.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstChuDe.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstChuDe.FormattingEnabled = true;
            this.lstChuDe.ItemHeight = 44;
            this.lstChuDe.Location = new System.Drawing.Point(12, 42);
            this.lstChuDe.Name = "lstChuDe";
            this.lstChuDe.Size = new System.Drawing.Size(276, 582);
            this.lstChuDe.TabIndex = 1;
            this.lstChuDe.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstChuDe_DrawItem);
            this.lstChuDe.SelectedIndexChanged += new System.EventHandler(this.lstChuDe_SelectedIndexChanged);
            // 
            // lblMucLuc
            // 
            this.lblMucLuc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMucLuc.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMucLuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblMucLuc.Location = new System.Drawing.Point(12, 12);
            this.lblMucLuc.Name = "lblMucLuc";
            this.lblMucLuc.Size = new System.Drawing.Size(276, 30);
            this.lblMucLuc.TabIndex = 0;
            this.lblMucLuc.Text = "CHá»¦ Ä á»€ HÆ¯á»šNG DáºªN";
            this.lblMucLuc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.White;
            this.panelRight.Controls.Add(this.wbNoiDung);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(300, 75);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(784, 636);
            this.panelRight.TabIndex = 2;
            // 
            // wbNoiDung
            // 
            this.wbNoiDung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbNoiDung.Location = new System.Drawing.Point(0, 0);
            this.wbNoiDung.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbNoiDung.Name = "wbNoiDung";
            this.wbNoiDung.Size = new System.Drawing.Size(784, 636);
            this.wbNoiDung.TabIndex = 0;
            // 
            // frmHuongDanSuDung
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 711);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "frmHuongDanSuDung";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HÆ°á»›ng dáº«n sá»­ dá»¥ng - Quáº£n lÃ½ Cá»­a hÃ ng NÃ´ng dÆ°á»£c";
            this.Load += new System.EventHandler(this.frmHuongDanSuDung_Load);
            this.panelTop.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label lblMucLuc;
        private System.Windows.Forms.ListBox lstChuDe;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.WebBrowser wbNoiDung;
    }
}
