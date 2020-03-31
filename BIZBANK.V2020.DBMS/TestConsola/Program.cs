using System;
using BIZBANK.V2020.CAD.DBMS;
using Newtonsoft.Json.Linq;

namespace TestConsola
{
    class Program
    {
        static void Main(string[] args)
        {

            ClienteDBMS dbms = new ClienteDBMS();

            dbms.cabeceraDBMS.LoginId = 131351;
            dbms.cabeceraDBMS.BankId = 1;
            dbms.cabeceraDBMS.CompanyId = 2304;
            dbms.cabeceraDBMS.SessionId = 0;
            dbms.cabeceraDBMS.StationId = "192.168.0.159";
            dbms.cabeceraDBMS.UserId = "CCN00LZAMBRAN";
            dbms.cabeceraDBMS.UserType = "B";

            dbms.AddParameter("returnparameter", DBType.dbInteger, ParamDirection.ParamInputOutput, 0, 0);
            dbms.AddParameter("i_banco", DBType.dbSmallint, ParamDirection.ParamInput, 0, 1);
            dbms.AddParameter("i_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, 2304);
            dbms.AddParameter("i_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, 3);
            dbms.AddParameter("i_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, "RU");
            dbms.AddParameter("i_cod_rol", DBType.dbInteger, ParamDirection.ParamInput, 0, 1);

            /*cliente.AddParameter("i_red_tipo", DBType.dbVarChar, ParamDirection.ParamInput, 3, "AG");
            cliente.AddParameter("i_red_ruc", DBType.dbVarChar, ParamDirection.ParamInput, 15, "0992092092");
            cliente.AddParameter("i_red_nombre", DBType.dbVarChar, ParamDirection.ParamInput, 100, "CXCD");
            cliente.AddParameter("i_tipo_switch", DBType.dbInteger, ParamDirection.ParamInput, 0, 1); //'Req 6336
            cliente.AddParameter("i_estado", DBType.dbVarChar, ParamDirection.ParamInput, 0, "A");// 'ror:01  REQ10710 20/03/2017
            cliente.AddParameter("i_red_judi_conecta_bce", DBType.dbVarChar, ParamDirection.ParamInput, 1, "S");// 'REQ10695 - ROR - 26/04/2017
            cliente.AddParameter("o_resultado", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);*/


            // obj_con.Execute(IdTran, permiso, par_servicio, "pr_consulta_red", "BZP_PQ_ADM_REDES_CPT_NET", ExecuteType.ResultSet, ExecuteMode.RemoteMode, 0)

            // if (!cliente.Execute("30809", "pf", "SRVGENERAL", "pr_consulta_red", "BZP_PQ_ADM_REDES_CPT_NET", ExecuteType.ResultSet, ExecuteMode.RemoteMode, 0))
            if (!dbms.Execute(
                "10205",
                "pf",
                "SRVGENERAL",
                "pr_desactiva_rol",
                "BZA_PQ_BIZ_EMPRESA",
                ExecuteType.NoResultSet,
                ExecuteMode.LocalMode,
                0
              ))
            {
                var jsonError = new
                {
                    error = false,
                    codigo = dbms.CodigoError,
                    mensaje = dbms.MensajeError,

                };

                Console.WriteLine("============");

                Console.WriteLine(jsonError.ToString());

                Console.WriteLine("============");

            }
            else
            {

                JObject parametros = dbms.GetParams();

                Console.WriteLine("============");
                Console.WriteLine("finalizado");
                Console.WriteLine( parametros.ToString() );
                Console.WriteLine("============");


            }

        }
    }
}
