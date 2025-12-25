using ClassBridge.API.Data;
using ClassBridge.Core.DTOs;
using ClassBridge.Core.Entities;
using ClassBridge.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ClassBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginDTO loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Parent)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

            if (user == null)
            {
                return Unauthorized(new { message = "Ongeldige inloggegevens" });
            }

            var passwordHash = HashPassword(loginDto.Password);
            if (user.PasswordHash != passwordHash)
            {
                return Unauthorized(new { message = "Ongeldige inloggegevens" });
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new LoginResponseDTO
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                TeacherId = user.Teacher?.Id,
                ParentId = user.Parent?.Id
            };
        }

        // POST: api/Auth/RegisterParent
        [HttpPost("RegisterParent")]
        public async Task<ActionResult<User>> RegisterParent(RegisterParentDTO dto)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Dit e-mailadres is al in gebruik" });
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = HashPassword(dto.Password),
                Role = UserRole.Parent,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var parent = new Parent
            {
                UserId = user.Id,
                Address = dto.Address,
                City = dto.City,
                PostalCode = dto.PostalCode,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactPhone = dto.EmergencyContactPhone,
                CreatedAt = DateTime.UtcNow
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // POST: api/Auth/RegisterTeacher
        [HttpPost("RegisterTeacher")]
        public async Task<ActionResult<User>> RegisterTeacher(RegisterTeacherDTO dto)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Dit e-mailadres is al in gebruik" });
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = HashPassword(dto.Password),
                Role = UserRole.Teacher,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var teacher = new Teacher
            {
                UserId = user.Id,
                Bio = dto.Bio,
                Qualification = dto.Qualification,
                YearsOfExperience = dto.YearsOfExperience,
                HourlyRate = dto.HourlyRate,
                ProfileImageUrl = dto.ProfileImageUrl,
                IsAvailableForMeetings = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            // Add strengths
            if (dto.Strengths != null && dto.Strengths.Any())
            {
                foreach (var strengthDto in dto.Strengths)
                {
                    var strength = new TeacherStrength
                    {
                        TeacherId = teacher.Id,
                        Subject = strengthDto.Subject,
                        ProficiencyLevel = strengthDto.ProficiencyLevel,
                        Description = strengthDto.Description,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.TeacherStrengths.Add(strength);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // GET: api/Auth/User/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Parent)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Auth/CheckEmail?email=test@test.nl
        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }

    // DTO voor login response
    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? TeacherId { get; set; }
        public int? ParentId { get; set; }
    }

    // DTO voor registratie
    public class RegisterParentDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }

    public class RegisterTeacherDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public decimal HourlyRate { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<TeacherStrengthDTO>? Strengths { get; set; }
    }
}