using Microsoft.EntityFrameworkCore;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;

namespace PetProject.Application.Services.Impl;

public class ReplyRepository : IReplyRepository
{
    
    private readonly EfContext _context;

    public ReplyRepository(EfContext context)
    {
        _context = context;
    }

    public async Task<Reply> GetReplyId(int id)
    {
        return await _context.Reply.AsNoTracking()
            .FirstAsync(p => p.ReplyId == id);
    }

    public async Task<Reply> CreateReply(Reply reply)
    {
        var comment = await _context.Comment
            .Include(p => p.Replies)
            .FirstOrDefaultAsync(p => p.CommentId == reply.CommentId);
        comment.Replies.Add(reply);
        await _context.SaveChangesAsync();
        return reply;
    }

    public async Task DeleteReplyById(int id)
    {
        Reply? reply = await _context.Reply.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ReplyId == id);
        _context.Reply.Remove(reply);
        await _context.SaveChangesAsync();
    }
}