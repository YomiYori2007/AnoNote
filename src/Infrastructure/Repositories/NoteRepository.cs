using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<Note?> GetAllCommAndRepl(int id)
    {
        return await _context.Notes
            .AsSplitQuery()
            .AsNoTracking()
            .Include(p => p.Comments)
            .ThenInclude(p => p.Replies)
            .FirstOrDefaultAsync(p => p.NoteId == id);
    }

    public async Task LikeNoteById(int commentId)
    {
        Note? note = await _context.Notes
            .FirstOrDefaultAsync(p => p.NoteId == commentId);
        note?.LikeNote();
        await _context.SaveChangesAsync();
    }
}