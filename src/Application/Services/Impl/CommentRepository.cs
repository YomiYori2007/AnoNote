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

    public async Task Create(Comment comment)
    {
        _context.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Comment comment)
    {
        _context.Remove(comment);
        await _context.SaveChangesAsync();
    }
}