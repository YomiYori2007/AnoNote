using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;
using PetProject.Application;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("note/note-create")]
public class NoteCreateController : ControllerBase
{
    private readonly EfContext _context;

    public NoteCreateController(EfContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDTO dto)
    {
        var note = new Note(author: dto.Author, text: dto.Text, noteId: dto.NoteId,
            publishedOn: DateTime.UtcNow);
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(CreateNote), new { id = note.NoteId }, 
            new { id = note.NoteId, message = "Note created successfully!" });
    }
}