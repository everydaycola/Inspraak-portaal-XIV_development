using Domain;
using Domain.CitizenPanel;
using Domain.Interfaces;
using Domain.Interfaces.Posts;
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
        var organisation1 = new Organisation
        {
            Id = "antwerpen",
            Name = "Antwerpen",
            BackgroundColor = "#cf252b",
            BackgroundImage = ""
        };
        var organisation2 = new Organisation
        {
            Id = "lwc",
            Name = "Lokale Waterpolo Club",
            BackgroundColor = "#42daf5",
            BackgroundImage = ""
        };

        //REPRESENTATION GROUPS
        //PANELS
        var newPanel = new Panel
        {
            Name = "Verkeersveiligheid in en rond Antwerpen.",
            SampleRate = 0.005,
            Owner = context.Users.Single(user => user.Email == "user@antwerpen.be"),
            IsRegistrationOpen = true,
            OrganisationId = "antwerpen",
            RepresentationGroup = new RepresentationGroup
            {
                CitizenCount = 20000,
                ReservePercentage = 0.2,
                ResponseRate = 0.1
            },
            Posts = new List<Post>
            {
                //PROJECT PAGE POSTS
                /*new TextPost()
                {
                    Content = "Test post!",
                    CreatedAt = DateTime.UtcNow,
                },
                new DocumentPost()
                {
                    DocumentName = "/mydocument",
                    CreatedAt = DateTime.UtcNow,
                },
                new EmbeddedVideoPost()
                {
                    VideoUrl = "/myvideo",
                    CreatedAt = DateTime.UtcNow,
                },*/
                new MeetingPost()
                {
                    CreatedAt = DateTime.UtcNow,
                    Title = "Bijeenkomst #1 - Gesprekken over duidelijkheid verkeersregels.",
                    DocumentNames = new List<string> { "Testeken1", "testeken2" }
                }
            },
            Criteria = new List<Criteria>
            {
                new()
                {
                    Name = "Rijbewijs",
                    Question = "Beschikt u over een rijbewijs?",
                    IsDefault = false,
                    IsDistributionKnown = true,
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
                    IsDistributionKnown = true,
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
                            DistributionPercentage = 0.34,
                            Option = "Auto"
                        }
                    }
                },
                new()
                {
                    Name = "Geslacht",
                    Question = "Wat is uw geslacht?",
                    IsDefault = true,
                    IsDistributionKnown = true,
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
        newPanel.RepresentationGroup.Panel = newPanel;

        //PlanningGroupMembers
        var pgm1 = new PlanningGroupMember
        {
            Panel = newPanel,
            User = new ApplicationUser
            {
                Email = "pgm@antwerpen.be",
                NormalizedEmail = "PGM@ANTWERPEN.BE",
                UserName = "PGM",
                NormalizedUserName = "PGM"
            },
            Functie = "Boekhouder"
        };
        var pgm2 = new PlanningGroupMember
        {
            Panel = newPanel,
            User = new ApplicationUser
            {
                Email = "owner@antwerpen.be",
                NormalizedEmail = "OWNER@ANTWERPEN.BE",
                UserName = "Owner",
                NormalizedUserName = "Owner"
            },
            Functie = "CEO"
        };
        var pgm3 = new PlanningGroupMember
        {
            Panel = newPanel,
            User = new ApplicationUser
            {
                Email = "JanDeRijke@antwerpen.be",
                NormalizedEmail = "JanDeRijke@ANTWERPEN.BE",
                UserName = "Jan De Rijke",
                NormalizedUserName = "JANDERIJKE"
            },
            Functie = "Software Architect"
        };

        //PanelMembers
        var panelMembersMen = Enumerable.Range(1, 100).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new()
                {
                    Criteria = newPanel.Criteria.First(c => c.Name == "Geslacht"),
                    SelectedOption = "Man"
                }
            }
        }).ToList();

        var panelMembersWomen = Enumerable.Range(1, 100).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new()
                {
                    Criteria = newPanel.Criteria.First(c => c.Name == "Geslacht"),
                    SelectedOption = "Vrouw"
                }
            }
        }).ToList();

        if (!context.Questions.Any())
        {
            if (!context.Questions.Any(q => q.QuestionText == "Heeft uw organisatie behoefte aan brede burgerbetrokkenheid?"))
            {
                var question1 = new Question
                {
                    QuestionText = "Heeft uw organisatie behoefte aan brede burgerbetrokkenheid?",
                    AnswerOptions = new List<AnswerOption>
                    {
                        new AnswerOption { AnswerOptionText = "Ja", Weight = 5 },
                        new AnswerOption { AnswerOptionText = "Nee", Weight = 0 }
                    }
                };
                context.Questions.Add(question1);
            }

            if (!context.Questions.Any(q => q.QuestionText == "Bent u bereid om de aanbevelingen van burgers serieus te overwegen?"))
            {
                var question2 = new Question
                {
                    QuestionText = "Bent u bereid om de aanbevelingen van burgers serieus te overwegen?",
                    AnswerOptions = new List<AnswerOption>
                    {
                        new AnswerOption { AnswerOptionText = "Ja", Weight = 10 },
                        new AnswerOption { AnswerOptionText = "Nee", Weight = -5 },
                        new AnswerOption { AnswerOptionText = "Misschien", Weight = 0 }
                    }
                };
                context.Questions.Add(question2);
            }
        }

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