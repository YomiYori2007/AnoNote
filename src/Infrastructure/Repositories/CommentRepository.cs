using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.DTOs.Requests;
using PetProject.Application.DTOs.Responses;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;

namespace PetProject.Application.Services.Impl;

public class CommentRepository : ICommentRepository
{
    private readonly EfContext _context;

    public CommentRepository(EfContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetCommentById(int id)
    {
        return await _context.Comment.AsNoTracking()
            .FirstAsync(p => p.CommentId == id);
    }
    
    public async Task<Comment> CreateComment(Comment comment)
    {
        var note = await _context.Notes.Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.NoteId == comment.NoteId);
        note.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task DeleteCommentById(int id)
    {
        Comment? comment = await _context.Comment.AsNoTracking()
            .FirstOrDefaultAsync(p => p.CommentId == id);
        _context.Remove(comment);
        await _context.SaveChangesAsync();
    }
}