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
    [Route("api/[controller]")]                                                                 // Defines the API route as "api/Enrolment"
    [ApiController]
    public class EnrolmentController : ControllerBase
    {
        private readonly StudentContext _context;                                               // Database context

        public EnrolmentController(StudentContext context)                                      // Constructor to inject the database context (Dependency Injection)
        {
            _context = context;
        }

        // GET: api/Enrolment
        // Returns all enrolments including StudentName and CourseTitle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEnrolments()
        {
            var enrolments = await _context.Enrolments                                          // Query the Enrolments table from the database
                .Include(e => e.Student)                                                        // Load Student entity for each enrolment
                .Include(e => e.Course)                                                         // Load Course entity for each enrolment
                .Select(e => new
                {
                    enrolmentId = e.EnrolmentId,                                                // Primary Key from Enrolment table
                    studentId = e.StudentId,                                                    // Foreign Key (Student)
                    courseId = e.CourseId,                                                      // Foreign Key (Course)
                    joiningDate = e.JoiningDate,                                                // Date when student joined the course
                    grade = e.Grade,                                                            // Grade

                    studentName = e.Student != null ? e.Student.Name : string.Empty,            // Safely read Student name → if null, use empty string
                    courseName = e.Course != null ? e.Course.Title : string.Empty               // Safely read Course title → if null, use empty string
                })
                .ToListAsync();                                                                 // Convert the query into a list asynchronously

            return Ok(enrolments);                                                              // Returns the result wrapped inside an HTTP Ok response
        }

        // GET: api/Enrolment/{id}
        // Returns a single enrolment by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);                            // Search enrolment by PK

            if (enrolment == null)
            {
                return NotFound();                                                              // 404 if not found
            }

            return enrolment;                                                                   // 200 OK if found
        }

        // PUT: api/Enrolment/{id}
        // Updates an existing enrolment
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrolment(int id, Enrolment enrolment)
        {
            if (id != enrolment.EnrolmentId)                                                    // Validate route ID == object ID
            {
                return BadRequest();                                                            // 400 Bad Request if mismatch
            }

            _context.Entry(enrolment).State = EntityState.Modified;                             // Mark entity as modified

            try
            {
                await _context.SaveChangesAsync();                                              // Save changes to DB
            }
            catch (DbUpdateConcurrencyException)                                                // Handle concurrency conflicts
            {
                if (!EnrolmentExists(id))                                                       // Check if enrolment exists
                {
                    return NotFound();                                                          // 404 if not found
                }
                else
                {
                    throw;                                                                      // Re-throw if unknown error
                }
            }

            return NoContent();                                                                 // 204 No Content on success
        }

        // POST: api/Enrolment
        // Creates a new enrolment
        [HttpPost]
        public async Task<ActionResult<Enrolment>> PostEnrolment(Enrolment enrolment)
        {
            _context.Enrolments.Add(enrolment);                                                // Add new enrolment to context
            await _context.SaveChangesAsync();                                                 // Save to DB

            // Returns 201 Created + URI of the new enrolment
            return CreatedAtAction("GetEnrolment", new { id = enrolment.EnrolmentId }, enrolment);
        }

        // DELETE: api/Enrolment/{id}
        // Deletes an enrolment by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);                           // Find enrolment by ID
            if (enrolment == null)
            {
                return NotFound();                                                             // 404 if not found
            }

            _context.Enrolments.Remove(enrolment);                                             // Remove from DB
            await _context.SaveChangesAsync();                                                 // Save changes

            return NoContent();                                                                // 204 No Content after deletion
        }

        // Checks if an enrolment exists by ID
        private bool EnrolmentExists(int id)
        {
            return _context.Enrolments.Any(e => e.EnrolmentId == id);                          // Returns true/false
        }
    }
}