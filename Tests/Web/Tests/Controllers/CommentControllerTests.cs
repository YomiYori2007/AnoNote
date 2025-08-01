using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;
using PetProject.Web.Controllers;

namespace PetProject.Tests.Web.Tests.Controllers;

public class CommentControllerTests
{
    [Fact]
    public async Task CommentControllerTests_GetCommentById_ExistingId()
    {
        Comment comment = new Comment
        {
            CommentId = 1,
            Author = "test",
            CommentText = "test",
            Like = 5,
            PublishedOn = DateTime.Now,
            NoteId = 1
        };
        var mockrepo = new Mock<ICommentRepository>();
        mockrepo.Setup(p => p.GetCommentById(1))
            .ReturnsAsync(comment);
        var controller = new CommentController(mockrepo.Object);
        Comment? result = await controller.GetCommentById(1);
        Assert.NotNull(result);
        Assert.IsType<Comment>(result);
    }

    [Fact]
    public async Task GetRepliesOfComment_ExistingId_ReturnsOk()
    {
        Comment comment = new Comment
        {
            CommentId = 1,
            Author = "test",
            CommentText = "test",
            Like = 5,
            PublishedOn = DateTime.Now,
            NoteId = 1,
            Replies = new List<Reply>
            {
                new Reply
                {
                    ReplyId = 1,
                    Author = "test",
                    CommentText = "test",
                    Like = 4,
                    PublishedOn = DateTime.Now,
                    CommentId = 1
                },
                new Reply
                {
                    ReplyId = 2,
                    Author = "test",
                    CommentText = "test",
                    Like = 6,
                    PublishedOn = DateTime.Now,
                    CommentId = 1
                }
            }
        };
        var mockrepo = new Mock<ICommentRepository>();
        mockrepo.Setup(p => p.GetCommentById(1))
            .ReturnsAsync(comment);
        var controller = new CommentController(mockrepo.Object);
        var result = await controller.GetCommentById(1);
        Assert.NotNull(result);
        Assert.IsType<Comment>(result);
    }

    [Fact]
    public async Task GetPagination_ExistingId_ReturnsOk()
    {
        List<Comment> comment = new List<Comment>()
        {
            new Comment
            {
                CommentId = 1,
                Author = "test",
                CommentText = "test",
                Like = 5,
                PublishedOn = DateTime.Now,
                NoteId = 1
            },
            new Comment
            {
                CommentId = 2,
                Author = "test",
                CommentText = "test",
                Like = 7,
                PublishedOn = DateTime.Now,
                Replies = null,
                NoteId = 0
            }
        };
        
        var mockrepo = new Mock<ICommentRepository>();
        mockrepo.Setup(p => p.GetCommentPagination(1, 2, 3))
            .ReturnsAsync(comment);
        var controller = new CommentController(mockrepo.Object);
        var result = await controller.GetPagination(1, 2, 3);
        Assert.NotNull(result);
        Assert.IsType<List<Comment>>(result);
    }

    [Fact]
    public async Task CreateComment_NotExistingId_ReturnsOK()
    {
        CreateCommentDto comment = new CreateCommentDto
        {
            Author = "test",
            Text = "test",
            CurrentDate = DateTime.Now,
            NoteId = 2
        };
        var mockrepo = new Mock<ICommentRepository>();
        var controller = new CommentController(mockrepo.Object);
        
        var result = await controller.CreateComment(comment);
        
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteComment_ExistingId_ReturnsOk()
    {
        var mockrepo = new Mock<ICommentRepository>();
        var controller = new CommentController(mockrepo.Object);
        var result = await controller.DeleteComment(1);
        
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task LikeComment_ExistingId_ReturnsOk()
    {
        var mockrepo = new Mock<ICommentRepository>();
        var controller = new CommentController(mockrepo.Object);
        var result = await controller.LikeComment(1);
        Assert.IsType<OkObjectResult>(result);
    }
}