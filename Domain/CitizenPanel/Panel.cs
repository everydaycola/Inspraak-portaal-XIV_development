namespace Domain.CitizenPanel;

public class Panel
{
    public Guid Id { get; set; }
    public string name { get; set; }
    public ICollection<PanelMember> PanelMembers { get; set; }
    public RepresentationGroup RepresentationGroup { get; set; }
    public double SampleRate { get; set; }
    
    public Panel(string name, double sampleRate)
    {
        this.name = name;
        this.SampleRate = sampleRate;
    }
}