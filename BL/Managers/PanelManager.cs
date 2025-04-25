using System.ComponentModel.DataAnnotations;
using BL.Interfaces;
using DAL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using UI_MVC;

namespace BL.Managers;

public class PanelManager : IPanelManager
{
    private readonly ILogger<PanelManager> _logger;
    private readonly IPanelRepository _repo;
    private readonly ICalculationManager _calculationManager;
    private readonly IUserRepository _userRepo;
    private readonly ICriteriaRepository _criteriaRepo;

    public PanelManager(ILogger<PanelManager> logger, IPanelRepository repo, ICalculationManager calcManager, IUserRepository userRepo, ICriteriaRepository criteriaRepo)
    {
        _logger = logger;
        _repo = repo;
        _calculationManager = calcManager;
        _userRepo = userRepo;
        _criteriaRepo = criteriaRepo;
    }

    //GET
    public Panel GetPanel(Guid id)
    {
        return _repo.ReadPanel(id);
    }

    public IEnumerable<Panel> GetAllPanels()
    {
        return _repo.ReadAllPanels();
    }

    public Panel GetPanelWithRepresentationGroup(Guid id)
    {
        return _repo.ReadPanelWithRepresentationGroup(id);
    }

    // public IEnumerable<PanelMember> GetPanelWithPanelMembersAndCriteria(Guid id)
    // {
    //     return _repo.ReadPanelMembersWithCriteria(id);
    // }
    
    // public Panel GetPanelWithCriteriaAndOptions(Guid id)
    // {
    //     return _repo.ReadPanelWithCriteriaAndAnsweroptions(id);
    //
    // }
    
    // public PanelMember GetPanelMemberById(Guid memberId)
    // {
    //     return _repo.ReadPanelMember(memberId);
    // }

    public PanelMember GetPanelMemberWithCriteriaResponses(Guid id)
    {
        return _repo.ReadPanelMemberWithCriteriaResponses(id);
    }

    public PanelMember GetPanelMemberWithPanel(Guid id)
    {
        return _repo.ReadPanelMemberWithPanel(id);
    }

    public IEnumerable<PanelMember> GetAllPanelMembersForPanel(Guid panelId)
    {
        return _repo.ReadPanelMembersWithCriteria(panelId);
    }

    // public IEnumerable<PanelMember> GetAllPanelMembersWhichAnsweredAllQuestionsWithCriteria(Guid id)
    // {
    //     return _repo.ReadPanelMembersWhichAnsweredAllQuestionsWithCriteria(id);
    // }

    public Panel GetPanelWithCriteriaAndCriteriaAnswerOptions(Guid panelId)
    {
        return _repo.ReadPanelWithCriteriaAndAnswerOptions(panelId);
    }

    public Panel GetPanelWithPosts(Guid panelId)
    {
        return _repo.ReadPanelWithPosts(panelId);
    }

    //ADD
    public Panel AddPanel(string name, double sampleRate,
        ICollection<Criteria> distribution, int citizenCount, double reservePercentage,
        double responseRate, string userId)
    {
        _logger.Log(LogLevel.Information, "Creating panel with name " + name + "...");
        
        var user = _userRepo.ReadUser(userId);
        if (user == null)
        {
            var errorMessage = "user with id " + userId + " is not a valid user!";
            _logger.Log(LogLevel.Critical, errorMessage);
            throw new UnauthorizedAccessException(errorMessage);
        }

        var userRole = _userRepo.ReadUserRole(userId);
        if (userRole == null || (userRole.Name != CustomIdentityConstants.AdminRole &&
                                 userRole.Name != CustomIdentityConstants.OrganisatieRole))
        {
            var errorMessage = "User with id " + userId + " is not in a valid role to create a panel!";
            _logger.Log(LogLevel.Critical, errorMessage);
            throw new UnauthorizedAccessException(errorMessage);
        }
        

        // list of objects to validate
        var objectsToValidate = new List<object>();
        
        // Calculate size of the Panel
        var size = (int)(citizenCount * sampleRate);

        // Create and initialize the panel
        var panel = new Panel
        {
            Name = name,
            SampleRate = sampleRate,
            RepresentationGroup = new RepresentationGroup
            {
                CitizenCount = citizenCount,
                ReservePercentage = reservePercentage,
                ResponseRate = responseRate
            },
            Owner = user
        };

        // validate panel and representationgroup
        objectsToValidate.Add(panel);
        objectsToValidate.Add(panel.RepresentationGroup);
        
        // calculate panel size
        var membersCount = _calculationManager.CalculateTotalInvitesNeeded(
            _calculationManager.CalculateAmountOfReserve(size, reservePercentage) + size, responseRate);

        // Generate members
        var panelMembers = Enumerable
            .Range(0, membersCount)
            .Select(_ => new PanelMember { Panel = panel })
            .ToList();

        // validate panelmembers
        objectsToValidate.AddRange(panelMembers);
        
        if (distribution.Count > 0)
        {
            // Fill in the criteria list with the given distribution
            panel.Criteria = distribution;
            
            // validate criteria & answer options
            objectsToValidate.AddRange(panel.Criteria);
            objectsToValidate.AddRange(panel.Criteria.SelectMany(c => c.AnswerOptions));
            
            // preforms an action very similar to a cartesian product, but with the options of each criteria
            var crossDistribution = HelperCalculateCrossDistribution(panel.Criteria.ToList());
            
            // validate created criteria
            objectsToValidate.AddRange(crossDistribution.Keys
                .SelectMany(r => r)
                .GroupBy(r => r)); // to remove duplicates

            // finally, give panel members their distributions
            HandOutAnsweredCriteriaToPanelMembers(panelMembers, crossDistribution);
        }
        
        // // validation
        var validationResults = new List<ValidationResult>();
        // validate panel members criteria responses
        var validationSuccess = objectsToValidate.All(r =>
            Validator.TryValidateObject(r, new ValidationContext(r), validationResults, true));
        // if validation failed
        if (!validationSuccess)
        {
            // collect error messages and send as exception
            throw new ValidationException(string.Join("\n", validationResults.Select(x => x.ErrorMessage)));
        }
        
        // if validation succeeded
        // adding panel members to repo also has dependencies to everything else so everything gets added
        panelMembers.ForEach(member => _repo.CreatePanelMember(member));
        
        _logger.Log(LogLevel.Information, "Panel with name " + panel.Name + " was created.");

        return panel;
    }

    public void AddTextPost(Guid panelId,string title,string content)
    {
        var textPost = new TextPost
        {
            Title =title,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };
        _repo.CreateTextPost(panelId, textPost);
    }

    public void AddPlanningsGroupMember(Guid panelId, string Email, string Naam, string Functie)
    {
        Panel panel = GetPanel(panelId);
        PlanningGroupMember member = new PlanningGroupMember
        {
            Panel = panel,
            User = new ApplicationUser()
            {
                Email = Email,
                NormalizedEmail = Email.ToUpper(),
                UserName = Naam,
                NormalizedUserName = Naam.ToUpper()
            },
            Functie = Functie
        };
        _repo.CreatePlanningsGroupMember(member);
    }

    /// <summary>
    /// Generates all possible combinations of criteria responses and calculates their percentage
    /// based on the distribution percentages of each answer option in the criteria list, multiplied.
    /// It performs a recursive process similar to a Cartesian product, combining response options
    /// across multiple criteria and computes the combined distributions.
    /// </summary>
    private Dictionary<ICollection<CriteriaResponse>, double> HelperCalculateCrossDistribution(List<Criteria> criteriaList)
    {
        // Handle edge case: If the criteriaList is null or empty, return an empty dictionary
        if (criteriaList == null || criteriaList.Count == 0)
        {
            _logger.Log(LogLevel.Warning, "CrossDistribution was called with an empty criteria list.");
            return new Dictionary<ICollection<CriteriaResponse>, double>();
        }

        // Dictionary to store combinations of CriteriaResponse and their calculated probabilities
        var combinations = new Dictionary<ICollection<CriteriaResponse>, double>();

        // Iterate through each answer option in the first Criteria
        foreach (var option in criteriaList[0].AnswerOptions)
        {
            // Create a new combination with one CriteriaResponse for the current answer option
            var currentResponse = new CriteriaResponse
            {
                Criteria = criteriaList[0],
                SelectedOption = option.Option
            };

            // If there is only one Criteria left, add the combination and its distribution percentage
            if (criteriaList.Count == 1)
            {
                combinations.Add([currentResponse], option.DistributionPercentage);
                continue;
            }

            // Recursively generate combinations for the remaining Criteria
            var subCombinations = HelperCalculateCrossDistribution(criteriaList.Skip(1).ToList());

            // Combine the current CriteriaResponse with each sub-combination
            foreach (var subCombo in subCombinations)
            {
                // Create a new combination by merging the current response with the sub-combination
                var newCombo = new List<CriteriaResponse> { currentResponse };
                newCombo.AddRange(subCombo.Key);

                // Calculate the combined probability by multiplying the distribution percentages 
                // Add the combined result to the dictionary
                combinations.Add(newCombo, option.DistributionPercentage * subCombo.Value);
            }
        }

        // Return the dictionary of all generated combinations and their probabilities
        return combinations;
    }

    private void HandOutAnsweredCriteriaToPanelMembers(ICollection<PanelMember> panelMembers,
        Dictionary<ICollection<CriteriaResponse>, double> crossDistribution)
    {
        // Convert percentages to actual member counts
        var totalCount = panelMembers.Count;
        var allCounts = new Dictionary<ICollection<CriteriaResponse>, int>();

        // Calculate how many members should get each collection of criteria responses
        var doneCount = 0;
        foreach (var distribution in crossDistribution)
        {
            // Calculate number of members for this distribution group
            var doingCount = (int)Math.Round(distribution.Value * totalCount);

            // Ensure we don't exceed total member count due to rounding
            // I can image small edge cases where this goes wrong but eh its good enough
            if (doneCount + doingCount > totalCount)
            {
                // if so assume this is the last criteria and just make the amount the remaining members
                doingCount = totalCount - doneCount;
                _logger.Log(LogLevel.Warning, "Calculated more criteria responses than members. Assuming last criteria");
            }

            // add distribution count and update 
            allCounts.Add(distribution.Key, doingCount);
            doneCount += doingCount;
        }

        // If we didn't assign enough, assign some more
        if (doneCount < totalCount)
        {
            _logger.Log(LogLevel.Warning, "Not enough criteria responses calculated. Adding to most common criteria");
            // add whatever needed to the first one. should not be that much.
            allCounts[
                allCounts
                    .Keys
                    .OrderByDescending(k => crossDistribution[k])
                    .First()
            ] += totalCount - doneCount;
        }

        // Iterate through members and assign criteria responses until no more
        using var memberIterator = panelMembers.GetEnumerator();
        foreach (var count in allCounts)
        {
            // Assign criteria to members by calculated counts
            for (var i = 0; i < count.Value; i++)
            {
                // Check if we have more members to process
                if (!memberIterator.MoveNext())
                {
                    // if so, whatever
                    _logger.Log(LogLevel.Warning, "Not enough members to assign criteria responses. skipping");
                    break;
                }

                // rider is dumb, can never be null because of check above
                // create a copy so EF recognises it as separate objects
                memberIterator.Current.Responses = count.Key
                    .Select(cr => new CriteriaResponse
                    {
                        Criteria = cr.Criteria,
                        SelectedOption = cr.SelectedOption
                    }).ToList();
            }
        }
    }
    //UPDATE
    public void UpdatePanel(Guid id, bool isRegistrationOpen)
    {
        var panel = _repo.ReadPanel(id);
        if (panel == null) throw new NullReferenceException("Panel with id: " + id + " was not found.");
        panel.IsRegistrationOpen = isRegistrationOpen;
        _repo.UpdatePanel(panel);
    }

    public void UpdatePanelRegistrationCount(Guid id, bool increase)
    {
        var panel = _repo.ReadPanel(id);
        if (panel == null) throw new NullReferenceException("Panel with id: " + id + " was not found");
        panel.SuccessfulRegistrationCount += increase ? 1 : -1;
        _repo.UpdatePanel(panel);
    }

    public PanelMember UpdatePanelMember(PanelMember member)
    {
        _repo.UpdatePanelMember(member);
        return member;
    }

    public void DeletePlanningsGroupmember(Guid planningsGroupMemberId)
    {
        var planningGroupmember = _repo.ReadPlanningGroupMember(planningsGroupMemberId);
        if (planningGroupmember == null)
            throw new NullReferenceException("Planninggroupmember with id " + planningsGroupMemberId +
                                             " was not found");
        _repo.RemovePlanningGroupMember(planningsGroupMemberId);
    }

    public void NewPanelPhase(Guid panelId, double newResponseRate)
    {
        // get panel members with panel, all crit responses, criteria and answer options
        // this repo call should not be pulling in this much.
        var panelMembers = GetAllPanelMembersForPanel(panelId).ToList();
        // get the panel with representation group
        var panel = GetPanelWithRepresentationGroup(panelId);
        // get the amount of invites sent originally
        var panelSize = _calculationManager.CalculatePanelSize(panel.RepresentationGroup.CitizenCount, panel.SampleRate);
        // get the amount of people that you want to be registered, including reserve
        var amountNeeded = _calculationManager.CalculateAmountOfReserve(panelSize, panel.RepresentationGroup.ReservePercentage) + panelSize;
        // get all default criteria with answer option and thus distribution percentages for the panel
        var criteriaList = _criteriaRepo.ReadAllCriteriaForPanelWithAnswerOptions(panelId, onlyDefault: true).ToList();
        
        // Create a dictionary with string keys of criteria groups, and value the ammount of
        // like key:"Man|30-39", value:60
        // !! criteriaList only has default criteria in it
        var amountOfRegistrationsDesired = HelperCalculateCrossDistribution(criteriaList)
            .ToDictionary(
                item => string.Join("|", item.Key
                    .OrderBy(r => r.Criteria.Name)
                    .Select(r => r.SelectedOption)),
                item => (int)(item.Value * amountNeeded)
            );
        
        // Gets the actual counts of registered people per default criteria group (same key as dict above)
        var amountOfRegistrationsActual = panelMembers
            .Where(pm => pm.HasRegistered)
            .GroupBy(pm =>
                string.Join("|", pm.Responses
                    .Where(r => r.Criteria.IsDefault)
                    .OrderBy(r => r.Criteria.Name)
                    .Select(r => r.SelectedOption))
            )
            .OrderBy(g => g.Key)
            .ToDictionary(
                kpv => kpv.Key,
                kpv => kpv.Count()
            );
        
        // list of all new made panelMembers
        var newPanelMembers = new List<PanelMember>();

        // Go through each criteria group
        // (amountOfRegistrationsActual and amountOfRegistrationsDesired should have the exact same keys)
        foreach (var key in amountOfRegistrationsActual.Keys.Union(amountOfRegistrationsDesired.Keys))
        {
            // Check if key exists in the dictionaries
            if (!amountOfRegistrationsDesired.TryGetValue(key, out var desired))
            {
                _logger.Log(LogLevel.Critical, "key: " + key + " does not exist in \"desired\" dictionary");
                continue; // skip
            }

            if (!amountOfRegistrationsActual.TryGetValue(key, out var actual))
            {
                _logger.Log(LogLevel.Critical, "key: " + key + " does not exist in \"actual\" dictionary");
                continue; // skip
            }

            // desired - actual = needed
            var needed = _calculationManager.CalculateTotalInvitesNeeded(desired - actual, newResponseRate);
            if (needed <= 0) continue; // we have enough
            newPanelMembers.AddRange(Enumerable.Range(1, needed).Select(_ => new PanelMember
            {
                Panel = panel,
                Responses = criteriaList
                    .SelectMany(criteria => criteria.AnswerOptions
                            .Where(option => key.Contains(option.Option))
                            .Select(option => new CriteriaResponse
                            {
                                Criteria = criteria,
                                SelectedOption = option.Option
                            }
                        )
                    ).ToList(),
                Phase = panel.LastPhase + 1 
            }));
        }

        // // validation
        var validationResults = new List<ValidationResult>();
        // validate panel members criteria responses
        var validationSuccess = newPanelMembers.All(r =>
            Validator.TryValidateObject(r, new ValidationContext(r), validationResults, true));
        // if validation failed
        if (!validationSuccess)
        {
            // collect error messages and send as exception
            _logger.Log(LogLevel.Critical,
                "Validation failed where it shouldn't: \n" +
                string.Join("\n", validationResults.Select(x => x.ErrorMessage)));
            throw new ValidationException(string.Join("\n", validationResults.Select(x => x.ErrorMessage)));
        }

        panel.LastPhase++;
        _repo.UpdatePanel(panel);
        _repo.CreatePanelMembers(newPanelMembers);
    }

    public void EndRegistration(Guid panelId,
        Dictionary<string, Dictionary<string, double>> allDesiredCriteriaPercentages)
    {
        var panel = GetPanelWithRepresentationGroup(panelId);
        var panelSize =
            _calculationManager.CalculatePanelSize(panel.RepresentationGroup.CitizenCount, panel.SampleRate);
        var amountSelectedNeeded =
            _calculationManager.CalculateAmountOfReserve(panelSize, panel.RepresentationGroup.ReservePercentage) + panelSize;
        var criteriaList = allDesiredCriteriaPercentages.Select(kpv => new Criteria
        {
            Name = kpv.Key,
            AnswerOptions = kpv.Value.Select(kpv2 => new CriteriaAnswerOption
            {
                Option = kpv2.Key,
                DistributionPercentage = kpv2.Value
            }).ToList()
        }).ToList();

        var crossDistribution = HelperCalculateCrossDistribution(criteriaList);

        // Create a dictionary with string keys
        var optionList = new Dictionary<string, int>();
        foreach (var item in crossDistribution)
        {
            var key = string.Join("|", item.Key.OrderBy(r => r.SelectedOption).Select(r => r.SelectedOption));
            optionList[key] = (int)(item.Value * amountSelectedNeeded);
        }

        var registeredMembers = GetAllPanelMembersForPanel(panelId)
            .Where(p => p.HasRegistered)
            .ToList();

        var selectedMembers = new List<PanelMember>();

        // Group by responses and process each group
        foreach (var group in registeredMembers.GroupBy(pm => string.Join("|", pm.Responses.OrderBy(r => r.SelectedOption).Select(r => r.SelectedOption))))
        {
            // Check if key exists in the dictionary
            if (optionList.TryGetValue(group.Key, out var count))
            {
                // Add shuffled selection to selected members
                selectedMembers.AddRange(
                    group.OrderBy(_ => Guid.NewGuid())
                        .Take(count)
                );
            }
            // If key doesn't exist, we can skip or handle as needed
        }

        _repo.UpdatePanelMembersToSelected(selectedMembers);
        _repo.RemoveAllUnselectedPanelmembers(panelId);
        panel.SuccessfulRegistrationCount = selectedMembers.Count;
        _repo.UpdatePanel(panel);
    }

    public IEnumerable<PlanningGroupMember> GetAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId)
    {
        return _repo.ReadAllPlanningGroupMembersWithIdentityUserForPanel(panelId);
    }

    public void AddDocumentPost(Guid panelId,string title, string documentUrl)
    {
        var docPost = new DocumentPost
        {
            Title = title,
            DocumentName = documentUrl,
            CreatedAt = DateTime.UtcNow
        };
        _repo.CreateDocumentPost(panelId, docPost);
    }
}