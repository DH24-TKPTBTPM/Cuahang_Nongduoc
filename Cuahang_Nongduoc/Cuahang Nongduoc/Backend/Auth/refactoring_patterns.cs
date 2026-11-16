using System;

namespace CuahangNongduoc.Auth
{
    /* Adapter Pattern: chuẩn hóa AuthenticationService để form đăng nhập
       không phụ thuộc trực tiếp vào lớp xác thực cụ thể. */
    public interface IAuthenticationAdapter
    {
        LoginResult DangNhap(string tenDangNhap, string matKhau, out NguoiDung nguoiDung);
        void DangXuat();
    }

    public sealed class AuthenticationAdapter : IAuthenticationAdapter
    {
        private readonly AuthenticationService _service = new AuthenticationService();

        public LoginResult DangNhap(string tenDangNhap, string matKhau, out NguoiDung nguoiDung)
        {
            return _service.DangNhap(tenDangNhap, matKhau, out nguoiDung);
        }

        public void DangXuat()
        {
            _service.DangXuat();
        }
    }

    /* Bridge Pattern: AuthorizationService là phần trừu tượng,
       IAuthorizationImplementor là phần triển khai ma trận quyền. */
    public interface IAuthorizationImplementor
    {
        bool CoQuyen(string role, string permission);
    }

    public sealed class RoleAuthorizationImplementor : IAuthorizationImplementor
    {
        public bool CoQuyen(string role, string permission)
        {
            switch ((role ?? "").ToLower())
            {
                case "admin": return true;
                case "sales": return SalesCoQuyen(permission);
                case "warehouse": return WarehouseCoQuyen(permission);
                default: return false;
            }
        }

        private bool SalesCoQuyen(string permission)
        {
            switch (permission)
            {
                case AuthorizationService.Permission.DoiMatKhau:
                case AuthorizationService.Permission.XemSanPham:
                case AuthorizationService.Permission.XemNhaCungCap:
                case AuthorizationService.Permission.XemKhachHang:
                case AuthorizationService.Permission.SuaKhachHang:
                case AuthorizationService.Permission.XemDaiLy:
                case AuthorizationService.Permission.BanHangLe:
                case AuthorizationService.Permission.BanHangSi:
                case AuthorizationService.Permission.PhieuThu:
                case AuthorizationService.Permission.CongNo:
                case AuthorizationService.Permission.BaoCaoDoanhThu:
                case AuthorizationService.Permission.BaoCaoSoLuongBan:
                    return true;
                default: return false;
            }
        }

        private bool WarehouseCoQuyen(string permission)
        {
            switch (permission)
            {
                case AuthorizationService.Permission.DoiMatKhau:
                case AuthorizationService.Permission.XemDonViTinh:
                case AuthorizationService.Permission.XemSanPham:
                case AuthorizationService.Permission.SuaSanPham:
                case AuthorizationService.Permission.XemNhaCungCap:
                case AuthorizationService.Permission.SuaNhaCungCap:
                case AuthorizationService.Permission.XemLyDoChi:
                case AuthorizationService.Permission.NhapHang:
                case AuthorizationService.Permission.BaoCaoTonKho:
                case AuthorizationService.Permission.BaoCaoHetHan:
                case AuthorizationService.Permission.BaoCaoSoLuongBan:
                    return true;
                default: return false;
            }
        }
    }

    /* Proxy Pattern: kiểm tra quyền trước khi cho phép mở chức năng. */
    public sealed class AuthorizationProxy
    {
        public bool Require(string permission, string tenChucNang = null)
        {
            return AuthorizationService.KiemTraVaThongBao(permission, tenChucNang);
        }
    }
}
