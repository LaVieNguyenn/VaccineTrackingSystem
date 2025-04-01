using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.ServicesService;
using VaccineTrakingSystem.BLL.UsersService;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrackingSystem.Controllers
{
    [Authorize(Roles = "4")]
    public class UsersController : Controller
    {
        private readonly IUsersService _services;
        public UsersController(IUsersService service)
        {
            _services = service;
        }
        public async Task<IActionResult> Index()
        {
            var Services = await _services.GetAllUsersAsync();
            return View(Services);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _services.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found" });
            }
            return Json(user);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { success = false, message = "Invalid user data" });
            }

            user.UpdatedAt = DateTime.Now;
            var isUpdated = await _services.UpdateUserAsync(user);

            if (isUpdated)
            {
                return Json(new { success = true, message = "User updated successfully" });
            }
            return Json(new { success = false, message = "Failed to update user" });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            Console.WriteLine($"Deleting user with ID: {id}"); 

            var user = await _services.GetUserByIdAsync(id);
            if (user == null)
            {
                Console.WriteLine("User not found");
                return NotFound(new { success = false, message = "User not found" });
            }

            var isDeleted = await _services.DeleteUserAsync(id);
            if (isDeleted)
            {
                Console.WriteLine("User deleted successfully");
                return Json(new { success = true, message = "User deleted successfully" });
            }

            Console.WriteLine("Failed to delete user");
            return Json(new { success = false, message = "Failed to delete user" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            //if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            //{
            //    return BadRequest(new { success = false, message = "Invalid user data" });
            //}

            try
            {
                int newUserId = await _services.CreateUsersAsync(user);
                if (newUserId > 0)
                {
                    return Json(new { success = true, message = "User created successfully", userId = newUserId });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            return Json(new { success = false, message = "Failed to create user" });
        }



    }
}
