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
            .ReturnsAsync(new Note("Test", "Test", "Test", DateTime.MinValue ));
        
        var controller = new NoteController(mockrepo.Object);

        var result = await controller.GetNoteById(1);
        
        Assert.IsType<OkObjectResult>(result);
    }
}