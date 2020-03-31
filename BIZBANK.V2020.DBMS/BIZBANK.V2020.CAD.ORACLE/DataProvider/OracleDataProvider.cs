using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BizTransactionalServer.DataProviders
{

	public struct DataProviderError
	{
		public int Number;
		public string Source;
		public string Message;
	}

	public enum ExecType : short
	{
		ResultSet,
		NoResultSet
	}

	public enum ParamType : short
	{
		dbDecimal,
		dbMoney,
		dbInteger,
		dbSmallint,
		dbTinyint,
		dbVarChar,
		dbChar,
		dbDate,
		dbDateTime,
		dbCursor,
		dbTimeStamp,
		dbCurrency,
		dbNumeric,
		dbDouble
	}

	public enum ParamDirection : short
	{
		ParamInput,
		ParamOutput,
		ParamInputOutput,
		ParamReturnValue
	}

	public enum ExecMode : short
	{
		LocalMode,
		RemoteMode,
		HostMode
	}

	struct DataProviderParameter
	{
		public int Size;
		public string Name;
		public object Value;
		public ParamType Type;
		public ParamDirection Direction;
		public bool Flag;
	}

	class OracleDataProvider
    {

		DataProviderError oraError = new DataProviderError();

		string connectionstring;
		string storeprocedure;
		string source;
		int timeout;

		string[] parametersOutvalues = null;

		int idxparameter = 0;
		int parameterscount = 0;
		int parametersOutcount = 0;

		OracleConnection oraConnection = null;
		OracleCommand oraCommand = null;
		OracleParameter[] oraParameters = null;

		public OracleDataProvider(string Source, string StoreProcedure, int ParametersCount, short LocalMode)
		{
			source = Source;
			storeprocedure = StoreProcedure;
			parameterscount = ParametersCount;

			if (LocalMode == (short)ExecMode.RemoteMode)
			{
				connectionstring = "data source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.200)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SISOEMP))):1531;password=BIZ_PKG;user id=BIZ_PKG;persist security info=false;Connection Timeout=120;"; ;
				timeout = 60000;
				// connectionstring = Configuration.Info.RemoteServer.Principal.ConnString;
				// timeout = Configuration.Info.RemoteServer.Principal.Timeout;  // XLV 20120306
			}
			else if (LocalMode == (short)ExecMode.LocalMode)
			{
				connectionstring = "data source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.200)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SISOEMP))):1531;password=OCP_PKG;user id=OCP_PKG;persist security info=false;Connection Timeout=120;";
				timeout = 60000;
				// connectionstring = Configuration.Info.LocalServer.Principal.ConnString;
				// timeout = Configuration.Info.LocalServer.Principal.Timeout;  // XLV 20120306
			}
			else
			{
				// connectionstring = Configuration.Info.HostServer.Principal.ConnString;
				// timeout = Configuration.Info.HostServer.Principal.Timeout;  // XLV 20120306
			}

			oraParameters = new OracleParameter[ParametersCount];
		}

		public OracleDataProvider(string Source, string StoreProcedure, short LocalMode)
		{
			source = Source;
			storeprocedure = StoreProcedure;

			if (LocalMode == (short)ExecMode.RemoteMode)
			{
				connectionstring = "data source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.200)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SISOEMP))):1531;password=BIZ_PKG;user id=BIZ_PKG;persist security info=false;Connection Timeout=120;"; ;
				timeout = 60000;
				// connectionstring = Configuration.Info.RemoteServer.Principal.ConnString;
				// timeout = Configuration.Info.RemoteServer.Principal.Timeout;  // XLV 20120306
			}
			else if (LocalMode == (short)ExecMode.LocalMode)
			{
				connectionstring = "data source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.200)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SISOEMP))):1531;password=OCP_PKG;user id=OCP_PKG;persist security info=false;Connection Timeout=120;";
				timeout = 60000;
				// connectionstring = Configuration.Info.LocalServer.Principal.ConnString;
				// timeout = Configuration.Info.LocalServer.Principal.Timeout;  // XLV 20120306
			}
			else
			{
				// connectionstring = Configuration.Info.HostServer.Principal.ConnString;
				// timeout = Configuration.Info.HostServer.Principal.Timeout;  // XLV 20120306
			}
		}

		private bool OpenConnection()
		{
			// gmv
			// oraConnection = new OracleConnection(connectionstring);
			oraConnection = new OracleConnection(this.connectionstring);
			oraConnection.Open();
			return true;
		}

		private void CreateCommand()
		{
			oraCommand = new OracleCommand();
			oraCommand.Connection = this.oraConnection;
			oraCommand.CommandType = CommandType.StoredProcedure;
			oraCommand.CommandText = String.Format("{0}.{1}", source, storeprocedure);
			oraCommand.CommandTimeout = timeout;  // XLV 20120306
		}

		private void AddParametersToCommand()
		{
			if (parameterscount > 0)
			{
				for (int i = 0; i < oraParameters.Length; i++)
				{
					oraCommand.Parameters.Add(oraParameters[i]);
					if ((oraParameters[i].Direction == ParameterDirection.InputOutput
						|| oraParameters[i].Direction == ParameterDirection.Output
						|| oraParameters[i].Direction == ParameterDirection.ReturnValue)
						&& oraParameters[i].OracleDbType != OracleDbType.RefCursor)
						parametersOutcount++;
				}

				oraParameters = null;

				if (parametersOutcount > 0)
					parametersOutvalues = new string[parametersOutcount];
			}
		}

		public void AddParameter(string Name, short Direction, short Type, int Size, object Value)
		{
			if (parameterscount > 0)
			{
				oraParameters[idxparameter] = new OracleParameter();
				oraParameters[idxparameter].ParameterName = Name;
				oraParameters[idxparameter].Direction = InferDirection(Direction);
				oraParameters[idxparameter].OracleDbType = InferType(Type);
				oraParameters[idxparameter].Size = Size;
				if (InferDirection(Direction) == ParameterDirection.Input
					|| InferDirection(Direction) == ParameterDirection.InputOutput)
					oraParameters[idxparameter].Value = Value;
				Value = null;
				idxparameter++;
			}
		}

		public void AddParameter(string Name, short Direction, short Type, int Size)
		{
			if (parameterscount > 0)
			{
				oraParameters[idxparameter] = new OracleParameter();
				oraParameters[idxparameter].ParameterName = Name;
				oraParameters[idxparameter].Direction = InferDirection(Direction);
				oraParameters[idxparameter].OracleDbType = InferType(Type);
				oraParameters[idxparameter].Size = Size;
				idxparameter++;
			}
		}

		public void CloseConnection()
		{
			parametersOutvalues = null;

			if (oraCommand != null)
			{
				oraCommand.Dispose();
				oraCommand = null;
			}

			if (oraConnection != null)
			{
				oraConnection.Close();
				//oraConnection.Dispose();
				oraConnection = null;
			}
		}

		public int ExecuteNoResultSet()
		{
			int rowsAffec = 0;

			try
			{
				if (OpenConnection())
				{
					CreateCommand();
					AddParametersToCommand();
					oraParameters = null;
					oraCommand.ExecuteNonQuery();
					return rowsAffec;
				}
			}
			catch (OracleException oraEx)
			{
				rowsAffec = -1;
				oraError.Number = -1111; // oraEx.Code;
				oraError.Source = "Biz Transactional Service Oracle Error";
				oraError.Message = oraEx.Message.Replace("\"", string.Empty);
				// SendConsole(oraError.Message);  // XLV 20110218
			}

			return rowsAffec;
		}

		public IDataReader ExecuteResultSet()
		{
			try
			{
				if (OpenConnection())
				{
					CreateCommand();
					AddParametersToCommand();
					return oraCommand.ExecuteReader(CommandBehavior.SingleResult);
				}
			}
			catch (OracleException oraEx)
 			{
				oraError.Number = -1111; // oraEx.Code;
				oraError.Source = "Biz Transactional Service Oracle Error";
				oraError.Message = oraEx.Message.Replace("\"",string.Empty);
				// SendConsole(oraError.Message);  // XLV 20110218
			}

			return null;
		}

		public void BindParametersOutValues()
		{
			if (parametersOutcount > 0)
			{
				int i = 0;
				for (int j = 0; j < oraCommand.Parameters.Count; j++)
				{
					if ((oraCommand.Parameters[j].Direction == ParameterDirection.InputOutput
						|| oraCommand.Parameters[j].Direction == ParameterDirection.Output
						|| oraCommand.Parameters[j].Direction == ParameterDirection.ReturnValue)
						&& oraCommand.Parameters[j].OracleDbType != OracleDbType.RefCursor)
					{
						parametersOutvalues[i] = String.Format("{0}", oraCommand.Parameters[j].Value);
						i++;
					}

				}
			}
		}

		public string BindParametersOutValuesString()
		{
			if (parametersOutcount > 0)
			{
				int i = 0;

				StringBuilder sb = new StringBuilder();
				sb.Append("\"parametros\": {");
				for (int j = 0; j < oraCommand.Parameters.Count; j++)
				{
					if ((oraCommand.Parameters[j].Direction == ParameterDirection.InputOutput
						|| oraCommand.Parameters[j].Direction == ParameterDirection.Output
						|| oraCommand.Parameters[j].Direction == ParameterDirection.ReturnValue)
						&& oraCommand.Parameters[j].OracleDbType != OracleDbType.RefCursor)
					{
						sb.Append(String.Format("\"{0}\" : \"{1}\",", oraCommand.Parameters[j].ParameterName, oraCommand.Parameters[j].Value));
						i++;
					}

				}
				sb.Remove(sb.Length - 1, 1);
				sb.Append("}");

				return sb.ToString();
			}

			return string.Empty;
		}

		private ParameterDirection InferDirection(short Direction)
		{
			switch (Direction)
			{
				case (short)ParamDirection.ParamInput: return ParameterDirection.Input;
				case (short)ParamDirection.ParamInputOutput: return ParameterDirection.InputOutput;
				case (short)ParamDirection.ParamOutput: return ParameterDirection.Output;
				case (short)ParamDirection.ParamReturnValue: return ParameterDirection.ReturnValue;
				default: return ParameterDirection.Input;
			}
		}

		private OracleDbType InferType(short Type)
		{
			switch (Type)
			{
				case (short)ParamType.dbChar: return OracleDbType.Varchar2;
				case (short)ParamType.dbCurrency: return OracleDbType.Long;
				case (short)ParamType.dbCursor: return OracleDbType.RefCursor;
				case (short)ParamType.dbDate: return OracleDbType.Date;
				case (short)ParamType.dbDateTime: return OracleDbType.Date;
				case (short)ParamType.dbDecimal: return OracleDbType.Long;
				case (short)ParamType.dbDouble: return OracleDbType.Double;
				case (short)ParamType.dbInteger: return OracleDbType.Int32;
				case (short)ParamType.dbMoney: return OracleDbType.Long;
				case (short)ParamType.dbNumeric: return OracleDbType.Int32;
				case (short)ParamType.dbSmallint: return OracleDbType.Int32;
				case (short)ParamType.dbTimeStamp: return OracleDbType.TimeStamp;
				case (short)ParamType.dbTinyint: return OracleDbType.Int16;
				case (short)ParamType.dbVarChar: return OracleDbType.Varchar2;
				default: return OracleDbType.Varchar2;
			}
		}

		public DataProviderError Error
		{
			get { return oraError; }
		}

		public string[] ParametersOutValues
		{
			get
			{
				return parametersOutvalues;
			}
		}

		public int ParametersOutCount
		{
			get { return parameterscount; }
		}


	}
}
