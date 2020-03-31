using BIZBANK.V2020.CAD.ORACLE;
using BIZBANK.V2020.CAD.ORACLE.Modelos;
using Newtonsoft.Json.Linq;
using System;

namespace BIZBANK.V2020.CAD.DBMS
{
    
    public enum ExecuteType : short
    {
        ResultSet = 0,
        NoResultSet = 1
    }

    public enum ExecuteMode : short
    {
        LocalMode = 0,
        RemoteMode = 1,
        HostMode = 2,
        ExternalMode = 3
    }

    public enum DBType : short
    {
        dbDecimal = 0,
        dbMoney = 1,
        dbInteger = 2,
        dbSmallint = 3,
        dbTinyint = 4,
        dbVarChar = 5,
        dbChar = 6,
        dbDate = 7,
        dbDateTime = 8,
        dbCursor = 9,
        dbTimeStamp = 10,
        dbCurrency = 11,
        dbNumeric = 12,
        dbDouble = 13
    }

    public enum ParamDirection : short
    {
        ParamInput = 0,
        ParamOutput = 1,
        ParamInputOutput = 2,
        ParamReturnValue = 3
    }

    public enum DataReturn : short
    {
        XML = 0,
        Array = 1,
        DataTable = 2
    }

    public enum ParameterReturn : short
    {
        XML = 0,
        Array = 1
        // Command = 2
    }

    public enum UserEvent : short
    {
        CreateUser = 0,
        RemoveUser = 1,
        ChangePassword = 2,
        ResetPassword = 3,
        MoveUser = 5,
        ResetPwdSE = 6
    }

    public enum ServiceType : short
    {
        Company = 0,
        User = 1
    }

    public enum ProductType : short
    {
        Company = 0,
        User = 1
    }

    public class CabeceraDBMS
    {
        public int LoginId;
        public int BankId;
        public int SessionId;
        public string StationId;
        public string UserId;
        public int CompanyId;
        public string UserType;
    }

    public class ClienteDBMS
    {

        #region "Estructuras"
        private struct Error
        {
            public int Codigo;
            public string Fuente;
            public string Mensaje;
        }

        #endregion "Estructuras"

        #region "Propiedades privadas"

        private PackageTransaction _Transaction;
        private Error _ErrorInfo;
        private string respuesta;

        // Revisar si se debe eliminar
        private byte[] _Data = null;
        private string _OrganizationUnit = null;
        private string _Domain = null;
        private const char FIELD_SEPARATOR = '�';
        private bool _BizMovil = false;

        #endregion

        #region "Propiedades públicas"

        public CabeceraDBMS cabeceraDBMS;

        public int CodigoError
        {
            get { return _ErrorInfo.Codigo; }
        }
        public string FuenteError
        {
            get { return _ErrorInfo.Fuente; }
        }
        public string MensajeError
        {
            get { return _ErrorInfo.Mensaje; }
        }
        public bool BizMovil
        {
            get { return _BizMovil; }
            set { _BizMovil = value; }
        }

        #endregion

        #region "Constructores y destructores"
        public ClienteDBMS()
        {
            _Transaction = new PackageTransaction();
            cabeceraDBMS = new CabeceraDBMS();
            respuesta = null;
        }
        ~ClienteDBMS()
        {
            _Transaction.Parameters.Clear();
            _Transaction.Parameters = null;
            _Data = null;
        }
        #endregion

        #region "Métodos privados"

        private void SetError(int Codigo, string Mensaje, string Fuente)
        {
            _ErrorInfo.Codigo = Codigo;
            _ErrorInfo.Mensaje = Mensaje;
            _ErrorInfo.Fuente = Fuente;
        }

        private void ClearErrorInfo()
        {
            _ErrorInfo.Codigo = 0;
            _ErrorInfo.Mensaje = string.Empty;
            _ErrorInfo.Fuente = string.Empty;
        }

        private void ClearTransactionInfo()
        {
            _Transaction.Name = null;
            _Transaction.Source = null;
            _Transaction.TranId = null;
            _Transaction.SessionId = null;
            _Transaction.UserId = null;
            _Transaction.OptionId = null;
            _Transaction.StationId = null;
            _Transaction.BankId = null;
            _Transaction.ServiceId = null;
            _Transaction.UserType = null;
            _Transaction.LoginId = null;
            _Transaction.ExecType = (int)ExecuteType.NoResultSet;
            _Transaction.ExecMode = (int)ExecuteMode.LocalMode;
            _Transaction.Parameters.Clear();
        }

        // CreatePackage

        // CreatePackageTest

        // CreateXmlSchema

        // ConvertSBtoDataTable

        // private object GetDataReturn(DataReturn Type)
        public JArray GetData()
        {

            var jsonObject = JObject.Parse(this.respuesta);

            var jsonArray = (JArray)jsonObject["respuesta"]["cursor"];

            return jsonArray;

        }

        public JObject GetParams()
        {

            var jsonObject = JObject.Parse(this.respuesta.ToString());

            var jsonParse = (JObject)jsonObject["respuesta"]["parametros"];

            return jsonParse;

        }

        public JObject GetAfectación()
        {

            var jsonObject = JObject.Parse(this.respuesta.ToString());

            var jsonParse = (JObject)jsonObject["respuesta"]["registros"];

            return jsonParse;

        }

        // private object GetDataReturn(ParameterReturn Type)

        // private void GetDataReturn(out object Data, DataReturn TypeData, out object Parameter, ParameterReturn TypeParameter)

        private bool CheckErrorBTS()
        {
            // Valida si existe error
            if (this.respuesta == null)
            {
                SetError(-1, "Mensaje de error vacio (BTS)", "");
                return true;
            }

            var jsonObject = JObject.Parse(this.respuesta.ToString());

            var jsonParse = (JObject)jsonObject["respuesta"];
            if (!jsonParse.ContainsKey("error"))
                return false;

            var jsonError = (JObject)jsonObject["respuesta"]["error"];

            SetError(
                (int)jsonError["Number"],
                jsonError["Message"].ToString(),
                jsonError["Source"].ToString()
            );

            return true;
        }

        #endregion

        #region "Métodos públicos"

        /*public object GetData(DataReturn Type)
        {
            return GetDataReturn(Type);
        }

        public object GetParameter(ParameterReturn Type)
        {
            return GetDataReturn(Type);
        }

        public void GetDataParameter(out object Data, DataReturn TypeData, out object Parameter, ParameterReturn TypeParameter)
        {
            GetDataReturn(out Data, TypeData, out Parameter, TypeParameter);
            return;
        }*/

        public void ClearParameters()
        {
            _Transaction.Parameters.Clear();
        }

        public void AddParameter(string Name, DBType Type, ParamDirection Direction, Int16 Size, object Value)
        {
            if ((short)Direction < 0 || (short)Direction > 3)
                throw new Exception("Tipo ParamDirection incorrecto");

            if (Direction == ParamDirection.ParamInput && Value == null)
                throw new Exception("Valor incorrecto cuando ParamDirection es Input");

            if ((short)Type < 1 || (short)Type > 13)
                throw new Exception("Tipo DBType incorrecto");

            if (Type == DBType.dbCursor && Direction == ParamDirection.ParamInput)
                throw new Exception("Tipo DBType.dbCursor incorrecto cuando ParamDirection es Input");


            PackageParameter iParameter = new PackageParameter();

            iParameter.Name = Name;
            iParameter.Direction = (short)Direction;
            iParameter.Type = (short)Type;
            iParameter.Size = Size;
            iParameter.Flag = true;
            if (Type == DBType.dbCursor)
                iParameter.Value = null;
            else
                iParameter.Value = Value.ToString();

            _Transaction.Parameters.Add(iParameter);

        }

        public bool Execute(string TranId, string OptionId, string ServiceId, string Name, string Source, ExecuteType ExecType, ExecuteMode ExecMode, byte ParSys)
        {
            if (_ErrorInfo.Codigo != 0)
                return false;

            if ((short)ExecType < 0 || (short)ExecType > 1)
            {
                SetError(-1, "Tipo ExecType incorrecto", "");
                return false;
            }

            if ((short)ExecMode < 0 || (short)ExecMode > 3)
            {
                SetError(-1, "Modo ExecMode incorrecto", "");
                return false;
            }

            // la sesion ya es validada en la capa ciu por el token
            /*if (!(TranId.Equals("00000") && OptionId.Equals("LG")))
                if ((HttpContext.Current.Session["BIZUserId"] == null) || (HttpContext.Current.Session["BIZLoginId"] == null) || ((string)HttpContext.Current.Session["BIZLoginId"] == "0"))
                {
                    SetError(-1, "Session expirada", "");
                    return false;
                }*/

            ClearErrorInfo();

            // gmv
            // Aqui ya se hace la llamada a la siguiente CAPA
            // ClientSocket socket = null;


            try
            {

                _Transaction.Name = Name;
                _Transaction.OptionId = OptionId;
                _Transaction.ServiceId = ServiceId;
                _Transaction.TranId = TranId;
                _Transaction.Source = Source;
                _Transaction.ExecMode = (short)ExecMode;
                _Transaction.ExecType = (short)ExecType;

                _Transaction.LoginId = cabeceraDBMS.LoginId.ToString();
                _Transaction.BankId = cabeceraDBMS.BankId.ToString();
                _Transaction.CompanyId = cabeceraDBMS.CompanyId.ToString();
                _Transaction.SessionId = cabeceraDBMS.SessionId.ToString();
                _Transaction.StationId = cabeceraDBMS.StationId.ToString();
                _Transaction.UserId = cabeceraDBMS.UserId.ToString();
                _Transaction.UserType = cabeceraDBMS.UserType.ToString();


                InternalProcess proceso = new InternalProcess(_Transaction);

                this.respuesta = proceso.Ejecutar();

                var myJObject = JObject.Parse(this.respuesta.ToString());

                ClearTransactionInfo();

                if (CheckErrorBTS())
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                SetError(-1, ex.Message, ex.Source);
                return false;
            }

        }

        #endregion

    }


}
