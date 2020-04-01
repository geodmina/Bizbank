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
    public class UsuarioFactoryController : ControllerBase
    {

        /// <summary>
        /// CONSULTAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="codUsuario"> CODIGO DE USUARIO </param>
        /// <param name="codOpcion"> CODIGO DE LA OPCION </param>
        /// <param name="codEmpresa"> CODIGO DE LA EMPRESA </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaIndividual")]
        public ActionResult ConsultaIndividual(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codUsuario")] string codUsuario,
            [FromQuery(Name = "codOpcion")] string codOpcion,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codBanco")] int codBanco,
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

                string usuario = codUsuario == string.Empty || codUsuario == null ? string.Empty : codUsuario;
                string opcion = codOpcion == string.Empty || codOpcion == null ? string.Empty : codOpcion;
                int idEmpresa = codEmpresa.ToString().Equals(string.Empty) || codEmpresa == 0 ? 0 : codEmpresa;
                int idBanco = codBanco.ToString().Equals(string.Empty) || codBanco == 0 ? 0 : codBanco;

                // AGREGAR PARAMETROS PARA LA EJECUÓN
                dbms.AddParameter("us_cod_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 50, usuario);
                dbms.AddParameter("i_table", DBType.dbVarChar, ParamDirection.ParamInput, 10, opcion);
                dbms.AddParameter("i_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 15, idEmpresa);
                dbms.AddParameter("i_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 15, idBanco);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_usuario_individual", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    usuarios = cursor
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
        /// VALIDAR CEDULA
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="cadena"> CADENA DE CEDULA A VALIDAR </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ValidaCedula")]
        public ActionResult ValidaCedula(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "cadena")] string cadena,
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
                dbms.AddParameter("cadena", DBType.dbVarChar, ParamDirection.ParamInput, 50, cadena);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_modulo10", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    cedulas = cursor
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
        /// GRABAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
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
            [FromBody] JObject usuario
        )
        {

            // ASIGNAR OBJETO JSON CON LOS DATOS DEL USUARIO A CREAR
            var jsonUsuario = usuario;

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
                dbms.AddParameter("i_us_cod_nivel", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonUsuario["codNivel"].ToString());
                dbms.AddParameter("i_us_tipo", DBType.dbVarChar, ParamDirection.ParamInput, 10, jsonUsuario["tipo"].ToString());
                dbms.AddParameter("i_us_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 10, int.Parse(jsonUsuario["codEmpresa"].ToString()));
                dbms.AddParameter("i_us_cod_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 20, jsonUsuario["codUsuario"].ToString());
                dbms.AddParameter("i_us_nom_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 50, jsonUsuario["nombre"].ToString());
                dbms.AddParameter("i_us_cedula", DBType.dbVarChar, ParamDirection.ParamInput, 13, jsonUsuario["cedula"].ToString());
                dbms.AddParameter("i_us_dir_email", DBType.dbVarChar, ParamDirection.ParamInput, 50, jsonUsuario["email"].ToString());
                dbms.AddParameter("i_us_est_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 1, jsonUsuario["estado"].ToString());
                // dbms.AddParameter("i_us_fch_nacimiento", DBType.dbDateTime, ParamDirection.ParamInput, 0, obj_us.p_fchNacimiento);
                dbms.AddParameter("i_us_fch_nacimiento", DBType.dbDateTime, ParamDirection.ParamInput, 0, "2000/01/01");
                dbms.AddParameter("i_us_localidad", DBType.dbVarChar, ParamDirection.ParamInput, 5, jsonUsuario["codLocalidad"].ToString());
                dbms.AddParameter("i_us_agencia", DBType.dbVarChar, ParamDirection.ParamInput, 5, "00");
                dbms.AddParameter("i_us_id_estacion", DBType.dbVarChar, ParamDirection.ParamInput, 30, jsonUsuario["estacion"].ToString());
                dbms.AddParameter("i_us_logon", DBType.dbVarChar, ParamDirection.ParamInput, 20, jsonUsuario["codEmpresa"].ToString());
                dbms.AddParameter("i_us_recibe_correo", DBType.dbVarChar, ParamDirection.ParamInput, 1, jsonUsuario["recibeCorreo"].ToString());
                dbms.AddParameter("o_cod_retorno", DBType.dbInteger, ParamDirection.ParamOutput, 0, 0);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(
                    jsonUsuario["idTransaccion"].ToString(),
                    jsonUsuario["permiso"].ToString(),
                    jsonUsuario["parServicio"].ToString(),
                    "pr_grb_usuario",
                    "BZA_PQ_BIZ_USUARIOS",
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

                // EXTRAER EL ARREGLO DE DATOS Y DEVOLVEMOS UN OBJETO CON CODIGO HTTP 200
                JObject respuesta = dbms.GetParams();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    codRetorno = respuesta["o_cod_retorno"].ToString()
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
        /// ACTUALIZA USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <returns></returns>
        [HttpPut]
        [Route("Actualiza")]
        public ActionResult Actualiza(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromBody] JObject usuario
        )
        {

            // ASIGNAR OBJETO JSON CON LOS DATOS DEL USUARIO A CREAR
            var jsonUsuario = usuario;

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

                dbms.AddParameter("i_cod_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 50, jsonUsuario["codUsuario"].ToString());
                dbms.AddParameter("i_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonUsuario["codBanco"].ToString()));
                dbms.AddParameter("i_estado", DBType.dbChar, ParamDirection.ParamInput, 1, jsonUsuario["estado"].ToString());
                dbms.AddParameter("i_usuario_in", DBType.dbVarChar, ParamDirection.ParamInput, 15, jsonUsuario["usuarioActualiza"].ToString());
                dbms.AddParameter("i_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, int.Parse(jsonUsuario["codEmpresa"].ToString()));

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(
                    jsonUsuario["idTransaccion"].ToString(),
                    jsonUsuario["permiso"].ToString(),
                    jsonUsuario["parServicio"].ToString(),
                    "pr_act_ad_usuario_banco_mnt",
                    "BZA_PQ_BIZ_USUARIOS",
                    ExecuteType.NoResultSet,
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

                // DEVOLVER UN OBJETO CON CODIGO HTTP 200
                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "Registro actualizado con éxito",
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
        /// ELIMINAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <returns></returns>
        [HttpPost]
        [Route("Eliminar")]
        public ActionResult Eliminar(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromBody] JObject usuario
        )
        {

            // ASIGNAR OBJETO JSON CON LOS DATOS DEL USUARIO A CREAR
            var jsonUsuario = usuario;

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

                dbms.AddParameter("i_us_cod_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 50, jsonUsuario["codUsuario"].ToString());
                dbms.AddParameter("o_cod_retorno", DBType.dbInteger, ParamDirection.ParamOutput, 0, 0);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(
                    jsonUsuario["idTransaccion"].ToString(),
                    jsonUsuario["permiso"].ToString(),
                    jsonUsuario["parServicio"].ToString(),
                    "pr_elm_usuario",
                    "BZA_PQ_BIZ_USUARIOS",
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

                // EXTRAER EL ARREGLO DE DATOS Y DEVOLVEMOS UN OBJETO CON CODIGO HTTP 200
                JObject respuesta = dbms.GetParams();

                var jsonRetorno = new
                {
                    error = false,
                    mensaje = "OK",
                    codRetorno = respuesta["o_cod_retorno"].ToString()
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
        /// CONSULTAR BANCO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="codEmpresa"> CODIGO DE EMPRESA </param>
        /// <param name="codUsuario"> CODIGO DE USUARIO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaEmpresaUsuario")]
        public ActionResult ConsultaEmpresaUsuario(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "codUsuario")] string codUsuario,
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
                dbms.AddParameter("i_us_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 0, codEmpresa);
                dbms.AddParameter("i_us_cod_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 50, codUsuario);
                dbms.AddParameter("o_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_empresa_usuario", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    empresa = cursor
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
