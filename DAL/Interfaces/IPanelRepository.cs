using Domain.CitizenPanel;

namespace DAL.Interfaces;

public interface IPanelRepository
{
    //READ
    public Panel ReadPanel(Guid id);
    public IEnumerable<Panel> ReadAllPanels();
    public PanelMember ReadPanelMember(Guid id);
    public PanelMember ReadPanelMemberWithCriteriaResponses(Guid id);
    public PanelMember ReadPanelMemberWithPanel(Guid id);
    public Panel ReadPanelWithRepresentationGroup(Guid id);
    public IEnumerable<PanelMember> ReadPanelMembersWithCriteria(Guid id);
    public IEnumerable<PanelMember> ReadPanelMembersWhichAnsweredAllQuestionsWithCriteria(Guid id);
    public ICollection<PanelMember> ReadPanelMembersAndRepresentationGroup(Guid id);
    public Panel ReadPanelWithCriteriaAndAnswerOptions(Guid panelId);
    //UPDATE
    public void UpdatePanel(Panel panel);
    public PanelMember UpdatePanelMember(PanelMember member);
    //CREATE
    public void CreatePanelMember(PanelMember panelMember);
}