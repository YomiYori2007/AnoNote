using PetProject.Domain.Entities;

namespace PetProject.Application.Services.Interfaces;

public interface IReplyRepository
{
    Task<Reply> GetReplyId(int id); 
    Task<Reply> CreateReply(Reply comment);
    Task DeleteReplyById(int id);
    Task LikeReplyById(int commentId);
}