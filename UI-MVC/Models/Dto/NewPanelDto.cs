namespace UI_MVC.Models.Dto;

public class NewPanelDto
{
    public String Name { get; set; }
    public int Size { get; set; }
    public int SampleRate { get; set; }
    public Dictionary<String, Dictionary<String, Double>> Distributions { get; set; }
}