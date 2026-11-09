using CalidadAireApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalidadAireApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<SensorCalidadAire> Sensores => Set<SensorCalidadAire>();
    public DbSet<LecturaAire> Lecturas => Set<LecturaAire>();
    public DbSet<AlertaAire> Alertas => Set<AlertaAire>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SensorCalidadAire>(entidad =>
        {
            entidad.ToTable("SensoresCalidadAire");
            entidad.Property(s => s.Estado).HasConversion<string>().HasMaxLength(20);
        });

        modelBuilder.Entity<LecturaAire>(entidad =>
        {
            entidad.ToTable("LecturasAire");
            entidad.Property(l => l.PM2_5).HasPrecision(10, 2);
            entidad.Property(l => l.PM10).HasPrecision(10, 2);
            entidad.Property(l => l.CO2).HasPrecision(10, 2);
            entidad.HasIndex(l => new { l.SensorId, l.FechaHora });
            entidad.HasOne(l => l.Sensor)
                .WithMany(s => s.Lecturas)
                .HasForeignKey(l => l.SensorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AlertaAire>(entidad =>
        {
            entidad.ToTable("AlertasAire");
            entidad.Property(a => a.Nivel).HasConversion<string>().HasMaxLength(20);
            entidad.HasOne(a => a.Sensor)
                .WithMany(s => s.Alertas)
                .HasForeignKey(a => a.SensorId)
                .OnDelete(DeleteBehavior.Restrict);
            entidad.HasOne(a => a.Lectura)
                .WithMany()
                .HasForeignKey(a => a.LecturaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Usuario>(entidad =>
        {
            entidad.ToTable("Usuarios");
            entidad.HasIndex(u => u.Correo).IsUnique();
        });
    }
}