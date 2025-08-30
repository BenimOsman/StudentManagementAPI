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
    [Route("api/[controller]")]                                                     // Defines the API route as "api/Course"
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly StudentContext _context;

        public CourseController(StudentContext context)                             // Constructor to inject the database context (Dependency Injection)
        {
            _context = context;
        }

        // GET: api/Course
        // Returns all courses from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Course/id
        // Returns a single course by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();                                                  // Returns 404 if course doesn't exist
            }

            return course;                                                          // Returns course if found
        }

        // PUT: api/Course/id
        // Updates an existing course
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)                                              // Check if route ID matches object ID
            {
                return BadRequest();                                                // 400 Bad Request if mismatch
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();                                  // Save changes to DB
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))                                              // Check if course exists
                {
                    return NotFound();                                              // 404 if not found
                }
                else
                {
                    throw;                                                          // Re-throw exception for other issues
                }
            }

            return NoContent();                                                     // 204 No Content after successful update
        }

        // POST: api/Course
        // Creates a new course
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Course/id
        // Deletes a course by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();                                                  // 404 if course doesn't exist
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();                                                     // 204 after deletion
        }

        // Checks if a course exists by ID
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}