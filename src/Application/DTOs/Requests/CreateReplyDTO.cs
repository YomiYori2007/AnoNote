namespace PetProject.Application.DTOs.Requests;

public class CreateReplyDTO
{
    public CreateReplyDTO(string author, string text, DateTime currentDate, int commentId)
    {
        Author = author;
        Text = text;
        CurrentDate = currentDate;
        CommentId = commentId;
    }
    public string Author { get; set; }
    
    public string Text { get; set; }
    
    public DateTime CurrentDate { get; set; }
    
    public int CommentId { get; set; }
}