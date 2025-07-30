using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Web.Controllers;

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
    public async Task<Note> GetCommAndRepl(int id)
    {
        var note = await _noteRepository.GetAllCommAndRepl(id);
        return note;
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        var note = await _noteRepository.GetNoteById(id);
        if (note == null) {return NotFound();}
        return Ok(note);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<List<Note?>> GetNotesPagination(int page, int pageSize)
    {
        List<Note> notes = await _noteRepository.GetNotesPagination(page, pageSize);
        return notes;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
    {
        Note note = new Note
        {
            Title = dto.Title,
            Author = dto.Author,
            Text = dto.Text,
            Like = 0,
            PublishedOn = DateTime.Now,
        };
        await _noteRepository.CreateNote(note);
        return Ok(note);
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteNote(string title) 
    {
        await _noteRepository.DeleteNote(title);
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