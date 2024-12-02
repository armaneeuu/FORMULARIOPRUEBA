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

        public FormularioController(ApplicationDbContext context, UserManager<IdentityUser>userManager)
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
        public async Task<IActionResult> Create(Prueba objFormulario)
        {
            _context.Add(objFormulario);
            _context.SaveChanges();
            ViewData["Message"] = "El formulario ya esta registrado";

            
            return RedirectToAction(nameof(Index2));
        }
        public IActionResult Edit(int id)
        {
            Prueba objFormulario = _context.DataPrueba.Find(id);
            if(objFormulario == null){
                return NotFound();
            }
            return View(objFormulario);
        }
        [HttpPost]
        public IActionResult Edit(int id,[Bind("Id,Titulo,Sinopsis,Estado1,Estado2,Estado3")] Prueba objFormulario)
        {
             _context.Update(objFormulario);
             _context.SaveChanges();
              ViewData["Message"] = "El formulario ya esta actualizado";
             return View(objFormulario);
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