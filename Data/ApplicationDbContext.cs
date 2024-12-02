using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FORMULARIOPRUEBA.Data;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<FORMULARIOPRUEBA.Models.Prueba> DataPrueba { get; set; }
    public DbSet<FORMULARIOPRUEBA.Models.Estados> DataEstados { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Asegúrate de llamar primero al método base
            
            // Configuración para la relación Prueba-Estado
            modelBuilder.Entity<FORMULARIOPRUEBA.Models.Prueba>()
                .HasMany(p => p.Estados)
        .WithOne(e => e.Prueba)
        .HasForeignKey(e => e.PruebaId)
        .OnDelete(DeleteBehavior.Cascade);
        }
}

