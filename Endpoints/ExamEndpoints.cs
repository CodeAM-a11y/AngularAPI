using Microsoft.EntityFrameworkCore;
using AngularAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AngularAPI.Endpoints;

public static class ExamEndpoints
{
    public static void MapExamEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Exam");

        group.MapGet("/", async (ApiDbContext db) =>
        {
            return await db.Exams.ToListAsync();
        })
        .WithName("GetAllExams");

        group.MapGet("/{id}", async Task<Results<Ok<Exam>, NotFound>> (int id, ApiDbContext db) =>
        {
            return await db.Exams.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Exam model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetExamById");

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Exam exam, ApiDbContext db) =>
        {
            var affected = await db.Exams
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, exam.Id)
                    .SetProperty(m => m.Name, exam.Name)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateExam");

        group.MapPost("/", async (Exam exam, ApiDbContext db) =>
        {
            db.Exams.Add(exam);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Exam/{exam.Id}",exam);
        })
        .WithName("CreateExam");

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApiDbContext db) =>
        {
            var affected = await db.Exams
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteExam");
    }
}
