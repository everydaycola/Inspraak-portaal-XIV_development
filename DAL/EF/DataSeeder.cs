using Domain;
using Domain.Admin;
using Domain.CitizenPanel;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Question;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces.Posts.PostItems;

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

        var panelMemberHendrick = new ApplicationUser(organisationId: organisation1.Id);
        var panelMemberAnika = new ApplicationUser(organisationId: organisation1.Id);
        var panelMemberMartin = new ApplicationUser(organisationId: organisation1.Id);
        var panelMemberLais = new ApplicationUser(organisationId: organisation1.Id);


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
                new TextPost()
                {
                    Title = "Welkom bij ons panel: Samen voor Verkeersveiligheid!",
                    Content =
                        "Hartelijk welkom aan alle panelleden! We zijn verheugd jullie te mogen verwelkomen op dit platform, speciaal opgezet om samen te werken aan een veiliger verkeer in Antwerpen. Jullie mening en inzicht zijn van onschatbare waarde. Dit panel is dé plek om ideeën uit te wisselen, knelpunten te bespreken en concrete voorstellen te doen die direct kunnen bijdragen aan het verbeteren van de verkeersveiligheid voor iedereen. We kijken ernaar uit om jullie actieve deelname en waardevolle bijdragen te zien. Laten we samen bouwen aan een toekomst waarin iedereen zich veilig voelt op de weg! (Meer info volgt spoedig)",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromHours(3.2)) - TimeSpan.FromDays(7),
                    IsVisibleForPanelMembers = true,
                },
                new TextPost()
                {
                    Title = "Onze eerste meeting: " + (DateTime.UtcNow - TimeSpan.FromDays(5)).ToShortDateString() +
                            " " + DateTime.UtcNow.ToShortTimeString(),
                    Content =
                        "De eerste werksessie staat op de planning! Om dit panel vlug te kunnen beginnen plannen wij graag al meteen de eerste sessie in . Details rond deze sessie volgen nog. Tijdens deze eerste meeting zullen we kennismaken, de doelstellingen van dit panel verder toelichten en de agenda voor de komende weken bespreken. Wij hopen dat jullie hier in grote aantallen te mogen ontvangen! Tot snel.",
                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(5),
                    IsVisibleForPanelMembers = true,
                },
                new MeetingPost
                {
                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(5),
                    Title = "Bijeenkomst #1 - Gesprekken over duidelijkheid verkeersregels.",
                    IsVisibleForPanelMembers = true,
                },
                new SuggestionPost
                {
                    Title = "Suggesties na Bijeenkomst #1: Wat zou jij graag meer/beter zien in Antwerpen?",
                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(5) + TimeSpan.FromHours(1),
                    IsVisibleForPanelMembers = true,
                    Suggestions = new List<Suggestion>
                    {
                        new()
                        {
                            Title = "Meer alcoholcontroles op de Noorderlaan!",
                            CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(3),
                            OwnerEmail = "chantal.verbruggen@gmail.com",
                            Votes = new List<Vote>()
                            {
                                new(owner: panelMemberLais, voteType: VoteType.Up),
                                new(owner: panelMemberAnika, voteType: VoteType.Down),
                                new(owner: panelMemberHendrick, voteType: VoteType.Up),
                                new(owner: panelMemberMartin, voteType: VoteType.Down),
                            }
                        },
                        new()
                        {
                            Title = "Fietspaden op de paardenmarkt!",
                            CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(4),
                            OwnerEmail = "gert.lambrechts@gmail.com",
                            Votes = new List<Vote>
                            {
                                new(owner: panelMemberLais, voteType: VoteType.Up),
                                new(owner: panelMemberAnika, voteType: VoteType.Up),
                                new(owner: panelMemberHendrick, voteType: VoteType.Up),
                                new(owner: panelMemberMartin, voteType: VoteType.Down),
                            }
                        }
                    }
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
        var oscarv = new PlanningGroupMember
        {
            Panel = newPanel,
            User = new ApplicationUser
            {
                Email = "oscar.vermeulen@antwerpen.be",
                NormalizedEmail = "OSCAR.VERMEULEN@ANTWERPEN.BE",
                UserName = "Oscar Vermeulen",
                NormalizedUserName = "OSCARV"
            },
            Functie = "Boekhouder"
        };
        var marcom = new PlanningGroupMember
        {
            Panel = newPanel,
            User = new ApplicationUser
            {
                Email = "marco.machtels@antwerpen.be",
                NormalizedEmail = "MARCO.MACHTELS@ANTWERPEN.BE",
                UserName = "Marco Machtels",
                NormalizedUserName = "MARCOM"
            },
            Functie = "CEO"
        };
        var tomr = new PlanningGroupMember
        {
            Panel = newPanel,
            User = new ApplicationUser
            {
                Email = "tom.riddle@antwerpen.be",
                NormalizedEmail = "TomRiddle@ANTWERPEN.BE",
                UserName = "Tom Riddle",
                NormalizedUserName = "TOMRIDDLE"
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

        var rijbewijsCriteria = newPanel.Criteria.FirstOrDefault(c => c.Name == "Rijbewijs");
        var vervoermethodeCriteria = newPanel.Criteria.FirstOrDefault(c => c.Name == "Vervoermethode");
        var geslachtCriteria = newPanel.Criteria.FirstOrDefault(c => c.Name == "Geslacht");

        List<PanelMember> panelMembersToSeed = new List<PanelMember>();
        int memberCount = 1; // Counter for unique emails

        panelMembersToSeed.AddRange(Enumerable.Range(1, 10).Select(_ => new PanelMember
        {
            Panel = newPanel, // Use the reference to the previously saved panel
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Ja" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Fiets" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Man" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));

        panelMembersToSeed.AddRange(Enumerable.Range(1, 10).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Nee" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Fiets" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Man" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));

        panelMembersToSeed.AddRange(Enumerable.Range(1, 8).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Ja" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Te voet" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Vrouw" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));

        panelMembersToSeed.AddRange(Enumerable.Range(1, 12).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Ja" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Auto" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Man" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));

        panelMembersToSeed.AddRange(Enumerable.Range(1, 11).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Nee" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Te voet" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Vrouw" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        panelMembersToSeed.AddRange(Enumerable.Range(1, 5).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Nee" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Te voet" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Man" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        panelMembersToSeed.AddRange(Enumerable.Range(1, 2).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Nee" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Auto" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Vrouw" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        panelMembersToSeed.AddRange(Enumerable.Range(1, 8).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Ja" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Fiets" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Vrouw" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        panelMembersToSeed.AddRange(Enumerable.Range(1, 15).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Ja" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Auto" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Vrouw" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        panelMembersToSeed.AddRange(Enumerable.Range(1, 5).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Ja" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Te voet" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Man" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        panelMembersToSeed.AddRange(Enumerable.Range(1, 2).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Nee" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Auto" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Man" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        panelMembersToSeed.AddRange(Enumerable.Range(1, 7).Select(_ => new PanelMember
        {
            Panel = newPanel,
            Responses = new List<CriteriaResponse>
            {
                new() { Criteria = rijbewijsCriteria, SelectedOption = "Nee" },
                new() { Criteria = vervoermethodeCriteria, SelectedOption = "Fiets" },
                new() { Criteria = geslachtCriteria, SelectedOption = "Vrouw" },
            },
            HasRegistered = true,
            Email = $"member{memberCount++}@example.com",
            User = AddOrUpdateUser(context, $"member{memberCount - 1}@example.com"),
            Selected = false
        }));
        
        
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
        

        AddMultipleEntities(panelMembersMen);
        AddMultipleEntities(panelMembersWomen);
        AddMultipleEntities(panelMembersToSeed);
        AddMultipleEntities([oscarv, marcom, tomr]);
        AddMultipleEntities([organisation1, organisation2]);
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }

    private static ApplicationUser AddOrUpdateUser(CitizenPanelDbContext context, string email)
    {
        var user = context.Users.SingleOrDefault(u => u.Email == email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                UserName = email.Split('@')[0], // Use part of email as username
                NormalizedUserName = email.Split('@')[0].ToUpperInvariant()
                // You might need to add a default password hash here if Identity requires it on creation
                // e.g., PasswordHash = "some_hashed_password"
            };
            context.Users.Add(user);
            // Don't call SaveChanges here, let the main SaveChanges handle it for performance
        }

        return user;
    }

    private static void AddMultipleEntities<T>(List<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            _context.Set<T>().Add(entity); // Using DbSet<T>.Add from the DbContext
        }
    }
}