using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.Auth
{
    /// <summary>
    /// Data Access Layer cho bảng NGUOI_DUNG trên SQL Server.
    /// Tất cả truy vấn đều dùng tham số để tránh SQL Injection.
    /// </summary>
    public class NguoiDungData
    {
        private static string ConnectString
        {
            get { return DataService.m_ConnectString; }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectString);
        }

        /// <summary>
        /// Lấy tài khoản theo tên đăng nhập (không phân biệt hoa thường).
        /// </summary>
        public NguoiDung LayTheoTenDangNhap(string tenDangNhap)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT ID, TEN_DANG_NHAP, MATKHAU, SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT " +
                    "FROM NGUOI_DUNG WHERE TEN_DANG_NHAP = @tenDangNhap", conn))
                {
                    cmd.Parameters.Add("@tenDangNhap", SqlDbType.NVarChar, 255).Value = tenDangNhap;
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT ID, TEN_DANG_NHAP, MATKHAU, SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT " +
                    "FROM NGUOI_DUNG WHERE ID = @id", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return DocNguoiDung(reader);
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách người dùng.
        /// </summary>
        public List<NguoiDung> LayDanhSach()
        {
            var ds = new List<NguoiDung>();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT ID, TEN_DANG_NHAP, MATKHAU, SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT " +
                    "FROM NGUOI_DUNG ORDER BY ID ASC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ds.Add(DocNguoiDung(reader));
                        }
                    }
                }
            }
            return ds;
        }

        public List<NguoiDung> LayTatCa()
        {
            return LayDanhSach();
        }

        /// <summary>
        /// Thêm người dùng mới vào hệ thống.
        /// </summary>
        public bool Them(NguoiDung nd)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                // Kiểm tra trùng tên đăng nhập
                using (SqlCommand check = new SqlCommand(
                    "SELECT COUNT(*) FROM NGUOI_DUNG WHERE TEN_DANG_NHAP = @ten", conn))
                {
                    check.Parameters.Add("@ten", SqlDbType.NVarChar, 255).Value = nd.TenDangNhap;
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    if (count > 0)
                        throw new InvalidOperationException("Tên đăng nhập '" + nd.TenDangNhap + "' đã tồn tại.");
                }

                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO NGUOI_DUNG (TEN_DANG_NHAP, MATKHAU, SALT, ROLE, HOTEN, IS_ACTIVE, MUST_CHANGE_PWD, CREATED_AT, UPDATED_AT) " +
                    "VALUES (@ten, @mk, @salt, @role, @hoten, @active, @must, @created, @updated)", conn))
                {
                    cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 255).Value = nd.TenDangNhap;
                    cmd.Parameters.Add("@mk", SqlDbType.NVarChar, 255).Value = nd.MatKhau ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@salt", SqlDbType.NVarChar, 255).Value = nd.Salt ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@role", SqlDbType.NVarChar, 50).Value = nd.Role ?? "sales";
                    cmd.Parameters.Add("@hoten", SqlDbType.NVarChar, 255).Value = nd.HoTen ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = nd.IsActive;
                    cmd.Parameters.Add("@must", SqlDbType.Bit).Value = nd.MustChangePwd;
                    cmd.Parameters.Add("@created", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@updated", SqlDbType.DateTime).Value = DateTime.Now;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản (không cập nhật mật khẩu).
        /// </summary>
        public bool CapNhatThongTin(NguoiDung nd)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand check = new SqlCommand(
                    "SELECT COUNT(*) FROM NGUOI_DUNG WHERE TEN_DANG_NHAP = @ten AND ID <> @id", conn))
                {
                    check.Parameters.Add("@ten", SqlDbType.NVarChar, 255).Value = nd.TenDangNhap;
                    check.Parameters.Add("@id", SqlDbType.Int).Value = nd.Id;
                    int count = Convert.ToInt32(check.ExecuteScalar());
                    if (count > 0)
                        throw new InvalidOperationException("Tên đăng nhập '" + nd.TenDangNhap + "' đã được sử dụng bởi tài khoản khác.");
                }

                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE NGUOI_DUNG SET TEN_DANG_NHAP=@ten, ROLE=@role, HOTEN=@hoten, IS_ACTIVE=@active, MUST_CHANGE_PWD=@must, UPDATED_AT=@updated WHERE ID=@id", conn))
                {
                    cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 255).Value = nd.TenDangNhap;
                    cmd.Parameters.Add("@role", SqlDbType.NVarChar, 50).Value = nd.Role;
                    cmd.Parameters.Add("@hoten", SqlDbType.NVarChar, 255).Value = nd.HoTen ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = nd.IsActive;
                    cmd.Parameters.Add("@must", SqlDbType.Bit).Value = nd.MustChangePwd;
                    cmd.Parameters.Add("@updated", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = nd.Id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cập nhật mật khẩu (hash + salt).
        /// </summary>
        public bool CapNhatMatKhau(int id, string hashMoi, string saltMoi)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE NGUOI_DUNG SET MATKHAU=@mk, SALT=@salt, MUST_CHANGE_PWD=0, UPDATED_AT=@updated WHERE ID=@id", conn))
                {
                    cmd.Parameters.Add("@mk", SqlDbType.NVarChar, 255).Value = hashMoi;
                    cmd.Parameters.Add("@salt", SqlDbType.NVarChar, 255).Value = saltMoi;
                    cmd.Parameters.Add("@updated", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Đặt lại mật khẩu và yêu cầu đổi mật khẩu khi đăng nhập tiếp theo.
        /// </summary>
        public bool DatLaiMatKhau(int id, string hashMoi, string saltMoi)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE NGUOI_DUNG SET MATKHAU=@mk, SALT=@salt, MUST_CHANGE_PWD=1, UPDATED_AT=@updated WHERE ID=@id", conn))
                {
                    cmd.Parameters.Add("@mk", SqlDbType.NVarChar, 255).Value = hashMoi;
                    cmd.Parameters.Add("@salt", SqlDbType.NVarChar, 255).Value = saltMoi;
                    cmd.Parameters.Add("@updated", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Khóa hoặc mở khóa tài khoản.
        /// </summary>
        public bool DatTrangThai(int id, bool isActive)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE NGUOI_DUNG SET IS_ACTIVE=@active, UPDATED_AT=@updated WHERE ID=@id", conn))
                {
                    cmd.Parameters.Add("@active", SqlDbType.Bit).Value = isActive;
                    cmd.Parameters.Add("@updated", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Đếm số tài khoản Admin đang hoạt động.
        /// </summary>
        public int DemAdminDangHoatDong()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM NGUOI_DUNG WHERE ROLE='admin' AND IS_ACTIVE=1", conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private NguoiDung DocNguoiDung(SqlDataReader reader)
        {
            var nd = new NguoiDung();
            nd.Id = reader.IsDBNull(reader.GetOrdinal("ID")) ? 0 : Convert.ToInt32(reader["ID"]);
            nd.TenDangNhap = reader.IsDBNull(reader.GetOrdinal("TEN_DANG_NHAP")) ? "" : reader["TEN_DANG_NHAP"].ToString();
            nd.MatKhau = reader.IsDBNull(reader.GetOrdinal("MATKHAU")) ? "" : reader["MATKHAU"].ToString();
            
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

        public NguoiDung XoaNguoiDung(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM NGUOI_DUNG WHERE ID=@id", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0 ? new NguoiDung { Id = id } : null;
                }
            }
        }
    }
}
