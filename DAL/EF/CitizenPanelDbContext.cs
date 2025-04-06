using System.Linq.Expressions;
using Domain;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using UI_MVC;

namespace DAL.EF;

public class CitizenPanelDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly OrganisationContext _organisationContext;
    public string OrganisationId => _organisationContext.Organisation.Id;
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PanelMember> PanelMembers { get; set; }
    public DbSet<RepresentationGroup> RepresentationGroups { get; set; }
    public DbSet<Criteria> Criteria { get; set; }
    public DbSet<CriteriaAnswerOption> CriteriaAnswerOptions { get; set; }
    public DbSet<CriteriaResponse> CriteriaResponses { get; set; }
    public DbSet<PlanningGroupMember> PlanningGroupMembers { get; set; }
    
    public CitizenPanelDbContext(DbContextOptions options, OrganisationContext organisationContext) : base(options)
    {
        _organisationContext = organisationContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var organisationalModels = modelBuilder.Model.GetEntityTypes()
                .Where(entity => typeof(IOrganisational).IsAssignableFrom(entity.ClrType)
                && !typeof(IdentityUser).IsAssignableFrom(entity.ClrType));
        foreach (var organisationalModel in organisationalModels)
        {
            modelBuilder.Entity(organisationalModel.ClrType)
                .HasQueryFilter<IOrganisational>(e => e.OrganisationId == OrganisationId )
                .HasIndex(nameof(IOrganisational.OrganisationId));
            
            modelBuilder.Entity(organisationalModel.ClrType)
                .Property(nameof(IOrganisational.OrganisationId))
                .IsRequired()
                .HasValueGenerator<TenantIdValueGenerator>();
        }
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CitizenPanelDbContext).Assembly);
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
        
        //Planningroepmember 1..*-* Panel
        modelBuilder.Entity<PlanningGroupMember>()
            .HasOne(pgm => pgm.Panel);
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
public static class QueryFilterExtensions
{
    public static EntityTypeBuilder HasQueryFilter<TInterface>(this EntityTypeBuilder entityTypeBuilder,
        Expression<Func<TInterface, bool>> filterExpression)
    {
        var param = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
        var body = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), param,
            filterExpression.Body);

        var lambdaExpression = Expression.Lambda(body, param);

        return entityTypeBuilder.HasQueryFilter(lambdaExpression);
    }
}
public class TenantIdValueGenerator : ValueGenerator<string>
{
    public override string Next(EntityEntry entry)
    {
        if (entry is { Entity: IOrganisational, Context: CitizenPanelDbContext appDbContext })
        {
            return appDbContext.OrganisationId;
        }

        throw new InvalidOperationException("Could not generate a new TenantId");
    }
    public override bool GeneratesTemporaryValues { get; }
        = false;
}