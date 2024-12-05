using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FORMULARIOPRUEBA.Models
{
    [Table("t_prueba")]
    public class Prueba
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public string? Titulo { get; set; }

        [Column("sinopsis")]
        public string? Sinopsis { get; set; }

        
        public List<Estados>? Estados { get; set; } 

        [Column("Imagen")]
        public Byte[]? Imagen { get; set; }
        [Column("imagename")]
        public String? ImagenName { get; set; }

        [Column("autores")]
        public string? Autores { get; set; }

        [Column("historial_medico")]
        public string? Historial_medico { get; set; }

        [Column("alergias")]
        public string? Alergias { get; set; }

        [Column("medicamentos")]
        public string? Medicamentos { get; set; }

        [Column("historial_familiar")]
        public string? Historia_familiar { get; set; }
        
    }
}