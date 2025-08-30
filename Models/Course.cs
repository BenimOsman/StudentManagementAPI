using System.ComponentModel.DataAnnotations;                                                    // Provides attributes for data validation
using System.ComponentModel.DataAnnotations.Schema;                                             // Provides attributes like [Table] for database mapping

namespace WebAPIStudent.Models
{
    [Table("Course")]                                                                           // Maps this class to the 'Course' table in the database
    public class Course
    {
        [Key]
        public int CourseId { get; set; }                                                       // Marked as primary key

        [Required]
        public string Title { get; set; }                                                       // Title can't be null

        public int Credits { get; set; }                                                        // Credits for the course - Not required
    }
}

// [Table("Course")] - Ensures the class is explicitly mapped to the database table, even if the class name changes