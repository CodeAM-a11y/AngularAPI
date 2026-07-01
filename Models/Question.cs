namespace AngularAPI.Models;

public partial class Question
{
    public int ExamId { get; set; }

    public int Id { get; set; }

    public string? Type { get; set; }

    public string? QuestionText { get; set; }

    public string? Hint { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Exam Exam { get; set; } = null!;
}
