using DevPath.Models;
using Microsoft.EntityFrameworkCore;

namespace DevPath.Data
{
    public class DevPathDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlite("Data Source=devpath.db");
        }

    }
} 

