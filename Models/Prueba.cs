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

        public List<Estadosa>? Estadosa { get; set; }

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

        [Column("orden_inicial")]
        public string? Orden_inicial { get; set; }

        [Column("distinguir")]
        public string? Distinguir { get; set; }

        [Column("indicar")]
        public string? Indicar { get; set; }

        [Column("analizar")]
        public string? Analizar { get; set; }

        [Column("evaluación")]
        public string? Evaluación { get; set; }

        [Column("aplicar")]
        public string? Aplicar { get; set; }

        [Column("medidas_esenciales")]
        public string? Medidas_esenciales { get; set; }

        [Column("baseline")]
        public string? Baseline { get; set; }

        [Column("preguntas_de_preparacion")]
        public string? Preguntas_de_preparacion { get; set; }

        [Column("equipos_de_suministro")]
        public string? Equipos_de_suministro { get; set; }

        public List<Dialogo>? Dialogo { get; set; }

        [Column("confederado")]
        public string? Confederado { get; set; }

        [Column("archivo")]
        public Byte[]? Archivo { get; set; }

        [Column("archivo_name")]
        public String? ArchivoName { get; set; }

        public string? ArchivoTextoExtraido { get; set; }

        public List<Status>? Status { get; set; }

        [Column("Imagenc")]
        public Byte[]? Imagenc { get; set; }

        [Column("imagenamec")]
        public String? ImagenNamec { get; set; }
    }
}