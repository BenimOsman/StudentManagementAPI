using System.ComponentModel.DataAnnotations;                                                    // Provides attributes for data validation
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;                                             // Provides attributes like [Table] for database mapping

namespace WebAPIStudent.Models
{
    [Table("Student")]                                                                          // Maps this class to the 'Student' table in the database
    public class Student
    {
        [Key]
        public int StudentId { get; set; }                                                      // Marked as primary key

        [Required]
        public string Name { get; set; }                                                        // Name can't be null

        public int Age { get; set; }                                                            // Age - Not required

    }
}

// [Table("Student")] - Ensures that the class is explicitly mapped to the database table, even if the class name changes

// e.g. - string? name = null;                                                                  // Explicitly saying this can be 'NULL'