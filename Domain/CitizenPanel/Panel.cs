namespace Domain.CitizenPanel;

public class Panel
{
    public Guid Id { get; set; }
    public string name { get; set; }
    // public Dictionary<string,Dictionary<string,double>> DesiredDistribution { get; set; }
    public ICollection<PanelMember> PanelMembers { get; set; }
    
    public Panel(string name)
    {
        this.name = name;
    }
}