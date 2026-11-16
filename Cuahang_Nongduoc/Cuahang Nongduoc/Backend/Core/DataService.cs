using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
    public delegate void DataServiceTransactionAction(SqlConnection connection, SqlTransaction transaction);

    public class DataService : DataTable
    {
        private static SqlConnection m_Connection;
        private static string _connectString = null;

        public static string m_ConnectString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectString))
                {
                    try
                    {
                        ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["CuahangSqlServer"];
                        if (cs != null && !string.IsNullOrEmpty(cs.ConnectionString))
                        {
                            _connectString = cs.ConnectionString;
                        }
                    }
                    catch { }

                    if (string.IsNullOrEmpty(_connectString))
                    {
                        _connectString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CuahangNongduoc;Integrated Security=True;TrustServerCertificate=True";
                    }
                }
                return _connectString;
            }
            set
            {
                _connectString = value;
                if (m_Connection != null)
                {
                    try { m_Connection.Close(); } catch { }
                    m_Connection = null;
                }
            }
        }

        private SqlCommand m_Command;
        private SqlDataAdapter m_DataAdapter;

        public DataService() { }

        /// <summary>
        /// Bảng kết quả (hỗ trợ các DAO gọi m_Ds.ResultTable).
        /// Do DataService kế thừa DataTable nên trả về chính nó.
        /// </summary>
        public DataTable ResultTable
        {
            get { return this; }
        }

        public SqlCommand Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        private static Exception m_LastError;
        public static Exception LastError
        {
            get { return m_LastError; }
        }

        public static bool OpenConnection()
        {
            try
            {
                if (m_Connection == null)
                {
                    m_Connection = new SqlConnection(m_ConnectString);
                }

                if (m_Connection.State == ConnectionState.Closed || m_Connection.State == ConnectionState.Broken)
                {
                    m_Connection.Open();
                }
                m_LastError = null;
                return true;
            }
            catch (Exception ex)
            {
                m_LastError = ex;
                if (m_Connection != null)
                {
                    try { m_Connection.Close(); } catch { }
                }
                return false;
            }
        }

        public void CloseConnection()
        {
            if (m_Connection != null && m_Connection.State == ConnectionState.Open)
            {
                try { m_Connection.Close(); } catch { }
            }
        }

        public void Load(SqlCommand command)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu SQL Server: " + (m_LastError != null ? m_LastError.Message : "Lỗi không xác định"));

            m_Command = command;
            m_Command.Connection = m_Connection;
            m_DataAdapter = new SqlDataAdapter();
            m_DataAdapter.SelectCommand = m_Command;
            this.Clear();
            m_DataAdapter.Fill(this);
        }

        /// <summary>
        /// Lưu thay đổi của DataTable xuống CSDL SQL Server bằng SqlDataAdapter và SqlCommandBuilder.
        /// </summary>
        public int ExecuteNoneQuery()
        {
            return UpdateDataTable();
        }

        public int UpdateDataTable()
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu SQL Server.");

            int result = 0;
            SqlTransaction tr = null;
            try
            {
                tr = m_Connection.BeginTransaction();

                m_Command.Connection = m_Connection;
                m_Command.Transaction = tr;

                m_DataAdapter = new SqlDataAdapter();
                m_DataAdapter.SelectCommand = m_Command;

                SqlCommandBuilder builder = new SqlCommandBuilder(m_DataAdapter);
                builder.QuotePrefix = "[";
                builder.QuoteSuffix = "]";

                m_DataAdapter.InsertCommand = builder.GetInsertCommand();
                m_DataAdapter.UpdateCommand = builder.GetUpdateCommand();
                m_DataAdapter.DeleteCommand = builder.GetDeleteCommand();

                m_DataAdapter.InsertCommand.Transaction = tr;
                m_DataAdapter.UpdateCommand.Transaction = tr;
                m_DataAdapter.DeleteCommand.Transaction = tr;

                result = m_DataAdapter.Update(this);

                tr.Commit();
            }
            catch
            {
                if (tr != null)
                {
                    try { tr.Rollback(); } catch { }
                }
                throw;
            }
            return result;
        }

        /// <summary>
        /// Thực thi lệnh INSERT/UPDATE/DELETE không trả về dữ liệu.
        /// </summary>
        public int ExecuteNoneQuery(SqlCommand cmd)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu SQL Server.");

            int result = 0;
            SqlTransaction tr = null;
            try
            {
                tr = m_Connection.BeginTransaction();
                cmd.Connection = m_Connection;
                cmd.Transaction = tr;

                result = cmd.ExecuteNonQuery();

                this.AcceptChanges();
                tr.Commit();
            }
            catch
            {
                if (tr != null)
                {
                    try { tr.Rollback(); } catch { }
                }
                throw;
            }
            return result;
        }

        public static void ExecuteTransaction(DataServiceTransactionAction action)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu SQL Server.");

            SqlTransaction transaction = m_Connection.BeginTransaction();
            try
            {
                action(m_Connection, transaction);
                transaction.Commit();
            }
            catch
            {
                try { transaction.Rollback(); } catch { }
                throw;
            }
        }

        public object ExecuteScalar(SqlCommand cmd)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu SQL Server.");

            object result = null;
            SqlTransaction tr = null;
            try
            {
                tr = m_Connection.BeginTransaction();
                cmd.Connection = m_Connection;
                cmd.Transaction = tr;

                result = cmd.ExecuteScalar();

                this.AcceptChanges();
                tr.Commit();

                if (result == DBNull.Value)
                {
                    result = null;
                }
            }
            catch
            {
                if (tr != null)
                {
                    try { tr.Rollback(); } catch { }
                }
                throw;
            }
            return result;
        }

        public static void EnsureColumn(string tableName, string columnName, string columnType)
        {
            if (!OpenConnection()) throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu SQL Server.");
            using (SqlCommand check = m_Connection.CreateCommand())
            {
                check.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@tbl AND COLUMN_NAME=@col";
                check.Parameters.AddWithValue("@tbl", tableName);
                check.Parameters.AddWithValue("@col", columnName);
                int count = Convert.ToInt32(check.ExecuteScalar());
                if (count > 0) return;
            }
            using (SqlCommand alter = m_Connection.CreateCommand())
            {
                string sqlType = columnType;
                if (columnType.Equals("LONG", StringComparison.OrdinalIgnoreCase)) sqlType = "INT";
                else if (columnType.Equals("CURRENCY", StringComparison.OrdinalIgnoreCase)) sqlType = "DECIMAL(18,2)";
                alter.CommandText = "ALTER TABLE [" + tableName + "] ADD [" + columnName + "] " + sqlType;
                alter.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Lưu nhiều DataTable và tính lại tồn kho/giá nhập bình quân trong cùng transaction.
        /// </summary>
        public static int ExecuteDataTableTransaction(IList<string> affectedProductIds, params DataService[] dataTables)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu SQL Server.");

            SqlTransaction transaction = null;
            List<DataService> changedTables = new List<DataService>();
            int result = 0;
            try
            {
                transaction = m_Connection.BeginTransaction();
                if (dataTables != null)
                {
                    foreach (DataService dataTable in dataTables)
                    {
                        if (dataTable == null || dataTable.GetChanges() == null)
                            continue;
                        if (dataTable.m_Command == null)
                            throw new InvalidOperationException("Không có câu lệnh SELECT để lưu một bảng dữ liệu.");

                        KiemTraLoDaBanTrongTransaction(dataTable, transaction);
                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            dataTable.m_Command.Connection = m_Connection;
                            dataTable.m_Command.Transaction = transaction;
                            adapter.SelectCommand = dataTable.m_Command;
                            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            using (SqlCommandBuilder builder = new SqlCommandBuilder(adapter))
                            {
                                builder.QuotePrefix = "[";
                                builder.QuoteSuffix = "]";
                                adapter.InsertCommand = builder.GetInsertCommand();
                                adapter.UpdateCommand = builder.GetUpdateCommand();
                                adapter.DeleteCommand = builder.GetDeleteCommand();
                                GanTransaction(adapter.InsertCommand, transaction);
                                GanTransaction(adapter.UpdateCommand, transaction);
                                GanTransaction(adapter.DeleteCommand, transaction);
                                adapter.AcceptChangesDuringUpdate = false;
                                result += adapter.Update(dataTable);
                            }
                        }
                        changedTables.Add(dataTable);
                    }
                }

                TinhLaiTonKhoTrongTransaction(affectedProductIds, transaction);
                transaction.Commit();
                foreach (DataService table in changedTables)
                    table.AcceptChanges();
                return result;
            }
            catch
            {
                RollbackSafe(transaction);
                throw;
            }
            finally
            {
                DisposeSafe(transaction);
            }
        }

        private static void DisposeSafe(SqlTransaction transaction)
        {
            if (transaction == null) return;
            try { transaction.Dispose(); } catch { }
        }

        private static void RollbackSafe(SqlTransaction transaction)
        {
            if (transaction == null) return;
            try { transaction.Rollback(); } catch { }
        }

        private static void GanTransaction(SqlCommand command, SqlTransaction transaction)
        {
            if (command == null) return;
            command.Connection = m_Connection;
            command.Transaction = transaction;
        }

        private static void KiemTraLoDaBanTrongTransaction(DataService dataTable, SqlTransaction transaction)
        {
            if (dataTable == null || dataTable.m_Command == null ||
                dataTable.m_Command.CommandText.IndexOf("MA_SAN_PHAM", StringComparison.OrdinalIgnoreCase) < 0 ||
                !dataTable.Columns.Contains("ID"))
                return;

            DataRow[] rowsDeleted = dataTable.Select(String.Empty, String.Empty, DataViewRowState.Deleted);
            foreach (DataRow deletedRow in rowsDeleted)
            {
                string idLo = Convert.ToString(deletedRow["ID", DataRowVersion.Original]);
                if (String.IsNullOrEmpty(idLo)) continue;
                using (SqlCommand checkLot = new SqlCommand(
                    "SELECT COUNT(*) FROM CHI_TIET_PHIEU_BAN WHERE ID_MA_SAN_PHAM = @idLo", m_Connection, transaction))
                {
                    checkLot.Parameters.Add("@idLo", SqlDbType.VarChar, 50).Value = idLo;
                    object value = checkLot.ExecuteScalar();
                    if (value != null && value != DBNull.Value && Convert.ToInt64(value) > 0)
                        throw new InvalidOperationException("Không thể xóa lô " + idLo + " vì lô đã được sử dụng trong hóa đơn bán.");
                }
            }
        }

        private static void TinhLaiTonKhoTrongTransaction(IList<string> affectedProductIds, SqlTransaction transaction)
        {
            if (affectedProductIds == null) return;

            List<string> processed = new List<string>();
            foreach (string idSanPham in affectedProductIds)
            {
                if (String.IsNullOrEmpty(idSanPham) || processed.Contains(idSanPham))
                    continue;
                processed.Add(idSanPham);

                long tongSoLuong = 0;
                decimal tongGiaTri = 0M;
                using (SqlCommand selectLots = new SqlCommand(
                    "SELECT SO_LUONG, DON_GIA_NHAP FROM MA_SAN_PHAM WHERE ID_SAN_PHAM = @id", m_Connection, transaction))
                {
                    selectLots.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idSanPham;
                    using (SqlDataReader reader = selectLots.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long soLuong = reader.IsDBNull(0) ? 0 : Convert.ToInt64(reader.GetValue(0));
                            long donGia = reader.IsDBNull(1) ? 0 : Convert.ToInt64(reader.GetValue(1));
                            if (soLuong < 0) soLuong = 0;
                            if (donGia < 0)
                                throw new InvalidOperationException("Đơn giá nhập của lô không được âm (sản phẩm " + idSanPham + ").");
                            tongSoLuong += soLuong;
                            tongGiaTri += (decimal)soLuong * donGia;
                            if (tongSoLuong > Int32.MaxValue)
                                throw new InvalidOperationException("Tổng tồn kho vượt giới hạn dữ liệu của sản phẩm " + idSanPham + ".");
                        }
                    }
                }

                int rowsUpdated;
                if (tongSoLuong > 0)
                {
                    using (SqlCommand updateProduct = new SqlCommand(
                        "UPDATE SAN_PHAM SET SO_LUONG = @soLuong, DON_GIA_NHAP = @giaNhap WHERE ID = @id", m_Connection, transaction))
                    {
                        updateProduct.Parameters.Add("@soLuong", SqlDbType.Int).Value = Convert.ToInt32(tongSoLuong);
                        updateProduct.Parameters.Add("@giaNhap", SqlDbType.Decimal).Value = (decimal)((long)(tongGiaTri / tongSoLuong));
                        updateProduct.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idSanPham;
                        rowsUpdated = updateProduct.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand updateProduct = new SqlCommand(
                        "UPDATE SAN_PHAM SET SO_LUONG = 0 WHERE ID = @id", m_Connection, transaction))
                    {
                        updateProduct.Parameters.Add("@id", SqlDbType.VarChar, 50).Value = idSanPham;
                        rowsUpdated = updateProduct.ExecuteNonQuery();
                    }
                }
                if (rowsUpdated <= 0)
                    throw new InvalidOperationException("Không tìm thấy sản phẩm " + idSanPham + " để đồng bộ tồn kho.");
            }
        }
    }
}
