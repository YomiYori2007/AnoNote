using System.ComponentModel.DataAnnotations;

namespace PetProject.Domain.Entities;

public class Comment
{
    private Comment() {}

    public int CommentId { get; private set; }
    [Required]
    [MaxLength(16)] 
    public string Author { get; private set; }
    [Required]
    [MaxLength(500)]
    public string CommentText { get; private set; }
    public DateTime PublishedOn { get; private set; }

    public Comment(string author, string commentText, DateTime publishedOn)
    {
        Author = author;
        CommentText = commentText;
        PublishedOn = publishedOn;
    }

    public int NoteId { get; private set; }
}