using System;
using System.IO;

namespace CuahangNongduoc
{
    /// <summary>
    /// [D02] VIẾT MỚI – Mẫu thiết kế: Singleton (Creational, GoF)
    /// Mục đích : ghi log lỗi ra file logs/app_yyyyMMdd.log (cạnh file .exe) để truy vết lỗi CSDL khi demo/kiểm thử
    /// Thay thế : trước đây lỗi chỉ hiện MessageBox rồi mất, không lưu lại ở đâu
    /// Tác giả  : BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên>
    /// Ngày tạo : 08/11/2026
    /// </summary>
    public sealed class AppLogger
    {
        private static readonly AppLogger m_Instance = new AppLogger();
        private readonly object m_Khoa = new object();
        private readonly string m_ThuMuc;

        private AppLogger()
        {
            m_ThuMuc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        }

        public static AppLogger Instance
        {
            get { return m_Instance; }
        }

        /// <summary>Đường dẫn file log của ngày hôm nay.</summary>
        public string DuongDanFile
        {
            get { return Path.Combine(m_ThuMuc, "app_" + DateTime.Now.ToString("yyyyMMdd") + ".log"); }
        }

        public void Info(string noiDung)
        {
            Ghi("INFO ", noiDung);
        }

        public void Error(string noiDung, Exception ex)
        {
            Ghi("ERROR", ex == null ? noiDung : noiDung + Environment.NewLine + ex.ToString());
        }

        private void Ghi(string muc, string noiDung)
        {
            try
            {
                lock (m_Khoa)
                {
                    Directory.CreateDirectory(m_ThuMuc);
                    File.AppendAllText(DuongDanFile,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [" + muc + "] " + noiDung + Environment.NewLine);
                }
            }
            catch (Exception)
            {
                // Ghi log hỏng (đĩa đầy, không có quyền) không được làm hỏng chương trình
            }
        }
    }
}
