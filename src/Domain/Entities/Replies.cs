namespace PetProject.Domain.Entities;

public class Replies
{
    public Replies(string author, string commentText, DateTime publishedOn)
    {
        Author = author;
        CommentText = commentText;
        PublishedOn = publishedOn;
    }
    public int RepliesId { get; private set; }
    public string Author { get; private set; }
    public string CommentText { get; private set; }
    public int Like {get; private set; }
    public DateTime PublishedOn { get; private set; }

    
    public int CommentId { get; private set; }
}