using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class CitizenPanelDbContext : DbContext
{
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PanelMember> PanelMembers { get; set; }

    public CitizenPanelDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=CitizenPanel_DB;Username=user;Password=password;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<PanelMember>()
            .HasOne(p => p.Panel)
            .WithMany(p => p.PanelMembers);
    }

    public bool CreateDatabase(bool dropDatabase)
    {
        if (dropDatabase)
        {
            Database.EnsureDeleted();
        }
        
        return Database.EnsureCreated();
    }
    
}