using Microsoft.AspNetCore.Identity;

namespace PetProject.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public ICollection<Note> Notes { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Reply> Replies { get; set; }
}