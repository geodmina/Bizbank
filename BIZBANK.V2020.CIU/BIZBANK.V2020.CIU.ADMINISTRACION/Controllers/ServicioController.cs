﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace BIZBANK.V2020.CIU.ADMINISTRACION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {

        private readonly IConfiguration configuracion;
        const string c_rutaAPI = "/api/Servicio/";
        private string segmentoCodigo = string.Empty;
        private readonly string c_rutaClase = "BIZBANK.V2020.CIU/BIZBANK.V2020.CIU.ADMINISTRACION/Controllers/";
        private readonly string c_claseNombre = "ServicioController";

        public ServicioController(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }

        /// <summary>
        /// CONSULTAR SERVICIOS
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="codProducto"> CODIGO DEL PRODUCTO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ServicioPlantillaModulo")]
        public ActionResult ServicioPlantillaModulo(
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

            const string c_metodoNombre = "ServicioPlantillaModulo";
            using (var cliente = new HttpClient())
            {
                try
                {

                    // ASIGNAR CABECERAS AL OBJETO CLIENTE
                    cliente.DefaultRequestHeaders.Add("cabUsuario", cabUsuario);
                    cliente.DefaultRequestHeaders.Add("cabLoginId", cabLoginId.ToString());
                    cliente.DefaultRequestHeaders.Add("cabCompania", cabCompania.ToString());
                    cliente.DefaultRequestHeaders.Add("cabBanco", cabBanco.ToString());
                    cliente.DefaultRequestHeaders.Add("cabTipoUsuario", cabTipoUsuario);
                    cliente.DefaultRequestHeaders.Add("cabEstacion", cabEstacion);

                    // OBTENER LA BASE DEL URL DESDE EL ARCHIVO DE CONFIGURACION appsettings.json.
                    segmentoCodigo = "BLOQUE 10";
                    cliente.BaseAddress = new Uri(configuracion["Servicio:Url"]);


                    // ARMAR LA RUTA DEL SERVICIO A CONSUMIR.
                    segmentoCodigo = "BLOQUE 20";
                    string url = string.Empty;
                    url = string.Format("{0}{1}?codProducto={2}&idTransaccion={3}&permiso={4}" +
                        "&parServicio={5}",
                        c_rutaAPI, c_metodoNombre, codProducto, idTransaccion, permiso, parServicio);


                    // EJECUTAR LA PETICION AL SERVICIO CAD DE CONSULTA DE ORDENES MEDIANTE
                    // METODO GET.
                    segmentoCodigo = "BLOQUE 30";
                    var respuesta = cliente.GetAsync(url).Result;
                    var contenido = respuesta.Content.ReadAsStringAsync();
                    var jsonRespuesta = JObject.Parse(contenido.Result);

                    // MAPEAR Y DEVOLVER LA RESPUESTA AL FRONT-END.
                    switch (respuesta.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return Ok(jsonRespuesta);
                        case HttpStatusCode.NotFound:
                            return NotFound(jsonRespuesta);
                        case HttpStatusCode.BadRequest:
                            return BadRequest(jsonRespuesta);
                        default:
                            return BadRequest(jsonRespuesta);
                    }
                }
                catch (Exception errorInformacion)
                {
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);
                }
            }

        }

    }
}