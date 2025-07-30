using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Application.DTOs.Responses;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<GetCommentDTO>> GetCommentById(int id)
    {
        var comment = await _commentRepository.GetCommentById(id);
        return Ok(comment);
    }

    [HttpGet]
    [Route("get-replies-of-comment")]
    public async Task<ActionResult> GetRepliesOfCommentById(int id)
    {
        var comment = await _commentRepository.GetAllRepliesOfCommentById(id);
        return Ok(comment);
    }
    
    [HttpGet]
    [Route("get-pagination")]
    public async Task<IActionResult> GetPagination(int pageNumber, int pageSize, int noteId)
    {
        List<Comment> comments = await _commentRepository.GetCommentPagination(pageNumber, pageSize, noteId);
        return Ok(comments);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto dto)
    {
        var comment = new Comment
        {
            Author = dto.Author,
            CommentText = dto.Text,
            Like = 0,
            PublishedOn = DateTime.Now,
            NoteId = dto.NoteId
        };
        await _commentRepository.CreateComment(comment);
        return Ok("Comment created");
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        await _commentRepository.DeleteCommentById(commentId);
        return Ok("Comment has been deleted");
    }

    [HttpPatch]
    [Route("like-comment")]
    public async Task<IActionResult> LikeComment(int commentId)
    {
        await _commentRepository.LikeCommentById(commentId);
        return Ok("Comment liked");
    }
}