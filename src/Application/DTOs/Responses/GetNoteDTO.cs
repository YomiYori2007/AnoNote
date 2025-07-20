namespace PetProject.Application.DTOs.Responses;

public class GetNoteDTO
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public int Likes { get; set; }
    public DateTime CurrentDate { get; set; }
}