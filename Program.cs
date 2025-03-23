using login_system_2030.Data; // Brings in access to AppDbContext (database connection class)
using Microsoft.EntityFrameworkCore; // Brings in Entity Framework Core for database operations

// Create a builder object that helps configure web app
var builder = WebApplication.CreateBuilder(args);

// Configure services (things the app will use)

// Add database context (AppDbContext) and tell it to use SQL Server
// It reads the connection string from appsettings.json under "DefaultConnection"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add support for Controllers (API endpoints like login/register)
builder.Services.AddControllers();

// Allow the app to accept requests from any website (important for frontend → backend communication)
builder.Services.AddCors(); // Enables Cross-Origin Resource Sharing (CORS) — needed for frontend calls

// Build the app using the builder configuration
var app = builder.Build();

// this is the pipeline that handles every request to the API

// Allow requests from any origin, any header, and any HTTP method (GET, POST, etc.)
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

// This allows app to use authorization features 
app.UseAuthorization();

// This maps all controller routes (like /api/account/login) to the correct controller methods
app.MapControllers();

// Run the application and start listening for HTTP requests
app.Run();