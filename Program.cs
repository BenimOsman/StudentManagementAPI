using Microsoft.EntityFrameworkCore;                                                        // EF Core for database operations
using WebAPIStudent.Models;                                                                 // Import your Models & DbContext

var builder = WebApplication.CreateBuilder(args);                                           // Create builder for web app

// Register DbContext with SQL Server
// StudentContext will be injected wherever needed (Dependency Injection)
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDBConnection")));

builder.Services.AddControllers();                                                          // Add controllers to the service container (MVC/Web API support)

builder.Services.AddEndpointsApiExplorer();                                                 // Enable API documentation & testing via Swagger/OpenAPI
builder.Services.AddSwaggerGen();

var app = builder.Build();                                                                  // Build the application

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                                                                       // Generate Swagger JSON
    app.UseSwaggerUI();                                                                     // Provide Swagger UI
}

app.UseHttpsRedirection();                                                                  // Redirect HTTP requests to HTTPS
app.UseAuthorization();                                                                     // Add Authorization middleware (no auth yet, placeholder)
app.MapControllers();                                                                       // Map controller routes to endpoints
app.Run();                                                                                  // Run the application