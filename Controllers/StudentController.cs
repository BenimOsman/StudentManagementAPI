using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIStudent.Models;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]                                                                     // Route will be: api/student
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentContext _context;

        public StudentController(StudentContext context)                                            // Dependency Injection - DbContext is injected here
        {
            _context = context;
        }

        // GET: api/Student
        // Fetch all students from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Student/id
        // Fetch a single student by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();                                                                  // returns 404 if student not found
            }

            return student;                                                                         // returns 200 OK with student data
        }

        // PUT: api/Student/id
        // Update an existing student
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentId)                                                            // Validate that the studentId in URL matches studentId in body
            {
                return BadRequest();                                                                // 400 Bad Request
            }

            _context.Entry(student).State = EntityState.Modified;                                   // Mark student as modified

            try
            {
                await _context.SaveChangesAsync();                                                  // Save changes to DB
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))                                                             // If student doesn't exist, return 404
                {
                    return NotFound();
                } else {
                    throw;                                                                          // If another error, rethrow it
                }
            }
            return NoContent();                                                                     // 204 - Success, but no response body
        }

        // POST: api/Student
        // Create a new student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);                                                         // Add new student
            await _context.SaveChangesAsync();                                                      // Save changes

            return CreatedAtAction("GetStudent", new { id = student.StudentId }, student);          // Return 201 Created with location of the new student
        }

        // DELETE: api/Student/id
        // Delete a student by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();                                                                  // 404 if student not found
            }

            _context.Students.Remove(student);                                                      // Remove student
            await _context.SaveChangesAsync();                                                      // Save changes

            return NoContent(); 
        }

        private bool StudentExists(int id)                                                          // Helper method to check if a student exists
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}