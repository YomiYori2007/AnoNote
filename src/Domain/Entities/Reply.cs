namespace PetProject.Domain.Entities;

public class Reply
{
    public int ReplyId { get; set; }
    public string Author { get; set; }
    public string CommentText { get;  set; }
    public int Like {get; set; } = 0;
    public DateTime PublishedOn { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser  User { get; set; }
    
    public int CommentId { get; set; }
    
    public void LikeReply() => Like++;
}