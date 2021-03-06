﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renacer.Nucleo.Entidades
{
    [Table("Inscripcion")]
    public class Inscripcion   
    {
        [Key]
        public int id { get; set; }

        public int idEvento { get; set; }
        [ForeignKey("idEvento")]
        public Evento evento { get; set; }

        public List<Pago> listaPagos { get; set; }

        public int idSocio { get; set; }
        [ForeignKey("idSocio")]
        public Socio socio { get; set; }

        public DateTime fechaCreacion { get; set; }
         public DateTime? fechaBaja { get; set; }

        public String estado { get; set; } = "ADEUDADO"; // PAGADO ADEUDADO
       
        // public DateTime? fechaModificacion { get; set; }

    }
}
