using Microsoft.EntityFrameworkCore;
using AngularAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AngularAPI.Endpoints;

public static class AnswerEndpoints
{
    public static void MapAnswerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Answer");

        group.MapGet("/", async (ApiDbContext db) =>
        {
            return await db.Answers.ToListAsync();
        })
        .WithName("GetAllAnswers");

        group.MapGet("/{examid}", async Task<Results<Ok<Answer>, NotFound>> (int examid, ApiDbContext db) =>
        {
            return await db.Answers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ExamId == examid)
                is Answer model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAnswerById");

        group.MapPut("/{examid}", async Task<Results<Ok, NotFound>> (int examid, Answer answer, ApiDbContext db) =>
        {
            var affected = await db.Answers
                .Where(model => model.ExamId == examid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.ExamId, answer.ExamId)
                    .SetProperty(m => m.QuestionId, answer.QuestionId)
                    .SetProperty(m => m.Id, answer.Id)
                    .SetProperty(m => m.AnswerText, answer.AnswerText)
                    .SetProperty(m => m.IsCorrect, answer.IsCorrect)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAnswer");

        group.MapPost("/", async (Answer answer, ApiDbContext db) =>
        {
            db.Answers.Add(answer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Answer/{answer.ExamId}",answer);
        })
        .WithName("CreateAnswer");

        group.MapDelete("/{examid}", async Task<Results<Ok, NotFound>> (int examid, ApiDbContext db) =>
        {
            var affected = await db.Answers
                .Where(model => model.ExamId == examid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAnswer");
    }
}
