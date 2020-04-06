using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BIZBANK.V2020.CAD.DBMS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BIZBANK.V2020.CAD.ADMINISTRACION.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmpProdServTranFactoryController : ControllerBase
    {

        /// <summary>
        /// CONSULTAR SERVICIOS
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
        /// <param name="codEmpresa"> CODIGO DE LA EMPRESA </param>
        /// <param name="codProducto"> CODIGO DE LA PRODUCTO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaServiciosEmpresaProducto")]
        public ActionResult ConsultaServiciosEmpresaProducto(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] string codEmpresa,
            [FromQuery(Name = "codProducto")] int codProducto,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio
        )
        {

            try
            {

                // INSTANCIAR OBJETO DE CONEXIÓN
                ClienteDBMS dbms = new ClienteDBMS();

                // ASIGANR VALORES DE LA CABECERA DE LA TRANSACCIÓN
                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;

                // AGREGAR PARAMETROS PARA LA EJECUÓN
                dbms.AddParameter("i_banco", DBType.dbSmallint, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("i_empresa", DBType.dbVarChar, ParamDirection.ParamInput, 10, codEmpresa);
                dbms.AddParameter("i_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_serv_em_prod", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return BadRequest(jsonError);

                }

                // EXTRAER EL ARREGLO DE DATOS Y DEVOLVEMOS UN OBJETO CON CODIGO HTTP 200
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

                // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);

            }

        }

        /// <summary>
        /// CONSULTAR PLANTILLA DE TRANSACCIONES
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
        /// <param name="codEmpresa"> CODIGO DE LA EMPRESA </param>
        /// <param name="codProducto"> CODIGO DE LA PRODUCTO </param>
        /// <param name="codModulo"> CODIGO DEL MODULO </param>
        /// <param name="codServicio"> CODIGO DEL SERVICIO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
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

                // INSTANCIAR OBJETO DE CONEXIÓN
                ClienteDBMS dbms = new ClienteDBMS();

                // ASIGANR VALORES DE LA CABECERA DE LA TRANSACCIÓN
                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;

                // AGREGAR PARAMETROS PARA LA EJECUÓN
                dbms.AddParameter("i_banco", DBType.dbSmallint, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("i_empresa", DBType.dbInteger, ParamDirection.ParamInput, 15, codEmpresa);
                dbms.AddParameter("i_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("i_modulo", DBType.dbInteger, ParamDirection.ParamInput, 0, codModulo);
                dbms.AddParameter("i_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, codServicio);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, "");

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_transcliente", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
                {

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };

                    return BadRequest(jsonError);

                }

                // EXTRAER EL ARREGLO DE DATOS Y DEVOLVEMOS UN OBJETO CON CODIGO HTTP 200
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

                // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                return BadRequest(jsonError);

            }

        }

        /// <summary>
        /// GRABAR PLANTILLA DE EMPRESA
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="plantillaTransaccion"> OBJETO JSON </param>
        /// <returns></returns>
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

                // INSTANCIAR OBJETO DE CONEXIÓN
                ClienteDBMS dbms = new ClienteDBMS();

                // ASIGANR VALORES DE LA CABECERA DE LA TRANSACCIÓN
                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;

                // AGREGAR PARAMETROS PARA LA EJECUÓN
                dbms.AddParameter("i_et_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codBanco"].ToString()));
                dbms.AddParameter("i_et_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codEmpresa"].ToString()));
                dbms.AddParameter("i_et_cod_producto", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codProducto"].ToString()));
                dbms.AddParameter("i_et_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonTransaccion["codServicio"].ToString());
                dbms.AddParameter("i_et_cod_modulo", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codModulo"].ToString()));
                dbms.AddParameter("i_et_cod_transaccion", DBType.dbInteger, ParamDirection.ParamInput, 0, 0);
                dbms.AddParameter("i_et_estado", DBType.dbVarChar, ParamDirection.ParamInput, 1, 'I');
                dbms.AddParameter("o_mensaje", DBType.dbVarChar, ParamDirection.ParamOutput, 250, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
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

                    // AGREGAR PARAMETROS PARA LA EJECUÓN
                    dbms.AddParameter("i_et_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codBanco"].ToString()));
                    dbms.AddParameter("i_et_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codEmpresa"].ToString()));
                    dbms.AddParameter("i_et_cod_producto", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codProducto"].ToString()));
                    dbms.AddParameter("i_et_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonTransaccion["codServicio"].ToString());
                    dbms.AddParameter("i_et_cod_modulo", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonTransaccion["codModulo"].ToString()));
                    dbms.AddParameter("i_et_cod_transaccion", DBType.dbInteger, ParamDirection.ParamInput, 0, codTransaccion);
                    dbms.AddParameter("i_et_estado", DBType.dbVarChar, ParamDirection.ParamInput, 1, estado);
                    dbms.AddParameter("o_mensaje", DBType.dbVarChar, ParamDirection.ParamOutput, 250, string.Empty);

                    // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
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

                        // AGREGAR UN OBJETO DE ERROR A LA LISTA
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

                        // AGREGAR UN OBJETO DE EJECUCIÓN EXITOSA A LA LISTA
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

                // EXTRAER EL ARREGLO DE DATOS Y DEVOLVEMOS UN OBJETO CON CODIGO HTTP 200
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

                // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
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
