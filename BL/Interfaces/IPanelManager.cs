using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface IPanelManager
{
    public Panel GetPanel(Guid id);
    public Panel GetPanelWithRepresentationGroup(Guid id);
    public Panel GetPanelWithPanelMembersAndCriteria(Guid id);
    public PanelMember GetPanelByUserId(Guid memberId);
    public IEnumerable<Panel> GetAllPanels();
    public void AddPanel(Panel panel);
    public Panel AddPanel(string name, int size, double sampleRate,
        Dictionary<string, Dictionary<string, double>> distribution, int citizenCount, double reservePercentage,
        double responseRate);
    public int CalculatePanelSize(int citizenCount, double samplePercentage);
    public int CalculateAmountOfReserve(int panelSize, double samplePercentage);
    public int CalculateTotalInvitesNeeded(int panelSizeIncludingReserve, double responseRate);
}