namespace PetProject.Domain.Entities;

public class Reply
{
    public Reply(int commentId, string author, string commentText, DateTime publishedOn)
    {
        Author = author;
        CommentText = commentText;
        PublishedOn = publishedOn;
        CommentId = commentId;
    }

    public int ReplyId { get; private set; }
    public string Author { get; private set; }
    public string CommentText { get; private set; }
    
    public int Like {get; private set; } = 0;
    public DateTime PublishedOn { get; private set; }

    
    public int CommentId { get; private set; }
    
    public void LikeReply() => Like++;
}