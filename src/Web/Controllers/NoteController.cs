using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;
using PetProject.Application;
using PetProject.Application.Services.Interfaces;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("api/note")]
public class NoteController : ControllerBase
{
    private readonly EfContext _context;
    private readonly INoteRepository _noteRepository;

    public NoteController(INoteRepository noteRepository, EfContext context)
    {
        _context = context;
        _noteRepository = noteRepository;
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        var note = await _noteRepository.GetNoteById(id);
        if (note == null) {return NotFound();}
        return Ok(note);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteDTO dto)
    {
        Note note = new Note(dto.Title, dto.Author, dto.Text, dto.CurrentDate);
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
}