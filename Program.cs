using AngularAPI.Endpoints;
using AngularAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var config = builder.Configuration;
var connStr = config.GetConnectionString("db");
builder.Services.AddDbContext<ApiDbContext>(opts => opts.UseSqlite(connStr));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapAnswerEndpoints();

app.MapQuestionEndpoints();

app.MapExamEndpoints();


app.Run();

