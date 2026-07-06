using Microsoft.EntityFrameworkCore;
using AngularAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AngularAPI.Endpoints;

public static class QuestionEndpoints
{
    public static void MapQuestionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Question");

        group.MapGet("/", async (ApiDbContext db) =>
            {
                var allQuestions =
                    db.Questions.Where(item => item.ExamId == 2 || item.ExamId == 7);
                //Änderung eventuell rückgängig machen
                
                return await allQuestions.ToListAsync();
            })
        .WithName("GetAllQuestions");
        group.MapGet("/GetQuestionsAnswers/{examid}", async Task<List<Question>>(int examid,ApiDbContext db) =>
            {
                var allQuestions = await db.Questions
                    .Where(item => item.ExamId == examid)
                    .Select(q => new Question
                    {
                        ExamId = q.ExamId,
                        Id = q.Id,
                        Type = q.Type,
                        QuestionText = q.QuestionText,
                        Hint = q.Hint,
                        Answers = q.Answers.Select(a => new Answer
                        {
                            ExamId = a.ExamId,
                            QuestionId = a.QuestionId,
                            Id = a.Id,
                            AnswerText = a.AnswerText,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    })
                    .ToListAsync();
                
                return allQuestions;
            })
            .WithName("GetQuestionsAnswers");

        group.MapGet("/{examid}/{questionid}", 
                async Task<Results<Ok<Question>,Ok<List<Question>>, NotFound>> (int examid, int questionid, ApiDbContext db) =>
        {
                return await db.Questions.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ExamId == examid&&model.Id==questionid)
                is Question model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetQuestionById");

        group.MapPut("/{examid}", async Task<Results<Ok, NotFound>> (int examid, Question question, ApiDbContext db) =>
        {
            var affected = await db.Questions
                .Where(model => model.ExamId == examid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.ExamId, question.ExamId)
                    .SetProperty(m => m.Id, question.Id)
                    .SetProperty(m => m.Type, question.Type)
                    .SetProperty(m => m.QuestionText, question.QuestionText)
                    .SetProperty(m => m.Hint, question.Hint)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateQuestion");

        group.MapPost("/", async (Question question, ApiDbContext db) =>
        {
            db.Questions.Add(question);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Question/{question.ExamId}",question);
        })
        .WithName("CreateQuestion");

        group.MapDelete("/{examid}", async Task<Results<Ok, NotFound>> (int examid, ApiDbContext db) =>
        {
            var affected = await db.Questions
                .Where(model => model.ExamId == examid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteQuestion");
    }
}
