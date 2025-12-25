using ClassBridge.Core.Entities;
using ClassBridge.Core.Interfaces;
using ClassBridge.Web.Services;
using ClassBridge.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassBridge.Web.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly AuthService _authService;

        public TeacherController(ITeacherService teacherService, AuthService authService)
        {
            _teacherService = teacherService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);

            if (teacher == null)
            {
                return NotFound();
            }

            var viewModel = new TeacherProfileViewModel
            {
                Id = teacher.Id,
                FirstName = teacher.User.FirstName,
                LastName = teacher.User.LastName,
                Email = teacher.User.Email,
                PhoneNumber = teacher.User.PhoneNumber,
                Bio = teacher.Bio,
                Qualification = teacher.Qualification,
                YearsOfExperience = teacher.YearsOfExperience,
                HourlyRate = teacher.HourlyRate,
                ProfileImageUrl = teacher.ProfileImageUrl,
                IsAvailableForMeetings = teacher.IsAvailableForMeetings,
                Strengths = teacher.Strengths.Select(s => new TeacherStrengthViewModel
                {
                    Subject = s.Subject,
                    ProficiencyLevel = s.ProficiencyLevel,
                    Description = s.Description
                }).ToList(),
                Availabilities = teacher.Availabilities.Select(a => new AvailabilityViewModel
                {
                    Id = a.Id,
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    IsBooked = a.IsBooked
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public Task<IActionResult> AddAvailability()
        {
            return Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailability(AvailabilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);

            if (teacher == null)
            {
                return NotFound();
            }

            var availability = new TeacherAvailability
            {
                TeacherId = teacher.Id,
                DayOfWeek = model.DayOfWeek,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };

            await _teacherService.AddAvailabilityAsync(availability);

            TempData["Success"] = "Beschikbaarheid toegevoegd";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAvailability(int id)
        {
            await _teacherService.RemoveAvailabilityAsync(id);
            TempData["Success"] = "Beschikbaarheid verwijderd";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAvailability()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);

            if (teacher == null)
            {
                return NotFound();
            }

            teacher.IsAvailableForMeetings = !teacher.IsAvailableForMeetings;
            await _teacherService.UpdateTeacherAsync(teacher);

            TempData["Success"] = teacher.IsAvailableForMeetings
                ? "U bent nu beschikbaar voor afspraken"
                : "U bent nu niet beschikbaar voor afspraken";

            return RedirectToAction("Profile");
        }
    }
}