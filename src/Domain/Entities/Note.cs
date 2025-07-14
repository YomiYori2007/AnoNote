using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using PetProject.Application.DTOs.Requests;
using StatusGeneric;

namespace PetProject.Domain.Entities;

public class Note
{
    private Note() {}
    
    public int NoteId { get; private set; }
    [Required]
    [MaxLength(16)]
    public string Author { get; private set; }
    [Required]
    [MaxLength(500)]
    public string Text { get; private set; }
    public DateTime PublishedOn { get; private set; }
    
    public ICollection<Comment> Comments { get; private set; }

    public Note(string author, string text, int noteId, DateTime publishedOn)
    {
        Author = author;
        Text = text;
        NoteId = noteId;
        PublishedOn = publishedOn;
    }
    
    public static IStatusGeneric<Note> CreateNote(string author, string text, DateTime publishedOn)
    {
        var status = new StatusGenericHandler<Note>();
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(author))
        {
            status.AddError("Author or Text is empty");
        }

        var note = new Note()
        {
            Author = author,
            Text = text,
            PublishedOn = publishedOn,
        };
        return status.SetResult(note);
    }
}