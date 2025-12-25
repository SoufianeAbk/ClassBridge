using ClassBridge.Core.Entities;
using ClassBridge.Core.Enums;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClassBridge.MAUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5192/api/";

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        #region Teachers

        public async Task<List<Teacher>> GetTeachersAsync()
        {
            try
            {
                var teachers = await _httpClient.GetFromJsonAsync<List<Teacher>>("teachers");
                return teachers ?? new List<Teacher>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting teachers: {ex.Message}");
                return new List<Teacher>();
            }
        }

        public async Task<List<Teacher>> GetAvailableTeachersAsync()
        {
            try
            {
                var teachers = await _httpClient.GetFromJsonAsync<List<Teacher>>("teachers/available");
                return teachers ?? new List<Teacher>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting available teachers: {ex.Message}");
                return new List<Teacher>();
            }
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Teacher>($"teachers/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting teacher: {ex.Message}");
                return null;
            }
        }

        public async Task<Teacher?> GetTeacherByUserIdAsync(int userId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Teacher>($"teachers/user/{userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting teacher by user id: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Teacher>> SearchTeachersBySubjectAsync(Subject subject)
        {
            try
            {
                var teachers = await _httpClient.GetFromJsonAsync<List<Teacher>>($"teachers/search/subject?subject={subject}");
                return teachers ?? new List<Teacher>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching teachers by subject: {ex.Message}");
                return new List<Teacher>();
            }
        }

        public async Task<List<Teacher>> SearchTeachersByNameAsync(string name)
        {
            try
            {
                var teachers = await _httpClient.GetFromJsonAsync<List<Teacher>>($"teachers/search/name?name={name}");
                return teachers ?? new List<Teacher>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching teachers by name: {ex.Message}");
                return new List<Teacher>();
            }
        }

        public async Task<bool> ToggleTeacherAvailabilityAsync(int teacherId)
        {
            try
            {
                var response = await _httpClient.PutAsync($"teachers/{teacherId}/toggleavailability", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling teacher availability: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Parents

        public async Task<List<Parent>> GetParentsAsync()
        {
            try
            {
                var parents = await _httpClient.GetFromJsonAsync<List<Parent>>("parents");
                return parents ?? new List<Parent>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting parents: {ex.Message}");
                return new List<Parent>();
            }
        }

        public async Task<Parent?> GetParentByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Parent>($"parents/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting parent: {ex.Message}");
                return null;
            }
        }

        public async Task<Parent?> GetParentByUserIdAsync(int userId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Parent>($"parents/user/{userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting parent by user id: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Children

        public async Task<List<Child>> GetChildrenByParentAsync(int parentId)
        {
            try
            {
                var children = await _httpClient.GetFromJsonAsync<List<Child>>($"children/parent/{parentId}");
                return children ?? new List<Child>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting children: {ex.Message}");
                return new List<Child>();
            }
        }

        public async Task<Child?> GetChildByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Child>($"children/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting child: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddChildAsync(Child child)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("children", child);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding child: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteChildAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"children/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting child: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region TeacherAvailabilities

        public async Task<List<TeacherAvailability>> GetTeacherAvailabilitiesAsync(int teacherId)
        {
            try
            {
                var availabilities = await _httpClient.GetFromJsonAsync<List<TeacherAvailability>>($"teacheravailabilities/teacher/{teacherId}");
                return availabilities ?? new List<TeacherAvailability>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting teacher availabilities: {ex.Message}");
                return new List<TeacherAvailability>();
            }
        }

        public async Task<List<TeacherAvailability>> GetAvailableSlotsAsync(int teacherId)
        {
            try
            {
                var availabilities = await _httpClient.GetFromJsonAsync<List<TeacherAvailability>>($"teacheravailabilities/teacher/{teacherId}/available");
                return availabilities ?? new List<TeacherAvailability>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting available slots: {ex.Message}");
                return new List<TeacherAvailability>();
            }
        }

        public async Task<bool> AddAvailabilityAsync(TeacherAvailability availability)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("teacheravailabilities", availability);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding availability: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> BookAvailabilityAsync(int availabilityId)
        {
            try
            {
                var response = await _httpClient.PutAsync($"teacheravailabilities/{availabilityId}/book", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error booking availability: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAvailabilityAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"teacheravailabilities/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting availability: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region TeacherStrengths

        public async Task<List<TeacherStrength>> GetTeacherStrengthsAsync(int teacherId)
        {
            try
            {
                var strengths = await _httpClient.GetFromJsonAsync<List<TeacherStrength>>($"teacherstrengths/teacher/{teacherId}");
                return strengths ?? new List<TeacherStrength>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting teacher strengths: {ex.Message}");
                return new List<TeacherStrength>();
            }
        }

        public async Task<bool> AddTeacherStrengthAsync(TeacherStrength strength)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("teacherstrengths", strength);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding teacher strength: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTeacherStrengthAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"teacherstrengths/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting teacher strength: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Auth

        public async Task<LoginResponse?> LoginAsync(string email, string password)
        {
            try
            {
                var loginDto = new { Email = email, Password = password };
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging in: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterParentAsync(RegisterParentRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/registerparent", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering parent: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegisterTeacherAsync(RegisterTeacherRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/registerteacher", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering teacher: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            try
            {
                var exists = await _httpClient.GetFromJsonAsync<bool>($"auth/checkemail?email={email}");
                return exists;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking email: {ex.Message}");
                return false;
            }
        }

        #endregion
    }

    #region DTOs

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? TeacherId { get; set; }
        public int? ParentId { get; set; }
    }

    public class RegisterParentRequest
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

    public class RegisterTeacherRequest
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
    }

    #endregion
}