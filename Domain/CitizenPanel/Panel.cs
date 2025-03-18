namespace Domain.CitizenPanel;

public class Panel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<PanelMember> PanelMembers { get; set; }
    public RepresentationGroup RepresentationGroup { get; set; }
    public double SampleRate { get; set; }
    public bool IsRegistrationOpen { get; set; }
    public int SuccesfulRegistrationCount { get; set; }
    
    public Panel(string name, double sampleRate)
    {
        Name = name;
        SampleRate = sampleRate;
        SuccesfulRegistrationCount = 0;
    }
}