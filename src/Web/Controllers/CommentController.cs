using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetProject.Application.DTOs.Requests;
using PetProject.Application.DTOs.Responses;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly EfContext _context;

    public CommentController(ICommentRepository commentRepository, EfContext context)
    {
        _commentRepository = commentRepository;
        _context = context;
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
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO dto)
    {
        var comment = new Comment(
            author: dto.Author, 
            commentText: dto.Text, 
            publishedOn: dto.CurrentDate,
            id: dto.NoteId);
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