using Microsoft.AspNetCore.Mvc;
using VaccineTrakingSystem.BLL.UsersService;

namespace VaccineTrackingSystem.ViewComponents
{
    public class UsersViewComponent : ViewComponent
    {
        private readonly IUserService _usersService;

        public UsersViewComponent(IUserService usersService)
        {
            _usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var service = await _usersService.GetAllUsersAsync();
            return View(service);
        }
    }
}
