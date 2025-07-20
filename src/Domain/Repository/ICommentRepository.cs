using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;

namespace PetProject.Application.Services.Interfaces;

public interface ICommentRepository
{
    Task<Comment> GetCommentById(int id); 
    Task<Comment> CreateComment(Comment comment);
    Task DeleteCommentById(int id);
}