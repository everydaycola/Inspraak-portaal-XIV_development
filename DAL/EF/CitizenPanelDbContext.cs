using Domain.CitizenPanel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class CitizenPanelDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PanelMember> PanelMembers { get; set; }
    public DbSet<RepresentationGroup> RepresentationGroups { get; set; }
    public DbSet<Criteria> Criteria { get; set; }
    public DbSet<CriteriaGroup> CriteriaGroups { get; set; }
    public DbSet<CriteriaValue> CriteriaValues { get; set; }
    public DbSet<CriteriaAnswer> CriteriaAnswers { get; set; }

    public CitizenPanelDbContext(DbContextOptions<CitizenPanelDbContext> options) : base(options)
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
        //Criteria * - 1 panels
        modelBuilder.Entity<Panel>()
            .HasMany(p => p.PanelCriteria)
            .WithOne(p => p.Panel);
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
            .WithOne(c => c.Criteria);

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