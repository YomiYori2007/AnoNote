using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetProject.Domain.Entities;
using PetProject.Domain.Repository;
using PetProject.Infrastructure.EfContext;
using PetProject.Infrastructure.Repositories;


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

builder.Services
    .AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<EfContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NoteOwner", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") ||
            context.Resource is Note note &&
            note.UserId == Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier))
        ));
});
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
Console.WriteLine($"JWT Config: {builder.Configuration["Jwt:Issuer"]}");
Console.WriteLine($"All config: {string.Join(", ", builder.Configuration.AsEnumerable())}");
app.Run();