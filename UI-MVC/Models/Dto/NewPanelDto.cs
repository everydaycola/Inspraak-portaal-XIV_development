namespace UI_MVC.Models.Dto;

public class NewPanelDto
{
    public String Name { get; set; }
    public int Size { get; set; }
    public double SampleRate { get; set; }
    public Dictionary<String, Dictionary<String, Double>> Distributions { get; set; }

    public int CitizenCount { get; set; }
    public double ReservePercentage { get; set; }
    public double ResponseRate { get; set; }
}