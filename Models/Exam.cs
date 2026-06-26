using System;
using System.Collections.Generic;

namespace AngularAPI.Models;

public partial class Exam
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
