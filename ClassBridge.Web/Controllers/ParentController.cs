using ClassBridge.Core.Entities;
using ClassBridge.Core.Interfaces;
using ClassBridge.Web.Services;
using ClassBridge.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassBridge.Web.Controllers
{
    [Authorize(Roles = "Parent")]
    public class ParentController : Controller
    {
        private readonly IParentService _parentService;
        private readonly ITeacherService _teacherService;
        private readonly AuthService _authService;

        public ParentController(
            IParentService parentService,
            ITeacherService teacherService,
            AuthService authService)
        {
            _parentService = parentService;
            _teacherService = teacherService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var parent = await _parentService.GetParentByUserIdAsync(userId);

            if (parent == null)
            {
                return NotFound();
            }

            var viewModel = new ParentProfileViewModel
            {
                Id = parent.Id,
                FirstName = parent.User.FirstName,
                LastName = parent.User.LastName,
                Email = parent.User.Email,
                PhoneNumber = parent.User.PhoneNumber,
                Address = parent.Address,
                City = parent.City,
                PostalCode = parent.PostalCode,
                EmergencyContactName = parent.EmergencyContactName,
                EmergencyContactPhone = parent.EmergencyContactPhone,
                Children = parent.Children.Select(c => new ChildViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DateOfBirth = c.DateOfBirth,
                    Grade = c.Grade,
                    SchoolName = c.SchoolName,
                    SpecialNeeds = c.SpecialNeeds,
                    Age = c.Age
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddChild()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChild(ChildViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var parent = await _parentService.GetParentByUserIdAsync(userId);

            if (parent == null)
            {
                return NotFound();
            }

            var child = new Child
            {
                ParentId = parent.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Grade = model.Grade,
                SchoolName = model.SchoolName,
                SpecialNeeds = model.SpecialNeeds
            };

            await _parentService.AddChildAsync(child);

            TempData["Success"] = "Kind succesvol toegevoegd";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveChild(int id)
        {
            await _parentService.RemoveChildAsync(id);
            TempData["Success"] = "Kind verwijderd";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> SearchTeachers()
        {
            var teachers = await _teacherService.GetAvailableTeachersAsync();

            var viewModels = teachers.Select(t => new TeacherProfileViewModel
            {
                Id = t.Id,
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                Bio = t.Bio,
                Qualification = t.Qualification,
                YearsOfExperience = t.YearsOfExperience,
                HourlyRate = t.HourlyRate,
                ProfileImageUrl = t.ProfileImageUrl,
                IsAvailableForMeetings = t.IsAvailableForMeetings,
                Strengths = t.Strengths.Select(s => new TeacherStrengthViewModel
                {
                    Subject = s.Subject,
                    ProficiencyLevel = s.ProficiencyLevel,
                    Description = s.Description
                }).ToList()
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> TeacherDetails(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);

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
    }
}