using Microsoft.EntityFrameworkCore;                                                        // EF Core base library for DbContext and DbSet

namespace WebAPIStudent.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { } // Configuration of database

        // DbSet represents a table in the database
        public DbSet<Student> Students { get; set; }                                        // Maps to a Student table
        public DbSet<Course> Courses { get; set; }                                          // Maps to a Course table
        public DbSet<Enrolment> Enrolments { get; set; }                                    // Maps to a Enrolment table
    }
}

// DbContext is the main class that contains the EF Core functionality for a data model