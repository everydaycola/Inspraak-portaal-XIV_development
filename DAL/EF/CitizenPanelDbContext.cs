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
    
    //public DbSet<CriteriaGroup> CriteriaGroups { get; set; }
    //public DbSet<CriteriaValue> CriteriaValues { get; set; }
    //public DbSet<CriteriaAnswer> CriteriaAnswers { get; set; }

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

        /*
        //panelmember 1-* CriteriaGroup
        modelBuilder.Entity<PanelMember>()
            .HasOne(pm => pm.CriteriaGroup)
            .WithMany(cg => cg.PanelMembers);
        //criteriagroup 1-* criteriaAnswer
        modelBuilder.Entity<CriteriaGroup>()
            .HasMany(cg => cg.CriteriaAnswers)
            .WithOne(c => c.CriteriaGroup);

        //criteriaAnswer * - 1 criteria
        modelBuilder.Entity<CriteriaAnswer>()
            .HasOne(ca => ca.Criteria)
            .WithMany(c => c.CriteriaAnswers);
        modelBuilder.Entity<CriteriaAnswer>()
            .HasOne(ca => ca.CriteriaValue)
            .WithMany(c => c.CriteriaAnswers);
    //public Criteria criteria { get; set; }
    //public CriteriaValue criteriaValue { get; set; }
        //criteria 1 - * criteriavalues
        modelBuilder.Entity<Criteria>()
            .HasMany(c => c.Values)
            .WithOne(c => c.Criteria);*/

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