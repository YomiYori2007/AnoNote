using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;

namespace PetProject.Web.Controllers;
[ApiController]
[Route("api/reply")]
public class ReplyController : ControllerBase
{
    private readonly IReplyRepository _replyRepository;
    private readonly EfContext _context;

    public ReplyController(IReplyRepository replyRepository, EfContext context)
    {
        _replyRepository = replyRepository;
        _context = context;
    }

    [HttpGet]
    [Route("get")]
    public async Task<ActionResult> Get(int id)
    {
        var reply = await _replyRepository.GetReplyId(id);
        return Ok(reply);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateReply([FromBody] CreateReplyDTO dto, int commentId)
    {
        var reply = new Reply(
            author:  dto.Author,
            commentId:  commentId,
            commentText:   dto.Text,
            publishedOn:  dto.CurrentDate
            );
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
}