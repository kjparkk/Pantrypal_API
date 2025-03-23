// These are the tools we're using in this file
using System.Text.RegularExpressions;              // To check if an email looks correct
using BCrypt.Net;                                 // To hash and check passwords securely
using Microsoft.AspNetCore.Mvc;                   // To create API endpoints (like register and login)
using login_system_2030.Data;                         // To connect to the database
using login_system_2030.Models;                       // Our User model (class)
using login_system_2030.DTOs;                         // The objects we use to receive data (DTOs)

namespace login_system_2030.Controllers
{
    // This tells the system it's a web API controller and that it will respond to requests at "api/account"
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        // This is our link to the database
        private readonly AppDbContext _context;

        // This runs when the controller is created, giving us access to the database
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // This is the endpoint to register a new user
        [HttpPost("register")]

        public IActionResult Register(RegisterDto dto)
        {
            try
            {
                // Make sure none of the fields are empty
                if (string.IsNullOrWhiteSpace(dto.Username) ||
                    string.IsNullOrWhiteSpace(dto.Email) ||
                    string.IsNullOrWhiteSpace(dto.Password))
                {
                    return BadRequest("All fields (username, email, password) are required.");
                }

                // Check if the email format is valid (e.g., something@domain.com)
                bool isValidEmail = Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!isValidEmail)
                {
                    return BadRequest("Invalid email format.");
                }

                // Check if the username is already taken
                if (_context.Users.Any(u => u.Username == dto.Username))
                    return Conflict("Username already exists.");

                // Check if the email is already used
                if (_context.Users.Any(u => u.Email == dto.Email))
                    return Conflict("Email already exists.");

                // Turn the password into a hashed (scrambled and secure) version
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                // Create a new user using the info from the form
                var user = new User(dto.Username, dto.Email, hashedPassword);

                // Add the user to the database
                _context.Users.Add(user);
                _context.SaveChanges(); // Save changes to the actual database

                // Tell the frontend everything went well
                return Ok("Registration successful!");
            }
            catch (Exception ex)
            {
                // If something goes wrong, send an error message
                return StatusCode(500, $"Something went wrong while registering: {ex.Message}");
            }
        }

        // This is the endpoint to log in a user
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            try
            {
                // Make sure both username and password are filled in
                if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    return BadRequest("Username and password are required.");
                }

                // Find the user with the matching username in the database
                var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);

                // If no user was found, send an error
                if (user == null)
                    return NotFound("User not found.");

                // Check if the password they entered matches the hashed password in the database
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    return Unauthorized("Incorrect password.");

                // If everything is correct, let them log in
                return Ok("Login successful!");
            }
            catch (Exception ex)
            {
                // If something goes wrong, send an error message
                return StatusCode(500, $"Something went wrong while logging in: {ex.Message}");
            }

        }

        [HttpGet("test")]
        public IActionResult Test() => Ok("API is live!");

    }
}