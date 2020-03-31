using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace BIZBANK.V2020.CIU.GENERAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {

        private readonly IConfiguration configuracion;
        const string c_rutaAPI = "/api/General/";
        private string segmentoCodigo = string.Empty;
        private readonly string c_rutaClase = "BIZBANK.V2020.CIU/BIZBANK.V2020.CIU.GENERAL/Controllers/";
        private readonly string c_claseNombre = "GeneralController";

        public GeneralController(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }

        [HttpGet]
        [Route("GetCatalogo")]
        public ActionResult GetCatalogo(
           [FromQuery(Name = "nombreTabla")] string nombreTabla
        )
        {
            const string c_metodoNombre = "GetCatalogo";
            using (var cliente = new HttpClient())
            {
                try
                {

                    // OBTENER LA BASE DEL URL DESDE EL ARCHIVO DE CONFIGURACION appsettings.json.
                    segmentoCodigo = "BLOQUE 10";
                    cliente.BaseAddress = new Uri(configuracion["Servicio:Url"]);


                    // ARMAR LA RUTA DEL SERVICIO A CONSUMIR.
                    segmentoCodigo = "BLOQUE 20";
                    string url = string.Empty;
                    url = string.Format("{0}{1}?nombreTabla={2}",
                        c_rutaAPI, c_metodoNombre, nombreTabla);


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

        [HttpGet]
        [Route("GetCatalogoEspecifico")]
        public ActionResult GetCatalogoEspecifico(
            [FromQuery(Name = "nombreTabla")] string nombreTabla,
            [FromQuery(Name = "nombreCatalogo")] string nombreCatalogo
       )
        {
            const string c_metodoNombre = "GetCatalogoEspecifico";
            using (var cliente = new HttpClient())
            {
                try
                {

                    // OBTENER LA BASE DEL URL DESDE EL ARCHIVO DE CONFIGURACION appsettings.json.
                    segmentoCodigo = "BLOQUE 10";
                    cliente.BaseAddress = new Uri(configuracion["Servicio:Url"]);


                    // ARMAR LA RUTA DEL SERVICIO A CONSUMIR.
                    segmentoCodigo = "BLOQUE 20";
                    string url = string.Empty;
                    url = string.Format("{0}{1}?nombreTabla={2}&nombreCatalogo={3}",
                        c_rutaAPI, c_metodoNombre, nombreTabla, nombreCatalogo);


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
            const string c_metodoNombre = "GetServicio";
            using (var cliente = new HttpClient())
            {
                try
                {

                    // OBTENER LA BASE DEL URL DESDE EL ARCHIVO DE CONFIGURACION appsettings.json.
                    segmentoCodigo = "BLOQUE 10";
                    cliente.BaseAddress = new Uri(configuracion["Servicio:Url"]);


                    // ARMAR LA RUTA DEL SERVICIO A CONSUMIR.
                    segmentoCodigo = "BLOQUE 20";
                    string url = string.Empty;
                    url = string.Format("{0}{1}?codTransaccion={2}&codUsuario={3}&tipoUsuario={4}" +
                        "&codBanco={5}&codEmpresa={6}&logId={7}&prodId={8}&opcion={9}",
                        c_rutaAPI, c_metodoNombre, codTransaccion, codUsuario, tipoUsuario,
                        codBanco, codEmpresa, logId, prodId, opcion );


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
            const string c_metodoNombre = "GetProducto";
            using (var cliente = new HttpClient())
            {
                try
                {

                    // OBTENER LA BASE DEL URL DESDE EL ARCHIVO DE CONFIGURACION appsettings.json.
                    segmentoCodigo = "BLOQUE 10";
                    cliente.BaseAddress = new Uri(configuracion["Servicio:Url"]);


                    // ARMAR LA RUTA DEL SERVICIO A CONSUMIR.
                    segmentoCodigo = "BLOQUE 20";
                    string url = string.Empty;
                    url = string.Format("{0}{1}?codTransaccion={2}&codUsuario={3}&tipoUsuario={4}" +
                        "&codBanco={5}&codEmpresa={6}&logId={7}&{8}&opcion={9}",
                        c_rutaAPI, c_metodoNombre, codTransaccion, codUsuario, tipoUsuario,
                        codBanco, codEmpresa, logId, opcion);


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

        [HttpGet]
        [Route("GetOpcion")]
        public ActionResult GetOpcion(
            [FromQuery(Name = "codTransaccion")] int codTransaccion,
            [FromQuery(Name = "codUsuario")] string codUsuario,
            [FromQuery(Name = "tipoUsuario")] string tipoUsuario,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] int codEmpresa,
            [FromQuery(Name = "logId")] int logId,
            [FromQuery(Name = "opcion")] string opcion
        )
        {
            const string c_metodoNombre = "GetOpcion";
            using (var cliente = new HttpClient())
            {
                try
                {

                    // OBTENER LA BASE DEL URL DESDE EL ARCHIVO DE CONFIGURACION appsettings.json.
                    segmentoCodigo = "BLOQUE 10";
                    cliente.BaseAddress = new Uri(configuracion["Servicio:Url"]);


                    // ARMAR LA RUTA DEL SERVICIO A CONSUMIR.
                    segmentoCodigo = "BLOQUE 20";
                    string url = string.Empty;
                    url = string.Format("{0}{1}?codTransaccion={2}&codUsuario={3}&tipoUsuario={4}" +
                        "&codBanco={5}&codEmpresa={6}&logId={7}&{8}&opcion={9}",
                        c_rutaAPI, c_metodoNombre, codTransaccion, codUsuario, tipoUsuario,
                        codBanco, codEmpresa, logId, opcion);


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

            const string c_metodoNombre = "GetEmpresaBIZ";
            using (var cliente = new HttpClient())
            {
                try
                {

                    // OBTENER LA BASE DEL URL DESDE EL ARCHIVO DE CONFIGURACION appsettings.json.
                    segmentoCodigo = "BLOQUE 10";
                    cliente.BaseAddress = new Uri(configuracion["Servicio:Url"]);


                    // ARMAR LA RUTA DEL SERVICIO A CONSUMIR.
                    segmentoCodigo = "BLOQUE 20";
                    string url = string.Empty;
                    url = string.Format("{0}{1}?codUsuario={2}&tipoUsuario={3}&codBanco={4}" +
                        "&codEmpresa={5}&codServicio={6}&estacion={7}&permiso={8}" +
                        "&logId={9}",
                        c_rutaAPI, c_metodoNombre, codUsuario, tipoUsuario, codBanco,
                        codEmpresa, codServicio, estacion, permiso, logId);

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