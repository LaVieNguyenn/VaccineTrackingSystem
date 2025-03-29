using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.ChildService;

namespace VaccineTrackingSystem.ViewComponents
{
    public class ChildViewComponent : ViewComponent
    {
        private readonly IChildService _childService;

        public ChildViewComponent(IChildService childService)
        {
            _childService = childService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var children = await _childService.GetAllChildrenAsync();
            return View(children);
        }
    }
}
