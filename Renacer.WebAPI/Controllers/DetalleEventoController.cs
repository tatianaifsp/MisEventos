﻿using Renacer.Nucleo.Control;
using Renacer.Nucleo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Renacer.WebAPI.Controllers
{
    public class DetalleEventoController : ApiController
    {
        // GET: api/Eventos
        public IEnumerable<DetalleEvento> Get([FromUri]DateTime fechaDesde, [FromUri]DateTime fechaHasta)
        {
            return ControlDetalleEvento.devolverInstancia().devolverTodos(fechaDesde,fechaHasta);
        }

        // GET: api/Eventos/5
        public DetalleEvento Get(int id)
        {
            return ControlDetalleEvento.devolverInstancia().devolver(id);
        }

        // POST: api/cliente
        public IHttpActionResult Post([FromBody]DetalleEvento value)
        {
            try
            {
                ControlDetalleEvento.devolverInstancia().grabar(value);
                return Ok(value);
            }
            catch (UsuarioException ex)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.errores));
            }
        }

        // PUT: api/cliente/5
        public void Put(int id, [FromBody]DetalleEvento value)
        {
            value.fechaModificacion = DateTime.Now;
            ControlDetalleEvento.devolverInstancia().grabar(value);
        }

        // DELETE: api/ApiCliente/5
        public void Delete(int id)
        {
            var DetalleEvento = ControlDetalleEvento.devolverInstancia().devolver(id);
            DetalleEvento.fechaBaja = DateTime.Now;
            ControlDetalleEvento.devolverInstancia().grabar(DetalleEvento);
        }
    }
}
