namespace CuahangNongduoc
{
    partial class frmLoading
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
            this.cardShadow = new Siticone.Desktop.UI.WinForms.SiticoneShadowPanel();
            this.wbLoader = new System.Windows.Forms.WebBrowser();
            this.elipseForm = new Siticone.Desktop.UI.WinForms.SiticoneElipse(this.components);
            this.cardShadow.SuspendLayout();
            this.SuspendLayout();
            // 
            // cardShadow
            // 
            this.cardShadow.BackColor = System.Drawing.Color.Transparent;
            this.cardShadow.Controls.Add(this.wbLoader);
            this.cardShadow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardShadow.FillColor = System.Drawing.Color.White;
            this.cardShadow.Location = new System.Drawing.Point(0, 0);
            this.cardShadow.Name = "cardShadow";
            this.cardShadow.Padding = new System.Windows.Forms.Padding(8);
            this.cardShadow.Radius = 10;
            this.cardShadow.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.cardShadow.ShadowDepth = 50;
            this.cardShadow.ShadowShift = 6;
            this.cardShadow.Size = new System.Drawing.Size(320, 200);
            this.cardShadow.TabIndex = 0;
            // 
            // wbLoader
            // 
            this.wbLoader.AllowWebBrowserDrop = false;
            this.wbLoader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbLoader.IsWebBrowserContextMenuEnabled = false;
            this.wbLoader.Location = new System.Drawing.Point(8, 8);
            this.wbLoader.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbLoader.Name = "wbLoader";
            this.wbLoader.ScrollBarsEnabled = false;
            this.wbLoader.Size = new System.Drawing.Size(304, 184);
            this.wbLoader.TabIndex = 0;
            this.wbLoader.WebBrowserShortcutsEnabled = false;
            // 
            // elipseForm
            // 
            this.elipseForm.BorderRadius = 12;
            this.elipseForm.TargetControl = this;
            // 
            // frmLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(320, 200);
            this.Controls.Add(this.cardShadow);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoading";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đang tải dữ liệu...";
            this.Load += new System.EventHandler(this.frmLoading_Load);
            this.cardShadow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Siticone.Desktop.UI.WinForms.SiticoneShadowPanel cardShadow;
        private System.Windows.Forms.WebBrowser wbLoader;
        private Siticone.Desktop.UI.WinForms.SiticoneElipse elipseForm;
    }
}
