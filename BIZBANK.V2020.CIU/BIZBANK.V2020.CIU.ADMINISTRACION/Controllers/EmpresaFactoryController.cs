﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BIZBANK.V2020.CIU.ADMINISTRACION.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaFactoryController : ControllerBase
    {

        private readonly IConfiguration configuracion;
        const string c_rutaAPI = "/api/EmpresaFactory/";
        private string segmentoCodigo = string.Empty;
        private readonly string c_rutaClase = "BIZBANK.V2020.CIU/BIZBANK.V2020.CIU.ADMINISTRACION/Controllers/";
        private readonly string c_claseNombre = "EmpresaFactoryController";

        public EmpresaFactoryController(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }

        /// <summary>
        /// CONSULTAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
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

            const string c_metodoNombre = "ConsultaComerciosLocal";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codBanco={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codBanco,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }

        /// <summary>
        /// CONSULTAR EMPRESA
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codProducto"> CODIGO DE PRODUCTO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaMasiva")]
        public ActionResult ConsultaMasiva(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "tipoNuc")] string tipoNuc,
            [FromQuery(Name = "tipoCliente")] string tipoCliente,
            [FromQuery(Name = "criterioEmpresa")] string criterioEmpresa,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio,
            [FromQuery(Name = "codProducto")] int codProducto = 0
        )
        {

            const string c_metodoNombre = "ConsultaMasiva";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codProducto={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codProducto,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }

        /// <summary>
        /// CONSULTAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaAyudaEmpresa")]
        public ActionResult ConsultaAyudaEmpresa(
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

            const string c_metodoNombre = "ConsultaAyudaEmpresa";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codBanco={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codBanco,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }


        /// <summary>
        /// CONSULTAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaAyudaEmpresaporBanco")]
        public ActionResult ConsultaAyudaEmpresaporBanco(
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

            const string c_metodoNombre = "ConsultaAyudaEmpresaporBanco";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codBanco={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codBanco,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }

        /// <summary>
        /// CONSULTAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaAyudaBancEmp")]
        public ActionResult ConsultaAyudaBancEmp(
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

            const string c_metodoNombre = "ConsultaAyudaBancEmp";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codBanco={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codBanco,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }
        
        /// <summary>
        /// CONSULTAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codBanco"> CODIGO DEL BANCO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaAyudaEmpPersLike")]
        public ActionResult ConsultaAyudaEmpPersLike(
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

            const string c_metodoNombre = "ConsultaAyudaEmpPersLike";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codBanco={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codBanco,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }

        /// <summary>
        /// CONSULTAR USUARIO
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> CODIGO DE USUARIO </param>
        /// <param name="tipoCliente"> CODIGO DE LA OPCION </param>
        /// <param name="criterioEmpresa"> TIPO DE CLIENTE </param>
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
            [FromQuery(Name = "codEmpresa")] string codEmpresa,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio
        )
        {

            const string c_metodoNombre = "ConsultaIndividual";
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
                    url = string.Format("{0}{1}?codEmpresa={2}&idTransaccion={3}&permiso={4}&parServicio={5}",
                        c_rutaAPI, c_metodoNombre, codEmpresa, idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }

        /// <summary>
        /// CONSULTAR EMPRESA
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
        [Route("ConsultaMasivaEmpresaBanco")]
        public ActionResult ConsultaMasivaEmpresaBanco(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "secuencia")] int secuencia,
            [FromQuery(Name = "tipoConsulta")] string tipoConsulta,
            [FromQuery(Name = "codBanco")] int codBanco,
            [FromQuery(Name = "codEmpresa")] string codEmpresa,
            [FromQuery(Name = "nombreEmpresa")] string nombreEmpresa,
            [FromQuery(Name = "filas")] int filas,
            [FromQuery(Name = "criterioBusqueda")] string criterioBusqueda,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio
        )
        {

            const string c_metodoNombre = "ConsultaMasivaEmpresaBanco";
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
                    url = string.Format("{0}{1}?secuencia={2}&tipoConsulta={3}&codBanco={4}&codEmpresa={5}" +
                        "&nombreEmpresa={6}&filas={7}&criterioBusqueda={8}&idTransaccion={9}&permiso={10}" +
                        "&parServicio={11}",
                        c_rutaAPI, c_metodoNombre, secuencia, tipoConsulta, codBanco, codEmpresa,
                        nombreEmpresa, filas, criterioBusqueda, idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }

        /// <summary>
        /// CONSULTAR EMPRESA
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codProducto"> CODIGO DE PRODUCTO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaMasivaEmpEspeciales")]
        public ActionResult ConsultaMasivaEmpEspeciales(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "tipoNuc")] string tipoNuc,
            [FromQuery(Name = "tipoCliente")] string tipoCliente,
            [FromQuery(Name = "criterioEmpresa")] string criterioEmpresa,
            [FromQuery(Name = "codProducto")] int codProducto,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio
        )
        {

            const string c_metodoNombre = "ConsultaMasivaEmpEspeciales";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codProducto={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codProducto,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return BadRequest(jsonError);

                }
            }

        }

        /// <summary>
        /// CONSULTAR EMPRESA
        /// </summary>
        /// <param name="cabUsuario"> CODIGO DEL USUARIO </param>
        /// <param name="cabLoginId"> CODIGO DE LA SESION </param>
        /// <param name="cabCompania"> CODIGO DE LA COMPAÑIA </param>
        /// <param name="cabBanco"> CODIGO DEL BANCO </param>
        /// <param name="cabTipoUsuario"> TIPO DE USUARIO QUE REALIZA LA PETICION </param>
        /// <param name="cabEstacion"> ESTACION DEL USUARIO </param>
        /// <param name="tipoNuc"> TIPO DE CRITERIO DE BUSQUEDA </param>
        /// <param name="tipoCliente"> TIPO DE CLIENTE </param>
        /// <param name="criterioEmpresa"> CRITERIO DE BUSQUEDA </param>
        /// <param name="codProducto"> CODIGO DE PRODUCTO </param>
        /// <param name="idTransaccion"> ID DE LA TRANSACCION </param>
        /// <param name="permiso"> PERMISO DE EJECUCIÓN </param>
        /// <param name="parServicio"> SERVICIO PARA VERIFICAR PERMISO DE EJECUCIÓ </param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaMasivaEmpAutori")]
        public ActionResult ConsultaMasivaEmpAutori(
            [FromHeader(Name = "cabUsuario")] string cabUsuario,
            [FromHeader(Name = "cabLoginId")] int cabLoginId,
            [FromHeader(Name = "cabCompania")] int cabCompania,
            [FromHeader(Name = "cabBanco")] int cabBanco,
            [FromHeader(Name = "cabTipoUsuario")] string cabTipoUsuario,
            [FromHeader(Name = "cabEstacion")] string cabEstacion,
            [FromQuery(Name = "tipoNuc")] string tipoNuc,
            [FromQuery(Name = "tipoCliente")] string tipoCliente,
            [FromQuery(Name = "criterioEmpresa")] string criterioEmpresa,
            [FromQuery(Name = "codProducto")] int codProducto,
            [FromQuery(Name = "idTransaccion")] string idTransaccion,
            [FromQuery(Name = "permiso")] string permiso,
            [FromQuery(Name = "parServicio")] string parServicio
        )
        {

            const string c_metodoNombre = "ConsultaMasivaEmpAutori";
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
                    url = string.Format("{0}{1}?tipoNuc={2}&tipoCliente={3}&criterioEmpresa={4}&codProducto={5}&" +
                        "idTransaccion={6}&permiso={7}&parServicio={8}",
                        c_rutaAPI, c_metodoNombre, tipoNuc, tipoCliente, criterioEmpresa, codProducto,
                        idTransaccion, permiso, parServicio);

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

                    // RETORNAR UN OBJETO DE ERROR CON EL CODIGO HTTP 400 EN CASO DE ERROR 
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
