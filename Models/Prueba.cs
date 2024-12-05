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

        [Column("situacion")]
        public string? Situacion { get; set; }

        [Column("nota_de_hospitalizacion")]
        public string? Nota_de_hospitalizacion { get; set; }

        [Column("signos_vitales")]
        public string? Signos_vitales { get; set; }

        [Column("estado_general")]
        public string? Estado_general { get; set; }

        [Column("piel")]
        public string? Piel { get; set; }

        [Column("torax")]
        public string? Torax { get; set; }

        [Column("cv")]
        public string? CV { get; set; }

        [Column("abdomen")]
        public string? Abdomen { get; set; }

        [Column("laboratorio")]
        public string? Laboratorio { get; set; }

        [Column("Imagena")]
        public Byte[]? Imagena { get; set; }
        [Column("imagenamea")]
        public String? ImagenNamea { get; set; }

        
    }
}