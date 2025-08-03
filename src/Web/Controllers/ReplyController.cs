using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Web.Controllers;

[Authorize]
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
    public async Task<Reply> GetReplyById(int id)
    {
        Reply reply = await _replyRepository.GetReplyId(id);
        return reply;
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
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        Reply reply = new Reply
        {
            Author = dto.Author,
            CommentText = dto.Text,
            Like = 0,
            PublishedOn = DateTime.UtcNow,
            CommentId = dto.CommentId,
            UserId  = userId
        };
        reply = await _replyRepository.CreateReply(reply);
        return Ok(reply);
    }

    [Authorize(Policy = "NoteOwner")]
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