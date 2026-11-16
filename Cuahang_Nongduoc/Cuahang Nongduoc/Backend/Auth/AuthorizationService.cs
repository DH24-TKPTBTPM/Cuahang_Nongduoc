using System;
using System.Windows.Forms;

namespace CuahangNongduoc.Auth
{
    /// <summary>
    /// Dịch vụ phân quyền dựa trên vai trò (RBAC).
    /// Định nghĩa ma trận quyền và cung cấp phương thức kiểm tra quyền tại runtime.
    /// </summary>
    public static class AuthorizationService
    {
        /* Bridge: tách ma trận vai trò khỏi dịch vụ phân quyền. */
        private static readonly IAuthorizationImplementor Implementor =
            new RoleAuthorizationImplementor();

        // ======================================================
        // ĐỊNH NGHĨA CÁC PERMISSION
        // ======================================================
        public static class Permission
        {
            // Hệ thống
            public const string QuanLyTaiKhoan   = "QuanLyTaiKhoan";
            public const string DoiMatKhau        = "DoiMatKhau";
            public const string CauHinhHeThong    = "CauHinhHeThong";

            // Quản lý danh mục
            public const string XemDonViTinh      = "XemDonViTinh";
            public const string SuaDonViTinh      = "SuaDonViTinh";
            public const string XemSanPham        = "XemSanPham";
            public const string SuaSanPham        = "SuaSanPham";
            public const string XemNhaCungCap     = "XemNhaCungCap";
            public const string SuaNhaCungCap     = "SuaNhaCungCap";
            public const string XemLyDoChi        = "XemLyDoChi";
            public const string SuaLyDoChi        = "SuaLyDoChi";

            // Quan hệ khách hàng
            public const string XemKhachHang      = "XemKhachHang";
            public const string SuaKhachHang      = "SuaKhachHang";
            public const string XemDaiLy          = "XemDaiLy";
            public const string SuaDaiLy          = "SuaDaiLy";

            // Nghiệp vụ
            public const string NhapHang          = "NhapHang";
            public const string BanHangLe         = "BanHangLe";
            public const string BanHangSi         = "BanHangSi";
            public const string PhieuThu          = "PhieuThu";  // ThanhToan
            public const string PhieuChi          = "PhieuChi";
            public const string CongNo            = "CongNo";    // DunoKhachhang

            // Báo cáo
            public const string BaoCaoTonKho      = "BaoCaoTonKho";
            public const string BaoCaoDoanhThu    = "BaoCaoDoanhThu";
            public const string BaoCaoSoLuongBan  = "BaoCaoSoLuongBan";
            public const string BaoCaoHetHan      = "BaoCaoHetHan";
        }

        // ======================================================
        // MA TRẬN PHÂN QUYỀN THEO VAI TRÒ
        // ======================================================
        public static bool CoQuyen(string permission)
        {
            if (!UserSession.IsLoggedIn)
                return false;

            return Implementor.CoQuyen(UserSession.Role, permission);
        }

        /// <summary>
        /// Kiểm tra quyền và hiển thị thông báo nếu không có quyền.
        /// Trả về true nếu có quyền, false nếu không.
        /// </summary>
        public static bool KiemTraVaThongBao(string permission, string tenChucNang = null)
        {
            if (CoQuyen(permission))
                return true;

            string msg = string.IsNullOrEmpty(tenChucNang)
                ? "Bạn không có quyền thực hiện chức năng này."
                : string.Format("Bạn không có quyền truy cập chức năng: {0}.", tenChucNang);

            MessageBox.Show(msg, "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>
        /// Áp dụng trạng thái Enabled cho một ToolStripMenuItem dựa trên quyền.
        /// </summary>
        public static void ApDungQuyenMenu(ToolStripMenuItem menuItem, string permission)
        {
            if (menuItem == null) return;
            menuItem.Enabled = CoQuyen(permission);
        }

        /// <summary>
        /// Áp dụng trạng thái Enabled cho một Control dựa trên quyền.
        /// </summary>
        public static void ApDungQuyenControl(Control control, string permission)
        {
            if (control == null) return;
            control.Enabled = CoQuyen(permission);
        }

        public static void ApDungQuyenToolStripItem(ToolStripItem item, string permission)
        {
            if (item == null) return;
            item.Enabled = CoQuyen(permission);
        }
    }
}
