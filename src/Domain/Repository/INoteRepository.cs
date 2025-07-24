using PetProject.Domain.Entities;

namespace PetProject.Application.Services.Interfaces;

public interface INoteRepository
{
    Task<Note?> GetNoteById(int id);
    Task CreateNote(Note note);
    Task DeleteNote(string title);
    Task<Note?> GetAllCommAndRepl(int id);
    Task LikeNoteById(int commentId);
}