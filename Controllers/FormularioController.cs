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
public async Task<IActionResult> Create(Prueba prueba, List<IFormFile> upload)
{
    if (ModelState.IsValid)
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

        // Add the main Prueba entity
        _context.DataPrueba.Add(prueba);
        await _context.SaveChangesAsync();

        // Handle associated Estados - Reset ID before adding
        if (prueba.Estados != null && prueba.Estados.Count > 0)
        {
            foreach (var estado in prueba.Estados)
            {
                // Asociar correctamente el ID de la prueba
                estado.PruebaId = prueba.Id;

                // Solo agregar si es un nuevo estado
                if (estado.Id == 0)
                {
                    _context.DataEstados.Add(estado);
                }
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index2));
    }

    return View(prueba);
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
        .FirstOrDefaultAsync(m => m.Id == id);

    if (prueba == null)
    {
        return NotFound();
    }

    return View(prueba);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Prueba prueba, List<IFormFile> upload)
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