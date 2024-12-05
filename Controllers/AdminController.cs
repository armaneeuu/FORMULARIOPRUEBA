using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FORMULARIOPRUEBA.Data;
using FORMULARIOPRUEBA.Models;
using Microsoft.AspNetCore.Authorization;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace FORMULARIOPRUEBA.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _converter;

        public AdminController(ApplicationDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataPrueba.ToListAsync());
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formulario = await _context.DataPrueba
                .Include(p => p.Estados) 
                .Include(p => p.Estadosa)
                .Include(p => p.Dialogo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (formulario == null)
            {
                return NotFound();
            }

            return View(formulario);
        }
        public IActionResult GetImage(int id)
        {
            var prueba = _context.DataPrueba.FirstOrDefault(p => p.Id == id);

            if (prueba == null || prueba.Imagen == null)
            {
                return NotFound();
            }

            return File(prueba.Imagen, "image/jpeg"); // Ajusta el tipo MIME según sea necesario
        }
        public IActionResult GetImagea(int id)
        {
            var prueba = _context.DataPrueba.FirstOrDefault(p => p.Id == id);

            if (prueba == null || prueba.Imagena == null)
            {
                return NotFound();
            }

            return File(prueba.Imagena, "image/jpeg"); // Ajusta el tipo MIME según sea necesario
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: Admin/Edit/5
        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.DataPrueba
                .Include(p => p.Estados)
                .Include(p => p.Estadosa)
                .Include(p => p.Dialogo)
                .FirstOrDefaultAsync(m => m.Id == id);
            

            if (prueba == null)
            {
                return NotFound();
            }

            return View(prueba);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prueba prueba, List<IFormFile> upload, List<IFormFile> uploada)
        {
            if (id != prueba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle file upload
                    if (upload != null && upload.Count > 0)
                    {
                        foreach (var up in upload)
                        {
                            using (var str = up.OpenReadStream())
                            {
                                using (var br = new BinaryReader(str))
                                {
                                    prueba.Imagen = br.ReadBytes((Int32)str.Length);
                                    prueba.ImagenName = Path.GetFileName(up.FileName);
                                }
                            }
                        }
                    }
                    if (uploada != null && uploada.Count > 0)
                    {
                        foreach (var up in uploada)
                        {
                            using (var str = up.OpenReadStream())
                            {
                                using (var br = new BinaryReader(str))
                                {
                                    prueba.Imagena = br.ReadBytes((Int32)str.Length);
                                    prueba.ImagenNamea = Path.GetFileName(up.FileName);
                                }
                            }
                        }
                    }
                    Console.WriteLine($"Imagen principal: {prueba.ImagenName}, Imagen secundaria: {prueba.ImagenNamea}");

                    // Update the main Prueba entity
                    _context.Update(prueba);
                    await _context.SaveChangesAsync();

                    // Handle associated Estados
                    if (prueba.Estados != null)
                    {
                        // Get existing estados for this Prueba
                        var existingEstados = await _context.DataEstados
                            .Where(e => e.PruebaId == prueba.Id)
                            .ToListAsync();

                        // Remove estados that are not in the current list
                        var estadosToRemove = existingEstados
                            .Where(existing => !prueba.Estados.Any(current =>
                                current.Id == existing.Id))
                            .ToList();

                        _context.DataEstados.RemoveRange(estadosToRemove);

                        // Update or add new estados
                        foreach (var estado in prueba.Estados)
                        {
                            estado.PruebaId = prueba.Id;

                            if (estado.Id == 0)
                            {
                                // New estado
                                _context.DataEstados.Add(estado);
                            }
                            else
                            {
                                // Existing estado
                                _context.Update(estado);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                    if (prueba.Estadosa != null)
                    {
                        // Get existing estados for this Prueba
                        var existingEstados = await _context.DataEstadosa
                            .Where(e => e.PruebaId == prueba.Id)
                            .ToListAsync();

                        // Remove estados that are not in the current list
                        var estadosToRemove = existingEstados
                            .Where(existing => !prueba.Estadosa.Any(current =>
                                current.Id == existing.Id))
                            .ToList();

                        _context.DataEstadosa.RemoveRange(estadosToRemove);

                        // Update or add new estados
                        foreach (var estadoa in prueba.Estadosa)
                        {
                            estadoa.PruebaId = prueba.Id;

                            if (estadoa.Id == 0)
                            {
                                // New estado
                                _context.DataEstadosa.Add(estadoa);
                            }
                            else
                            {
                                // Existing estado
                                _context.Update(estadoa);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                    if (prueba.Dialogo != null)
                    {
                        // Get existing estados for this Prueba
                        var existingDialogo = await _context.DataDialogo
                            .Where(e => e.PruebaId == prueba.Id)
                            .ToListAsync();

                        // Remove estados that are not in the current list
                        var dialogoToRemove = existingDialogo
                            .Where(existing => !prueba.Dialogo.Any(current =>
                                current.Id == existing.Id))
                            .ToList();

                        _context.DataDialogo.RemoveRange(dialogoToRemove);

                        // Update or add new estados
                        foreach (var dialogo in prueba.Dialogo)
                        {
                            dialogo.PruebaId = prueba.Id;

                            if (dialogo.Id == 0)
                            {
                                // New estado
                                _context.DataDialogo.Add(dialogo);
                            }
                            else
                            {
                                // Existing estado
                                _context.Update(dialogo);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PruebaExists(prueba.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(prueba);
        }
        private bool PruebaExists(int id)
        {
            return _context.DataPrueba.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formulario = await _context.DataPrueba
                .FirstOrDefaultAsync(m => m.Id == id);
            if (formulario == null)
            {
                return NotFound();
            }

            return View(formulario);
        }
        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formulario = await _context.DataPrueba.FindAsync(id);
            if (formulario != null)
            {
                _context.DataPrueba.Remove(formulario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormularioExists(int id)
        {
            return _context.DataPrueba.Any(e => e.Id == id);
        }
        public IActionResult ExportPdf(int id)
        {
            var formulario = _context.DataPrueba
                .Include(f => f.Estados) 
                .Include(f => f.Estadosa) 
                .FirstOrDefault(f => f.Id == id);

            if (formulario == null)
            {
                return NotFound();
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var logoUrl = $"{baseUrl}/images/logo.jpeg";

            // Crear contenido HTML para el PDF
            var estadosHtml = new StringBuilder();
            foreach (var estado in formulario.Estados)
            {
                estadosHtml.AppendLine($@"
            <h2>Estado {estado.Numero}: {estado.Nombre}</h2>
            <p>{estado.Descripcion}</p>");
            }

            var estadosaHtml = new StringBuilder();
            foreach (var estadoa in formulario.Estadosa)
            {
                estadosaHtml.AppendLine($@"
            <h2>Estado {estadoa.Numero}: {estadoa.Nombre}</h2>
            <p>{estadoa.Descripcion}</p>");
            }

            string imageSection = formulario.Imagen != null
            ? $"<div class='image-container'><img src='data:image/png;base64,{Convert.ToBase64String(formulario.Imagen)}' alt='Imagen de {formulario.Titulo}' style='width: 300px; height: auto;' /></div>"
            : "";
            string imageSectiona = formulario.Imagena != null
            ? $"<div class='image-container'><img src='data:image/png;base64,{Convert.ToBase64String(formulario.Imagena)}' alt='Imagen de {formulario.Titulo}' style='width: 300px; height: auto;' /></div>"
            : "";


            var htmlContent = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='utf-8' />
    <title>Formulario - {formulario.Titulo}</title>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        table {{ width: 100%; border-collapse: collapse; }}
        td, th {{ padding: 10px; border: 1px solid #000; }}
        h1 {{ font-size: 1.2em; margin: 0; }}
        .logo {{ text-align: left; }}
        .logo img {{ width: 120px; }}
        .info-table {{ text-align: right; font-size: 0.9em; }}
        .center-text {{ text-align: center; }}
        .highlight {{ color: #007bff; text-decoration: underline; }}
    </style>
</head>
<body>
    <table>
        <tr>
            <td class='logo'>
                <img src='{logoUrl}' alt='USMP Logo'>
            </td>
            <td class='center-text'>
                <h1>Formulario de desarrollo de ECS<br />FRT_002</h1>
                <p><a href='#' class='highlight'>{formulario.Titulo}</a></p>
            </td>
            <td class='info-table'>
                <p><strong>Código:</strong> FRT_002</p>
                <p><strong>Versión:</strong> 1.0</p>
                <p><strong>Fecha:</strong> {DateTime.Now:MMMM yyyy}</p>
                <p><strong>Página:</strong> 1</p>
            </td>
        </tr>
    </table>

    <hr />

    <section>
        <h1>Resumen del caso</h1>
        <h2>Sinopsis</h2>
        <p>{formulario.Sinopsis.Replace(Environment.NewLine, "<br />")}</p>

        <h1>Estados</h1>
        {estadosHtml}
        <h1>Flujograma</h1>
        {imageSection}

        <h2>Autores</h2>
        <p>{formulario.Autores.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Historial médico</h2>
        <p>{formulario.Historial_medico.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Alergias</h2>
        <p>{formulario.Alergias.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Medicamentos</h2>
        <p>{formulario.Medicamentos.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Historia familiar/social</h2>
        <p>{formulario.Historia_familiar.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Nota de ingreso</h2>
        <h2>Situación</h2>
        <p>{formulario.Situacion.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Nota de hospitalización</h2>
        <p>{formulario.Nota_de_hospitalizacion.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Exámen físico</h2>
        <h2>Signos vitales</h2>
        <p>{formulario.Signos_vitales.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Estado general</h2>
        <p>{formulario.Estado_general.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Piel</h2>
        <p>{formulario.Piel.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Tórax</h2>
        <p>{formulario.Torax.Replace(Environment.NewLine, "<br />")}</p>

        <h2>CV</h2>
        <p>{formulario.CV.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Abdomen</h2>
        <p>{formulario.Abdomen.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Laboratorio</h2>
        <p>{formulario.Laboratorio.Replace(Environment.NewLine, "<br />")}</p>

        <h1>Imágenes</h1>
        {imageSectiona}

        <h2>Órdenes</h2>
        <p>{formulario.Orden_inicial.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Preparación</h2>
        <h2>Objetivos del aprendizaje</h2>
        <h2>Distinguir</h2>
        <p>{formulario.Distinguir.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Indicar</h2>
        <p>{formulario.Indicar.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Analizar</h2>
        <p>{formulario.Analizar.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Evaluación</h2>
        <p>{formulario.Evaluación.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Aplicar</h2>
        <p>{formulario.Aplicar.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Desempeño como medida del aprendizaje</h2>
        <h2>Medidas esenciales de rendimiento del escenario</h2>
        <p>{formulario.Medidas_esenciales.Replace(Environment.NewLine, "<br />")}</p>

        <h2>Baseline</h2>
        <p>{formulario.Baseline.Replace(Environment.NewLine, "<br />")}</p>

        <h1>Estados</h1>
        {estadosaHtml}

    </section>
</body>
</html>";

            // Configuración del documento PDF
            var pdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    Margins = new MarginSettings { Top = 10, Bottom = 10 }
                },
                Objects = {
            new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = { Right = "Página [page] de [toPage]" }
            }
        }
            };

            // Convertir HTML a PDF y devolver el archivo
            var pdf = _converter.Convert(pdfDocument);
            return File(pdf, "application/pdf", "Formulario.pdf");
        }

    }
}
