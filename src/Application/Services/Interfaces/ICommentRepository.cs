using PetProject.Domain.Entities;

namespace PetProject.Application.Services.Interfaces;

public interface ICommentRepository
{
    Task Create(Comment comment);
    Task Delete(Comment comment);
}