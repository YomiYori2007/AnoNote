using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Web.Controllers;
[ApiController]
[Route("api/reply")]
public class ReplyController : ControllerBase
{
    private readonly IReplyRepository _replyRepository;
    
    public ReplyController(IReplyRepository replyRepository)
    {
        _replyRepository = replyRepository;
    }

    [HttpGet]
    [Route("get")]
    public async Task<ActionResult> Get(int id)
    {
        var reply = await _replyRepository.GetReplyId(id);
        return Ok(reply);
    }

    [HttpGet]
    [Route("get-pagination")]
    public async Task<ActionResult> GetPagination(int pageNumber, int pageSize, int commentId)
    {
        List<Reply> replies = await _replyRepository.GetRepliesPagination(pageNumber, pageSize, commentId);
        return Ok(replies);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateReply([FromBody] CreateReplyDto dto)
    {
        var reply = new Reply
        {
            Author = dto.Author,
            CommentText = dto.Text,
            Like = 0,
            PublishedOn = DateTime.Now,
            CommentId = dto.CommentId
        };
        await _replyRepository.CreateReply(reply);
        return Ok("Comment created!");
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteComment(int replyId)
    {
        await _replyRepository.DeleteReplyById(replyId);
        return Ok("Comment has been deleted");
    }

    [HttpPatch]
    [Route("like-reply")]
    public async Task<IActionResult> LikeReply(int replyId)
    {
        await _replyRepository.LikeReplyById(replyId);
        return Ok("Reply has been liked");
    }
}