﻿using Renacer.Nucleo.Entidades;
using Renacer.Nucleo.Servicio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Renacer.Nucleo.Control
{
    public class ControlEvento
    {
        private static ControlEvento control;

        private ControlEvento() { }

        /// <summary>
        /// Metodo para devolver una instancia unica de la Clase.
        /// </summary>
        /// <returns></returns>
        public static ControlEvento devolverInstancia()
        {
            if (control == null)
                control = new ControlEvento();
            return control;
        }

        /// <summary>
        /// Metodo utilizado para Insertar un nuevo evento.
        /// </summary>
        /// <param name="evento"></param>
        public void grabar(Evento evento)
        {
            try
            {
                var errores = this.validar(evento);
                if (errores.Count > 0)
                {
                    throw new UsuarioException(errores);
                }

                evento.fechaCreacion = DateTime.Now;
                evento.fechaModificacion = DateTime.Now;

                using (var db = new ModeloRenacer())
                {
                    if (evento.responsable != null)
                    {
                        evento.responsable = db.encargado.Single(a => a.id == evento.responsable.id);
                        db.Entry(evento.responsable).State = System.Data.Entity.EntityState.Unchanged;
                        evento.responsable.domicilio = db.domicilio.Single(a => a.id == evento.responsable.idDomicilio);
                    }

                    if (evento.espacio != null)
                    {
                        evento.espacio = db.espacioComun.Single(a => a.id == evento.espacio.id);
                        db.Entry(evento.espacio).State = System.Data.Entity.EntityState.Unchanged;
                    }

                    if (evento.tipoEvento != null)
                    {
                        evento.tipoEvento = db.tipoEvento.Single(a => a.id == evento.tipoEvento.id);
                        evento.idTipoEvento = evento.tipoEvento.id;
                        db.Entry(evento.tipoEvento).State = System.Data.Entity.EntityState.Unchanged;
                    }

                    Tag[] listaTags = { };
                    if (evento.listaTags != null && evento.listaTags.Count > 0)
                    {
                        listaTags = new Tag[evento.listaTags.Count];
                        evento.listaTags.CopyTo(listaTags);
                        evento.listaTags.RemoveAll(tag => true);
                    }

                    foreach (DetalleEvento detalle in evento.listaDetalleEvento)
                    {
                        detalle.espacio = evento.espacio;
                        detalle.idEspacio = evento.idEspacio;
                        db.Entry(detalle.espacio).State = System.Data.Entity.EntityState.Unchanged;

                        detalle.idEncargado = evento.idEncargado;
                        detalle.responsable = evento.responsable;
                        detalle.idEspacio = evento.idEspacio;
                        db.Entry(detalle.responsable).State = System.Data.Entity.EntityState.Unchanged;
                        db.Entry(detalle.espacio).State = System.Data.Entity.EntityState.Unchanged;

                        if (string.IsNullOrEmpty(detalle.nombre)) detalle.nombre = evento.nombre;
                        if (string.IsNullOrEmpty(detalle.descripcion)) detalle.descripcion = evento.descripcion;

                        detalle.fechaCreacion = DateTime.Now;
                        detalle.fechaModificacion = DateTime.Now;
                    }

                    Socio[] listaSocios = { };
                    if (evento.listaSocios != null && evento.listaSocios.Count > 0)
                    {
                        listaSocios = new Socio[evento.listaSocios.Count];
                        evento.listaSocios.CopyTo(listaSocios);
                        evento.listaSocios.RemoveAll(socio => true);
                    }

                    Inscripcion[] listaInscripciones = { };
                    if (evento.listaInscripciones != null && evento.listaInscripciones.Count > 0)
                    {
                        listaInscripciones = new Inscripcion[evento.listaInscripciones.Count];
                        evento.listaInscripciones.CopyTo(listaInscripciones);
                        evento.listaInscripciones = new List<Inscripcion>();
                    }

                    db.evento.AddOrUpdate(evento);
                    db.SaveChanges();

                    if (listaInscripciones.Length > 0)
                    {
                        foreach (var inscripto in listaInscripciones) {
                            inscripto.fechaCreacion = DateTime.Now;
                            inscripto.fechaModificacion = DateTime.Now;
                            inscripto.Socio = db.socio.Where(enc => enc.id == inscripto.idSocio).FirstOrDefault();
                            inscripto.Evento = evento;
                            db.Entry(inscripto).State = System.Data.Entity.EntityState.Added;
                        }

                        evento.listaInscripciones.AddRange(listaInscripciones);
                    }

                    db.evento.AddOrUpdate(evento);
                    db.SaveChanges();

                    Evento eventoAux = db.evento.Include("listaTags").Single(a => a.id == evento.id);
                    ControlTag.devolverInstancia().actualizarListaDeTags(db, listaTags, eventoAux.listaTags);
                    if (evento.listaSocios != null && evento.listaSocios.Count > 0) ControlSocio.devolverInstancia().actualizarListaDeSocios(db, listaSocios, eventoAux.listaSocios);

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

        public void actualizar(Evento evento) {
            try
            {
                var errores = this.validar(evento);
                if (errores.Count > 0)
                {
                    throw new UsuarioException(errores);
                }

                evento.fechaModificacion = DateTime.Now;

                using (var db = new ModeloRenacer())
                {
                    if (evento.responsable != null)
                    {
                        evento.responsable = db.encargado.Single(a => a.id == evento.responsable.id);
                        db.Entry(evento.responsable).State = System.Data.Entity.EntityState.Unchanged;
                        evento.responsable.domicilio = db.domicilio.Single(a => a.id == evento.responsable.idDomicilio);
                    }

                    if (evento.espacio != null)
                    {
                        evento.espacio = db.espacioComun.Single(a => a.id == evento.espacio.id);
                        db.Entry(evento.espacio).State = System.Data.Entity.EntityState.Unchanged;
                    }

                    if (evento.tipoEvento != null)
                    {
                        evento.tipoEvento = db.tipoEvento.Single(a => a.id == evento.tipoEvento.id);
                        evento.idTipoEvento = evento.tipoEvento.id;
                        db.Entry(evento.tipoEvento).State = System.Data.Entity.EntityState.Unchanged;
                    }

                    Tag[] listaTags = { };
                    if (evento.listaTags != null && evento.listaTags.Count > 0)
                    {
                        listaTags = new Tag[evento.listaTags.Count];
                        evento.listaTags.CopyTo(listaTags);
                        evento.listaTags.RemoveAll(tag => true);
                    }
                    
                    Socio[] listaSocios = { };
                    if (evento.listaSocios != null && evento.listaSocios.Count > 0)
                    {
                        listaSocios = new Socio[evento.listaSocios.Count];
                        evento.listaSocios.CopyTo(listaSocios);
                        evento.listaSocios.RemoveAll(socio => true);
                    }

                    Inscripcion[] listaInscripciones = { };
                    if (evento.listaInscripciones != null && evento.listaInscripciones.Count > 0)
                    {
                        listaInscripciones = new Inscripcion[evento.listaInscripciones.Count];
                        evento.listaInscripciones.CopyTo(listaInscripciones);
                        evento.listaInscripciones.RemoveAll(inscrip => true);

                        foreach (var inscripto in listaInscripciones)
                        {
                            inscripto.fechaCreacion = DateTime.Now;
                            inscripto.fechaModificacion = DateTime.Now;
                            inscripto.Socio = db.socio.Where(enc => enc.id == inscripto.idSocio).FirstOrDefault();
                            inscripto.Evento = evento;
                            db.Entry(inscripto).State = System.Data.Entity.EntityState.Added;
                        }
                        evento.listaInscripciones.AddRange(listaInscripciones);
                    }

                    db.evento.AddOrUpdate(evento);
                    db.SaveChanges();

                    Evento eventoAux = db.evento.Include("listaTags").Single(a => a.id == evento.id);
                    ControlTag.devolverInstancia().actualizarListaDeTags(db, listaTags, eventoAux.listaTags);

                    if (evento.listaSocios != null && evento.listaSocios.Count > 0) ControlSocio.devolverInstancia().actualizarListaDeSocios(db, listaSocios, eventoAux.listaSocios);
                    // ControlDetalleEvento.devolverInstancia().actualizarListaDeDetalleEventos(db, listaDetalleEvento, eventoAux.listaDetalleEvento);
               
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

        public void darDeBaja(Evento evento) {
            var consulta = @"UPDATE detalleevento SET fechaBaja = NOW() where idEvento = {idEvento};
                             UPDATE evento SET fechaBaja = NOW() where id = {idEvento};";
            consulta = consulta.Replace("{idEvento}", evento.id.ToString());
            ControlReporte.GetInstance().ExecuteScalar(consulta);
        }
      
        public IList<Inscripcion> devolverInscriptos(int id)
        {
            try
            {
                using (var db = new ModeloRenacer())
                {
                    var inscriptos = db.inscripcion
                        .Include("Socio")
                        .Where(x => x.idEvento.Equals(id)).ToList();
                    return inscriptos;
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
            return new List<Inscripcion>();
        }

        /// var item = (from i in db.evento
        ///             where i.id.Equals(id)
        //              select i).FirstOrDefault();
        /// 
        /// 
        /// SELECT * FROM Evento WHERE id = 1;
        public Evento devolver(int id)
        {
            try
            {
                using (var db = new ModeloRenacer())
                {
                    var evento = db.evento
                        //Include("listaDetalleEvento")
                        .Include("espacio")
                        .Include("responsable")
                        .Include("tipoEvento")
                        .Where(x => x.id.Equals(id)).FirstOrDefault();

                    //if (evento.idEncargado > 0)  evento.responsable = db.encargado.FirstOrDefault(x => x.id.Equals(evento.idEncargado));
                    //if (evento.idEspacio > 0) evento.espacio = db.espacioComun.FirstOrDefault(x => x.id.Equals(evento.idEspacio));
                    //if (evento.idTipoEvento > 0)  evento.tipoEvento = db.tipoEvento.FirstOrDefault(x => x.id.Equals(evento.idTipoEvento));

                    evento.listaTags = db.tag.Where(x => x.listaEventos.Any(xy => xy.id.Equals(id))).ToList();
                    evento.listaDetalleEvento = db.detalleEvento.Where(x => x.idEvento.Equals(id)).ToList();
                    evento.listaSocios = db.socio.Where(x => x.listaInscripciones.Any(xy => xy.idEvento.Equals(id))).ToList();
                    // evento.listaInscripciones = db.inscripcion.Where(x => x.idEvento.Equals(id)).ToList();
                    return evento;
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
            return null;
        }

        /// <summary>
        /// Metodo utilizado para devolver todos los Evento
        /// SELECT * FROM Evento
        /// </summary>
        /// <returns></returns>
        public List<Evento> devolverTodos(String search)
        {
            try
            {
                using (var db = new ModeloRenacer())
                {
                    return db.evento
                        .Where(ev => ev.nombre.Contains(search) && ev.fechaBaja == null)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
            return null;
        }

        /// <summary>
        /// Metodo utilizado para eliminar un evento.
        /// TODO: El metodo se tiene que cambiar para actualizar un atributo del evento 
        /// para ver si esta eliminado o no, no se eliminan datos.
        /// </summary>
        public void eliminar(int id)
        {
            try
            {
                using (var db = new ModeloRenacer())
                {
                    db.evento.Remove(db.evento.Where(x => x.id.Equals(id)).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                ServicioSentry.devolverSentry().informarExcepcion(ex);
            }
        }

        private List<string> validar(Evento evento)
        {
            var errores = new List<string>();


            if (evento == null)
            {
                errores.Add("No se informo el evento");
            }

            try
            {
                if (string.IsNullOrEmpty(evento.nombre))
                    errores.Add("No se ingreso el nombre del evento");
                
            }
            catch
            {
                errores.Add("Faltan propiedades del evento");
            }


            return errores;
        }

    }
}