namespace Domain.CitizenPanel;

public class RepresentationGroup
{
    public Guid Id { get; set; }
    public int CitizenCount { get; set; }
    public double ReservePercentage { get; set; }
    public double ResponseRate { get; set; }
    public Panel Panel { get; set; }
    
}