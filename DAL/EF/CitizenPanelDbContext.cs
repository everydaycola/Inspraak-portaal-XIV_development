using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class CitizenPanelDbContext : DbContext
{
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PanelMember> PanelMembers { get; set; }
    public DbSet<RepresentationGroup> RepresentationGroups { get; set; }
    public DbSet<Criteria> Criteria { get; set; }
    public DbSet<PanelMemberCriteria> PanelMemberCriteria { get; set; }

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
        // panel 1-* panelmember
        modelBuilder.Entity<PanelMember>()
            .HasOne(p => p.Panel)
            .WithMany(p => p.PanelMembers);
        // panel 1-1 representationgroup
        modelBuilder.Entity<RepresentationGroup>()
            .HasOne(p => p.Panel)
            .WithOne(p => p.RepresentationGroup)
            .HasForeignKey<RepresentationGroup>("PanelId");
        // panelmember 1-* panelmembercriteria *-1 criteria
        modelBuilder.Entity<PanelMember>()
            .HasMany(pm => pm.Criteria)
            .WithOne(pmc => pmc.PanelMember);
        modelBuilder.Entity<Criteria>()
            .HasMany(c => c.PanelMembers)
            .WithOne(pmc => pmc.Criteria);
        
        
        // shared primary key of PanelMemberCriteria
        modelBuilder.Entity<PanelMemberCriteria>()
            .Property("CriteriaId");
        modelBuilder.Entity<PanelMemberCriteria>()
            .Property("PanelMemberId");
        modelBuilder.Entity<PanelMemberCriteria>()
            .HasKey("CriteriaId", "PanelMemberId");
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