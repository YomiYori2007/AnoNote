using PetProject.Domain.Entities;

namespace PetProject.Application.Services.Interfaces;

public interface INoteRepository
{
    Task Create(Note note);
    Task Delete(Note note);
}