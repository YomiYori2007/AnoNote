using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Models;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly EfContext.EfContext _context;

    public NoteRepository(EfContext.EfContext context)
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

    public async Task<OperationResult> DeleteNote(int noteId, Guid userId) 
    {
        Note? note = await _context.Notes.AsNoTracking()
            .FirstOrDefaultAsync(p => p.NoteId == noteId && p.UserId == userId);

        if (note == null)
        {
            return new OperationResult()
            {
                Success = false,
                Message = "Note not found or this is not you are note",
                ErrorCode = "not_found"
            };
        }
        _context.Remove(note);
        await _context.SaveChangesAsync();
        return new OperationResult() {Message = "Note deleted!"};
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

    public async Task LikeNoteById(int noteId)
    {
        Note? note = await _context.Notes
            .FirstOrDefaultAsync(p => p.NoteId == noteId);
        note?.LikeNote();
        await _context.SaveChangesAsync();
    }

    public async Task<List<Note>> GetNotesPagination(int page, int pageSize)
    {
        return await _context.Notes
            .AsNoTracking()
            .OrderBy(p => p.PublishedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}