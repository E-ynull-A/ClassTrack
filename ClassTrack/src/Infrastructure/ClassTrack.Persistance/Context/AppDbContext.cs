using ClassTrack.Domain.Entities;
using ClassTrack.Persistance.Context;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClassTrack.Persistance.DAL
{
    internal class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyAllQueryFilters();
            base.OnModelCreating(builder);   
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<QuizAnswer> QuizAnswers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<StudentQuiz> StudentQuizes { get; set; }
        public DbSet<StudentTaskWork> StudentTaskWorks { get; set; }
        public DbSet<TaskWork> TaskWorks { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherClass> TeacherClasses { get; set; }
        public DbSet<Class> Classes { get; set; }





    }
}
