using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BIZBANK.V2020.CAD.DBMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BIZBANK.V2020.CAD.ADMINISTRACION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantillaGeneralEmpresaController : ControllerBase
    {

        [HttpGet]
        [Route("GetModuloServicio")]
        public ActionResult GetModuloServicio(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codProducto")] int codProducto,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio
        )
        {

            try
            {

                JArray cursorServicios;
                JArray cursorModulos;

                // LISTA DE SERVICIOS
                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;

                dbms.AddParameter("i_cod_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, "");


                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_crg_modelos_plantilla", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return BadRequest(jsonError);
                }

                cursorServicios = dbms.GetData();

                // LISTA DE MODULOS
                dbms.ClearParameters();

                dbms.AddParameter("i_cod_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, "");


                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_crg_ad_modulo_por_producto", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return BadRequest(jsonError);
                }

                cursorModulos = dbms.GetData();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    servicios = cursorServicios,
                    modulos = cursorModulos
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
        [Route("GetTransaccionesCliente")]
        public ActionResult GetTransaccionesCliente(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codProducto")] int codProducto,
            [FromQuery(Name = "codModulo")] int codModulo,
            [FromQuery(Name = "codServicio")] string codServicio,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio
        )
        {

            try
            {

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;

                dbms.AddParameter("i_banco", DBType.dbSmallint, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("i_empresa", DBType.dbInteger, ParamDirection.ParamInput, 15, codEmpresa);
                dbms.AddParameter("i_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("i_modulo", DBType.dbInteger, ParamDirection.ParamInput, 0, codModulo);
                dbms.AddParameter("i_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, codServicio);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, "");

                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_transdbms", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return BadRequest(jsonError);
                }

                JArray cursor = dbms.GetData();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    transacciones = cursor
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

        [HttpPost]
        [Route("GrabarPlantilla")]
        public ActionResult GrabarPlantilla(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromBody] JObject plantillaTransaccion
        )
        {
            var jsonTransaccion = plantillaTransaccion;

            try
            {

                List<dynamic> listaMensajes = new List<dynamic>();
                
                JArray jArrayTransacciones = (JArray)jsonTransaccion["transacciones"];

                ClienteDBMS dbms = new ClienteDBMS();

                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;

                dbms.AddParameter("i_et_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codBanco"].ToString()));
                dbms.AddParameter("i_et_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codEmpresa"].ToString()));
                dbms.AddParameter("i_et_cod_producto", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codProducto"].ToString()));
                dbms.AddParameter("i_et_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonTransaccion["codServicio"].ToString());
                dbms.AddParameter("i_et_cod_modulo", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codModulo"].ToString()));
                dbms.AddParameter("i_et_cod_transaccion", DBType.dbInteger, ParamDirection.ParamInput, 0, 0);
                dbms.AddParameter("i_et_estado", DBType.dbVarChar, ParamDirection.ParamInput, 1, 'I');
                dbms.AddParameter("o_mensaje", DBType.dbVarChar, ParamDirection.ParamOutput, 250, string.Empty);


                if (!dbms.Execute(
                    jsonTransaccion["idTransaccion"].ToString(),
                    jsonTransaccion["permiso"].ToString(),
                    jsonTransaccion["parServicio"].ToString(),
                    "pr_asigno_tran_plantilla",
                    "BZA_PQ_BIZ_EMPRESA",
                    ExecuteType.ResultSet,
                    ExecuteMode.LocalMode,
                    0
                ))
                {

                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return BadRequest(jsonError);

                }

                foreach (JObject item in jArrayTransacciones)
                {

                    dbms.ClearParameters();

                    int codTransaccion = int.Parse(item.GetValue("codTransaccion").ToString());
                    string estado = item.GetValue("estado").ToString();

                    dbms.AddParameter("i_et_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codBanco"].ToString()));
                    dbms.AddParameter("i_et_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codEmpresa"].ToString()));
                    dbms.AddParameter("i_et_cod_producto", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codProducto"].ToString()));
                    dbms.AddParameter("i_et_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonTransaccion["codServicio"].ToString());
                    dbms.AddParameter("i_et_cod_modulo", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codModulo"].ToString()));
                    dbms.AddParameter("i_et_cod_transaccion", DBType.dbInteger, ParamDirection.ParamInput, 0, codTransaccion);
                    dbms.AddParameter("i_et_estado", DBType.dbVarChar, ParamDirection.ParamInput, 1, estado);
                    dbms.AddParameter("o_mensaje", DBType.dbVarChar, ParamDirection.ParamOutput, 250, string.Empty);


                    if (!dbms.Execute(
                        jsonTransaccion["idTransaccion"].ToString(),
                        jsonTransaccion["permiso"].ToString(),
                        jsonTransaccion["parServicio"].ToString(),
                        "pr_asigno_tran_plantilla",
                        "BZA_PQ_BIZ_EMPRESA",
                        ExecuteType.ResultSet,
                        ExecuteMode.LocalMode,
                        0
                    ))
                    {

                        dynamic itemMensaje = new
                        {
                            error = true,
                            codigo = dbms.CodigoError,
                            mensaje = String.Format("{0} : {1}", codTransaccion, dbms.MensajeError)
                        };

                        listaMensajes.Add(itemMensaje);

                    }
                    else
                    {

                        JObject parametros = dbms.GetParams();

                        dynamic itemMensaje = new
                        {
                            error = false,
                            codigo = 0,
                            mensaje = String.Format("{0} : {1}", codTransaccion, parametros["o_mensaje"])
                        };

                        listaMensajes.Add(itemMensaje);

                    }

                }

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    respuestas = listaMensajes
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