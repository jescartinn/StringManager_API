using Microsoft.EntityFrameworkCore;
using StringManager_API.Models;

namespace StringManager_API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Racquet> Racquets { get; set; } = null!;
    public DbSet<StringType> StringTypes { get; set; } = null!;
    public DbSet<Stringer> Stringers { get; set; } = null!;
    public DbSet<Tournament> Tournaments { get; set; } = null!;
    public DbSet<StringJob> StringJobs { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar User entity
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configurar relaciones y restricciones

        // Player - Racquet (1:N)
        modelBuilder.Entity<Racquet>()
            .HasOne(r => r.Player)
            .WithMany(p => p.Racquets)
            .HasForeignKey(r => r.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        // StringJob - Player (N:1)
        modelBuilder.Entity<StringJob>()
            .HasOne(sj => sj.Player)
            .WithMany(p => p.StringJobs)
            .HasForeignKey(sj => sj.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        // StringJob - Racquet (N:1)
        modelBuilder.Entity<StringJob>()
            .HasOne(sj => sj.Racquet)
            .WithMany(r => r.StringJobs)
            .HasForeignKey(sj => sj.RacquetId)
            .OnDelete(DeleteBehavior.Restrict);

        // StringJob - MainString (N:1)
        modelBuilder.Entity<StringJob>()
            .HasOne(sj => sj.MainString)
            .WithMany()
            .HasForeignKey(sj => sj.MainStringId)
            .OnDelete(DeleteBehavior.Restrict);

        // StringJob - CrossString (N:1)
        modelBuilder.Entity<StringJob>()
            .HasOne(sj => sj.CrossString)
            .WithMany()
            .HasForeignKey(sj => sj.CrossStringId)
            .OnDelete(DeleteBehavior.Restrict);

        // StringJob - Stringer (N:1)
        modelBuilder.Entity<StringJob>()
            .HasOne(sj => sj.Stringer)
            .WithMany(s => s.StringJobs)
            .HasForeignKey(sj => sj.StringerId)
            .OnDelete(DeleteBehavior.Restrict);

        // StringJob - Tournament (N:1)
        modelBuilder.Entity<StringJob>()
            .HasOne(sj => sj.Tournament)
            .WithMany(t => t.StringJobs)
            .HasForeignKey(sj => sj.TournamentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración para evitar columnas de navegación inversa
        modelBuilder.Entity<StringType>()
            .Ignore(st => st.StringJobs);
    }
}