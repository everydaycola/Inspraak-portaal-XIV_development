namespace UI_MVC.Models.Dto;

public class GroupedUniqueCodesDto
{
    public bool IsDefaultGroup { get; set; }
    public string GroupKey { get; set; } // The combined criteria key
    public ICollection<UniqueCodesDto> Members { get; set; }
    public string Name { get; set; }
}