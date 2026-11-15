using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CuahangNongduoc.Frontend.Common1
{
    public static class GridErrorHelper
    {
        /// <summary>
        /// Xử lý sự kiện DataError của DataGridView, hiển thị thông báo lỗi chi tiết theo tên cột và dòng.
        /// </summary>
        public static void HandleDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Ngăn chặn chương trình bị quăng Exception làm crash app hoặc hiện hộp thoại mặc định của Windows Forms
            e.ThrowException = false;
            e.Cancel = false;

            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            // Lấy tên cột gây ra lỗi (HeaderText nếu có, hoặc Name)
            string columnName = "Không xác định";
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dgv.Columns.Count)
            {
                var column = dgv.Columns[e.ColumnIndex];
                columnName = !string.IsNullOrEmpty(column.HeaderText) ? column.HeaderText : column.Name;
            }

            // Số dòng (chuyển về dạng 1-based index cho người dùng dễ hiểu)
            int rowIndex = e.RowIndex + 1;

            // Nội dung chi tiết lỗi từ hệ thống
            string details = e.Exception != null ? e.Exception.Message : "Dữ liệu nhập không đúng định dạng.";

            // Thông báo rõ ràng cho người dùng
            string message = $"Cột [{columnName}] tại dòng {rowIndex} đang có lỗi dữ liệu!\n\n" +
                             $"Chi tiết: {details}\n\n" +
                             $"Vui lòng kiểm tra và nhập lại giá trị hợp lệ.";

            MessageBox.Show(message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
