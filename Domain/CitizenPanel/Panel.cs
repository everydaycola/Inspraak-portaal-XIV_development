using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.CitizenPanel;

public class Panel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Panel moet een naam hebben.")]
    [MinLength(4, ErrorMessage = "Panel naam moet minimaal 4 karakters lang zijn.")]
    [MaxLength(100, ErrorMessage = "Panel naam mag maximaal 100 karakters lang zijn.")]
    public string Name { get; set; }
    [MaxLength(10, ErrorMessage = "Panel mag maximaal 10 criteria hebben.")]
    public ICollection<Criteria> Criteria { get; set; }
    [Required(ErrorMessage = "Panel moet een representatie groep hebben.")]
    public RepresentationGroup RepresentationGroup { get; set; }
    [Required(ErrorMessage = "Panel moet een  hebben sample rate hebben.")]
    [Range(0, 1, ErrorMessage = "Sample rate moet een percentage tussen 0 en 100% zijn.")]
    public double SampleRate { get; set; }
    public bool IsRegistrationOpen { get; set; }
    [Range(0,int.MaxValue, ErrorMessage = "Succesvol geregistreerde personen mag niet negatief zijn.")]
    public int SuccesfulRegistrationCount { get; set; }
    [Required(ErrorMessage = "Panel moet een eigenaar hebben.")]
    public IdentityUser Owner { get; set; }
}