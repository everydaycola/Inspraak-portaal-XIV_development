using Domain.CitizenPanel;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class CitizenPanelDbContext : DbContext
{
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PanelMember> PanelMembers { get; set; }
    public DbSet<RepresentationGroup> RepresentationGroups { get; set; }
    public DbSet<Criteria> Criteria { get; set; }
    public DbSet<CriteriaAnswerOption> CriteriaAnswerOptions { get; set; }
    public DbSet<CriteriaResponse> CriteriaResponses { get; set; }
    
    public CitizenPanelDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // panel 1-1 representationgroup
        modelBuilder.Entity<RepresentationGroup>()
            .HasOne(p => p.Panel)
            .WithOne(p => p.RepresentationGroup)
            .HasForeignKey<RepresentationGroup>("PanelId");
        
        //Criteria * - 1 panels
        modelBuilder.Entity<Panel>()
            .HasMany(p => p.Criteria);
        
        //Criteria 1 - * Answeroptions.
        modelBuilder.Entity<Criteria>()
            .HasMany(c => c.AnswerOptions)
            .WithOne();

        // CriteriaResponse 1 - * Criteria
        modelBuilder.Entity<CriteriaResponse>()
            .HasOne(c => c.Criteria);
        
        // Panelmember 1 - * CriteriaResponse
        modelBuilder.Entity<PanelMember>()
            .HasMany(pm => pm.Responses);
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