using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BIZBANK.V2020.CAD.DBMS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BIZBANK.V2020.CAD.ADMINISTRACION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolFactoryController : ControllerBase
    {

        /// <summary>
        /// REGISTRAR UN NUEVO ROL
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="objRol"> OBJETO CON LOS DATOS DEL ROL A CREAR </param>
        /// <returns></returns>
        [HttpPost]
        [Route("GrabarAuto")]
        public ActionResult GrabarAuto(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromBody] JObject jObjectRol
        )
        {

            // ASIGANAR EL OBJETO JSON CON LOS DATOS DEL ROL
            var jsonRol = jObjectRol;

            try
            {

                // INSTANCIAR DEL CLIENTE PARA LA EJECUCIÓN DE LOS SP
                ClienteDBMS dbms = new ClienteDBMS();

                // ASIGNAR VALORES DE CABECERA DEL CLIENTE
                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;


                // AGREGAR PARAMETROS PARA EJECUCION DEL SP
                dbms.AddParameter("i_ro_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonRol["codBanco"].ToString()));
                dbms.AddParameter("i_ro_cod_empresa", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonRol["codEmpresa"].ToString());
                dbms.AddParameter("i_ro_cod_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, int.Parse(jsonRol["codProducto"].ToString()));
                dbms.AddParameter("i_ro_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonRol["codServicio"].ToString());
                dbms.AddParameter("i_ro_cod_rol", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonRol["codRol"].ToString()));
                dbms.AddParameter("i_ro_nom_rol", DBType.dbVarChar, ParamDirection.ParamInput, 50, jsonRol["nomRol"].ToString());
                dbms.AddParameter("i_ro_est_rol", DBType.dbChar, ParamDirection.ParamInput, 1, jsonRol["estRol"].ToString());
                dbms.AddParameter("i_ro_desc_rol", DBType.dbVarChar, ParamDirection.ParamInput, 60, jsonRol["desRol"].ToString());

                // LLAMAR AL METODO QUE EJECUTA EL SP
                if (!dbms.Execute(jsonRol["idTransaccion"].ToString(),
                        jsonRol["permiso"].ToString(),
                        jsonRol["parServicio"].ToString(),
                        "pr_grb_rol_auto",
                        "BZA_PQ_BIZ_EMPRESA",
                        ExecuteType.NoResultSet,
                        ExecuteMode.LocalMode,
                        0
                    ))
                {

                    // RETORNAR OBJETO DE ERROR CON UN CODIGO HTTP 400 EN CASO DE ERRORES EN LA EJECUCION
                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };
                    return BadRequest(jsonError);

                }

                // RETORNAR RESPUESTA CON CODIGO HTTP 200
                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    respuestas = "Transacción exitosa"
                };
                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                // EN CASO DE ERRORES EN LA EJECUCION RETORNAMOS EL OBJETO DE ERROR CON UN CODIGO HTTP 400
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
        /// CONSULTAR ROLES
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
        /// <param name="codServicio"> CODIGO DE SERVICIO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("CargarRolesTodos")]
        public ActionResult CargarRolesTodos(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codProducto")] int codProducto,
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
                dbms.AddParameter("i_cod_banco", DBType.dbNumeric, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("i_cod_empresa", DBType.dbNumeric, ParamDirection.ParamInput, 10, codEmpresa);
                dbms.AddParameter("i_cod_producto", DBType.dbNumeric, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("i_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, codServicio);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_crg_rol_por_BancEmpProSer", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    roles = cursor
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
        /// VERIFICAR NOMBRE ROL
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
        /// <param name="codServicio"> CODIGO DE SERVICIO </param>
        /// <param name="nombreRol"> NOMBRE DEL ROL </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaNombreIndividual")]
        public ActionResult ConsultaNombreIndividual(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codProducto")] int codProducto,
            [FromQuery(Name = "codServicio")] string codServicio,
            [FromQuery(Name = "nombreRol")] string nombreRol,
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
                dbms.AddParameter("i_ro_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("i_ro_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, codEmpresa);
                dbms.AddParameter("i_ro_cod_producto", DBType.dbInteger, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("i_ro_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 50, codServicio);
                dbms.AddParameter("i_ro_nom_rol", DBType.dbVarChar, ParamDirection.ParamInput, 50, nombreRol);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_nom_rol", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    roles = cursor
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
        /// REGISTRAR UN NUEVO ROL
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="objRol"> OBJETO CON LOS DATOS DEL ROL A CREAR </param>
        /// <returns></returns>
        [HttpPost]
        [Route("Grabar")]
        public ActionResult Grabar(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromBody] JObject jObjectRol
        )
        {

            // ASIGANAR EL OBJETO JSON CON LOS DATOS DEL ROL
            var jsonRol = jObjectRol;

            try
            {

                // INSTANCIAR DEL CLIENTE PARA LA EJECUCIÓN DE LOS SP
                ClienteDBMS dbms = new ClienteDBMS();

                // ASIGNAR VALORES DE CABECERA DEL CLIENTE
                dbms.cabeceraDBMS.LoginId = cabLoginId;
                dbms.cabeceraDBMS.BankId = cabBanco;
                dbms.cabeceraDBMS.CompanyId = cabCompania;
                dbms.cabeceraDBMS.SessionId = 0;
                dbms.cabeceraDBMS.StationId = cabEstacion;
                dbms.cabeceraDBMS.UserId = cabUsuario;
                dbms.cabeceraDBMS.UserType = cabTipoUsuario;


                // AGREGAR PARAMETROS PARA EJECUCION DEL SP
                dbms.AddParameter("i_ro_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonRol["codBanco"].ToString()));
                dbms.AddParameter("i_ro_cod_empresa", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonRol["codEmpresa"].ToString());
                dbms.AddParameter("i_ro_cod_producto", DBType.dbSmallint, ParamDirection.ParamInput, 0, int.Parse(jsonRol["codProducto"].ToString()));
                dbms.AddParameter("i_ro_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonRol["codServicio"].ToString());
                dbms.AddParameter("i_ro_cod_rol", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonRol["codRol"].ToString()));
                dbms.AddParameter("i_ro_nom_rol", DBType.dbVarChar, ParamDirection.ParamInput, 50, jsonRol["nomRol"].ToString());
                dbms.AddParameter("i_ro_est_rol", DBType.dbChar, ParamDirection.ParamInput, 1, jsonRol["estRol"].ToString());
                dbms.AddParameter("i_ro_desc_rol", DBType.dbVarChar, ParamDirection.ParamInput, 60, jsonRol["desRol"].ToString());

                // LLAMAR AL METODO QUE EJECUTA EL SP
                if (!dbms.Execute(jsonRol["idTransaccion"].ToString(),
                        jsonRol["permiso"].ToString(),
                        jsonRol["parServicio"].ToString(),
                        "pr_grb_rol",
                        "BZA_PQ_BIZ_EMPRESA",
                        ExecuteType.NoResultSet,
                        ExecuteMode.LocalMode,
                        0
                    ))
                {

                    // RETORNAR OBJETO DE ERROR CON UN CODIGO HTTP 400 EN CASO DE ERRORES EN LA EJECUCION
                    var jsonError = new
                    {
                        error = true,
                        codigo = dbms.CodigoError,
                        mensaje = dbms.MensajeError
                    };
                    return BadRequest(jsonError);

                }

                // RETORNAR RESPUESTA CON CODIGO HTTP 200
                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    respuestas = "Transacción exitosa"
                };
                return Ok(jsonRetorno);

            }
            catch (Exception ex)
            {

                // EN CASO DE ERRORES EN LA EJECUCION RETORNAMOS EL OBJETO DE ERROR CON UN CODIGO HTTP 400
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
        /// CONSULTAR ROLES
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
        /// <param name="codServicio"> CODIGO DE SERVICIO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("CargarRoles")]
        public ActionResult CargarRoles(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codProducto")] int codProducto,
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
                dbms.AddParameter("i_cod_banco", DBType.dbNumeric, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("i_cod_empresa", DBType.dbNumeric, ParamDirection.ParamInput, 10, codEmpresa);
                dbms.AddParameter("i_cod_producto", DBType.dbNumeric, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("i_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, codServicio);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_crg_rol_por_BancEmpPr", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    roles = cursor
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
