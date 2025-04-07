namespace UI_MVC.Models.Dto;

public class NewPanelDto
{
    public string Name { get; set; }
    public int Size { get; set; }
    public double SampleRate { get; set; }
    public ICollection<CriteriaDto> Distributions { get; set; }
    public int CitizenCount { get; set; }
    public ICollection<SubRegionDto> SubRegions { get; set; } = new List<SubRegionDto>();
    public double ReservePercentage { get; set; }
    public double ResponseRate { get; set; }
}