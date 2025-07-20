namespace PetProject.Application.DTOs.Responses;

public class GetCommentDTO
{
    public string Author { get; set; }
    
    public string Text { get; set; }
    
    public DateTime CurrentDate { get; set; }
}