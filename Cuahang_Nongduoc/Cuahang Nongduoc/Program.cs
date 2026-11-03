using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // [R02] Sau khi DataService thôi nuốt lỗi, mọi lỗi CSDL chưa được Form bắt
            // sẽ hiện thông báo ở đây thay vì làm tắt chương trình.
            // Tác giả: tuananh (Team Lead, làm thay BE1) | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – Dư Thị Trúc Hòa | Ngày chỉnh sửa: 02/11/2026 | Ngày kiểm thử: 03/11/2026
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            string chiTiet = ex.InnerException != null ? "\n\nChi tiết: " + ex.InnerException.Message : "";
            MessageBox.Show(ex.Message + chiTiet, "Cửa hàng Nông dược - Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
