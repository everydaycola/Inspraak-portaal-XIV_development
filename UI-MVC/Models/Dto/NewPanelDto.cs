using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.Dto;

public class NewPanelDto
{
    [Required(ErrorMessage = "Panel moet een naam hebben.")]
    [MinLength(4, ErrorMessage = "Panel naam moet minimaal 4 karakters lang zijn.")]
    [MaxLength(100, ErrorMessage = "Panel naam mag maximaal 100 karakters lang zijn.")]
    public string Name { get; set; }

    public int Size { get; set; }

    [Required(ErrorMessage = "Panel moet een sample rate hebben.")]
    [Range(0, 100, ErrorMessage = "Sample rate moet een percentage tussen 0 en 100% zijn.")]
    public double SampleRate { get; set; }

    public ICollection<CriteriaDto> Distributions { get; set; } = new List<CriteriaDto>();
    public int CitizenCount { get; set; }
    public ICollection<SubRegionDto> SubRegions { get; set; } = new List<SubRegionDto>();

    [Required(ErrorMessage = "Panel moet een reserve percentage hebben")]
    [Range(0, 100, ErrorMessage = "Percentage moet tussen 0 en 100 liggen")]
    public double ReservePercentage { get; set; }

    [Required(ErrorMessage = "Panel moet een reserve percentage hebben")]
    [Range(0, 100, ErrorMessage = "Percentage moet tussen 0 en 100 liggen")]
    public double ResponseRate { get; set; }
}