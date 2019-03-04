﻿using Renacer.Nucleo;
using Renacer.Nucleo.Control;
using Renacer.Nucleo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Renacer.WebAPI.Controllers
{
    [RoutePrefix("api/reporte")]
    public class ReporteController : ApiController
    {

        // GET: api/comun/tiposDocumentos
        [ActionName("tags")]
        [Route("tags")]
        [AcceptVerbs("POST")]
        public List<Dictionary<string, object>> Get([FromBody] ControlReporte.filterSocio filter)
        {
            return ControlReporte.GetInstance().GetTagsPorSocios(filter);
        }

        [ActionName("socios")]
        [Route("socios")]
        [AcceptVerbs("POST")]
        public List<Dictionary<string, object>> GetSocios([FromBody] ControlReporte.filterSocio filter)
        {
            return ControlReporte.GetInstance().GetSocios(filter);
        }
        [ActionName("crecimiento-socios")]
        [Route("crecimiento-socios")]
        [AcceptVerbs("POST")]
        public List<Dictionary<string, object>> GetCrecimientoSocios([FromBody] ControlReporte.filterSocio filter)
        {
            return ControlReporte.GetInstance().GetCrecimientoSocios(filter);
        }

        [ActionName("socios-por-edad")]
        [Route("socios-por-edad")]
        [AcceptVerbs("POST")]
        public List<Dictionary<string, object>> GetSociosPorEdad([FromBody] ControlReporte.filterSocio filter)
        {
            return ControlReporte.GetInstance().GetSociosPorEdad(filter);
        }



        [ActionName("dashboard")]
        [Route("dashboard")]
        [AcceptVerbs("POST")]
        public Dictionary<string, object> GetDashboard()
        {
            return ControlReporte.GetInstance().GetDashboard();
        }

        [ActionName("count")]
        [Route("count")]
        [AcceptVerbs("GET")]
        public object GetCount([FromUri] String Entidad)
        {
            var result = new Dictionary<string, long>();
            result.Add("count", ControlReporte.GetInstance().GetCount(Entidad));
            return result;
        }

        


    }
}