using PetProject.Application.Models;
using PetProject.Domain.Entities;

namespace PetProject.Domain.Repository;

public interface ICommentRepository
{
    Task<Comment> GetCommentById(int id); 
    Task<Comment> CreateComment(Comment comment);
    Task<OperationResult> DeleteCommentById(int id, Guid userId);
    Task<Comment?> GetAllRepliesOfCommentById(int id);
    Task LikeCommentById(int commentId);
    Task<List<Comment>> GetCommentPagination (int pageNumber, int pageSize, int noteId);
}