using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BIZBANK.V2020.CAD.GENERAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InicioController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Servicio Levantado CAD GENERAL");
        }

    }
}