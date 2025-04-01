using BL.Interfaces;
using DAL.Interfaces;
using Domain.CitizenPanel;

namespace BL.Managers;

public class PanelManager : IPanelManager
{
    private readonly IPanelRepository _repo;

    public PanelManager(IPanelRepository repo)
    {
        _repo = repo;
    }

    public Panel GetPanel(Guid id)
    {
        return _repo.ReadPanel(id);
    }

    public Panel GetPanelWithRepresentationGroup(Guid id)
    {
        return _repo.ReadPanelWithRepresentationGroup(id);
    }
    
    public ICollection<PanelMember> GetPanelMembersAndRepresentationGroup(Guid id)
    {
        return _repo.ReadPanelMembersAndRepresentationGroup(id);
    }

    public IEnumerable<PanelMember> GetPanelWithPanelMembersAndCriteria(Guid id)
    {
        return _repo.ReadPanelMembersWithCriteria(id);
    }

    public PanelMember GetPanelByUserId(Guid memberId)
    {
        return _repo.ReadPanelByUserId(memberId);
    }

    public IEnumerable<Panel> GetAllPanels()
    {
        return _repo.ReadAllPanels();
    }

    public PanelMember GetPanelMemberById(Guid memberId)
    {
        return _repo.ReadPanelMember(memberId);
    }

    public PanelMember GetPanelMemberWithCriteriaResponses(Guid id)
    {
        return _repo.ReadPanelMemberWithCriteriaResponses(id);
    }

    public PanelMember GetPanelMemberWithPanel(Guid id)
    {
        return _repo.ReadPanelMemberWithPanel(id);
    }

    public Panel AddPanel(string name, int size, double sampleRate,
        Dictionary<string, Dictionary<string, double>> distribution, int citizenCount, double reservePercentage,
        double responseRate)
    {
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
            }
        };

        // Generate members
        var PanelMembers = Enumerable
            .Range(0, size)
            .Select(_ => new PanelMember { Panel = panel })
            .ToList();

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

        // preforms an action very similar to a cartesian product, but with the options of each criteria
        var crossDistribution = CrossDistribution(panel.Criteria.ToList());
        // finally, give panel members their distributions
        HandOutAnsweredCriteriaToPanelMembers(PanelMembers, crossDistribution);
        
        // adding panel members to repo also has dependencies to everything else so everything gets added
        PanelMembers.ForEach(member => _repo.CreatePanelMember(member));

        return panel;
    }

    /// <summary>
    /// Generates all possible combinations of criteria responses and calculates their percentage
    /// based on the distribution percentages of each answer option in the criteria list, multiplied.
    /// It performs a recursive process similar to a Cartesian product, combining response options
    /// across multiple criteria and computes the combined distributions.
    /// </summary>
    private Dictionary<ICollection<CriteriaResponse>, double> CrossDistribution(List<Criteria> criteriaList)
    {
        // Handle edge case: If the criteriaList is null or empty, return an empty dictionary
        if (criteriaList == null || criteriaList.Count == 0)
        {
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
            var subCombinations = CrossDistribution(criteriaList.Skip(1).ToList());

            // Combine the current CriteriaResponse with each sub-combination
            foreach (var subCombo in subCombinations)
            {
                // Create a new combination by merging the current response with the sub-combination
                var newCombo = new List<CriteriaResponse> {currentResponse};
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
            }

            // add distribution count and update 
            allCounts.Add(distribution.Key, doingCount);
            doneCount += doingCount;
        }

        // If we didn't assign enough, assign some more
        if (doneCount < totalCount)
        {
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
                    // if so, whatever
                    break;
                // rider is dumb, can never be null because of check above
                memberIterator.Current.Responses = count.Key;
            }
        }
    }

    public int CalculatePanelSize(int citizenCount, double samplePercentage)
    {
        //CitizenCount = amount of citizens in gemeente.
        return (int)(citizenCount * samplePercentage);
    }

    public int CalculateAmountOfReserve(int panelSize, double samplePercentage)
    {
        //panelSize = calculatedByCalculatePanelSize
        return (int)(panelSize * samplePercentage);
    }

    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate)
    {
        //basePanelSize = calculated by CalculatePanelSize
        //Response rate is a percentage which indicates the expected rate of response to invites.
        return (int)(panelSizeIncludingReserve / responseRate);
    }

    public IEnumerable<PanelMember> GetAllPanelMembersForPanel(Guid panelId)
    {
        return _repo.ReadPanelMembersWithCriteria(panelId);
    }

    public Panel GetPanelWithCriteriaAndCriteriaAnswerOptions(Guid panelId)
    {
        return _repo.ReadPanelWithCriteriaAndAnswerOptions(panelId);
    }

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
        if (panel != null)
        {
            if (increase)
            {
                panel.SuccesfulRegistrationCount++;
            }
            else
            {
                panel.SuccesfulRegistrationCount--;
            }

            _repo.UpdatePanel(panel);
            return;
        }

        throw new NullReferenceException("Panel with id: " + id + " was not found");
    }

    public PanelMember UpdatePanelMember(PanelMember member)
    {
        return _repo.UpdatePanelMember(member);
    }
}