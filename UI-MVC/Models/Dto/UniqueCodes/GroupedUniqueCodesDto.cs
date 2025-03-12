namespace UI_MVC.Models.Dto;

public class GroupedUniqueCodesDto
{
    public string GroupKey { get; set; } // The combined criteria key
    public List<UniqueCodesDto> Members { get; set; }
}