using ClassBridge.Core.Entities;
using ClassBridge.Core.Enums;
using ClassBridge.Core.Interfaces;
using ClassBridge.Web.Services;
using ClassBridge.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassBridge.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly IParentService _parentService;
        private readonly ITeacherService _teacherService;

        public AccountController(
            AuthService authService,
            IParentService parentService,
            ITeacherService teacherService)
        {
            _authService = authService;
            _parentService = parentService;
            _teacherService = teacherService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.ValidateUserAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Ongeldige inloggegevens");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirect based on role
            return user.Role switch
            {
                UserRole.Teacher => RedirectToAction("Profile", "Teacher"),
                UserRole.Parent => RedirectToAction("Profile", "Parent"),
                UserRole.Admin => RedirectToAction("Index", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public IActionResult RegisterParent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterParent(RegisterParentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _authService.EmailExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Dit e-mailadres is al in gebruik");
                return View(model);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Role = UserRole.Parent
            };

            user = await _authService.RegisterUserAsync(user, model.Password);

            var parent = new Parent
            {
                UserId = user.Id,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                EmergencyContactName = model.EmergencyContactName,
                EmergencyContactPhone = model.EmergencyContactPhone
            };

            await _parentService.CreateParentAsync(parent);

            TempData["Success"] = "Registratie succesvol! U kunt nu inloggen.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult RegisterTeacher()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _authService.EmailExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Dit e-mailadres is al in gebruik");
                return View(model);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Role = UserRole.Teacher
            };

            user = await _authService.RegisterUserAsync(user, model.Password);

            var teacher = new Teacher
            {
                UserId = user.Id,
                Bio = model.Bio,
                Qualification = model.Qualification,
                YearsOfExperience = model.YearsOfExperience,
                HourlyRate = model.HourlyRate,
                ProfileImageUrl = model.ProfileImageUrl
            };

            teacher = await _teacherService.CreateTeacherAsync(teacher);

            // Add strengths
            if (model.Strengths != null && model.Strengths.Any())
            {
                foreach (var strength in model.Strengths)
                {
                    await _teacherService.AddTeacherStrengthAsync(new TeacherStrength
                    {
                        TeacherId = teacher.Id,
                        Subject = strength.Subject,
                        ProficiencyLevel = strength.ProficiencyLevel,
                        Description = strength.Description
                    });
                }
            }

            TempData["Success"] = "Registratie succesvol! U kunt nu inloggen.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}