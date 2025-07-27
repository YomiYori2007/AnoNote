using GenericServices;
using PetProject.Domain.Entities;

namespace PetProject.Application.DTOs.Requests;

public class CreateNoteDto : ILinkToEntity<Note>
{
    public CreateNoteDto(string text, DateTime currentDate, string title, string author)
    {
        Text = text;
        CurrentDate = currentDate;
        Title = title;
        Author = author;
    }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public DateTime CurrentDate { get; set; }
    
}