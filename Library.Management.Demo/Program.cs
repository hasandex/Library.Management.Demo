using Library.Management.Demo.IRepositories;
using Library.Management.Demo.Models;
using Library.Management.Demo.Repositories;
using Library.Management.Demo.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt=>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("conStr"));
});

builder.Services.AddScoped<IBookeService, BookService>();
builder.Services.AddScoped<IBookReo, BookRepo>();
builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IPublisherRepo, PublisherRepo>();
builder.Services.AddScoped<ILIbraryRepo, LibraryRepo>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
