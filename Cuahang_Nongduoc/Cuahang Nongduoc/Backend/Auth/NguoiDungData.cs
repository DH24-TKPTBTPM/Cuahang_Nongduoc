using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace CuahangNongduoc.Auth
{
    /// <summary>
    /// Data Access Layer cho bảng NGUOI_DUNG.
    /// Tất cả truy vấn đều dùng tham số để tránh SQL Injection.
    /// </summary>
    public class NguoiDungData
    {
        // Chuỗi kết nối lấy từ DataService (dùng chung một nguồn kết nối)
        private static string ConnectString
        {
            get { return DataService.m_ConnectString; }
        }

        private OleDbConnection GetConnection()
        {
            return new OleDbConnection(ConnectString);
        }

        /// <summary>
        /// Lấy tài khoản theo tên đăng nhập (không phân biệt hoa thường).
        /// </summary>
        public NguoiDung LayTheoTenDangNhap(string tenDangNhap)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "SELECT ID, TEN_DANG_NHAP, MATKHAU, SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT " +
                    "FROM NGUOI_DUNG WHERE TEN_DANG_NHAP = ?", conn))
                {
                    cmd.Parameters.Add("tenDangNhap", OleDbType.VarChar, 255).Value = tenDangNhap;
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return DocNguoiDung(reader);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy tài khoản theo ID.
        /// </summary>
        public NguoiDung LayTheoId(int id)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "SELECT ID, TEN_DANG_NHAP, MATKHAU, SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT " +
                    "FROM NGUOI_DUNG WHERE ID = ?", conn))
                {
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return DocNguoiDung(reader);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách tài khoản (không kèm mật khẩu).
        /// </summary>
        public List<NguoiDung> LayTatCa()
        {
            var list = new List<NguoiDung>();
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "SELECT ID, TEN_DANG_NHAP, '' AS MATKHAU, '' AS SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT " +
                    "FROM NGUOI_DUNG ORDER BY TEN_DANG_NHAP", conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(DocNguoiDung(reader));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Thêm tài khoản mới.
        /// </summary>
        public bool Them(NguoiDung nd)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                // Kiểm tra trùng tên đăng nhập
                using (OleDbCommand check = new OleDbCommand(
                    "SELECT COUNT(*) FROM NGUOI_DUNG WHERE TEN_DANG_NHAP = ?", conn))
                {
                    check.Parameters.Add("ten", OleDbType.VarChar, 255).Value = nd.TenDangNhap;
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    if (count > 0)
                        throw new InvalidOperationException("Tên đăng nhập '" + nd.TenDangNhap + "' đã tồn tại.");
                }

                using (OleDbCommand cmd = new OleDbCommand(
                    "INSERT INTO NGUOI_DUNG (TEN_DANG_NHAP, MATKHAU, SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT) " +
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)", conn))
                {
                    cmd.Parameters.Add("ten", OleDbType.VarChar, 255).Value = nd.TenDangNhap;
                    cmd.Parameters.Add("mk", OleDbType.VarChar, 255).Value = nd.MatKhau ?? (object)DBNull.Value;
                    cmd.Parameters.Add("salt", OleDbType.VarChar, 255).Value = nd.Salt ?? (object)DBNull.Value;
                    cmd.Parameters.Add("role", OleDbType.VarChar, 255).Value = nd.Role ?? "sales";
                    cmd.Parameters.Add("hoten", OleDbType.VarChar, 255).Value = nd.HoTen ?? (object)DBNull.Value;
                    cmd.Parameters.Add("active", OleDbType.Boolean).Value = nd.IsActive;
                    cmd.Parameters.Add("must", OleDbType.Boolean).Value = nd.MustChangePwd;
                    cmd.Parameters.Add("created", OleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("updated", OleDbType.Date).Value = DateTime.Now;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản (không cập nhật mật khẩu).
        /// </summary>
        public bool CapNhatThongTin(NguoiDung nd)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                // Kiểm tra tên trùng với tài khoản khác
                using (OleDbCommand check = new OleDbCommand(
                    "SELECT COUNT(*) FROM NGUOI_DUNG WHERE TEN_DANG_NHAP = ? AND ID <> ?", conn))
                {
                    check.Parameters.Add("ten", OleDbType.VarChar, 255).Value = nd.TenDangNhap;
                    check.Parameters.Add("id", OleDbType.Integer).Value = nd.Id;
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    if (count > 0)
                        throw new InvalidOperationException("Tên đăng nhập '" + nd.TenDangNhap + "' đã được sử dụng bởi tài khoản khác.");
                }

                using (OleDbCommand cmd = new OleDbCommand(
                    "UPDATE NGUOI_DUNG SET TEN_DANG_NHAP=?, ROLE=?, HOTEN=?, IS_ACTIVE=?, MUST_CHANGE_PWD=?, UPDATED_AT=? WHERE ID=?", conn))
                {
                    cmd.Parameters.Add("ten", OleDbType.VarChar, 255).Value = nd.TenDangNhap;
                    cmd.Parameters.Add("role", OleDbType.VarChar, 255).Value = nd.Role;
                    cmd.Parameters.Add("hoten", OleDbType.VarChar, 255).Value = nd.HoTen ?? (object)DBNull.Value;
                    cmd.Parameters.Add("active", OleDbType.Boolean).Value = nd.IsActive;
                    cmd.Parameters.Add("must", OleDbType.Boolean).Value = nd.MustChangePwd;
                    cmd.Parameters.Add("updated", OleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = nd.Id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cập nhật mật khẩu (hash + salt).
        /// </summary>
        public bool CapNhatMatKhau(int id, string hashMoi, string saltMoi)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "UPDATE NGUOI_DUNG SET MATKHAU=?, SALT=?, MUST_CHANGE_PWD=FALSE, UPDATED_AT=? WHERE ID=?", conn))
                {
                    cmd.Parameters.Add("mk", OleDbType.VarChar, 255).Value = hashMoi;
                    cmd.Parameters.Add("salt", OleDbType.VarChar, 255).Value = saltMoi;
                    cmd.Parameters.Add("updated", OleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Đặt lại mật khẩu và yêu cầu đổi mật khẩu khi đăng nhập tiếp theo.
        /// </summary>
        public bool DatLaiMatKhau(int id, string hashMoi, string saltMoi)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "UPDATE NGUOI_DUNG SET MATKHAU=?, SALT=?, MUST_CHANGE_PWD=TRUE, UPDATED_AT=? WHERE ID=?", conn))
                {
                    cmd.Parameters.Add("mk", OleDbType.VarChar, 255).Value = hashMoi;
                    cmd.Parameters.Add("salt", OleDbType.VarChar, 255).Value = saltMoi;
                    cmd.Parameters.Add("updated", OleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Khóa hoặc mở khóa tài khoản.
        /// </summary>
        public bool DatTrangThai(int id, bool isActive)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "UPDATE NGUOI_DUNG SET IS_ACTIVE=?, UPDATED_AT=? WHERE ID=?", conn))
                {
                    cmd.Parameters.Add("active", OleDbType.Boolean).Value = isActive;
                    cmd.Parameters.Add("updated", OleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Đếm số tài khoản Admin đang hoạt động.
        /// </summary>
        public int DemAdminDangHoatDong()
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "SELECT COUNT(*) FROM NGUOI_DUNG WHERE ROLE='admin' AND IS_ACTIVE=TRUE", conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        // Đọc một dòng từ reader thành đối tượng NguoiDung
        private NguoiDung DocNguoiDung(OleDbDataReader reader)
        {
            var nd = new NguoiDung();
            nd.Id = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : Convert.ToInt32(reader["ID"]);
            nd.TenDangNhap = reader.IsDBNull(reader.GetOrdinal("TEN_DANG_NHAP")) ? "" : reader["TEN_DANG_NHAP"].ToString();
            nd.MatKhau = reader.IsDBNull(reader.GetOrdinal("MATKHAU")) ? "" : reader["MATKHAU"].ToString();
            
            // Cột SALT có thể không tồn tại ở database cũ - xử lý an toàn
            try { nd.Salt = reader.IsDBNull(reader.GetOrdinal("SALT")) ? null : reader["SALT"].ToString(); }
            catch { nd.Salt = null; }

            nd.Role = reader.IsDBNull(reader.GetOrdinal("ROLE")) ? "sales" : reader["ROLE"].ToString();
            nd.HoTen = reader.IsDBNull(reader.GetOrdinal("HOTEN")) ? "" : reader["HOTEN"].ToString();
            
            try { nd.IsActive = reader.IsDBNull(reader.GetOrdinal("IS_ACTIVE")) ? true : Convert.ToBoolean(reader["IS_ACTIVE"]); }
            catch { nd.IsActive = true; }
            
            try { nd.MustChangePwd = reader.IsDBNull(reader.GetOrdinal("MUST_CHANGE_PWD")) ? false : Convert.ToBoolean(reader["MUST_CHANGE_PWD"]); }
            catch { nd.MustChangePwd = false; }

            try { nd.CreatedAt = reader.IsDBNull(reader.GetOrdinal("CREATED_AT")) ? (DateTime?)null : Convert.ToDateTime(reader["CREATED_AT"]); }
            catch { nd.CreatedAt = null; }

            try { nd.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UPDATED_AT")) ? (DateTime?)null : Convert.ToDateTime(reader["UPDATED_AT"]); }
            catch { nd.UpdatedAt = null; }

            return nd;
        }

        public NguoiDung XoaNguoiDung (int id)
        {
            using (OleDbConnection conn = GetConnection()) {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(
                    "DELETE FROM NGUOI_DUNG WHERE ID=?", conn)) {
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0 ? new NguoiDung { Id = id } : null;
                }
            }
        }
    }
}

