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
        .Include(p => p.Estados) // Incluir los estados relacionados
        .FirstOrDefaultAsync(m => m.Id == id);
    if (formulario == null)
    {
        return NotFound();
    }

    return View(formulario);
}


        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Prueba prueba)
{
    if (ModelState.IsValid)
    {
        // Guardar la entidad Prueba
        _context.DataPrueba.Add(prueba);
        _context.SaveChanges();

        // Guardar los estados asociados con esta prueba
        for (int i = 0; i < prueba.Estados.Count; i++)
        {
            // Asignar el nombre a cada estado como "Estado X"
            prueba.Estados[i].Nombre = "Estado " + (i + 1);
            prueba.Estados[i].PruebaId = prueba.Id; // Asociar cada estado con la prueba
            _context.DataEstados.Add(prueba.Estados[i]);
        }

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    return View(prueba);
}




        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Prueba objFormulario = _context.DataPrueba.Find(id);
            if (objFormulario == null)
            {
                return NotFound();
            }
            return View(objFormulario);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Titulo,Sinopsis,Estado1,Estado2,Estado3")] Prueba objFormulario)
        {
            _context.Update(objFormulario);
            _context.SaveChanges();
            ViewData["Message"] = "El formulario ya esta actualizado";
            return View(objFormulario);
        }

        // GET: Admin/Delete/5
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
        .Include(f => f.Estados) // Asegúrate de incluir los Estados
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
   