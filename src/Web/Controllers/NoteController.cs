using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Application.DTOs.Responses;
using PetProject.Application.Services.Impl;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/note")]
public class NoteController : ControllerBase
{
    private readonly INoteRepository _noteRepository;
    private readonly RedisService _cache;

    public NoteController(INoteRepository noteRepository, RedisService cache)
    {
        _noteRepository = noteRepository;
        _cache = cache;
    }

    [HttpGet]
    [Route("get-comm-and-repl")]
    public async Task<IActionResult> GetCommAndRepl(int id)
    {
        string key = $"note:{id}";

        var cachedNote = await _cache.GetAsync<Note>(key);
        if (cachedNote != null)
        {
            return Ok(cachedNote);
        }
        
        var note = await _noteRepository.GetAllCommAndRepl(id);
        if (note == null) { return NotFound();}
        
        await _cache.SetAsync(key, note, TimeSpan.FromSeconds(10));
        return Ok(note);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        string key = $"note:{id}";

        var cachedNote = await _cache.GetAsync<GetNoteDto>(key);
        if (cachedNote != null)
        {
            GetNoteDto cachedDto = new GetNoteDto()
            {
                Title = cachedNote.Title,
                Author = cachedNote.Author,
                PublishedOn = cachedNote.PublishedOn,
                Likes = cachedNote.Likes,
                Text = cachedNote.Text,
            };
            return Ok(cachedDto);
        }
        
        var note = await _noteRepository.GetAllCommAndRepl(id);
        if (note == null) { return NotFound(); }

        GetNoteDto dto = new GetNoteDto()
        {
            Title = note.Title,
            Author = note.Author,
            PublishedOn = note.PublishedOn,
            Likes = note.Like,
            Text = note.Text,
        };
        
        await _cache.SetAsync(key, note, TimeSpan.FromSeconds(10));
        return Ok(dto);
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

        await _cache.RemoveAsync($"note:{noteId}");
        
        return Ok("Note deleted!");
    }

    [HttpPatch]
    [Route("like-note")]
    public async Task<IActionResult> LikeNote(int noteId)
    {
        await _noteRepository.LikeNoteById(noteId);
        await _cache.RemoveAsync($"note:{noteId}");
        return Ok("Note liked!");
    }
}