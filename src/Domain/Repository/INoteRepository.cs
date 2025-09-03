using PetProject.Domain.Entities;
using PetProject.Application.Models;

namespace PetProject.Domain.Repository;

public interface INoteRepository
{
    Task<Note?> GetNoteById(int id);
    Task CreateNote(Note note);
    Task<OperationResult> DeleteNote(int id, Guid userId);
    Task<Note?> GetAllCommAndRepl(int id);
    Task LikeNoteById(int commentId);
    Task <List<Note>> GetNotesPagination(int page, int pageSize);
}