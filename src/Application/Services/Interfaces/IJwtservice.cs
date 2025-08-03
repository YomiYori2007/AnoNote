using PetProject.Domain.Entities;

namespace PetProject.Application.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}