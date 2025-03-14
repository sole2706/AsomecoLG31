using ASOMAMECO.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ASOMAMECO.Data.Context
{
    public class ASOMAMECOContext : DbContext
    {
        public ASOMAMECOContext(DbContextOptions<ASOMAMECOContext> options)
            : base(options)
        {
        }

        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones y restricciones
            modelBuilder.Entity<Miembro>()
                .HasIndex(m => m.Id)
                .IsUnique();

            modelBuilder.Entity<Miembro>()
                .HasIndex(m => m.Cedula)
                .IsUnique();

            modelBuilder.Entity<Asistencia>()
                .HasOne(a => a.Miembro)
                .WithMany(m => m.Asistencias)
                .HasForeignKey(a => a.MiembroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asistencia>()
                .HasOne(a => a.Evento)
                .WithMany(e => e.Asistencias)
                .HasForeignKey(a => a.EventoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Evitar que un miembro se registre más de una vez en un evento
            modelBuilder.Entity<Asistencia>()
                .HasIndex(a => new { a.MiembroId, a.EventoId })
                .IsUnique();

            modelBuilder.Entity<Usuario>()
           .HasIndex(u => u.UsuarioName)
           .IsUnique(); 

            
        }
    }
    
}
