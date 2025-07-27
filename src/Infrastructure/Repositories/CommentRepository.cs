using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly EfContext.EfContext _context;

    public CommentRepository(EfContext.EfContext context)
    {
        _context = context;
    }

    public async Task<Comment> GetCommentById(int id)
    {
        return await _context.Comment.AsNoTracking()
            .FirstAsync(p => p.CommentId == id);
    }

    public async Task<Comment?> GetAllRepliesOfCommentById(int id)
    {
        return await _context.Comment
            .AsNoTracking()
            .OrderBy(p => p.PublishedOn)
            .Include(p => p.Replies)
            .FirstOrDefaultAsync(p => p.CommentId == id);
    }
    
    public async Task<Comment> CreateComment(Comment comment)
    {
        var note = await _context.Notes.Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.NoteId == comment.NoteId);
        note?.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task DeleteCommentById(int id)
    {
        Comment? comment = await _context.Comment.AsNoTracking()
            .FirstOrDefaultAsync(p => p.CommentId == id);
        if (comment != null) _context.Comment.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task LikeCommentById(int commentId)
    {
        Comment? comment = await _context.Comment
            .FirstOrDefaultAsync(p => p.CommentId == commentId);
        comment?.LikeComm();
        await _context.SaveChangesAsync();
    }

    public async Task<List<Comment>> GetCommentPagination(int pageNumber, int pageSize, int noteId)
    {
        return await _context.Comment
            .AsNoTracking()
            .OrderBy(p => p.PublishedOn)
            .Where(p => p.NoteId == noteId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}