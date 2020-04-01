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
    public class RolUsuarioFactoryController : ControllerBase
    {

        /// <summary>
        /// CONSULTAR ROLES POR USUARIOS
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
        /// <param name="usuario"> USUARIO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("CargaPerfilPorUsuario")]
        public ActionResult CargaPerfilPorUsuario(
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
            [FromQuery(Name = "usuario")] string usuario,
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
                dbms.AddParameter("i_ru_cod_banco", DBType.dbNumeric, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("i_ru_cod_empresa", DBType.dbNumeric, ParamDirection.ParamInput, 10, codEmpresa);
                dbms.AddParameter("i_ru_cod_producto", DBType.dbNumeric, ParamDirection.ParamInput, 0, codProducto);
                dbms.AddParameter("i_ru_cod_servicio", DBType.dbVarChar, ParamDirection.ParamInput, 10, codServicio);
                dbms.AddParameter("i_ru_cod_usuario", DBType.dbVarChar, ParamDirection.ParamInput, 20, usuario);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_rol_por_usuarioA", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
