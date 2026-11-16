using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    public partial class frmTimPhieuChi : Form
    {
        public frmTimPhieuChi()
        {
            InitializeComponent();
        }

        private void frmTimPhieuChi_Load(object sender, EventArgs e)
        {
            Controller.LyDoChiController ctrl = new CuahangNongduoc.Controller.LyDoChiController();
            ctrl.HienthiAutoComboBox(cmbLyDo);

            // Ensure closing by X is treated as Cancel
            this.FormClosing -= FrmTimPhieuChi_FormClosing;
            this.FormClosing += FrmTimPhieuChi_FormClosing;
        }

        private void FrmTimPhieuChi_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If user closed the form (not via OK), ensure DialogResult is Cancel so caller can react.
            if (this.DialogResult != DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Explicit cancel: set DialogResult and close so caller can reset datagrid
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Confirm search
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}