using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FORMULARIOPRUEBA.Models;
using FORMULARIOPRUEBA.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;


namespace FORMULARIOPRUEBA.Controllers
{
    [Authorize]
    public class FormularioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FormularioController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index2()
        {
            return View(_context.DataPrueba.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prueba prueba, List<IFormFile> upload, List<IFormFile> uploada, List<IFormFile> uploadArchivo)
        {
            if (ModelState.IsValid)
            {
                // Primera imagen
                if (upload != null && upload.Count > 0)
                {
                    var up = upload.First();
                    using (var str = up.OpenReadStream())
                    {
                        using (var br = new BinaryReader(str))
                        {
                            prueba.Imagen = br.ReadBytes((Int32)str.Length);
                            prueba.ImagenName = Path.GetFileName(up.FileName);
                        }
                    }
                }

                // Segunda imagen
                if (uploada != null && uploada.Count > 0)
                {
                    var up = uploada.First();
                    using (var str = up.OpenReadStream())
                    {
                        using (var br = new BinaryReader(str))
                        {
                            prueba.Imagena = br.ReadBytes((Int32)str.Length);
                            prueba.ImagenNamea = Path.GetFileName(up.FileName);
                        }
                    }
                }

                if (uploadArchivo != null && uploadArchivo.Count > 0)
                {
                    var up = uploadArchivo.First();
                    using (var str = up.OpenReadStream())
                    {
                        using (var br = new BinaryReader(str))
                        {
                            prueba.Archivo = br.ReadBytes((Int32)str.Length);
                            prueba.ArchivoName = Path.GetFileName(up.FileName);

                            // Extraer texto del PDF
                            if (Path.GetExtension(up.FileName).ToLower() == ".pdf")
                            {
                                prueba.ArchivoTextoExtraido = ExtractTextFromPdf(prueba.Archivo);
                            }
                        }
                    }
                }


                // Depuración: Verificar valores antes de guardar
                Console.WriteLine($"Imagen principal: {prueba.ImagenName}, Imagen secundaria: {prueba.ImagenNamea}, Archivo: {prueba.ArchivoName}");

                // Agregar entidad principal
                _context.DataPrueba.Add(prueba);
                await _context.SaveChangesAsync();

                // Manejar Estados asociados
                if (prueba.Estados != null && prueba.Estados.Count > 0)
                {
                    foreach (var estado in prueba.Estados)
                    {
                        estado.PruebaId = prueba.Id;

                        if (estado.Id == 0)
                        {
                            _context.DataEstados.Add(estado);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                if (prueba.Estadosa != null && prueba.Estadosa.Count > 0)
                {
                    foreach (var estadoa in prueba.Estadosa)
                    {
                        estadoa.PruebaId = prueba.Id;

                        if (estadoa.Id == 0)
                        {
                            _context.DataEstadosa.Add(estadoa);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                if (prueba.Dialogo != null && prueba.Dialogo.Count > 0)
                {
                    foreach (var dialogo in prueba.Dialogo)
                    {
                        dialogo.PruebaId = prueba.Id;

                        if (dialogo.Id == 0)
                        {
                            _context.DataDialogo.Add(dialogo);
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index2));
            }

            return View(prueba);
        }
        private string ExtractTextFromPdf(byte[] pdfBytes)
        {
            using (var memoryStream = new MemoryStream(pdfBytes))
            {
                using (var pdfReader = new PdfReader(memoryStream))
                {
                    using (var pdfDocument = new PdfDocument(pdfReader))
                    {
                        var textBuilder = new StringBuilder();

                        for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                        {
                            var page = pdfDocument.GetPage(i);
                            var strategy = new LocationTextExtractionStrategy();

                            PdfTextExtractor.GetTextFromPage(page, strategy);
                            textBuilder.Append(strategy.GetResultantText());
                        }

                        return textBuilder.ToString();
                    }
                }
            }
        }
        // Existing method to display the image


        // Existing method to display the image

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
                        var existingEstados = await _context.DataDialogo
                            .Where(e => e.PruebaId == prueba.Id)
                            .ToListAsync();

                        // Remove estados that are not in the current list
                        var dialogoToRemove = existingEstados
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

                    return RedirectToAction(nameof(Index2));
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

        public IActionResult Delete(int id)
        {
            Prueba objFormulario = _context.DataPrueba.Find(id);
            _context.DataPrueba.Remove(objFormulario);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index2));
        }
    }
}