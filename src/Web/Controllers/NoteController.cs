using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/note")]
public class NoteController : ControllerBase
{
    private readonly INoteRepository _noteRepository;

    public NoteController(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    [HttpGet]
    [Route("get-comm-and-repl")]
    public async Task<IActionResult> GetCommAndRepl(int id)
    {
        var note = await _noteRepository.GetAllCommAndRepl(id);
        if (note == null) { return NotFound();}
        return Ok(note);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        var note = await _noteRepository.GetNoteById(id);
        if (note == null) { return NotFound(); }
        return Ok(note);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<List<Note>> GetNotesPagination(int page, int pageSize)
    {
        List<Note> notes = await _noteRepository.GetNotesPagination(page, pageSize);
        return notes;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<Note> CreateNote([FromBody] CreateNoteDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        Note note = new Note
        {
            Title = dto.Title,
            Author = dto.Author,
            Text = dto.Text,
            Like = 0,
            PublishedOn = DateTime.UtcNow,
            UserId  = userId
        };
        
        await _noteRepository.CreateNote(note);
        return note;
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteNote(int noteId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        var result = await _noteRepository.DeleteNote(noteId, userId);

        switch (result.ErrorCode)
        {
            case "not_found":
                return NotFound("Note not found or you are not owner");
        }
        return Ok("Note deleted!");
    }

    [HttpPatch]
    [Route("like-note")]
    public async Task<IActionResult> LikeNote(int noteId)
    {
        await _noteRepository.LikeNoteById(noteId);
        return Ok("Note liked!");
    }
}