using Domain;
using Domain.Admin;
using Domain.CitizenPanel;
using Domain.Interfaces;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Question;
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
                new MeetingPost
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
                    Question = "Wat is uw voorkeursvervoersmethode?",
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

        var participationMethod = new ParticipationMethod
        {
            Name = "Burgerpanel",
            Description =
                "Op basis van je gegeven antwoorden lijkt een burgerpanel het meest geschikte instrument. "
        };
        var participationMethod2 = new ParticipationMethod
        {
            Name = "Enquête",
            Description =
                "Je werkt best niet met een burgerpanel maar met een online enquête die je breed uitstuurt. "
        };
        var participationMethod3 = new ParticipationMethod
        {
            Name = "Informatieve campagne",
            Description =
                "Je werkt best niet met een burgerpanel maar zet een informatieve campagne op."
        };

        AddMultipleEntities([participationMethod, participationMethod2, participationMethod3]);
        var answerOptionsQuestion1 = new List<AnswerOption>
        {
            new AnswerOption
            {
                AnswerOptionText = "Ja",
                Weight = 5,
                Impacts = new List<AnswerOptionImpact>
                {
                    new AnswerOptionImpact
                    {
                        ParticipationMethod = participationMethod,
                        ImpactWeight = 10
                    },
                    new AnswerOptionImpact
                    {
                        ParticipationMethod = participationMethod2,
                        ImpactWeight = 1
                    },
                    new AnswerOptionImpact()
                    {
                        ParticipationMethod = participationMethod3,
                        ImpactWeight = 5
                    }
                }
            },
            new AnswerOption
            {
                AnswerOptionText = "Nee",
                Weight = 0,
                Impacts = new List<AnswerOptionImpact>
                {
                    new AnswerOptionImpact
                    {
                        ParticipationMethod = participationMethod,
                        ImpactWeight = 1
                    },
                    new AnswerOptionImpact
                    {
                        ParticipationMethod = participationMethod2,
                        ImpactWeight = 5
                    },
                    new AnswerOptionImpact()
                    {
                        ParticipationMethod = participationMethod3,
                        ImpactWeight = 10
                    }
                }
            }
        };


        if (!context.Questions.Any())
        {
            if (!context.Questions.Any(q =>
                    q.QuestionText ==
                    "Beschik je als organisator nog over minstens 6 maanden voordat de input van de\nparticipatie klaar moet zijn voor de politieke besluitvorming?"))
            {
                var question1 = new Question
                {
                    QuestionText =
                        "Beschik je als organisator nog over minstens 6 maanden voordat de input van de\nparticipatie klaar moet zijn voor de politieke besluitvorming?",
                    AnswerOptions = answerOptionsQuestion1
                };
                context.Questions.Add(question1);
            }


            var answerOptionsQuestion2 = new List<AnswerOption>
            {
                new AnswerOption
                {
                    AnswerOptionText = "Ja",
                    Weight = 5,
                    Impacts = new List<AnswerOptionImpact>
                    {
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod,
                            ImpactWeight = 5
                        },
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod2,
                            ImpactWeight = 1
                        },
                        new AnswerOptionImpact()
                        {
                            ParticipationMethod = participationMethod3,
                            ImpactWeight = 2
                        }
                    }
                },
                new AnswerOption
                {
                    AnswerOptionText = "Nee",
                    Weight = 0,
                    Impacts = new List<AnswerOptionImpact>
                    {
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod,
                            ImpactWeight = 1
                        },
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod2,
                            ImpactWeight = 2
                        },
                        new AnswerOptionImpact()
                        {
                            ParticipationMethod = participationMethod3,
                            ImpactWeight = 5
                        }
                    }
                }
            };
            
            if (!context.Questions.Any(q =>
                    q.QuestionText ==
                    "Is de gemeente bereid om de realisatie van de voorstellen van het burgerpanel\nernstig te overwegen en minstens publiek te motiveren waarom dat niet is gebeurd?"))
            {
                var question2 = new Question
                {
                    QuestionText =
                        "Is de gemeente bereid om de realisatie van de voorstellen van het burgerpanel\nernstig te overwegen en minstens publiek te motiveren waarom dat niet is gebeurd?",
                    AnswerOptions = answerOptionsQuestion2
                };
                context.Questions.Add(question2);
            }

            var answerOptionsQuestion3 = new List<AnswerOption>
            {
                new AnswerOption
                {
                    AnswerOptionText = "We willen de mening horen van al wie vrijwillig wil deelnemen aan het debat. Iedereen moet kunnen deelnemen",
                    Weight = 5,
                    Impacts = new List<AnswerOptionImpact>
                    {
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod,
                            ImpactWeight = 10
                        },
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod2,
                            ImpactWeight = 1
                        },
                        new AnswerOptionImpact()
                        {
                            ParticipationMethod = participationMethod3,
                            ImpactWeight = 5
                        }
                    }
                },
                new AnswerOption
                {
                    AnswerOptionText = "We willen de mening horen van doelgroepen die vaak afwezig blijven bij participatie",
                    Weight = 0,
                    Impacts = new List<AnswerOptionImpact>
                    {
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod,
                            ImpactWeight = -5
                        },
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod2,
                            ImpactWeight = 10
                        },
                        new AnswerOptionImpact()
                        {
                            ParticipationMethod = participationMethod3,
                            ImpactWeight = 2
                        }
                    }
                },
                new AnswerOption
                {
                    AnswerOptionText =  "We willen de mening horen een representatief staal van participanten horen",
                    Weight = 0,
                    Impacts = new List<AnswerOptionImpact>
                    {
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod,
                            ImpactWeight = 5
                        },
                        new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod2,
                            ImpactWeight = 2
                        },
                        new AnswerOptionImpact()
                        {
                            ParticipationMethod = participationMethod3,
                            ImpactWeight = 2
                        }
                    }
                }
            };
            
            if (!context.Questions.Any(q =>
                    q.QuestionText == "Wat is de bedoeling van het participatieproces bij dit vraagstuk?"))
            {
                var question3 = new Question
                {
                    QuestionText = "Wat is de bedoeling van het participatieproces bij dit vraagstuk?",
                    AnswerOptions = answerOptionsQuestion3
                };
                context.Questions.Add(question3);
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