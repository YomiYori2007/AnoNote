using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PetProject.Application.Services.Impl;
using PetProject.Application.Services.Interfaces;
using PetProject.Infrastructure.EfContext;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<EfContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IReplyRepository, ReplyRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();