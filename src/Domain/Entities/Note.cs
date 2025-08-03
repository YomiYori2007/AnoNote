namespace PetProject.Domain.Entities;

public class Note
{
    public int NoteId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public int Like { get; set; } = 0;
    public DateTime PublishedOn { get; set; }
    public Guid UserId { get; set; }
    public User  User { get; set; }
    
    public ICollection<Comment> Comments { get; set; }

    public void LikeNote() => Like++;
}