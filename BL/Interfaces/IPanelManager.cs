using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface IPanelManager
{
    public Panel GetPanel(Guid id);
    public Panel GetPanelWithRepresentationGroup(Guid id);
    public ICollection<PanelMember> GetPanelMembersAndRepresentationGroup(Guid id);
    public IEnumerable<PanelMember> GetPanelWithPanelMembersAndCriteria(Guid id);
    public PanelMember GetPanelByUserId(Guid memberId);
    public IEnumerable<Panel> GetAllPanels();
    public PanelMember GetPanelMemberById(Guid memberId);
    public PanelMember GetPanelMemberWithPanel(Guid id);
    public Panel AddPanel(string name, int size, double sampleRate,
        Dictionary<string, Dictionary<string, double>> distribution, int citizenCount, double reservePercentage,
        double responseRate);
    public void UpdatePanel(Guid id, bool isRegistrationOpen);
    public void UpdatePanelRegistrationCount(Guid id, bool increase);
    public PanelMember UpdatePanelMember(PanelMember member);
    public int CalculatePanelSize(int citizenCount, double samplePercentage);
    public int CalculateAmountOfReserve(int panelSize, double samplePercentage);
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate);
    IEnumerable<PanelMember> GetAllPanelMembersForPanel(Guid panelId);
}