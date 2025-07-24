using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using PetProject.Application.DTOs.Requests;
using StatusGeneric;

namespace PetProject.Domain.Entities;

public class Note
{
    private Note() {}
    
    public Note(string title, string author, string text, DateTime publishedOn)
    {
        Title = title;
        Author = author;
        Text = text;
        PublishedOn = publishedOn;
        Validate();
    }
    public int NoteId { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Text { get; private set; }
    public int Like { get; private set; } = 0;
    public DateTime PublishedOn { get; private set; }
    
    public ICollection<Comment> Comments { get; private set; }
    
    private void Validate()
    {
        if (string.IsNullOrEmpty(Author)) {throw new Exception("Author is required");}
        
        if (string.IsNullOrEmpty(Text)) {throw new Exception("Text is required");}
    }

    public void LikeNote() => Like++;
}