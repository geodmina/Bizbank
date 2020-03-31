using System;
using System.Data;
using System.Text;
using BIZBANK.V2020.CAD.ORACLE.Modelos;
using BizTransactionalServer.DataProviders;
using Oracle.ManagedDataAccess.Client;

namespace BIZBANK.V2020.CAD.ORACLE
{
    public class InternalProcess
    {

        PackageTransaction transaccion;

        const string FIELD_SEPARATOR = "_";
        string cadena = "data source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.200)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SISOEMP))):1531;password=OCP_PKG;user id=OCP_PKG;persist security info=false;Connection Timeout=120;";

        public InternalProcess(PackageTransaction transaccion)
        {
            this.transaccion = transaccion;
        }


        public string Ejecutar()
        {

            long lsecaudit = 0;
            string mensajeCheckExecute = string.Empty;
            DateTime dtstarexec;
            DateTime dtendexec;
            StringBuilder respuesta = new StringBuilder(); ;

            bool bchecktran = !("sp_prc_logon,sp_prc_logonbank,sp_prc_logoff,sp_prc_externo,sp_prc_getservice,sp_prc_getproduct,sp_prc_getcatalog,sp_prc_getoptions,sp_prc_estadisticas,sp_prc_consola".Contains(this.transaccion.Name) && this.transaccion.TranId.Equals("00000") && this.transaccion.OptionId.Equals("LG"));

            if (bchecktran)
            {
                if (!CheckExecute(this.transaccion, out lsecaudit, out mensajeCheckExecute))
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("{ \"respuesta\" : {");
                    sb.Append("\"error\": {");
                    sb.Append(String.Format("\"{0}\" : \"{1}\",", "Number", -1));
                    sb.Append(String.Format("\"{0}\" : \"{1}\",", "Source", "BizTransaccional"));
                    sb.Append(String.Format("\"{0}\" : \"{1}\"", "Message", mensajeCheckExecute));
                    sb.Append("}");
                    sb.Append("}}");

                    return sb.ToString();

                }

            }

            try
            {

                dtstarexec = DateTime.Now;
                respuesta = this.LlamadaBase();
                //rsTransaction.ResultSets.Add(this.LlamadaBase());
                dtendexec = DateTime.Now;


                if (bchecktran && lsecaudit > 0)
                    UpdateExecute(lsecaudit, dtstarexec, dtendexec, "");


            }
            catch (Exception ex)
            {

                Console.WriteLine("Error : {0}", ex.Message);

                if (bchecktran && lsecaudit > 0)
                    UpdateExecute(lsecaudit, DateTime.Now, DateTime.Now, ex.Message);

                // Maperar error al json

            }

            return respuesta.ToString();
        }

        private bool CheckExecute(PackageTransaction transaction, out long isecaudit, out string mensajeCheckExecute)
        {

            OracleConnection sqlConnection = null;
            OracleCommand sqlCommand = null;
            OracleParameter sqlParameter = null;

            int iparamret = -1;

            isecaudit = 0;

            mensajeCheckExecute = string.Empty;

            sqlConnection = new OracleConnection(this.cadena);

            try
            {
                sqlCommand = new OracleCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = String.Format("{0}.{1}", "bza_pq_bts", "sp_prc_checkexecute2");

                sqlParameter = sqlCommand.Parameters.Add("o_return", OracleDbType.Int32);
                sqlParameter.Direction = ParameterDirection.InputOutput;
                sqlParameter.Value = 0;

                sqlParameter = sqlCommand.Parameters.Add("i_loginid", OracleDbType.Int32);
                sqlParameter.Value = transaction.LoginId;

                sqlParameter = sqlCommand.Parameters.Add("i_sessionid", OracleDbType.Int32);
                sqlParameter.Value = transaction.SessionId;

                sqlParameter = sqlCommand.Parameters.Add("i_bancoid", OracleDbType.Int16);
                sqlParameter.Value = transaction.BankId;

                sqlParameter = sqlCommand.Parameters.Add("i_tranid", OracleDbType.Int32);
                sqlParameter.Value = transaction.TranId;

                sqlParameter = sqlCommand.Parameters.Add("i_userid", OracleDbType.Varchar2, 20);
                sqlParameter.Value = transaction.UserId;

                sqlParameter = sqlCommand.Parameters.Add("i_companyid", OracleDbType.Int32);
                sqlParameter.Value = transaction.CompanyId;

                sqlParameter = sqlCommand.Parameters.Add("i_usertype", OracleDbType.Varchar2, 1);
                sqlParameter.Value = transaction.UserType;

                sqlParameter = sqlCommand.Parameters.Add("i_serviceid", OracleDbType.Varchar2, 10);
                sqlParameter.Value = transaction.ServiceId;

                sqlParameter = sqlCommand.Parameters.Add("i_optionid", OracleDbType.Varchar2, 2);
                sqlParameter.Value = transaction.OptionId;

                sqlParameter = sqlCommand.Parameters.Add("i_stationid", OracleDbType.Varchar2, 30);
                sqlParameter.Value = transaction.StationId;

                sqlParameter = sqlCommand.Parameters.Add("i_spname", OracleDbType.Varchar2, 50);
                sqlParameter.Value = transaction.Name;

                sqlParameter = sqlCommand.Parameters.Add("i_source", OracleDbType.Varchar2, 50);
                sqlParameter.Value = transaction.Source;

                sqlParameter = sqlCommand.Parameters.Add("i_paramsxml", OracleDbType.Varchar2, 8000);
                sqlParameter.Value = string.Empty;

                sqlParameter = sqlCommand.Parameters.Add("o_auditoria", OracleDbType.Int32);
                sqlParameter.Direction = ParameterDirection.Output;

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();

                iparamret = int.Parse(sqlCommand.Parameters["o_return"].Value.ToString());
                isecaudit = int.Parse(sqlCommand.Parameters["o_auditoria"].Value.ToString());

                if (iparamret != 0)
                    mensajeCheckExecute = string.Format("No tiene permiso para ejecutar la transacción. Ret:{0}", iparamret);

            }
            catch (Exception ex)
            {
                mensajeCheckExecute = ex.Message;

                // BizTransactionalServer.Log.ServerEventLog.WriteEntry("CheckExecute :" + sMessage);
            }
            finally
            {
                sqlParameter = null;

                if (sqlCommand != null)
                {
                    //sqlCommand.Dispose();
                    sqlCommand = null;
                }

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    //sqlConnection.Dispose();
                    sqlConnection = null;
                }

            }

            if (iparamret == 0)
                return true;
            else
            {
                // PackageWriter pWriter = new PackageWriter(ResultSetError("100030", ReplaceXmlChars(sMessage), TypeResultSet.DbError));
                // pWriter.WritePackage(out this._dataPackage);
                // pWriter = null;

                return false;
            }

        }

        private bool UpdateExecute(long isecaudit, DateTime startexec, DateTime endexec, string message)
        {
            OracleConnection sqlConnection = null;
            OracleCommand sqlCommand = null;
            OracleParameter sqlParameter = null;

            // gmv
            // Aqui se obtiene a cadena de conexion
            // sqlConnection = new OracleConnection(Configuration.Info.LocalServer.Principal.ConnString);
            sqlConnection = new OracleConnection(this.cadena);

            try
            {
                sqlCommand = new OracleCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = String.Format("{0}.{1}", "bza_pq_bts", "sp_prc_updateexecute");

                sqlParameter = sqlCommand.Parameters.Add("i_secaudit", OracleDbType.Int32);
                sqlParameter.Value = isecaudit;

                sqlParameter = sqlCommand.Parameters.Add("i_startexec", OracleDbType.Varchar2, 20);
                sqlParameter.Value = startexec.ToString("yyyy-MM-dd hh:mm:ss");

                sqlParameter = sqlCommand.Parameters.Add("i_endexec", OracleDbType.Varchar2, 20);
                sqlParameter.Value = endexec.ToString("yyyy-MM-dd hh:mm:ss");

                sqlParameter = sqlCommand.Parameters.Add("i_message", OracleDbType.Varchar2, 250);
                sqlParameter.Value = message.PadRight(250, ' ').Substring(0, 250).Trim();

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                // gmv log
                // BizTransactionalServer.Log.ServerEventLog.WriteEntry("UpdateExecute :" + ex.Message);

                // PackageWriter pWriter = new PackageWriter(ResultSetError("UpdateExecute :" + ex.Message, TypeResultSet.DbError));
                // pWriter.WritePackage(out this._dataPackage);
                // pWriter = null;
                // salida de la respuesta

            }
            finally
            {
                sqlParameter = null;

                if (sqlCommand != null)
                {
                    //sqlCommand.Dispose();
                    sqlCommand = null;
                }

                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    //sqlConnection.Dispose();
                    sqlConnection = null;
                }

            }

            return true;
        }

        internal StringBuilder LlamadaBase()
        {

            int parametersCount = 0;
            short localMode = 0;

            parametersCount = this.transaccion.Parameters.Count;
            localMode = this.transaccion.ExecMode;
            OracleDataProvider dataProvider;

            if (parametersCount > 0)
            {
                dataProvider = new OracleDataProvider(this.transaccion.Source, this.transaccion.Name, parametersCount, localMode);
                for (int i = 0; i < parametersCount; i++)
                {
                    if (this.transaccion.Parameters[i].Flag)
                    {
                        dataProvider.AddParameter(this.transaccion.Parameters[i].Name,
                                                  this.transaccion.Parameters[i].Direction,
                                                  this.transaccion.Parameters[i].Type,
                                                  this.transaccion.Parameters[i].Size,
                                                  this.transaccion.Parameters[i].Value);
                    }
                    else
                    {
                        dataProvider.AddParameter(this.transaccion.Parameters[i].Name,
                                                  this.transaccion.Parameters[i].Direction,
                                                  this.transaccion.Parameters[i].Type,
                                                  this.transaccion.Parameters[i].Size);
                    }

                }
            }
            else
            {
                dataProvider = new OracleDataProvider(this.transaccion.Source, this.transaccion.Name, localMode);
            }


            IDataReader dataReader = null;

            // Almacena objeto JSON
            StringBuilder sb = new StringBuilder();
            sb.Append("{ \"respuesta\" : {");

            if (InferExecType(this.transaccion.ExecType) == ExecType.NoResultSet)
            {
                int rowAffect = dataProvider.ExecuteNoResultSet();
                if (rowAffect == -1)
                    goto CreateRsErrorJson;

                sb.Append(dataProvider.BindParametersOutValuesString());

            }
            else
            {
                // xlv 20070119 IDataReader dataReader = dataProvider.ExecuteResultSet();
                dataReader = dataProvider.ExecuteResultSet();


                // GMV 10/03/2020
                // Pasar de DataReader a JSON

                /*
                 * Si la ejecuci�n no hubo errores
                 */

                if (dataReader != null)
                {

                    sb.Append("\"cursor\": [");

                    int filasDataReader = 0;
                    int numCampos = dataReader.FieldCount;
                    int idxCampo = 0;
                    string[] titulos = new string[numCampos];
                    string[] tipos = new string[numCampos];

                    for (idxCampo = 0; idxCampo < numCampos; idxCampo++)
                    {
                        titulos[idxCampo] = dataReader.GetName(idxCampo);
                        tipos[idxCampo] = dataReader.GetFieldType(idxCampo).Name;
                    }

                    while (dataReader.Read())
                    {

                        ++filasDataReader;
                        sb.Append("{");

                        for (idxCampo = 0; idxCampo < numCampos; idxCampo++)
                        {

                            switch (tipos[idxCampo])
                            {
                                case "Int64":
                                    sb.Append(String.Format("\"{0}\" : {1}", titulos[idxCampo], dataReader.GetValue(idxCampo)));
                                    break;
                                case "String":
                                    sb.Append(String.Format("\"{0}\" : \"{1}\"", titulos[idxCampo], dataReader.GetValue(idxCampo)));
                                    break;
                                default:
                                    sb.Append(String.Format("\"{0}\" : \"{1}\"", titulos[idxCampo], dataReader.GetValue(idxCampo)));
                                    break;
                            }


                            sb.Append(",");

                        }

                        sb.Remove(sb.Length - 1, 1);
                        sb.Append("}");
                        sb.Append(",");

                    }

                    if (filasDataReader > 0) sb.Remove(sb.Length - 1, 1);

                    sb.Append("]");

                    string parametrosSalida = dataProvider.BindParametersOutValuesString();

                    if (!parametrosSalida.Equals(string.Empty))
                    {
                        sb.Append(",");
                        sb.Append(parametrosSalida);
                    }

                }
                else
                {
                    goto CreateRsErrorJson;
                }

            }


            if (dataReader != null)
            {
                dataReader.Close();
                dataReader = null;
            }

            goto CloseConn;

        CreateRsErrorJson:
            sb.Append("\"error\": {");
            sb.Append(String.Format("\"{0}\" : \"{1}\",", "Number", dataProvider.Error.Number));
            sb.Append(String.Format("\"{0}\" : \"{1}\",", "Source", dataProvider.Error.Source));
            sb.Append(String.Format("\"{0}\" : \"{1}\"", "Message", dataProvider.Error.Message));
            sb.Append("}");


        CloseConn:
            dataProvider.CloseConnection();
            dataProvider = null;

            sb.Append("}}");
            return sb;

        }

        private ExecType InferExecType(short Exectype)
        {
            switch (Exectype)
            {
                case (short)ExecType.ResultSet:
                    return ExecType.ResultSet;
                case (short)ExecType.NoResultSet:
                    return ExecType.NoResultSet;
                default:
                    return ExecType.NoResultSet;
            }
        }

        private ExecMode InferExecMode(short Execmode)
        {
            switch (Execmode)
            {
                case (short)ExecMode.LocalMode:
                    return ExecMode.LocalMode;
                case (short)ExecMode.RemoteMode:
                    return ExecMode.RemoteMode;
                case (short)ExecMode.HostMode:
                    return ExecMode.HostMode;
                default:
                    return ExecMode.LocalMode;
            }
        }

    }
}
