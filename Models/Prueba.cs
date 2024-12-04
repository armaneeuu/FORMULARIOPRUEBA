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
        
    }
}