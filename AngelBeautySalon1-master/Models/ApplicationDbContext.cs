using Microsoft.EntityFrameworkCore;

namespace AngelBeautySalon1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cita> Citas { get; set; }

        public DbSet<Producto> Productos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.ClienteId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Direccion).HasMaxLength(200);
            });

            // Configuración de Servicio
            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.HasKey(e => e.ServicioId);
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Categoria).IsRequired().HasMaxLength(50);
            });

            // Configuración de Cita
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.CitaId);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Notas).HasMaxLength(500);

                // Relación con Cliente
                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.Citas)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación con Servicio
                entity.HasOne(e => e.Servicio)
                    .WithMany(s => s.Citas)
                    .HasForeignKey(e => e.ServicioId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Índices
                entity.HasIndex(e => e.Hora);
                entity.HasIndex(e => e.Estado);
                entity.HasIndex(e => new { e.ClienteId, e.Hora }); 
            });
        }
    }
}