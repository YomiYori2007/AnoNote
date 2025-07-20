using Microsoft.EntityFrameworkCore;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;

namespace PetProject.Application.Services.Impl;

public class NoteRepository : INoteRepository
{
    private readonly EfContext _context;

    public NoteRepository(EfContext context)
    {
        _context = context;
    }

    public async Task<Note?> GetNoteById(int id)
    {
        return await _context.Notes.AsNoTracking()
            .FirstOrDefaultAsync(p => p.NoteId == id);
    }
    
    public async Task CreateNote(Note note)
    {
        await _context.AddAsync(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNote(string title) // Реализовать поиск из контроллера
    {
        Note? note = await _context.Notes.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Title == title);
        _context.Remove(note);
        await _context.SaveChangesAsync();
    }
}