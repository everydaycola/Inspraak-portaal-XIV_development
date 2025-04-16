using System.ComponentModel.DataAnnotations;
using BL.Interfaces;
using DAL.Interfaces;
using Domain.CitizenPanel;
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

    //ADD
    public Panel AddPanel(string name, int size, double sampleRate,
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
        size = (int)(citizenCount * sampleRate);

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
        var membersCount = _calculationManager.CalculateTotalInvitesNeeded(_calculationManager.CalculateAmountOfReserve(size, reservePercentage), responseRate);
        // todo use this count 
        
        // Generate members
        var panelMembers = Enumerable
            .Range(0, size)
            .Select(_ => new PanelMember { Panel = panel })
            .ToList();

        // validate panelmembers
        objectsToValidate.AddRange(panelMembers);

        // Fill in the criteria list with the given distribution
        panel.Criteria = distribution;
        
        /*
        // Create the criteria list from the dictionary, initializing all Criteria and CriteriaAnswerOptions
        panel.Criteria = distribution.Select(outerKvp => new Criteria
        {
            Name = outerKvp.Key,
            IsDefault = true,
            AnswerOptions = outerKvp.Value.Select(innerKvp => new CriteriaAnswerOption
            {
                Option = innerKvp.Key,
                DistributionPercentage = innerKvp.Value
            }).ToList() // Create the List<CriteriaAnswerOption> for the property
        }).ToList(); // Create the final List<Criteria>
        */

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

    /// <summary>
    /// Generates all possible combinations of criteria responses and calculates their percentage
    /// based on the distribution percentages of each answer option in the criteria list, multiplied.
    /// It performs a recursive process similar to a Cartesian product, combining response options
    /// across multiple criteria and computes the combined distributions.
    /// </summary>
    public Dictionary<ICollection<CriteriaResponse>, double> HelperCalculateCrossDistribution(List<Criteria> criteriaList)
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

    public void RemoveUnselectedPanelMembers(Guid panelId)
    {
        _repo.RemoveAllUnselectedPanelmembers(panelId);
    }

    public PanelMember UpdatePanelMember(PanelMember member)
    {
        _repo.UpdatePanelMember(member);
        return member;
    }

    public void ChangePanelMembersToSelected(ICollection<PanelMember> selectedMembers)
    {
        _repo.UpdatePanelMembersToSelected(selectedMembers);
    }

    public void NewPanelPhase(Guid guid, double newResponseRate)
    {
        var panelMembers = _repo.ReadPanelMembersWithResponses(guid);
        var panel = _criteriaRepo.ReadAllDesiredCriteriaPercentages(guid);
        throw new NotImplementedException();
    }

    public void EndRegistration(Guid id)
    {
        _repo.UpdatePanel(GetPanel(id));
        _repo.RemoveAllUnselectedPanelmembers(id);
    }

    public IEnumerable<PlanningGroupMember> GetAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId)
    {
        return _repo.ReadAllPlanningGroupMembersWithIdentityUserForPanel(panelId);
    }
}