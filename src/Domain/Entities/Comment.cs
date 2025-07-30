

namespace PetProject.Domain.Entities;

public class Comment
{
    public int CommentId { get; set; }
    public string Author { get; set; }
    public string CommentText { get; set; }
    public int Like { get; set; } = 0;
    public DateTime PublishedOn { get; set; }
    public ICollection<Reply>  Replies { get; set; }
    
    public int NoteId { get; set; }

    public void LikeComm() => Like++;

}