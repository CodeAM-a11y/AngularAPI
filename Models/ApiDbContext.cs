using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AngularAPI.Models;

public partial class ApiDbContext : DbContext
{
    public ApiDbContext()
    {
    }

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Question> Questions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Name=ConnectionStrings:DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => new { e.ExamId, e.QuestionId, e.Id });

            entity.ToTable("answers");

            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnswerText).HasColumnName("answerText");
            entity.Property(e => e.IsCorrect).HasColumnName("isCorrect");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => new { d.ExamId, d.QuestionId })
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.ToTable("exams");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => new { e.ExamId, e.Id });

            entity.ToTable("questions");

            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Hint).HasColumnName("hint");
            entity.Property(e => e.QuestionText).HasColumnName("questionText");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Exam).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
