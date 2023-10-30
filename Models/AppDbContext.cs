using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogixTask.Models
{
    public class AppDbContext:IdentityDbContext<WebUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WebUser>(entity =>
            {
                entity.ToTable("WebUsers");
            });

            builder.Entity<UserClass>(entity =>
            {
                entity.ToTable("UserClasses");
            });

            builder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
            });
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<WebUser> WebUsers { get; set; }
        public virtual DbSet<UserClass> UserClasses { get; set; }
    }
}
