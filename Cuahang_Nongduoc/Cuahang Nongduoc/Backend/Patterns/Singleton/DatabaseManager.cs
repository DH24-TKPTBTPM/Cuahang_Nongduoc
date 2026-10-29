using System;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.Patterns.Singleton
{
    /// <summary>
    /// Singleton Pattern (Creational): Quản lý tập trung kết nối CSDL Microsoft Access (OLEDB),
    /// đảm bảo duy nhất 1 thể hiện (Thread-safe) trong toàn bộ ứng dụng, tránh rò rỉ kết nối.
    /// </summary>
    public sealed class DatabaseManager
    {
        private static volatile DatabaseManager _instance = null;
        private static readonly object _lock = new object();
        private OleDbConnection _connection;
        private string _connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cuahang.dll;";

        private DatabaseManager()
        {
            _connection = new OleDbConnection(_connectionString);
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

        public OleDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new OleDbConnection(_connectionString);
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

        public OleDbConnection GetOpenConnection()
        {
            if (_connection == null)
            {
                _connection = new OleDbConnection(_connectionString);
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
