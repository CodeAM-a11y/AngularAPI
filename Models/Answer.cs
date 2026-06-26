using System;
using System.Collections.Generic;

namespace AngularAPI.Models;

public partial class Answer
{
    public int ExamId { get; set; }

    public int QuestionId { get; set; }

    public int Id { get; set; }

    public string? AnswerText { get; set; }

    public int? IsCorrect { get; set; }

    public virtual Question Question { get; set; } = null!;
}
