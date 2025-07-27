using PetProject.Domain.Entities;

namespace PetProject.Domain.Repository;

public interface IReplyRepository
{
    Task<Reply> GetReplyId(int id); 
    Task<Reply> CreateReply(Reply comment);
    Task DeleteReplyById(int id);
    Task LikeReplyById(int commentId);
    Task<List<Reply>> GetRepliesPagination(int pageNumber, int pageSize, int commentId);
}