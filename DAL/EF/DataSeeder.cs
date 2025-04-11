using Domain;
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
        
        //ORGANISATIONS
        var organisation1 = new Organisation()
        {
            Id = "antwerpen",
            Name = "Antwerpen",
            BackgroundColor = "#cf252b",
            BackgroundImage = ""
        };
        var organisation2 = new Organisation()
        {
            Id = "lwc",
            Name = "Lokale Waterpolo Club",
            BackgroundColor = "#42daf5",
            BackgroundImage = ""
        };
        
        //REPRESENTATION GROUPS
        //PANELS
        var panel1 = new Panel
        {
            Name="Verkeersveiligheid in en rond Antwerpen.", 
            SampleRate = 0.005,
            Owner = context.Users.Single(user => user.Email == "user@antwerpen.be"),
            IsRegistrationOpen = true,
            RepresentationGroup = new RepresentationGroup
            {
                CitizenCount = 20000,
                ReservePercentage = 0.2, 
                ResponseRate = 0.1
            
            },
            Criteria = new List<Criteria>
            {
                new()
                {
                Name = "Rijbewijs",
                Question = "Beschikt u over een rijbewijs?",
                IsDefault = false,
                AnswerOptions = new List<CriteriaAnswerOption>
                {
                    new()
                    {
                        DistributionPercentage = 0.5,
                        Option = "Ja"
                    },
                    new()
                    {
                        DistributionPercentage = 0.5,
                        Option = "Nee"
                    }
                }
            },
            new()
            {
            Name = "Vervoermethode",
            Question = "Wat is uw voorkeurs vervoersmethode?",
            IsDefault = false,
            AnswerOptions = new List<CriteriaAnswerOption>
            {
                new()
                {
                    DistributionPercentage = 0.33,
                    Option = "Te voet"
                },
                new()
                {
                    DistributionPercentage = 0.33,
                    Option = "Fiets"
                },
                new()
                {
                    DistributionPercentage = 0.33,
                    Option = "Auto"
                }
            }
        },
        new()
        {
            Name = "Geslacht",
            Question = "Wat is uw geslacht?",
            IsDefault = true,
            AnswerOptions = new List<CriteriaAnswerOption>
            {
                new()
                {
                    DistributionPercentage = 0.5,
                    Option = "Man"
                },
                new()
                {
                    DistributionPercentage = 0.5,
                    Option = "Vrouw"
                }
            }
        }
            }
        };
        
        // link rpg both ways
        panel1.RepresentationGroup.Panel = panel1;
        
        //PlanningGroupMembers
        var pgm1 = new PlanningGroupMember
        {
            Panel = panel1,
            User = new IdentityUser
            {
                Email = "pgm@antwerpen.be",
                NormalizedEmail = "PGM@ANTWERPEN.BE",
                UserName = "PGM",
                NormalizedUserName = "PGM"
            }
        };
        var pgm2 = new PlanningGroupMember
        {
            Panel = panel1,
            User = new IdentityUser
            {
                Email = "owner@antwerpen.be",
                NormalizedEmail = "OWNER@ANTWERPEN.BE",
                UserName = "Owner",
                NormalizedUserName = "Owner"
            }
        };
        var pgm3 = new PlanningGroupMember
        {
            Panel = panel1,
            User = new IdentityUser
            {
                Email = "JanDeRijke@antwerpen.be",
                NormalizedEmail = "JanDeRijke@ANTWERPEN.BE",
                UserName = "Jan De Rijke",
                NormalizedUserName = "JANDERIJKE"
            }
        };
        
        //PanelMembers
        var panelMembersMen = Enumerable.Range(1, 50).Select(_ => new PanelMember
        {
            Panel = panel1,
            Responses = new List<CriteriaResponse>
            {
                new()
                {
                    Criteria = panel1.Criteria.First(c => c.Name == "Geslacht"),
                    SelectedOption = "Man"
                }
            }
        }).ToList();
        
        var panelMembersWomen = Enumerable.Range(1, 50).Select(_ => new PanelMember
        {
            Panel = panel1,
            Responses = new List<CriteriaResponse>
            {
                new()
                {
                    Criteria = panel1.Criteria.First(c => c.Name == "Geslacht"),
                    SelectedOption = "Vrouw"
                }
            }
        }).ToList();
        
        // adding panel members also adds dependant objects
        // so panel member => panel
        //    panel => representation group
        //    panel => criteria
        //    criteria => criteria answer option
        //    plannings group member => identityUser
        AddMultipleEntities(panelMembersMen);
        AddMultipleEntities(panelMembersWomen);
        AddMultipleEntities([pgm1, pgm2, pgm3]);
        AddMultipleEntities([organisation1, organisation2]);
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