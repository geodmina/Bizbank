﻿using System;
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

    }
}