using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.Patterns.Singleton
{
    /// <summary>
    /// [REFACTORING GURU]
    /// - Code Smell: Primitive Obsession & Hardcoded String
    /// - Kỹ thuật Refactoring (1 trong 64 mẫu): Replace Magic Number with Symbolic Constant / Move Field
    /// - Trước khi sửa: Dùng OleDbConnection, chuỗi kết nối gán cứng "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cuahang.dll;"
    /// - Sau khi sửa: Chuyển sang SqlConnection (SQL Server). Chuỗi kết nối đọc từ App.config (CuahangSqlServer),
    ///   không gán cứng trong code nữa.
    /// - Tác giả (Author): Bùi Lê Tuấn Anh
    /// - Hỗ trợ (Supporter): Claude (AI)
    /// - Kiểm tra (Reviewer/Tester): QA2 – Dư Thị Trúc Hòa
    /// - Ngày chỉnh sửa (Date): 04/11/2026
    /// </summary>
    public sealed class DatabaseManager
    {
        private const string SQLSERVER_KEY = "CuahangSqlServer";

        private static volatile DatabaseManager _instance = null;
        private static readonly object _lock = new object();
        private SqlConnection _connection;

        private DatabaseManager()
        {
        }

        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>Chuỗi kết nối SQL Server (chỉ đọc, lấy từ App.config).</summary>
        public string ConnectionString
        {
            get
            {
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[SQLSERVER_KEY];
                if (cs == null || String.IsNullOrEmpty(cs.ConnectionString))
                    throw new DataAccessException("Thiếu chuỗi kết nối '" + SQLSERVER_KEY + "' trong App.config.", null);
                return cs.ConnectionString;
            }
        }

        public SqlConnection GetOpenConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(ConnectionString);
            }
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
