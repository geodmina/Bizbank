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
    public class EmpresaFactoryController : ControllerBase
    {

        /// <summary>
        /// CONSULTAR BANCO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaComerciosLocal")]
        public ActionResult ConsultaComerciosLocal(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "tipoNuc")] string tipoNuc,
            [FromQuery(Name = "tipoCliente")] string tipoCliente,
            [FromQuery(Name = "criterioEmpresa")] string criterioEmpresa,
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

                // AGREGAR PARAMETROS PARA LA EJECUÓN
                dbms.AddParameter("i_tipo_nuc", DBType.dbChar, ParamDirection.ParamInput, 1, tipoNuc);
                dbms.AddParameter("i_tipo_cliente", DBType.dbChar, ParamDirection.ParamInput, 1, tipoCliente);
                dbms.AddParameter("i_em_crit_empresa", DBType.dbVarChar, ParamDirection.ParamInput, 50, criterioEmpresa);
                dbms.AddParameter("i_cod_banco", DBType.dbInteger, ParamDirection.ParamInput, 0, codBanco);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_empr_ayudaTipoEspc", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    empresas = cursor
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
        /// <param name="codEmpresa"> CODIGO DE LA EMPRESA </param>
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
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
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
                dbms.AddParameter("i_em_cod_empresa", DBType.dbInteger, ParamDirection.ParamInput, 1, codEmpresa);
                dbms.AddParameter("t_retorno", DBType.dbCursor, ParamDirection.ParamOutput, 0, string.Empty);

                // LLAMADA DE LA EJECUCIÓN DE LA TRANSACCIÓN
                if (!dbms.Execute(idTransaccion, permiso, parServicio, "pr_con_empresa", "BZA_PQ_BIZ_EMPRESA", ExecuteType.ResultSet, ExecuteMode.LocalMode, 0))
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
                    empresas = cursor
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
