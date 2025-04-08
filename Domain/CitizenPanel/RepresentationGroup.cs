using System.ComponentModel.DataAnnotations;

namespace Domain.CitizenPanel;

public class RepresentationGroup
{
    public Guid Id { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Citizen count moet een positief getal zijn.")]
    public int CitizenCount { get; set; }
    [Range(0, 5, ErrorMessage = "Reserve percentage moet een percentage tussen 0 en 500% zijn.")]
    public double ReservePercentage { get; set; }
    [Required]
    [Range(0, 0.20, ErrorMessage = "Response rate moet een percentage tussen 0 en 20% zijn.")]
    public double ResponseRate { get; set; }
    public Panel Panel { get; set; }
}