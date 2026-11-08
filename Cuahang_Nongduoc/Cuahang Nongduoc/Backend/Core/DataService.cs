using System;
using System.Data;
using System.Data.SqlClient;
using CuahangNongduoc.DataLayer;
using CuahangNongduoc.Patterns.Singleton;

namespace CuahangNongduoc
{
    // ==================== [R03] REFACTORING ====================
    // Lỗi (Code smell) : Hardcoded Configuration & Inappropriate Intimacy
    // Kỹ thuật         : Move Field + Encapsulate Field
    // Trước            : public static String m_ConnectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cuahang.dll;";
    //                    (public, lớp nào cũng đọc/ghi đè được, đổi máy là phải sửa code)
    // Sau              : 100% chạy trên SQL Server. Chuỗi kết nối nằm trong App.config (CuahangSqlServer),
    //                    kết nối do Singleton DatabaseManager quản lý; DataService chỉ nhận SqlCommand.
    //                    Đã bỏ hẳn OleDb/Access (cuahang.dll).
    // Tác giả          : tuananh (Team Lead, làm thay BE1)
    // Hỗ trợ           : Claude (AI)
    // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
    // Ngày chỉnh sửa   : 04/11/2026
    // ===========================================================
    //
    // ==================== [R04] REFACTORING ====================
    // Lỗi (Code smell) : Refused Bequest – DataService kế thừa DataTable chỉ để chứa kết quả,
    //                    nhận về hàng trăm thành viên không dùng; DAO trả "m_Ds" ra ngoài
    //                    làm lộ cả lớp truy cập CSDL lên tầng trên
    // Kỹ thuật         : Replace Inheritance with Delegation
    // Trước            : public class DataService : DataTable { ... m_DataAdapter.Fill(this); ... }
    //                    DAO: return m_Ds;
    // Sau              : DataService giữ 1 DataTable riêng (m_Table), chỉ chuyển tiếp Rows/NewRow;
    //                    DAO: return m_Ds.ResultTable;
    // Tác giả          : BE1 – Nguyễn Tuấn Anh
    // Hỗ trợ           : Claude (AI)
    // Kiểm tra         : QA2 – <Họ tên>
    // Ngày chỉnh sửa   : 08/11/2026
    // ===========================================================
    public class DataService
    {
        private readonly DataTable m_Table = new DataTable();

        // Lệnh SELECT gần nhất (dùng lại khi UpdateDataTable sinh lệnh INSERT/UPDATE/DELETE)
        private SqlCommand m_Command;
        private SqlDataAdapter m_DataAdapter;

        public DataService() { }

        /// <summary>Bảng kết quả của lần Load() gần nhất.</summary>
        public DataTable ResultTable
        {
            get { return m_Table; }
        }

        /// <summary>Chuyển tiếp tới ResultTable.Rows (giữ cách gọi m_Ds.Rows.Add(row) của DAO).</summary>
        public DataRowCollection Rows
        {
            get { return m_Table.Rows; }
        }

        /// <summary>Chuyển tiếp tới ResultTable.NewRow().</summary>
        public DataRow NewRow()
        {
            return m_Table.NewRow();
        }

        public SqlCommand Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        // ==================== [R01] REFACTORING ====================
        // Lỗi (Code smell) : Dead Code & NullReferenceException trong catch
        // Kỹ thuật         : Replace Nested Conditional with Guard Clauses
        // Trước            : catch (Exception e) { m_Connection.Close(); return false; }  // m_Connection có thể null
        // Sau              : catch không đụng vào kết nối null, lỗi được giữ trong LastError để Form hiển thị.
        // Tác giả          : tuananh (Team Lead, làm thay BE1)
        // Hỗ trợ           : Claude (AI)
        // Kiểm tra         : QA2 – Dư Thị Trúc Hòa
        // Ngày chỉnh sửa   : 04/11/2026
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
                DatabaseManager.Instance.GetOpenConnection();
                m_LastError = null;
                return true;
            }
            catch (Exception e)
            {
                m_LastError = e;
                return false;
            }
        }

        /// <summary>Đóng kết nối đang mở.</summary>
        public void CloseConnection()
        {
            DatabaseManager.Instance.CloseConnection();
        }

        // ==================== [D08] UNIT OF WORK ====================
        // Khi đang ở trong 1 UnitOfWork: dùng chung transaction của nó, KHÔNG tự Commit/Rollback
        // (UnitOfWork quyết định). Ngoài UnitOfWork: tự mở transaction riêng như trước.
        // Tác giả: BE1 – Nguyễn Tuấn Anh | Hỗ trợ: Claude (AI) | Kiểm tra: QA2 – <Họ tên> | Ngày chỉnh sửa: 08/11/2026
        // ===========================================================
        private static SqlTransaction GanGiaoDich(SqlCommand cmd, out bool tuQuanLy)
        {
            SqlTransaction tr = UnitOfWork.CurrentTransaction;
            tuQuanLy = (tr == null);
            if (tuQuanLy)
            {
                tr = DatabaseManager.Instance.GetOpenConnection().BeginTransaction();
            }
            cmd.Connection = tr.Connection;
            cmd.Transaction = tr;
            return tr;
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
        // Ngày chỉnh sửa   : 04/11/2026
        // ===========================================================
        public void Load(SqlCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            m_Command = command;
            try
            {
                // Kết nối dùng chung: đang có transaction của UnitOfWork thì lệnh SELECT cũng phải gắn vào
                m_Command.Connection = DatabaseManager.Instance.GetOpenConnection();
                m_Command.Transaction = UnitOfWork.CurrentTransaction;
                m_DataAdapter = new SqlDataAdapter(m_Command);

                m_Table.Clear();
                m_DataAdapter.Fill(m_Table);
            }
            catch (Exception e)
            {
                throw new DataAccessException("Lỗi nạp dữ liệu từ CSDL: " + command.CommandText, e);
            }
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
        // Ngày chỉnh sửa   : 04/11/2026
        // ===========================================================
        public int UpdateDataTable()
        {
            if (m_Command == null)
                throw new DataAccessException("Phải gọi Load() trước khi UpdateDataTable().", null);

            int result = 0;
            bool tuQuanLy = false;
            SqlTransaction tr = null;
            try
            {
                tr = GanGiaoDich(m_Command, out tuQuanLy);

                m_DataAdapter = new SqlDataAdapter(m_Command);
                SqlCommandBuilder builder = new SqlCommandBuilder(m_DataAdapter);

                result = m_DataAdapter.Update(m_Table);

                if (tuQuanLy) tr.Commit();
            }
            catch (Exception e)
            {
                if (tuQuanLy && tr != null) tr.Rollback();
                throw new DataAccessException("Lỗi lưu dữ liệu xuống CSDL.", e);
            }
            finally
            {
                if (m_Command != null) m_Command.Transaction = null;
            }
            return result;
        }

        /// <summary>Thực thi một lệnh INSERT/UPDATE/DELETE trong transaction.</summary>
        public int ExecuteNoneQuery(SqlCommand cmd)
        {
            if (cmd == null) throw new ArgumentNullException("cmd");

            int result = 0;
            bool tuQuanLy = false;
            SqlTransaction tr = null;
            try
            {
                tr = GanGiaoDich(cmd, out tuQuanLy);

                result = cmd.ExecuteNonQuery();

                m_Table.AcceptChanges();
                if (tuQuanLy) tr.Commit();
            }
            catch (Exception e)
            {
                if (tuQuanLy && tr != null) tr.Rollback();
                throw new DataAccessException("Lỗi thực thi lệnh: " + cmd.CommandText, e);
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
        // Ngày chỉnh sửa   : 04/11/2026
        // ===========================================================
        public object ExecuteScalar(SqlCommand cmd)
        {
            if (cmd == null) throw new ArgumentNullException("cmd");

            object result = null;
            bool tuQuanLy = false;
            SqlTransaction tr = null;
            try
            {
                tr = GanGiaoDich(cmd, out tuQuanLy);

                result = cmd.ExecuteScalar();

                if (tuQuanLy) tr.Commit();

                if (result == DBNull.Value)
                {
                    result = null;
                }
            }
            catch (Exception e)
            {
                if (tuQuanLy && tr != null) tr.Rollback();
                throw new DataAccessException("Lỗi đọc giá trị: " + cmd.CommandText, e);
            }
            return result;
        }
    }
}
