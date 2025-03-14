using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class CitizenPanelDbContext : DbContext
{
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PanelMember> PanelMembers { get; set; }
    public DbSet<RepresentationGroup> RepresentationGroups { get; set; }
    public DbSet<Criteria> Criteria { get; set; }
    public DbSet<CriteriaGroup> CriteriaGroups { get; set; }

    public CitizenPanelDbContext(DbContextOptions options) : base(options)
    {
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
        
        //panelmember 1-* CriteriaGroup 1-*
        modelBuilder.Entity<PanelMember>()
            .HasOne(pm => pm.CriteriaGroup)
            .WithMany(cg => cg.PanelMembers);
        //criteriagroup 1-* criteria
        modelBuilder.Entity<CriteriaGroup>()
            .HasMany(cg => cg.Criteria)
            .WithOne(c => c.CriteriaGroup);
        
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