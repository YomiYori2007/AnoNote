using Microsoft.EntityFrameworkCore;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;

namespace PetProject.Application.Services.Impl;

public class NoteRepository : INoteRepository
{
    private readonly DbContext _dbContext;

    public NoteRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(Note note)
    {
        await _dbContext.AddAsync(note);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Note note)
    {
        _dbContext.Remove(note);
        await _dbContext.SaveChangesAsync();
    }
}