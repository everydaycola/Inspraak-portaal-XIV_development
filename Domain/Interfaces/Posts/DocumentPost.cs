using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public class DocumentPost: Post
{
    [Required(ErrorMessage = "Document post moet een document hebben")]
    [MaxLength(300, ErrorMessage = "Documentname is too long")]
    public string DocumentName { get; set; }
    public bool isImage { get; set; } = false;
}