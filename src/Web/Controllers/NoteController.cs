using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;
using PetProject.Application;
using PetProject.Application.Services.Interfaces;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("note/")]
public class NoteController : ControllerBase
{
    private readonly EfContext _context;
    private readonly INoteRepository _noteRepository;

    public NoteController(INoteRepository noteRepository, EfContext context)
    {
        _context = context;
        _noteRepository = noteRepository;
    }

    [HttpPost]
    [Route("/create")]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDTO dto)
    {
        var note = new Note(dto.Author, dto.Text, dto.CurrentDate);
        await _noteRepository.Create(note);
        return Ok(note);
    }

    [HttpDelete]
    [Route("/delete")]
    public async Task<IActionResult> DeleteNote(int noteId) // Ваще тут title должно быть,
                                                            // но я забыл про него :D
    {
        var note = _context.Notes.AsNoTracking()
            .FirstOrDefaultAsync(p => p.NoteId == noteId).Result;
        await _noteRepository.Delete(note);
        return Ok("Note deleted!");
    }
}