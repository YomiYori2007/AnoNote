using GenericServices;
using PetProject.Domain.Entities;

namespace PetProject.Application.DTOs.Requests;

public class CreateNoteDTO : ILinkToEntity<Note>
{
    public int NoteId { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public DateTime CurrentDate { get; set; }
}