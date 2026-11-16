using System;
using System.Windows.Forms;

namespace CuahangNongduoc
{
    /// <summary>
    /// Hàm tiện ích dùng chung cho 5 form danh sách phiếu (Nhập, Bán sỉ, Bán lẻ, Phiếu chi, Phiếu thu):
    /// - Khi mới mở form (hoặc sau khi bấm Xem lại): Nút Thêm mới và Xem lại SÁNG (Enabled = true); nút Chỉnh sửa và Xóa ĐEN/MỜ (Enabled = false).
    /// - Khi click vào dòng dưới lưới: Nút Thêm mới ĐEN/MỜ (Enabled = false); nút Chỉnh sửa, Xóa và Xem lại SÁNG (Enabled = true).
    /// Tất cả các nút luôn hiển thị (Visible = true) trên thanh công cụ.
    /// </summary>
    public static class FormToolbarHelper
    {
        public static void CapNhatTrangThaiNut(
            bool daChonDong,
            ToolStripItem btnThemMoi,
            ToolStripItem btnChinhSua,
            ToolStripItem btnXemLai,
            ToolStripItem btnXoa)
        {
            if (btnThemMoi != null)
            {
                btnThemMoi.Visible = true;
                btnThemMoi.Enabled = !daChonDong;
            }

            if (btnChinhSua != null)
            {
                btnChinhSua.Visible = true;
                btnChinhSua.Enabled = daChonDong;
            }

            if (btnXoa != null)
            {
                btnXoa.Visible = true;
                btnXoa.Enabled = daChonDong;
            }

            if (btnXemLai != null)
            {
                btnXemLai.Visible = true;
                btnXemLai.Enabled = true;
            }
        }

        public static void ThietLapTrangThai(
            DataGridView dgv,
            ToolStripItem btnThemMoi,
            ToolStripItem btnChinhSua,
            ToolStripItem btnXemLai,
            ToolStripItem btnXoa)
        {
            ThietLapTrangThai(null, dgv, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);
        }

        public static void ThietLapTrangThai(
            Form form,
            DataGridView dgv,
            ToolStripItem btnThemMoi,
            ToolStripItem btnChinhSua,
            ToolStripItem btnXemLai,
            ToolStripItem btnXoa)
        {
            // Trạng thái ban đầu: Thêm mới sáng, Xem lại sáng; Chỉnh sửa đen, Xóa đen
            CapNhatTrangThaiNut(false, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);

            if (dgv != null && dgv.Tag as string == "ToolbarHelperConfigured")
            {
                dgv.ClearSelection();
                return;
            }

            if (dgv != null)
            {
                dgv.Tag = "ToolbarHelperConfigured";
            }

            Form targetForm = form ?? (dgv != null ? dgv.FindForm() : null);
            if (targetForm != null)
            {
                targetForm.Shown += delegate
                {
                    if (dgv != null)
                    {
                        dgv.ClearSelection();
                    }
                    CapNhatTrangThaiNut(false, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);
                };
            }

            if (dgv != null)
            {
                dgv.CellMouseDown += delegate(object sender, DataGridViewCellMouseEventArgs e)
                {
                    if (e.RowIndex >= 0)
                    {
                        CapNhatTrangThaiNut(true, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);
                    }
                };

                dgv.CellClick += delegate(object sender, DataGridViewCellEventArgs e)
                {
                    if (e.RowIndex >= 0)
                    {
                        CapNhatTrangThaiNut(true, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);
                    }
                };

                dgv.RowHeaderMouseClick += delegate(object sender, DataGridViewCellMouseEventArgs e)
                {
                    if (e.RowIndex >= 0)
                    {
                        CapNhatTrangThaiNut(true, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);
                    }
                };

                dgv.SelectionChanged += delegate
                {
                    if (dgv.SelectedRows.Count > 0 || (dgv.SelectedCells.Count > 0 && dgv.CurrentRow != null))
                    {
                        // Chỉ kích hoạt khi đã có tương tác chọn dòng
                        CapNhatTrangThaiNut(true, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);
                    }
                };
            }

            if (btnXemLai != null)
            {
                btnXemLai.Click += delegate
                {
                    if (dgv != null)
                    {
                        dgv.ClearSelection();
                    }
                    CapNhatTrangThaiNut(false, btnThemMoi, btnChinhSua, btnXemLai, btnXoa);
                };
            }
        }
    }
}
