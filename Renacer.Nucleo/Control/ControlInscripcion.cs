﻿

using Renacer.Nucleo.Entidades;
using Renacer.Nucleo.Servicio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Renacer.Nucleo.Control
{
    public class ControlInscripcion
    {
        private static ControlInscripcion control;

        private ControlInscripcion() { }

        /// <summary>
        /// Metodo para devolver una instancia unica de la Clase.
        /// </summary>
        /// <returns></returns>
        public static ControlInscripcion devolverInstacia()
        {
            if (control == null)
                control = new ControlInscripcion();
            return control;
        }

        /// <summary>
        /// Metodo utilizado para Insertar un nuevo inscripcion.
        /// </summary>
        /// <param name="inscripcion"></param>
        public void grabar(Inscripcion inscripcion)
        {
            try
            {
                var errores = this.validar(inscripcion);
                if (errores.Count > 0)
                {
                    throw new UsuarioException(errores);
                }
                Evento evento = ControlEvento.devolverInstancia().devolver(inscripcion.idEvento);
                List<Pago> listaPagos = new List<Pago>();
                if (inscripcion.id == 0) inscripcion.fechaCreacion = DateTime.Now;

                if (!evento.gratuito)
                {
                    int contador = 1;
                    foreach (var cuota in evento.listaCuotas)
                    {
                        Pago pago = new Pago();
                        pago.nombre = "Cuota " + contador;
                        pago.monto = (float)(evento.monto / evento.cantidadCuota);
                        pago.idCuota = cuota.id;
                        listaPagos.Add(pago);
                        contador++;
                    }
                    inscripcion.listaPagos = listaPagos;
                }

                //if (!inscripcion.evento.gratuito)
                //{
                //    for (int i = 1; i <= inscripcion.evento.cantidadCuota; i++)
                //    {
                //        Pago pago = new Pago();
                //        pago.nombre = "Cuota " + i;
                //        pago.monto = (float)(inscripcion.evento.monto / inscripcion.evento.cantidadCuota);

                //        listaPagos.Add(pago);

                //    }
                //    inscripcion.listaPagos = listaPagos;

                //}



                using (var db = new ModeloRenacer())
                {
                    if (inscripcion.socio != null)
                    {
                        db.Entry(inscripcion.socio).State = System.Data.Entity.EntityState.Unchanged;
                    }

                    if (inscripcion.evento != null)
                    {
                        db.Entry(inscripcion.evento).State = System.Data.Entity.EntityState.Unchanged;
                    }
                    
                    db.inscripcion.AddOrUpdate(inscripcion);
                    db.SaveChanges();
                    //db.SaveChanges();
                }
            }
            catch (UsuarioException ex)
            {
                ServicioSentry.devolverSentry().informarExcepcionUsuario(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
        }


        public Inscripcion devolver(int id)
        {
            try
            {
                using (var db = new ModeloRenacer())
                {
                    var item = db.inscripcion.
                        Include("socio").
                        Include("evento").
                        Include("listaPagos").Where(x => x.id.Equals(id) && x.fechaBaja == null).FirstOrDefault();
                    return item;
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
            return null;
        }

        /// <summary>
        /// Metodo utilizado para devolver todos los Inscripcion
        /// SELECT * FROM Inscripcion
        /// </summary>
        /// <returns></returns>
        public List<Inscripcion> devolverTodos(int? idEvento, int? idSocio)
        {
            try
            {
                using (var db = new ModeloRenacer())
                {
                    if (idSocio != null && idSocio > 0)
                        return db.inscripcion
                                .Include("listaPagos")
                                .Include("evento")
                                .Where(x => x.idSocio == idSocio && x.fechaBaja == null)
                                .ToList();
                    else if (idEvento != null && idEvento > 0)
                        return db.inscripcion
                             .Include("listaPagos")
                             .Include("socio")
                             .Where(x => x.evento.id == idEvento && x.fechaBaja == null).ToList();

                    else
                        return db.inscripcion.ToList();
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
            return null;
        }

        public List<Inscripcion> devolverInscripcionEvento(int idEvento)
        {

            try
            {
                using (var db = new ModeloRenacer())
                {
                    return db.inscripcion
                            .Include("ListaPagos")
                            .Include("Socio")
                            .Where(x => x.evento.id == idEvento && x.fechaBaja == null).ToList();
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
            return null;
        }

        /// <summary>
        /// Metodo utilizado para eliminar un inscripcion.
        /// TODO: El metodo se tiene que cambiar para actualizar un atributo del inscripcion 
        /// para ver si esta eliminado o no, no se eliminan datos.
        /// </summary>
        public void eliminar(int id)
        {
            try
            {
                using (var db = new ModeloRenacer())
                {
                    db.inscripcion.Remove(db.inscripcion.Where(x => x.id.Equals(id)).FirstOrDefault());
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
        }

        public List<Inscripcion> devolverInscripcionXSocio(int idSocio) {

            try
            {
                using (var db = new ModeloRenacer())
                {
                   
                        return db.inscripcion
                                .Include("listaPagos")
                                .Include("evento")
                                
                                .Where(x => x.idSocio == idSocio )
                                .ToList();
                  
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
            return null;

        }

        public void update(Inscripcion inscripcion)
        {
            try
            {

                using (var db = new ModeloRenacer())
                {


                    db.inscripcion.AddOrUpdate(inscripcion);
                    db.SaveChanges();
                }
            }
            catch (UsuarioException ex)
            {
                ServicioSentry.devolverSentry().informarExcepcionUsuario(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
        }

        private List<string> validar(Inscripcion inscripcion)
        {
            var errores = new List<string>();

            if (inscripcion == null)
            {
                errores.Add("No se informo la inscripcion");
            }

            return errores;
        }
    }
}
