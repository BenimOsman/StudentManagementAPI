using System.ComponentModel.DataAnnotations;                                                    // Provides attributes for data validation
using System.ComponentModel.DataAnnotations.Schema;                                             // Provides attributes like [Table] for database mapping

namespace WebAPIStudent.Models
{
    [Table("Enrolment")]                                                                        // Maps this class to the 'Enrolment' table in the database
    public class Enrolment
    {
        [Key]
        public int EnrolmentId { get; set; }                                                    // Primary key 

        [Required]
        public int StudentId { get; set; }                                                      // Foreign key referencing Student

        [Required]
        public int CourseId { get; set; }                                                       // Foreign key referencing Course

        public DateTime JoiningDate { get; set; }                                               // DOJ

        public string? Grade { get; set; }                                                      // Grade - Not required

        // Navigation properties
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }      

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }        
    }
}