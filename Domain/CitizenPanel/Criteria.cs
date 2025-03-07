namespace Domain.CitizenPanel;

public class Criteria
{
    public string name { get; set; }
    public string value { get; set; }

    public Criteria(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}
