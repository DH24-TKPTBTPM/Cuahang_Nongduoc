using System;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.Patterns.Singleton
{
    /// <summary>
    /// [REFACTORING GURU]
    /// - Code Smell: Primitive Obsession & Hardcoded String
    /// - Kỹ thuật Refactoring (1 trong 64 mẫu): Replace Magic Number with Symbolic Constant / Move Field
    /// - Trước khi sửa: Dùng OleDbConnection, chuỗi kết nối gán cứng "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cuahang.dll;"
    /// - Sau khi sửa: Chuyển sang SqlConnection (SQL Server) để đáp ứng yêu cầu mới. Đổi tên chuỗi kết nối.
    /// - Tác giả (Author): Bùi Lê Tuấn Anh
    /// - Hỗ trợ (Supporter): 
    /// - Kiểm tra (Reviewer/Tester): 
    /// - Ngày chỉnh sửa (Date): 30/10/2026
    /// </summary>
    public sealed class DatabaseManager
    {
        private static volatile DatabaseManager _instance = null;
        private static readonly object _lock = new object();
        private SqlConnection _connection;

        // Cập nhật chuỗi kết nối cho SQL Server Express (Sửa lại Data Source nếu tên server khác)
        private string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CuahangNongduoc;Integrated Security=True;";

        private DatabaseManager()
        {
            _connection = new SqlConnection(_connectionString);
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

        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                }
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public SqlConnection GetOpenConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
            }
            if (_connection.State == ConnectionState.Closed)
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