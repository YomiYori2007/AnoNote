﻿using GenericServices;
using PetProject.Domain.Entities;

namespace PetProject.Application.DTOs.Requests;

public class CreateCommentDto : ILinkToEntity<Comment>
{
    public CreateCommentDto()
    {
    }

    public CreateCommentDto(string author, string text, DateTime currentDate, int noteId)
    {
        Author = author;
        Text = text;
        CurrentDate = currentDate;
        NoteId = noteId;
    }
    public string Author { get; set; }
    
    public string Text { get; set; }
    
    public DateTime CurrentDate { get; set; }
    
    public int NoteId { get; set; }
}
