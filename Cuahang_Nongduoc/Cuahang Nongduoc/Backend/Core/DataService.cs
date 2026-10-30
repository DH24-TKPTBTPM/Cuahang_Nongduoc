using System;
using System.Data;
using System.Data.SqlClient;
using CuahangNongduoc.Patterns.Singleton;

namespace CuahangNongduoc
{
    /// <summary>
    /// [REFACTORING GURU]
    /// - Code Smell: Duplicated Connection Management & Swallowing Exceptions
    /// - Kỹ thuật Refactoring (1 trong 64 mẫu): Extract Class (chuyển quản lý kết nối sang DatabaseManager) & Replace Error Code with Exception
    /// - Trước khi sửa: Dùng OleDbConnection kết nối file Access cuahang.dll, khối catch nuốt lỗi (String str = e.Message;).
    /// - Sau khi sửa: Chuyển sang SqlClient kết nối SQL Server qua Singleton DatabaseManager, xử lý Transaction và quăng lỗi rõ ràng.
    /// - Tác giả (Author): Bùi Lê Tuấn Anh
    /// - Hỗ trợ (Supporter): BE Team
    /// - Kiểm tra (Reviewer/Tester): QA Team
    /// - Ngày chỉnh sửa (Date): 30/10/2026
    /// </summary>
    public class DataService : DataTable
    {
        private SqlCommand m_Command;
        private SqlDataAdapter m_DataAdapter;

        public DataService() { }

        // Bổ sung thuộc tính ResultTable để tương thích với cả cách gọi m_Ds và m_Ds.ResultTable trong DataLayer
        public DataTable ResultTable
        {
            get { return this; }
        }

        public SqlCommand Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        public static bool OpenConnection()
        {
            try
            {
                SqlConnection conn = DatabaseManager.Instance.GetOpenConnection();
                return conn.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        public void CloseConnection()
        {
            DatabaseManager.Instance.CloseConnection();
        }

        public void Load(SqlCommand command)
        {
            m_Command = command;
            try
            {
                m_Command.Connection = DatabaseManager.Instance.GetOpenConnection();
                m_DataAdapter = new SqlDataAdapter(m_Command);
                this.Clear();
                m_DataAdapter.Fill(this);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi truy vấn SQL Server: " + ex.Message, ex);
            }
            }

        public int ExecuteNoneQuery()
        {
            int result = 0;
            SqlTransaction tr = null;
            try
            {
                SqlConnection conn = DatabaseManager.Instance.GetOpenConnection();
                tr = conn.BeginTransaction();

                m_Command.Connection = conn;
                m_Command.Transaction = tr;

                m_DataAdapter = new SqlDataAdapter(m_Command);
                SqlCommandBuilder builder = new SqlCommandBuilder(m_DataAdapter);
                result = m_DataAdapter.Update(this);

                tr.Commit();
            }
            catch (Exception ex)
            {
                if (tr != null) tr.Rollback();
                throw new Exception("Lỗi cập nhật dữ liệu SQL Server: " + ex.Message, ex);
            }
            return result;
        }

        public int ExecuteNoneQuery(SqlCommand cmd)
        {
            int result = 0;
            SqlTransaction tr = null;
            try
            {
                SqlConnection conn = DatabaseManager.Instance.GetOpenConnection();
                tr = conn.BeginTransaction();

                cmd.Connection = conn;
                cmd.Transaction = tr;

                result = cmd.ExecuteNonQuery();
                this.AcceptChanges();

                tr.Commit();
            }
            catch (Exception ex)
            {
                if (tr != null) tr.Rollback();
                throw new Exception("Lỗi thực thi lệnh SQL Server: " + ex.Message, ex);
            }
            return result;
        }

        public object ExecuteScalar(SqlCommand cmd)
        {
            object result = null;
            SqlTransaction tr = null;
            try
            {
                SqlConnection conn = DatabaseManager.Instance.GetOpenConnection();
                tr = conn.BeginTransaction();

                cmd.Connection = conn;
                cmd.Transaction = tr;

                result = cmd.ExecuteScalar();
                this.AcceptChanges();

                tr.Commit();
                if (result == DBNull.Value) result = null;
            }
            catch (Exception ex)
            {
                if (tr != null) tr.Rollback();
                throw new Exception("Lỗi ExecuteScalar SQL Server: " + ex.Message, ex);
            }
            return result;
        }
    }
}