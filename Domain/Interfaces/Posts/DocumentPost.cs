namespace Domain.Interfaces;

public class DocumentPost: Post
{
    public string DocumentName { get; set; }
    public bool isImage { get; set; } = false;
}