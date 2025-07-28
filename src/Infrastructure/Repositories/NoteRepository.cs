using Microsoft.EntityFrameworkCore;
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

    public async Task DeleteNote(string title) 
    {
        Note? note = await _context.Notes.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Title == title);
        if (note != null)
            _context.Remove(note);
        else
        {
            throw new Exception("Note not found");
        }
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