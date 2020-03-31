using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BIZBANK.V2020.CAD.DBMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BIZBANK.V2020.CAD.GENERAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {

        [HttpGet]
        [Route("GetCatalogo")]
        public ActionResult GetCatalogo(
            [FromQuery(Name = "nombreTabla")] string nombreTabla
        )
        {

            try
            {

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = 0;
                dbms.cabeceraDBMS.BankId = 0;
                dbms.cabeceraDBMS.CompanyId = 0;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = string.Empty;
                dbms.cabeceraDBMS.UserId = string.Empty;
                dbms.cabeceraDBMS.UserType = string.Empty;

                dbms.AddParameter("i_tablename", DBType.dbVarChar, ParamDirection.ParamInput, 40, nombreTabla);
                dbms.AddParameter("i_catalogname", DBType.dbVarChar, ParamDirection.ParamInput, 40, string.Empty);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, 0);

                if (!dbms.Execute("00000", "LG", "SG", "sp_prc_getcatalog", "bza_pq_bts", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return Ok(jsonError);
                }

                JObject jo = dbms.GetParams();

                JArray cursor = dbms.GetData();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    catalogos = cursor
                };

                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);

            }
        }

        [HttpGet]
        [Route("GetCatalogoEspecifico")]
        public ActionResult GetCatalogoEspecifico(
            [FromQuery(Name = "nombreTabla")] string nombreTabla,
            [FromQuery(Name = "nombreCatalogo")] string nombreCatalogo
        )
        {

            try
            {

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = 0;
                dbms.cabeceraDBMS.BankId = 0;
                dbms.cabeceraDBMS.CompanyId = 0;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = string.Empty;
                dbms.cabeceraDBMS.UserId = string.Empty;
                dbms.cabeceraDBMS.UserType = string.Empty;

                dbms.AddParameter("i_tablename", DBType.dbVarChar, ParamDirection.ParamInput, 40, nombreTabla);
                dbms.AddParameter("i_catalogname", DBType.dbVarChar, ParamDirection.ParamInput, 40, nombreCatalogo);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, 0);

                if (!dbms.Execute("00000", "LG", "SG", "sp_prc_getcatalog", "bza_pq_bts", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return Ok(jsonError);
                }

                JObject jo = dbms.GetParams();

                JArray cursor = dbms.GetData();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    catalogos = cursor
                };

                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);

            }
        }

        [HttpGet]
        [Route("GetServicio")]
        public ActionResult GetServicio(
            [FromQuery(Name = "codTransaccion")] int codTransaccion,
            [FromQuery(Name = "codUsuario")] string codUsuario,
            [FromQuery(Name = "tipoUsuario")] string tipoUsuario,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "logId")] int logId,
            [FromQuery(Name = "prodId")] int prodId,
            [FromQuery(Name = "opcion")] string opcion
        )
        {

            try
            {

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = 0;
                dbms.cabeceraDBMS.BankId = 0;
                dbms.cabeceraDBMS.CompanyId = 0;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = string.Empty;
                dbms.cabeceraDBMS.UserId = string.Empty;
                dbms.cabeceraDBMS.UserType = string.Empty;

                dbms.AddParameter("s_tranid", DBType.dbInteger, ParamDirection.ParamInput, 0, codTransaccion);
                dbms.AddParameter("s_optionid", DBType.dbVarChar, ParamDirection.ParamInput, 0, string.Empty);
                dbms.AddParameter("s_userid", DBType.dbVarChar, ParamDirection.ParamInput, 0, codUsuario);
                dbms.AddParameter("s_usertype", DBType.dbVarChar, ParamDirection.ParamInput, 0, tipoUsuario);
                dbms.AddParameter("s_bankid", DBType.dbInteger, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("s_companyid", DBType.dbInteger, ParamDirection.ParamInput, 0, codEmpresa);
                dbms.AddParameter("s_logid", DBType.dbInteger, ParamDirection.ParamInput, 0, logId);
                dbms.AddParameter("i_prodid", DBType.dbInteger, ParamDirection.ParamInput, 0, prodId);
                dbms.AddParameter("i_tranid", DBType.dbInteger, ParamDirection.ParamInput, 0, codTransaccion);
                dbms.AddParameter("i_companyid", DBType.dbInteger, ParamDirection.ParamInput, 0, codEmpresa);
                dbms.AddParameter("i_opcion", DBType.dbVarChar, ParamDirection.ParamInput, 0, opcion);
                dbms.AddParameter("o_recordset", DBType.dbCursor, ParamDirection.ParamOutput, 0, 0);

                if (!dbms.Execute("00000", "LG", "SG", "sp_prc_getservice", "bza_pq_bts", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return Ok(jsonError);
                }

                JArray cursor = dbms.GetData();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    servicios = cursor
                };

                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);
            }

        }

        [HttpGet]
        [Route("GetProducto")]
        public ActionResult GetProducto(
            [FromQuery(Name = "codTransaccion")] int codTransaccion,
            [FromQuery(Name = "codUsuario")] string codUsuario,
            [FromQuery(Name = "tipoUsuario")] string tipoUsuario,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "logId")] int logId,
            [FromQuery(Name = "opcion")] string opcion
        )
        {

            try
            {

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = 0;
                dbms.cabeceraDBMS.BankId = 0;
                dbms.cabeceraDBMS.CompanyId = 0;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = string.Empty;
                dbms.cabeceraDBMS.UserId = string.Empty;
                dbms.cabeceraDBMS.UserType = string.Empty;

                dbms.AddParameter("s_tranid", DBType.dbInteger, ParamDirection.ParamInput, 0, codTransaccion);
                dbms.AddParameter("s_optionid", DBType.dbVarChar, ParamDirection.ParamInput, 0, string.Empty);
                dbms.AddParameter("s_userid", DBType.dbVarChar, ParamDirection.ParamInput, 0, codUsuario);
                dbms.AddParameter("s_usertype", DBType.dbVarChar, ParamDirection.ParamInput, 0, tipoUsuario);
                dbms.AddParameter("s_bankid", DBType.dbInteger, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("s_companyid", DBType.dbInteger, ParamDirection.ParamInput, 0, codEmpresa);
                dbms.AddParameter("s_logid", DBType.dbInteger, ParamDirection.ParamInput, 0, logId);
                dbms.AddParameter("i_companyid", DBType.dbInteger, ParamDirection.ParamInput, 0, codEmpresa);
                dbms.AddParameter("i_opcion", DBType.dbVarChar, ParamDirection.ParamInput, 0, opcion);
                dbms.AddParameter("o_recordset", DBType.dbCursor, ParamDirection.ParamOutput, 0, 0);

                if (!dbms.Execute("00000", "LG", "SG", "sp_prc_getproduct", "bza_pq_bts", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return Ok(jsonError);
                }

                JArray cursor = dbms.GetData();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    productos = cursor
                };

                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);
            }

        }

        [HttpGet]
        [Route("GetOpcion")]
        public ActionResult GetOpcion(
            [FromQuery(Name = "codTransaccion")] int codTransaccion,
            [FromQuery(Name = "codUsuario")] string codUsuario,
            [FromQuery(Name = "tipoUsuario")] string tipoUsuario,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codServicio")] string codServicio,
            [FromQuery(Name = "logId")] int logId
        )
        {

            try
            {

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = 0;
                dbms.cabeceraDBMS.BankId = 0;
                dbms.cabeceraDBMS.CompanyId = 0;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = string.Empty;
                dbms.cabeceraDBMS.UserId = string.Empty;
                dbms.cabeceraDBMS.UserType = string.Empty;

                dbms.AddParameter("s_tranid", DBType.dbInteger, ParamDirection.ParamInput, 0, codTransaccion);
                dbms.AddParameter("s_optionid", DBType.dbVarChar, ParamDirection.ParamInput, 0, string.Empty);
                dbms.AddParameter("s_userid", DBType.dbVarChar, ParamDirection.ParamInput, 0, codUsuario);
                dbms.AddParameter("s_usertype", DBType.dbVarChar, ParamDirection.ParamInput, 0, tipoUsuario);
                dbms.AddParameter("s_bankid", DBType.dbInteger, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("s_companyid", DBType.dbInteger, ParamDirection.ParamInput, 0, codEmpresa);
                dbms.AddParameter("s_logid", DBType.dbInteger, ParamDirection.ParamInput, 0, logId);
                dbms.AddParameter("i_tranid", DBType.dbInteger, ParamDirection.ParamInput, 0, codTransaccion);
                dbms.AddParameter("i_serviceid", DBType.dbVarChar, ParamDirection.ParamInput, 0, codServicio);
                dbms.AddParameter("o_options", DBType.dbVarChar, ParamDirection.ParamOutput, 500, string.Empty);


                if (!dbms.Execute("00000", "LG", "SG", "sp_prc_getoptions", "bza_pq_bts", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return Ok(jsonError);
                }

                JObject parametros = dbms.GetParams();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    opcion = parametros["o_options"]
                };

                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);
            }

        }

        [HttpGet]
        [Route("GetEmpresaBIZ")]
        public ActionResult GetEmpresaBIZ(
            [FromQuery(Name = "codUsuario")] string codUsuario,
            [FromQuery(Name = "tipoUsuario")] string tipoUsuario,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codServicio")] string codServicio,
            [FromQuery(Name = "estacion")] string estacion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "logId")] int logId
        )
        {

            try
            {

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = logId;
                dbms.cabeceraDBMS.BankId = codBanco;
                dbms.cabeceraDBMS.CompanyId = codEmpresa;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = estacion;
                dbms.cabeceraDBMS.UserId = codUsuario;
                dbms.cabeceraDBMS.UserType = tipoUsuario;

                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                if (!dbms.Execute(permiso.ToString(), permiso, codServicio, "pr_con_empresa_Biz", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {
                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return BadRequest(jsonError);
                }

                JArray empresas = dbms.GetData();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    empresas = empresas
                };

                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);
            }

        }

    }
}