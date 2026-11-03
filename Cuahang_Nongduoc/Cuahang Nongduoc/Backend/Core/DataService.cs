using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
    // ==================== [R03] REFACTORING ====================
    // Lỗi (Code smell) : Hardcoded Configuration & Inappropriate Intimacy
    // Kỹ thuật         : Move Field + Encapsulate Field
    // Trước            : public static String m_ConnectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cuahang.dll;";
    //                    (public, lớp nào cũng đọc/ghi đè được, đổi máy là phải sửa code)
    // Sau              : chuỗi kết nối nằm trong App.config (CuahangAccess, CuahangSqlServer),
    //                    đọc qua ConfigurationManager, field là private, chỉ có property chỉ-đọc.
    //                    DataService nhận DbCommand: OleDbCommand -> Access (code cũ chạy như cũ),
    //                    SqlCommand -> SQL Server, để BE2/BE3 chuyển từng Factory sang SqlClient (R28).
    // Tác giả          : tuananh (Team Lead, làm thay BE1)
    // Hỗ trợ           : Claude (AI)
    // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
    // Ngày chỉnh sửa   : 02/11/2026
    // Ngày kiểm thử    : 03/11/2026
    // ===========================================================
    public class DataService : DataTable
    {
        private const string ACCESS_KEY = "CuahangAccess";
        private const string SQLSERVER_KEY = "CuahangSqlServer";
        private const string ACCESS_DEFAULT = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cuahang.dll;";

        // Kết nối dùng chung toàn ứng dụng: 1 cho Access (code cũ), 1 cho SQL Server (code đã chuyển R28)
        private static OleDbConnection m_Connection;
        private static SqlConnection m_SqlConnection;

        // Lệnh SELECT gần nhất (dùng lại khi UpdateDataTable sinh lệnh INSERT/UPDATE/DELETE)
        private DbCommand m_Command;
        private DbDataAdapter m_DataAdapter;

        public DataService() { }

        /// <summary>Chuỗi kết nối Access đang dùng (chỉ đọc, lấy từ App.config).</summary>
        public static string ConnectString
        {
            get { return LayChuoiKetNoi(ACCESS_KEY, ACCESS_DEFAULT); }
        }

        /// <summary>Chuỗi kết nối SQL Server (chỉ đọc, lấy từ App.config).</summary>
        public static string SqlConnectString
        {
            get { return LayChuoiKetNoi(SQLSERVER_KEY, null); }
        }

        public DbCommand Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        private static string LayChuoiKetNoi(string ten, string macDinh)
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[ten];
            if (cs != null && !String.IsNullOrEmpty(cs.ConnectionString))
                return cs.ConnectionString;
            if (macDinh != null)
                return macDinh;
            throw new DataAccessException("Thiếu chuỗi kết nối '" + ten + "' trong App.config.", null);
        }

        // ==================== [R02] REFACTORING ====================
        // Lỗi (Code smell) : Error Hiding – nuốt ngoại lệ im lặng
        // Kỹ thuật         : Replace Error Code with Exception
        // Trước            : catch (Exception e) { String str = e.Message; }
        //                    -> truy vấn lỗi thì bảng trả về rỗng, không ai biết
        // Sau              : ném DataAccessException (giữ lỗi gốc ở InnerException) lên tầng trên
        // Tác giả          : tuananh (Team Lead, làm thay BE1)
        // Hỗ trợ           : Claude (AI)
        // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 02/11/2026
        // Ngày kiểm thử    : 03/11/2026
        // ===========================================================
        public void Load(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            m_Command = command;
            try
            {
                m_Command.Connection = LayKetNoi(command);
                m_DataAdapter = TaoAdapter(command);
                m_DataAdapter.SelectCommand = m_Command;

                this.Clear();
                m_DataAdapter.Fill(this);
            }
            catch (Exception e)
            {
                throw new DataAccessException("Lỗi nạp dữ liệu từ CSDL: " + command.CommandText, e);
            }
        }

        // ==================== [R01] REFACTORING ====================
        // Lỗi (Code smell) : Dead Code & NullReferenceException trong catch
        // Kỹ thuật         : Replace Nested Conditional with Guard Clauses
        // Trước            : if (m_Connection == null)          // IF không có ngoặc nhọn
        //                        m_Connection = new OleDbConnection(m_ConnectString);
        //                    ...
        //                    catch (Exception e) { m_Connection.Close(); return false; }  // m_Connection có thể null
        // Sau              : tách hàm LayKetNoiAccess() dùng guard clause, catch không đụng vào kết nối null,
        //                    lỗi được giữ trong LastError để Form hiển thị.
        // Tác giả          : tuananh (Team Lead, làm thay BE1)
        // Hỗ trợ           : Claude (AI)
        // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 02/11/2026
        // Ngày kiểm thử    : 03/11/2026
        // ===========================================================
        private static Exception m_LastError;

        /// <summary>Lỗi của lần OpenConnection() thất bại gần nhất (null nếu không có lỗi).</summary>
        public static Exception LastError
        {
            get { return m_LastError; }
        }

        public static bool OpenConnection()
        {
            try
            {
                LayKetNoiAccess();
                m_LastError = null;
                return true;
            }
            catch (Exception e)
            {
                m_LastError = e;
                return false;
            }
        }

        private static OleDbConnection LayKetNoiAccess()
        {
            if (m_Connection == null)
            {
                m_Connection = new OleDbConnection(ConnectString);
            }
            if (m_Connection.State != ConnectionState.Open)
            {
                m_Connection.Open();
            }
            return m_Connection;
        }

        private static SqlConnection LayKetNoiSql()
        {
            if (m_SqlConnection == null)
            {
                m_SqlConnection = new SqlConnection(SqlConnectString);
            }
            if (m_SqlConnection.State != ConnectionState.Open)
            {
                m_SqlConnection.Open();
            }
            return m_SqlConnection;
        }

        private static DbConnection LayKetNoi(DbCommand cmd)
        {
            if (cmd is SqlCommand) return LayKetNoiSql();
            return LayKetNoiAccess();
        }

        private static DbDataAdapter TaoAdapter(DbCommand cmd)
        {
            if (cmd is SqlCommand) return new SqlDataAdapter();
            return new OleDbDataAdapter();
        }

        private static DbCommandBuilder TaoCommandBuilder(DbDataAdapter adapter)
        {
            if (adapter is SqlDataAdapter) return new SqlCommandBuilder((SqlDataAdapter)adapter);
            return new OleDbCommandBuilder((OleDbDataAdapter)adapter);
        }

        /// <summary>Đóng các kết nối đang mở.</summary>
        public void CloseConnection()
        {
            if (m_Connection != null) m_Connection.Close();
            if (m_SqlConnection != null) m_SqlConnection.Close();
        }

        // ==================== [R05] REFACTORING ====================
        // Lỗi (Code smell) : Misleading Name & nuốt ngoại lệ trong catch
        // Kỹ thuật         : Rename Method
        // Trước            : public int ExecuteNoneQuery()   // thực chất là DataAdapter.Update(this)
        //                    catch (Exception e) { if (tr != null) tr.Rollback(); }   // lỗi bị nuốt, trả về 0
        // Sau              : đổi tên thành UpdateDataTable(); rollback rồi ném DataAccessException.
        //                    Đã đổi lời gọi trong 13 lớp DataLayer/*Factory.cs.
        // Tác giả          : tuananh (Team Lead, làm thay BE1)
        // Hỗ trợ           : Claude (AI)
        // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 02/11/2026
        // Ngày kiểm thử    : 03/11/2026
        // ===========================================================
        public int UpdateDataTable()
        {
            if (m_Command == null)
                throw new DataAccessException("Phải gọi Load() trước khi UpdateDataTable().", null);

            int result = 0;
            DbTransaction tr = null;
            try
            {
                DbConnection cn = LayKetNoi(m_Command);
                tr = cn.BeginTransaction();

                m_Command.Connection = cn;
                m_Command.Transaction = tr;

                m_DataAdapter = TaoAdapter(m_Command);
                m_DataAdapter.SelectCommand = m_Command;
                DbCommandBuilder builder = TaoCommandBuilder(m_DataAdapter);

                result = m_DataAdapter.Update(this);

                tr.Commit();
            }
            catch (Exception e)
            {
                if (tr != null) tr.Rollback();
                throw new DataAccessException("Lỗi lưu dữ liệu xuống CSDL.", e);
            }
            finally
            {
                if (m_Command != null) m_Command.Transaction = null;
            }
            return result;
        }

        /// <summary>Thực thi một lệnh INSERT/UPDATE/DELETE trong transaction.</summary>
        public int ExecuteNoneQuery(DbCommand cmd)
        {
            int result = 0;
            DbTransaction tr = null;
            try
            {
                DbConnection cn = LayKetNoi(cmd);
                tr = cn.BeginTransaction();
                cmd.Connection = cn;
                cmd.Transaction = tr;

                result = cmd.ExecuteNonQuery();

                this.AcceptChanges();
                tr.Commit();
            }
            catch
            {
                if (tr != null) tr.Rollback();
                throw;
            }
            return result;
        }

        // ==================== [R06] REFACTORING ====================
        // Lỗi (Code smell) : Bizarre Side Effect – hàm đọc lại xóa trạng thái thay đổi của bảng
        // Kỹ thuật         : Separate Query from Modifier
        // Trước            : result = cmd.ExecuteScalar();
        //                    this.AcceptChanges();   // làm mất cờ Added/Modified/Deleted của các dòng chưa lưu
        // Sau              : bỏ AcceptChanges(); ExecuteScalar chỉ đọc 1 giá trị, không đụng vào DataTable.
        // Tác giả          : tuananh (Team Lead, làm thay BE1)
        // Hỗ trợ           : Claude (AI)
        // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 02/11/2026
        // Ngày kiểm thử    : 03/11/2026
        // ===========================================================
        public object ExecuteScalar(DbCommand cmd)
        {
            object result = null;
            DbTransaction tr = null;
            try
            {
                DbConnection cn = LayKetNoi(cmd);
                tr = cn.BeginTransaction();
                cmd.Connection = cn;
                cmd.Transaction = tr;

                result = cmd.ExecuteScalar();

                tr.Commit();

                if (result == DBNull.Value)
                {
                    result = null;
                }
            }
            catch
            {
                if (tr != null) tr.Rollback();
                throw;
            }
            return result;
        }
    }
}
