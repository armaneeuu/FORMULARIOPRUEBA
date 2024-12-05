using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FORMULARIOPRUEBA.Models
{
    [Table("t_dialogo")]
    public class Dialogo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Personaje { get; set; }

        public string? Guion { get; set; }

        [ForeignKey("Prueba")]
        public int PruebaId { get; set; }

        public Prueba? Prueba { get; set; }
    }
}