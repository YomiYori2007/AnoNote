using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Infrastructure.Repositories;

public class ReplyRepository : IReplyRepository
{
    
    private readonly EfContext.EfContext _context;

    public ReplyRepository(EfContext.EfContext context)
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
        comment?.Replies.Add(reply);
        await _context.SaveChangesAsync();
        return reply;
    }

    public async Task DeleteReplyById(int id)
    {
        Reply? reply = await _context.Reply.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ReplyId == id);
        if (reply != null) _context.Reply.Remove(reply);
        await _context.SaveChangesAsync();
    }

    public async Task LikeReplyById(int commentId)
    {
        Reply? reply = await _context.Reply
            .FirstOrDefaultAsync(p => p.ReplyId == commentId);
        reply?.LikeReply();
        await _context.SaveChangesAsync();
    }

    public async Task<List<Reply>> GetRepliesPagination(int pageNumber, int pageSize, int commentId)
    {
        return await _context.Reply
            .AsNoTracking()
            .Where(p => p.CommentId == commentId)
            .OrderBy(p => p.PublishedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}