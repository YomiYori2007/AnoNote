using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PetProject.Domain.Entities;

public class Comment
{
    public Comment() {}
    public Comment(string author, string commentText, DateTime publishedOn, int id)
    {
        Author = author;
        CommentText = commentText;
        PublishedOn = publishedOn;
        NoteId = id;
    }
    public int CommentId { get; private set; }
    public string Author { get; private set; }
    public string CommentText { get; private set; }
    public int Like { get; private set; } = 0;
    public DateTime PublishedOn { get; private set; }
    public ICollection<Reply>  Replies { get; private set; }
    
    public int NoteId { get; private set; }

    public void LikeComm() => Like++;

}