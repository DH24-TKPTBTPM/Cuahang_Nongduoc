using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace CuahangNongduoc.DataLayer
{
    public class DonViTinhFactory
    {
        DataService m_Ds = new DataService();

        public const string DonViDangDuocSuDung = "Không thể xóa đơn vị tính này vì đang được sử dụng bởi sản phẩm.";
        private readonly Dictionary<DataRow, object[]> duLieuTruocXoa = new Dictionary<DataRow, object[]>();
        private bool daXoaTrongDatabase;

        public DonViTinhFactory()
        {
            m_Ds.RowDeleting += KiemTraTruocKhiXoaDong;
        }

        public bool DangDuocSuDung(int id)
        {
            using (OleDbConnection connection = new OleDbConnection(DataService.m_ConnectString))
            {
                connection.Open();
                return DangDuocSuDung(id, connection, null);
            }
        }

        private bool DangDuocSuDung(int id, OleDbConnection connection, OleDbTransaction transaction)
        {
            using (OleDbCommand cmd = new OleDbCommand(
                "SELECT COUNT(*) FROM SAN_PHAM WHERE ID_DON_VI_TINH = ?", connection, transaction))
            {
                cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
                return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
            }
        }

        public bool LayIdDaLuu(DataRow row, out int id)
        {
            id = 0;
            if (row == null || row.Table != m_Ds ||
                !row.HasVersion(DataRowVersion.Original)) return false;
            object value = row["ID", DataRowVersion.Original];
            if (value == DBNull.Value || !Int32.TryParse(Convert.ToString(value), out id))
                throw new InvalidOperationException("ID đơn vị tính không hợp lệ.");
            return true;
        }

        private void KiemTraTruocKhiXoaDong(object sender, DataRowChangeEventArgs e)
        {
            if (daXoaTrongDatabase) return;
            int id;
            if (LayIdDaLuu(e.Row, out id) && DangDuocSuDung(id))
                throw new InvalidOperationException(DonViDangDuocSuDung);
            // Giữ cả giá trị đang sửa để phục hồi riêng dòng xóa nếu lưu thất bại.
            duLieuTruocXoa[e.Row] = e.Row.ItemArray;
        }

        public void Xoa(DataRow row)
        {
            if (row == null || row.Table != m_Ds || row.RowState == DataRowState.Detached ||
                row.RowState == DataRowState.Deleted)
                throw new InvalidOperationException("Chưa chọn đơn vị tính hợp lệ.");
            int id;
            if (LayIdDaLuu(row, out id))
            {
                using (OleDbConnection connection = new OleDbConnection(DataService.m_ConnectString))
                {
                    connection.Open();
                    using (OleDbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            if (DangDuocSuDung(id, connection, transaction))
                                throw new InvalidOperationException(DonViDangDuocSuDung);
                            using (OleDbCommand cmd = new OleDbCommand(
                                "DELETE FROM DON_VI_TINH WHERE ID = ?", connection, transaction))
                            {
                                cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
                                if (cmd.ExecuteNonQuery() != 1)
                                    throw new DBConcurrencyException("Đơn vị tính đã thay đổi hoặc không còn tồn tại. Hãy mở lại form sau khi lưu các thay đổi khác.");
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            try { transaction.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
            }
            // Chỉ cập nhật dòng được chọn sau khi database đã commit; không lưu các dòng khác.
            daXoaTrongDatabase = true;
            try
            {
                row.Delete();
                if (row.RowState == DataRowState.Deleted) row.AcceptChanges();
                duLieuTruocXoa.Remove(row);
            }
            finally { daXoaTrongDatabase = false; }
        }

        public DataTable DanhsachDVT()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DON_VI_TINH");
            m_Ds.Load(cmd);

            // Access sinh ID AutoNumber; dùng ID tạm âm cho dòng mới trong bộ nhớ.
            m_Ds.Columns["ID"].AutoIncrement = true;
            m_Ds.Columns["ID"].AutoIncrementSeed = -1;
            m_Ds.Columns["ID"].AutoIncrementStep = -1;

            return m_Ds;
        }


        public DataTable LayDVT(int id)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM DON_VI_TINH WHERE ID = @id");
            cmd.Parameters.Add("id", OleDbType.Integer).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }
        public bool Save()
        {
            DataRow[] changes = m_Ds.Select(null, null, DataViewRowState.Added |
                DataViewRowState.ModifiedCurrent | DataViewRowState.Deleted);
            if (changes.Length == 0) return false;
            // Adapter làm việc trên bản sao để rollback không làm đổi ID/trạng thái dòng gốc.
            DataTable pending = m_Ds.Clone();
            foreach (DataRow row in changes) pending.ImportRow(row);
            try
            {
                using (OleDbConnection connection = new OleDbConnection(DataService.m_ConnectString))
                {
                    connection.Open();
                    using (OleDbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Kiểm tra toàn bộ trước khi adapter gửi bất kỳ lệnh sửa/xóa nào.
                            foreach (DataRow row in changes)
                            {
                                int id;
                                if (row.RowState == DataRowState.Deleted)
                                {
                                    if (!LayIdDaLuu(row, out id))
                                        throw new InvalidOperationException("ID đơn vị tính không hợp lệ.");
                                    if (DangDuocSuDung(id, connection, transaction))
                                        throw new InvalidOperationException(DonViDangDuocSuDung);
                                }
                                else
                                {
                                    if (row.RowState != DataRowState.Added &&
                                        (row.IsNull("ID") || !Int32.TryParse(Convert.ToString(row["ID"]), out id)))
                                        throw new InvalidOperationException("ID đơn vị tính không hợp lệ.");
                                    string name = Convert.ToString(row["TEN_DON_VI"]).Trim();
                                    if (name.Length == 0 || name.Length > 50)
                                        throw new InvalidOperationException("Tên đơn vị tính phải có từ 1 đến 50 ký tự.");
                                    if (LayIdDaLuu(row, out id) && !Object.Equals(row["ID"], row["ID", DataRowVersion.Original]))
                                        throw new InvalidOperationException("Không được thay đổi ID đơn vị tính.");
                                }
                            }
                            using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM DON_VI_TINH", connection))
                            using (OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter))
                            {
                                adapter.SelectCommand.Transaction = transaction;
                                builder.QuotePrefix = "[";
                                builder.QuoteSuffix = "]";
                                adapter.InsertCommand = builder.GetInsertCommand();
                                adapter.UpdateCommand = builder.GetUpdateCommand();
                                adapter.DeleteCommand = builder.GetDeleteCommand();
                                adapter.InsertCommand.Transaction = transaction;
                                adapter.UpdateCommand.Transaction = transaction;
                                adapter.DeleteCommand.Transaction = transaction;
                                adapter.AcceptChangesDuringUpdate = false;
                                adapter.RowUpdated += delegate(object sender, OleDbRowUpdatedEventArgs e)
                                {
                                    if (e.StatementType != StatementType.Insert || e.Status != UpdateStatus.Continue) return;
                                    using (OleDbCommand identity = new OleDbCommand("SELECT @@IDENTITY", connection, transaction))
                                        e.Row["ID"] = Convert.ToInt32(identity.ExecuteScalar());
                                };
                                adapter.Update(pending);
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            try { transaction.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
                for (int i = 0; i < changes.Length; i++)
                    if (changes[i].RowState == DataRowState.Added)
                        changes[i]["ID"] = pending.Rows[i]["ID"];
                m_Ds.AcceptChanges();
                duLieuTruocXoa.Clear();
                return true;
            }
            catch
            {
                KhoiPhucDongXoa();
                throw;
            }
        }

        private void KhoiPhucDongXoa()
        {
            foreach (DataRow row in m_Ds.Select(null, null, DataViewRowState.Deleted))
            {
                row.RejectChanges();
                object[] values;
                if (duLieuTruocXoa.TryGetValue(row, out values)) row.ItemArray = values;
            }
            duLieuTruocXoa.Clear();
        }

        /// <summary>
        /// Hoan nguyen toan bo thay doi chua commit trong DataTable ve trang thai ban dau.
        /// </summary>
        public void RejectChanges()
        {
            m_Ds.RejectChanges();
            duLieuTruocXoa.Clear();
        }
    }
}
