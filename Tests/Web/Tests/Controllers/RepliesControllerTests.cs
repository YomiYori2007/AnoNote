using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;
using PetProject.Web.Controllers;

namespace PetProject.Tests.Web.Tests.Controllers;

public class RepliesControllerTests
{
    [Fact]
    public async Task GetReplyById_ExistingId()
    {
        Reply reply = new Reply
        {
            ReplyId = 1,
            Author = "test",
            CommentText = "test",
            Like = 2,
            PublishedOn = DateTime.Now,
            CommentId = 1
        };

        var mockrepo = new Mock<IReplyRepository>();
        mockrepo.Setup(p => p.GetReplyId(1))
            .ReturnsAsync(reply);
        
        var controller = new ReplyController(mockrepo.Object);
        var result = await controller.GetReplyById(1);
        
        Assert.IsType<Reply>(result);
    }

    [Fact]
    public async Task CreateReply_ReturnsOk()
    {
        CreateReplyDto reply = new CreateReplyDto
        {
            Author = "test",
            Text = "test",
            CurrentDate = DateTime.Now,
            CommentId = 1
        };
        
        var mockrepo = new Mock<IReplyRepository>();
        var controller = new ReplyController(mockrepo.Object);
        var result = await controller.CreateReply(reply);
        
        Assert.IsType<IActionResult>(result, exactMatch: false);
    }

    [Fact]
    public async Task GetPagination_ReturnsOk()
    {
        List<Reply> replies = new List<Reply>()
        {
            new Reply
            {
                ReplyId = 1,
                Author = "test",
                CommentText = "test",
                Like = 2,
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
        };
        
        var mockrepo = new Mock<IReplyRepository>();
        mockrepo.Setup(p => p.GetRepliesPagination(1, 2, 1))
            .ReturnsAsync(replies);
        
        var controller = new ReplyController(mockrepo.Object);
        var result = await controller.GetPagination(1, 2, 1);
        Assert.IsType<IActionResult>(result, exactMatch: false);
    }

    [Fact]
    public async Task DeleteReplyById_ExistingId()
    {
        var mockrepo = new Mock<IReplyRepository>();
        var controller = new ReplyController(mockrepo.Object);
        var result = await controller.DeleteComment(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteReplyById_ReturnsOk()
    {
        var mockrepo = new Mock<IReplyRepository>();
        var controller = new ReplyController(mockrepo.Object);
        var result = await controller.DeleteComment(1);
        Assert.IsType<OkObjectResult>(result);
    }
}