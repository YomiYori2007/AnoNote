using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.DTOs.Requests;
using PetProject.Application.Services.Interfaces;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.EfContext;

namespace PetProject.Web.Controllers;

[ApiController]
[Route("comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly EfContext _context;

    public CommentController(ICommentRepository commentRepository, EfContext context)
    {
        _commentRepository = commentRepository;
        _context = context;
    }

    [HttpPost]
    [Route("/create")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO dto)
    {
        var comment = new Comment(dto.Author, dto.Text, dto.CurrentDate);
        await _commentRepository.Create(comment);
        return Ok("Comment created");
    }

    [HttpDelete]
    [Route("/delete")]
    public async Task<IActionResult> DeleteComment(int noteid)
    {
        var comment = _context.Comment.AsNoTracking()
            .FirstOrDefaultAsync(p => p.CommentId == noteid);
        await _commentRepository.Delete(await comment);
        return Ok("Comment deleted");
    }
}