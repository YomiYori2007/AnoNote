using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PetProject.Application.DTOs.Requests;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;
using PetProject.Web.Controllers;

namespace PetProject.Tests.Web.Tests.Controllers;

public class NoteControllerTests
{
    [Fact]
    public async Task GetNoteById_ExistingId_ReturnsOk()
    {
        Note note = new Note
        {
            NoteId = 1,
            Title = "Test",
            Author = "Test",
            Text = "Test",
            Like = 0,
            PublishedOn = DateTime.Now
        };
        
        var mockrepo = new Mock<INoteRepository>();
        mockrepo.Setup(r => r.GetNoteById(1))
            .ReturnsAsync(note);
        
        var controller = new NoteController(mockrepo.Object);

        Note? result = await controller.GetNoteById(1);
        
        Assert.IsType<Note>(result);
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
        
        Assert.IsType<Note>(result);
    }

    [Fact]
    public async Task GetNotesPagination_NonExistingId_ReturnsOK()
    {
        var now = DateTime.Now;
        List<Note> notes = new List<Note>()
        {
            new Note
            {
                NoteId = 1,
                Title = "Test",
                Author = "Test",
                Text = "Test",
                Like = 5,
                PublishedOn = now,
                Comments = null
            },
            new Note
            {
                NoteId = 2,
                Title = "Test",
                Author = "Test",
                Text = "Test",
                Like = 2,
                PublishedOn = now,
                Comments = null
            }
        };
        
        var mockrepo = new Mock<INoteRepository>();

        mockrepo.Setup(r => r.GetNotesPagination(1, 2))
            .ReturnsAsync(notes);
        
        var controller = new NoteController(mockrepo.Object);
        var result = await controller.GetNotesPagination(1, 2);
        
        Assert.Equal(notes, result);
        Assert.IsType<List<Note>>(result);

    }

    [Fact]
    public async Task CreateNote_ReturnsOk()
    {
        CreateNoteDto note = new CreateNoteDto(author: "Test",
            text: "Test",
            currentDate: DateTime.Now,
            title: "Test");

        
        var mockrepo = new Mock<INoteRepository>();
        var controller = new NoteController(mockrepo.Object);
        
        var result = await controller.CreateNote(note);
        Assert.Equal(note.Author, result.Author);
        Assert.Equal(note.Text, result.Text);
        Assert.Equal(note.Title, result.Title);
    }

    [Fact]
    public async Task DeleteNote_ExistingId_ReturnsOk()
    {
        var mockrepo = new Mock<INoteRepository>();
        var controller = new NoteController(mockrepo.Object);

        var result = await controller.DeleteNote("text");
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task LikeNote_ExistingId_ReturnsOk()
    {
        var note = new Note
        {
            NoteId = 0,
            Title = null,
            Author = null,
            Text = null,
            Like = 0,
            PublishedOn = default,
            Comments = null
        };
        var mockrepo = new Mock<INoteRepository>();
        var controller = new NoteController(mockrepo.Object);
        var result = await controller.LikeNote(1);
        
        Assert.IsType<OkObjectResult>(result);
    }
}