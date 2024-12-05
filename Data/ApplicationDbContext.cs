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
    public DbSet<FORMULARIOPRUEBA.Models.Estadosa> DataEstadosa { get; set; }
    public DbSet<FORMULARIOPRUEBA.Models.Dialogo> DataDialogo { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Llamar al método base primero

        // Configuración para la relación Prueba-Estados
        modelBuilder.Entity<FORMULARIOPRUEBA.Models.Prueba>()
            .HasMany(p => p.Estados)
            .WithOne(e => e.Prueba)
            .HasForeignKey(e => e.PruebaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración para la relación Prueba-Estadosa
        modelBuilder.Entity<FORMULARIOPRUEBA.Models.Prueba>()
            .HasMany(p => p.Estadosa)
            .WithOne(e => e.Prueba)
            .HasForeignKey(e => e.PruebaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FORMULARIOPRUEBA.Models.Prueba>()
        .HasMany(p => p.Dialogo)
        .WithOne(e => e.Prueba)
        .HasForeignKey(e => e.PruebaId)
        .OnDelete(DeleteBehavior.Cascade);
    }




}

