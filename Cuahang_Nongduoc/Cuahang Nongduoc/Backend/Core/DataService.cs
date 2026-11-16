using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
namespace CuahangNongduoc
{
    public delegate void DataServiceTransactionAction(OleDbConnection connection, OleDbTransaction transaction);

    public class DataService : DataTable
    {
        // The connection to a database of this data service.
        private static OleDbConnection m_Connection;

        //
        public static String m_ConnectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cuahang.dll;";
        // The command to execute query or non-query command on a database of this data service.
        private OleDbCommand m_Command;

        // The data adapter to execute query on a database of this data service.
        private OleDbDataAdapter m_DataAdapter;

        public DataService() { }


        public OleDbCommand Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        public void Load(OleDbCommand command)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu.");
            m_Command = command;
            m_Command.Connection = m_Connection;
            m_DataAdapter = new OleDbDataAdapter();
            m_DataAdapter.SelectCommand = m_Command;
            this.Clear();
            m_DataAdapter.Fill(this);
        }


        public static bool OpenConnection()
        {
            try
            {
                if (m_Connection == null)
                    //m_Connection = new OleDbConnection("Data Source=LAPTOP07\\OleDbEXPRESS;Initial Catalog=VCB;Integrated Security=True;");
                    //m_Connection = new OleDbConnection("Data Source=localhost;Initial Catalog=phongkham;User ID=sa; Password=tvteo;");
                    m_Connection = new OleDbConnection(m_ConnectString);


                if (m_Connection.State == ConnectionState.Closed)
                    m_Connection.Open();
                return true;
            }
            catch
            {
                if (m_Connection != null) m_Connection.Close();
                return false;
            }

        }
        /// <summary>
        /// Closes the connection of this data service.
        /// </summary>
        public void CloseConnection()
        {
            m_Connection.Close();
        }

        /// <summary>
        /// Update DataTable
        /// </summary>
        /// <returns></returns>
        public int ExecuteNoneQuery()
        {
            int result = 0;
            OleDbTransaction tr = null;
            try
            {
                tr = m_Connection.BeginTransaction();

                m_Command.Connection = m_Connection;
                m_Command.Transaction = tr;

                m_DataAdapter = new OleDbDataAdapter();
                m_DataAdapter.SelectCommand = m_Command;

                OleDbCommandBuilder builder = new OleDbCommandBuilder(m_DataAdapter);

                result = m_DataAdapter.Update(this);


                tr.Commit();

            }
            catch
            {
                if (tr != null)
                {
                    try { tr.Rollback(); }
                    catch { }
                }
                throw;
            }
            return result;
        }
        /// <summary>
        /// Thuc thi mot command
        /// </summary>
        /// <param name="command">OleDb hay Store Procedure</param>
        /// <returns></returns>
        public int ExecuteNoneQuery(OleDbCommand cmd)
        {

            int result = 0;
            OleDbTransaction tr = null;

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
                    try { tr.Rollback(); }
                    catch { }
                }
                throw;
            }
            return result;

        }

        public static void ExecuteTransaction(DataServiceTransactionAction action)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu.");

            OleDbTransaction transaction = m_Connection.BeginTransaction();
            try
            {
                action(m_Connection, transaction);
                transaction.Commit();
            }
            catch
            {
                try { transaction.Rollback(); }
                catch { }
                throw;
            }
        }

        public static void EnsureColumn(string tableName, string columnName, string columnType)
        {
            if (!OpenConnection()) throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu Access.");
            using (OleDbCommand check = m_Connection.CreateCommand())
            {
                check.CommandText = "SELECT * FROM [" + tableName + "] WHERE 1=0";
                using (OleDbDataReader reader = check.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        if (String.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase)) return;
                }
            }
            using (OleDbCommand alter = m_Connection.CreateCommand())
            {
                alter.CommandText = "ALTER TABLE [" + tableName + "] ADD COLUMN [" + columnName + "] " + columnType;
                alter.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(OleDbCommand cmd)
        {
            object result = null;
            OleDbTransaction tr = null;

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
                    try { tr.Rollback(); }
                    catch { }
                }
                throw;
            }
            return result;
        }


        /// <summary>
        /// Lưu nhiều DataTable và tính lại tồn kho/giá nhập bình quân trong cùng transaction.
        /// Dữ liệu trong bộ nhớ chỉ AcceptChanges sau khi database Commit thành công.
        /// </summary>
        public static int ExecuteDataTableTransaction(IList<string> affectedProductIds, params DataService[] dataTables)
        {
            if (!OpenConnection())
                throw new InvalidOperationException("Không thể kết nối cơ sở dữ liệu Access.");

            OleDbTransaction transaction = null;
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
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                        {
                            dataTable.m_Command.Connection = m_Connection;
                            dataTable.m_Command.Transaction = transaction;
                            adapter.SelectCommand = dataTable.m_Command;
                            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            using (OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter))
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

        // Dispose transaction an toàn, không ném ngoại lệ.
        private static void DisposeSafe(OleDbTransaction transaction)
        {
            if (transaction == null)
                return;
            // Không để lỗi dọn dẹp transaction che mất ngoại lệ gốc của thao tác lưu.
            try { transaction.Dispose(); }
            catch { }
        }


        // Dispose transaction an toàn, không ném ngoại lệ.
        private static void RollbackSafe(OleDbTransaction transaction)
        {
            if (transaction == null)
                return;
            try { transaction.Rollback(); }
            catch { }
        }

        // Rollback transaction an toàn, không ném ngoại lệ.
        private static void GanTransaction(OleDbCommand command, OleDbTransaction transaction)
        {
            if (command == null)
                return;
            command.Connection = m_Connection;
            command.Transaction = transaction;
        }

        //Kiểm tra lô đã bán trong transaction, nếu có thì ném ngoại lệ.
        private static void KiemTraLoDaBanTrongTransaction(DataService dataTable, OleDbTransaction transaction)
        {
            if (dataTable == null || dataTable.m_Command == null ||
                dataTable.m_Command.CommandText.IndexOf("MA_SAN_PHAM", StringComparison.OrdinalIgnoreCase) < 0 ||
                !dataTable.Columns.Contains("ID"))
                return;

            DataRow[] rowsDeleted = dataTable.Select(String.Empty, String.Empty, DataViewRowState.Deleted);
            foreach (DataRow deletedRow in rowsDeleted)
            {
                string idLo = Convert.ToString(deletedRow["ID", DataRowVersion.Original]);
                if (String.IsNullOrEmpty(idLo))
                    continue;
                using (OleDbCommand checkLot = new OleDbCommand(
                    "SELECT COUNT(*) FROM CHI_TIET_PHIEU_BAN WHERE ID_MA_SAN_PHAM=?", m_Connection, transaction))
                {
                    checkLot.Parameters.Add("idLo", OleDbType.VarChar, 50).Value = idLo;
                    object value = checkLot.ExecuteScalar();
                    if (value != null && value != DBNull.Value && Convert.ToInt64(value) > 0)
                        throw new InvalidOperationException("Không thể xóa lô " + idLo + " vì lô đã được sử dụng trong hóa đơn bán.");
                }
            }
        }

        /// <summary>
        /// Gán transaction cho command, nếu command không null.
        /// </summary>
        /// <param name="affectedProductIds"></param>
        /// <param name="transaction"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private static void TinhLaiTonKhoTrongTransaction(IList<string> affectedProductIds, OleDbTransaction transaction)
        {
            if (affectedProductIds == null)
                return;

            List<string> processed = new List<string>();
            foreach (string idSanPham in affectedProductIds)
            {
                if (String.IsNullOrEmpty(idSanPham) || processed.Contains(idSanPham))
                    continue;
                processed.Add(idSanPham);

                long tongSoLuong = 0;
                decimal tongGiaTri = 0M;
                using (OleDbCommand selectLots = new OleDbCommand(
                    "SELECT SO_LUONG, DON_GIA_NHAP FROM MA_SAN_PHAM WHERE ID_SAN_PHAM=?", m_Connection, transaction))
                {
                    selectLots.Parameters.Add("id", OleDbType.VarChar, 50).Value = idSanPham;
                    using (OleDbDataReader reader = selectLots.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long soLuong = reader.IsDBNull(0) ? 0 : Convert.ToInt64(reader.GetValue(0));
                            long donGia = reader.IsDBNull(1) ? 0 : Convert.ToInt64(reader.GetValue(1));
                            // Lô bị bán vượt tồn (số lượng âm - dữ liệu cũ) không được làm hỏng việc tính lại tồn kho của cả sản phẩm:
                            // coi như 0 khi cộng tồn thay vì chặn mọi thao tác nhập/xóa của sản phẩm đó.
                            if (soLuong < 0)
                                soLuong = 0;
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
                    using (OleDbCommand updateProduct = new OleDbCommand(
                        "UPDATE SAN_PHAM SET SO_LUONG=?, DON_GIA_NHAP=? WHERE ID=?", m_Connection, transaction))
                    {
                        updateProduct.Parameters.Add("soLuong", OleDbType.Integer).Value = Convert.ToInt32(tongSoLuong);
                        // Giá nhập bình quân lưu theo đơn vị tiền nguyên như các lớp nghiệp vụ hiện có.
                        updateProduct.Parameters.Add("giaNhap", OleDbType.Currency).Value = Convert.ToDecimal((long)(tongGiaTri / tongSoLuong));
                        updateProduct.Parameters.Add("id", OleDbType.VarChar, 50).Value = idSanPham;
                        rowsUpdated = updateProduct.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Khi tồn kho bằng 0, giữ nguyên giá nhập gần nhất để không xóa thông tin tham khảo.
                    using (OleDbCommand updateProduct = new OleDbCommand(
                        "UPDATE SAN_PHAM SET SO_LUONG=? WHERE ID=?", m_Connection, transaction))
                    {
                        updateProduct.Parameters.Add("soLuong", OleDbType.Integer).Value = 0;
                        updateProduct.Parameters.Add("id", OleDbType.VarChar, 50).Value = idSanPham;
                        rowsUpdated = updateProduct.ExecuteNonQuery();
                    }
                }
                if (rowsUpdated <= 0)
                    throw new InvalidOperationException("Không tìm thấy sản phẩm " + idSanPham + " để đồng bộ tồn kho.");
            }
        }
    }
}
