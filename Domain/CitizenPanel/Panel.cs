namespace Domain.CitizenPanel;

public class Panel
{
    public Guid Id { get; set; }
    public string name { get; set; }
    
    public Panel(string name)
    {
        this.name = name;
    }
}