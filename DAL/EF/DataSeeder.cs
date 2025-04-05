using Domain.CitizenPanel;
using Microsoft.AspNetCore.Identity;

namespace DAL.EF;

public static class DataSeeder
{
    private static CitizenPanelDbContext _context;

    public static void Seed(CitizenPanelDbContext context)
    {
        Console.WriteLine("Seeding...");
        _context = context;
        //REPRESENTATION GROUPS
        var rg1 = new RepresentationGroup
        {
            CitizenCount =20000,
            ReservePercentage = 0.2, 
            ResponseRate = 0.1
            
        };
        //PANELS
        var panel1 = new Panel
        {
            Name="Verkeersveiligheid in en rond Antwerpen.", 
            SampleRate = 0.005,
            Owner = context.Users.Single(user => user.Email == "user@antwerpen.be"),
            OrganisationId = "antwerpen"
        };
        //PlanningGroupMembers
        var PlanningGroupUser1 = new IdentityUser
        {
            Email = "pgm@antwerpen.be",
            NormalizedEmail = "PGM@ANTWERPEN.BE",
            UserName = "PGM",
            NormalizedUserName = "PGM"
        };
        var PlanningGroupUser2 = new IdentityUser
        {
            Email = "owner@antwerpen.be",
            NormalizedEmail = "OWNER@ANTWERPEN.BE",
            UserName = "Owner",
            NormalizedUserName = "Owner"
        };
        var PlanningGroupUser3 = new IdentityUser
        {
            Email = "JanDeRijke@antwerpen.be",
            NormalizedEmail = "JanDeRijke@ANTWERPEN.BE",
            UserName = "Jan De Rijke",
            NormalizedUserName = "JANDERIJKE"
        };
        _context.Users.Add(PlanningGroupUser1);
        _context.Users.Add(PlanningGroupUser2);
        _context.Users.Add(PlanningGroupUser3);
        var pgm1 = new PlanningGroupMember
        {
            Panel = panel1,
            User = PlanningGroupUser1
        };
        var pgm2 = new PlanningGroupMember
        {
            Panel = panel1,
            User = PlanningGroupUser2
        };
        var pgm3 = new PlanningGroupMember
        {
            Panel = panel1,
            User = PlanningGroupUser3
        };
        
        //PanelMembers
        var panelMember1 = new PanelMember
        {
            Panel = panel1, 
            Responses = new List<CriteriaResponse>()
        };
        var panelMember2 = new PanelMember
        {
            Panel = panel1,
            Responses = new List<CriteriaResponse>()
            
        };
        var panelMember3 = new PanelMember
        {
            Panel = panel1,
            Responses = new List<CriteriaResponse>()
        };
        var panelMember4 = new PanelMember
        {
            Panel = panel1,
            Responses = new List<CriteriaResponse>()
        };
        var panelMember5 = new PanelMember
        {
            Panel = panel1,
            Responses = new List<CriteriaResponse>()
        };
        
        //CriteriaGroup (default groups SHOULD be based on default values only)!
        //CRITERIA
        var criteria1 = new Criteria
        {
            Name="Rijbewijs",
            Question="Beschikt u over een rijbewijs?",
            IsDefault = false,
            AnswerOptions =new List<CriteriaAnswerOption>()
        };
        var criteria2 = new Criteria
        {
            Name="Vervoermethode",
            Question="Wat is uw voorkeurs vervoersmethode?", 
            IsDefault = false,
            AnswerOptions =new List<CriteriaAnswerOption>()
        };
        var criteria3 = new Criteria
        {
            Name="Geslacht",
            Question="Wat is uw geslacht?",
            IsDefault=true,
            AnswerOptions =new List<CriteriaAnswerOption>()
        };
        
        //criteriaAnswerOptions
        var cao1 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Ja"
        };
        var cao2 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Nee"
        };
        var cao3 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.33,
            Option = "Te voet"
        };
        var cao4 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.33,
            Option = "Fiets"
        };
        var cao5 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.33,
            Option = "Auto"
        };
        var cao6 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Man"
        };
        var cao7 = new CriteriaAnswerOption
        {
            DistributionPercentage = 0.5,
            Option = "Vrouw"
        };
        
        //seeding default answers to default criteria
        var cr1 = new CriteriaResponse
        {
            Criteria = criteria3,
            SelectedOption = "Man"
        };
        var cr2 = new CriteriaResponse
        {
            Criteria = criteria3,
            SelectedOption = "Man"
        };
        var cr3 = new CriteriaResponse
        {
            Criteria = criteria3,
            SelectedOption = "Vrouw"
        };
        var cr4 = new CriteriaResponse
        {
            Criteria = criteria3,
            SelectedOption = "Vrouw"
        };
        var cr5 = new CriteriaResponse
        {
            Criteria = criteria3,
            SelectedOption = "Man"
        };
        
        
        //LINK PANELMEMBERS WITH CRITERIARESPONSES
        panelMember1.Responses.Add(cr1);
        panelMember2.Responses.Add(cr2);
        panelMember3.Responses.Add(cr3);
        panelMember4.Responses.Add(cr4);
        panelMember5.Responses.Add(cr5);
        
    
        //LINK REP. GROUP WITH PANEL
        rg1.Panel = panel1;
        panel1.RepresentationGroup = rg1;
        
        //LINK ANSWER OPTIONS WITH CRITERIA
        criteria1.AnswerOptions.Add(cao1);
        criteria1.AnswerOptions.Add(cao2);
        criteria2.AnswerOptions.Add(cao3);
        criteria2.AnswerOptions.Add(cao4);
        criteria2.AnswerOptions.Add(cao5);
        criteria3.AnswerOptions.Add(cao6);
        criteria3.AnswerOptions.Add(cao7);
        

        //LINK CRITERIA WITH PANEL
        panel1.Criteria = new List<Criteria>() { criteria1, criteria2 , criteria3};
        
        //SET REGISTRATION TO OPEN
        panel1.IsRegistrationOpen = true;

        //SAVE TO DATABASE
        context.RepresentationGroups.Add(rg1);
        context.Panels.Add(panel1);
        
        AddMultipleEntities([cao1, cao2,cao3,cao4,cao5,cao6,cao7]);
        AddMultipleEntities([cr1,cr2,cr3,cr4,cr5]);
        AddMultipleEntities([panelMember1, panelMember2, panelMember3, panelMember4, panelMember5]);
        AddMultipleEntities([pgm1, pgm2,pgm3]);
        AddMultipleEntities([criteria1, criteria2, criteria3]);
        //AddMultipleEntities([criteriaGroup1, criteriaGroup2 ]);
        //AddMultipleEntities([value1, value2, value3, value4, value5, value6, value7 ]);
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private static void AddMultipleEntities<T>(List<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            _context.Set<T>().Add(entity); // Using DbSet<T>.Add from the DbContext
        }
    }
}