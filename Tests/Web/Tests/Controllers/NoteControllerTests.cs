using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;
using PetProject.Web.Controllers;

namespace PetProject.Tests.Web.Tests.Controllers;

public class NoteControllerTests
{
    [Fact]
    public async Task GetNote_ExistingId_ReturnsOk()
    {
        var mockrepo = new Mock<INoteRepository>();
        mockrepo.Setup(r => r.GetNoteById(1))
            .ReturnsAsync(new Note
            {
                NoteId = 1,
                Title = "Test",
                Author = "Test",
                Text = "Test",
                Like = 0,
                PublishedOn = DateTime.Now
            });
        
        var controller = new NoteController(mockrepo.Object);

        var result = await controller.GetNoteById(1);
        
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetNoteWithCommAndRepl_ReturnsOk()
    {
        var now = DateTime.Now;
        Note expectedNote = new Note
        {
            NoteId = 1,
            Title = "Test",
            Author = "Test",
            Text = "Test",
            Like = 1,
            PublishedOn = now,
            Comments = new List<Comment>
            {
                new Comment
                {
                    CommentId = 1,
                    Author = "Test",
                    CommentText = "Test",
                    Like = 1,
                    PublishedOn = now,
                    NoteId = 1,
                    Replies = new List<Reply>
                    {
                        new Reply
                        {
                            ReplyId = 1,
                            Author = "Test",
                            CommentText = "Test",
                            Like = 1,
                            PublishedOn = now,
                            CommentId = 1
                        }
                    }
                }
            }
        };

        Mock<INoteRepository> mockrepo = new Mock<INoteRepository>();
        mockrepo.Setup(r => r.GetAllCommAndRepl(1))
            .ReturnsAsync(expectedNote);
        
        NoteController controller = new NoteController(mockrepo.Object);

        var result = await controller.GetCommAndRepl(1);
        Assert.Equal(expectedNote.NoteId, result.NoteId);
        
        Assert.IsType<OkObjectResult>(result);
    }
}