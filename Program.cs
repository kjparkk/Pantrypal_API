using login_system_2030.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to container (DB Context, Controllers)
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LoginRegisterDb;Trusted_Connection=True;"));

builder.Services.AddControllers();

// Build the app
var app = builder.Build();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();
app.MapControllers();

app.Run();