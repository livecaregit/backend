using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;

namespace LC_Service
{
    public class ClassOLEDB
	{
        private bool _mIsConnected = false;
        private string _mErrorCode = string.Empty;
        private string _mErrorMessage = string.Empty;
        private string _mConnectionString = string.Empty;

        private readonly OleDbConnection _mConnection;
        private OleDbTransaction _mTransaction;

        public ClassOLEDB()
        {
            string sDBServer = string.Empty;
            string sDBName = string.Empty;
            string sDBUser = string.Empty;
            string sDBPassword = string.Empty;

#if DEBUG
            sDBServer = "livecare-testdb.cdh4z4yvbsq3.ap-northeast-2.rds.amazonaws.com";
            sDBName = "LIVECARE";
            sDBUser = "lcservice";
            sDBPassword = "!lcservice!";
#else
            sDBServer = "livecaredb.cdh4z4yvbsq3.ap-northeast-2.rds.amazonaws.com";
            sDBName = "LIVECARE";
            sDBUser = "lcappapi";
            sDBPassword = "!lcappapi!";
#endif
            _mConnection = new OleDbConnection();
            _mConnectionString = string.Format("Provider=SQLOLEDB;data source={0};Initial Catalog={1};user id={2};password={3}", sDBServer, sDBName, sDBUser, sDBPassword);
        }

        public void SetConnectionString(string pDBServer, string pDBName, string pDBUser, string pDBPassword)
        {
            _mConnectionString = string.Format("Provider=SQLOLEDB;data source={0};Initial Catalog={1};user id={2};password={3}", pDBServer, pDBName, pDBUser, pDBPassword);
        }

        public bool OpenDatabase()
        {
            try
            {
                _mConnection.ConnectionString = _mConnectionString;
                _mConnection.Open();

                _mIsConnected = true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;
                _mIsConnected = false;
            }

            return _mIsConnected;
        }

        public bool CloseDatabase()
        {
            try
            {
                if (_mIsConnected && _mConnection != null)
                {
                    _mConnection.Close();
                    _mConnection.Dispose();
                }

                _mIsConnected = false;

                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;
                _mIsConnected = false;

                return false;
            }
        }

        public bool ReopenDatabase(string pDBServer, string pDBName, string pDBUser, string pDBPassword)
        {
            _mIsConnected = false;

            try
            {
                if (CloseDatabase())
                {
                    _mConnectionString = string.Format("server={0}; database={1}; User ID={2}; Pwd={3};", pDBServer, pDBName, pDBUser, pDBPassword);
                    OpenDatabase();
                }
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;
            }

            return _mIsConnected;
        }

        public bool TransBegin()
        {
            try
            {
                _mTransaction = _mConnection.BeginTransaction();

                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;
                if (_mTransaction != null) _mTransaction.Rollback();

                return false;
            }
        }

        public bool TransCommit()
        {
            try
            {
                if (_mTransaction == null || _mTransaction.Connection == null) return true;

                _mTransaction.Commit();
                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;
                if (_mTransaction != null) _mTransaction.Rollback();

                return false;
            }
        }

        public bool TransRollback()
        {
            try
            {
                if (_mTransaction == null || _mTransaction.Connection == null) return true;

                _mTransaction.Rollback();
                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;
                if (_mTransaction != null) _mTransaction.Rollback();

                return false;
            }
        }

        public bool QueryOpen(string pQuery, ref OleDbDataReader pReader)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pQuery, _mConnection);
                else
                    command = new OleDbCommand(pQuery, _mConnection, _mTransaction);

                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 100;

                pReader = command.ExecuteReader();

                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return false;
            }
        }

        public bool QueryOpen(string pQuery, ref DataSet pDataSet)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pQuery, _mConnection);
                else
                    command = new OleDbCommand(pQuery, _mConnection, _mTransaction);

                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 100;
                command.CommandText = pQuery;

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                pDataSet = dataSet;

                return true;
            }
            catch (OleDbException Exp)
            {
                _mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return false;
            }
        }

        public bool QueryOpen(string pQuery, List<OleDbParameter> pParameters, ref OleDbDataReader pReader)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pQuery, _mConnection);
                else
                    command = new OleDbCommand(pQuery, _mConnection, _mTransaction);

                command.Parameters.Clear();
                if (pParameters.Count > 0)
                {
                    foreach (OleDbParameter parameter in pParameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 100;
                command.Prepare();

                pReader = command.ExecuteReader();

                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return false;
            }
        }

        public int QueryExecute(string pQuery)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pQuery, _mConnection);
                else
                    command = new OleDbCommand(pQuery, _mConnection, _mTransaction);

                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 100;

                return command.ExecuteNonQuery();
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return -1;
            }
        }

        public int QueryExecute(string pQuery, List<OleDbParameter> pParameters)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pQuery, _mConnection);
                else
                    command = new OleDbCommand(pQuery, _mConnection, _mTransaction);

                command.Parameters.Clear();
                if (pParameters.Count > 0)
                {
                    foreach (OleDbParameter parameter in pParameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 100;
                command.Prepare();

                return command.ExecuteNonQuery();
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return -1;
            }
        }

        public int QueryExecuteScalar(string pQuery)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pQuery, _mConnection);
                else
                    command = new OleDbCommand(pQuery, _mConnection, _mTransaction);

                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 100;

                return (int)command.ExecuteScalar();
            }
            catch (OleDbException Exp)
            {
                _mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return -1;
            }

        }

        public bool ProcedureOpen(string pName, ref OleDbDataReader pReader)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pName, _mConnection);
                else
                    command = new OleDbCommand(pName, _mConnection, _mTransaction);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 100;

                pReader = command.ExecuteReader();

                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return false;
            }
        }

        public bool ProcedureOpen(string pName, List<OleDbParameter> pParameters, ref OleDbDataReader pReader)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pName, _mConnection);
                else
                    command = new OleDbCommand(pName, _mConnection, _mTransaction);

                command.Parameters.Clear();
                if (pParameters.Count > 0)
                {
                    foreach (OleDbParameter parameter in pParameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 100;
                command.Prepare();

                pReader = command.ExecuteReader();

                return true;
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return false;
            }
        }

        public int ProcedureExecute(string pName)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pName, _mConnection);
                else
                    command = new OleDbCommand(pName, _mConnection, _mTransaction);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 100;

                return command.ExecuteNonQuery();
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return -1;
            }
        }

        public int ProcedureExecute(string pName, List<OleDbParameter> pParameters)
        {
            try
            {
                OleDbCommand command;

                if (_mTransaction == null || _mTransaction.Connection == null)
                    command = new OleDbCommand(pName, _mConnection);
                else
                    command = new OleDbCommand(pName, _mConnection, _mTransaction);

                command.Parameters.Clear();
                if (pParameters.Count > 0)
                {
                    foreach (OleDbParameter parameter in pParameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 100;
                command.Prepare();

                return command.ExecuteNonQuery();
            }
            catch (OleDbException Exp)
            {
				_mErrorCode = Exp.ErrorCode.ToString();
                _mErrorMessage = Exp.Message;

                return -1;
            }
        }

        public string GetLastErrorCode()
        {
            return _mErrorCode;
        }

        public string GetLastErrorMessage()
        {
            return _mErrorMessage;
        }

        public bool GetConnectionState()
        {
            bool result = false;

            switch (_mConnection.State)
            {
                case ConnectionState.Broken: result = false; break;
                case ConnectionState.Closed: result = false; break;
                case ConnectionState.Connecting: result = true; break;
                case ConnectionState.Executing: result = true; break;
                case ConnectionState.Fetching: result = true; break;
                case ConnectionState.Open: result = true; break;
            }

            return result;
        }

        public int GetSafeInteger(OleDbDataReader pReader, int pIndex)
        {
            if (pReader.IsDBNull(pIndex)) return 0;
            else return pReader.GetInt32(pIndex);
        }

        public string GetSafeString(OleDbDataReader pReader, int pIndex)
        {
            if (pReader.IsDBNull(pIndex)) return string.Empty;
            else return pReader.GetString(pIndex);
        }

        public string GetSafeDateTime(OleDbDataReader pReader, int pIndex)
        {
            if (pReader.IsDBNull(pIndex)) return string.Empty;
            else return pReader.GetDateTime(pIndex).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
