using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CMS_API.Models
{
    public partial class CMSContext : DbContext
    {
        public CMSContext()
        {
        }

        public CMSContext(DbContextOptions<CMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Assignment> Assignments { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<EnrollCourse> EnrollCourses { get; set; } = null!;
        public virtual DbSet<LearningMaterial> LearningMaterials { get; set; } = null!;
        public virtual DbSet<Submission> Submissions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory
                .GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("ConnectionStrings"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.ToTable("Assignment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.Deadline)
                    .HasColumnType("date")
                    .HasColumnName("deadline");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Assignment_Course");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_Course_User");
            });

            modelBuilder.Entity<EnrollCourse>(entity =>
            {
                entity.ToTable("Enroll_Course");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.EnrollCourses)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Enroll_Course_Course");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.EnrollCourses)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_Enroll_Course_User1");
            });

            modelBuilder.Entity<LearningMaterial>(entity =>
            {
                entity.ToTable("Learning_Material");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.Information).HasColumnName("information");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.LearningMaterials)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Learning_Material_Course");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");

                entity.Property(e => e.StudentJd).HasColumnName("student_jd");

                entity.Property(e => e.SubmissionTime)
                    .HasColumnType("date")
                    .HasColumnName("submission_time");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("FK_Submissions_Assignment");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.ToTable("User_Detail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Major)
                    .HasMaxLength(50)
                    .HasColumnName("major");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .HasColumnName("phone");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_User_Detail_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
