namespace Domain.CitizenPanel;

public class RepresentationGroup
{
    public Guid Id { get; set; }
    public int memberCount { get; set; }
    public double reservePercentage { get; set; }
    public double responseRate { get; set; }
    public Panel Panel { get; set; }

    public RepresentationGroup(int memberCount, double reservePercentage, double responseRate)
    {
        this.memberCount = memberCount;
        this.reservePercentage = reservePercentage;
        this.responseRate = responseRate;
    }
}