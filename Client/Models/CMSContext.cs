using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Client.Models
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

        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<EnrollCourse> EnrollCourses { get; set; }
        public virtual DbSet<LearningMaterial> LearningMaterials { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database =CMS;uid=sa;pwd=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(e => e.AsignmentId);

                entity.ToTable("Assignment");

                entity.Property(e => e.AsignmentId).HasColumnName("asignment_id");

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

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_Course_User");
            });

            modelBuilder.Entity<EnrollCourse>(entity =>
            {
                entity.HasKey(e => e.IdEnrollCourse);

                entity.ToTable("Enroll_Course");

                entity.Property(e => e.IdEnrollCourse).HasColumnName("id_enroll_course");

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
                entity.HasKey(e => e.LmId);

                entity.ToTable("Learning_Material");

                entity.Property(e => e.LmId).HasColumnName("lm_id");

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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("role_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => e.IdSubmission);

                entity.Property(e => e.IdSubmission).HasColumnName("id_submission");

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

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Major)
                    .HasMaxLength(50)
                    .HasColumnName("major");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .HasColumnName("phone")
                    .IsFixedLength();

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
