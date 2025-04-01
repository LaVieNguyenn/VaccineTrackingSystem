using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VaccineTrakingSystem.BLL.Services.UserService;
using VaccineTrakingSystem.BLL.DTOs;
using Microsoft.AspNetCore.Authorization;
using VaccineTrakingSystem.BLL.CustomerService;
using VaccineTrackingSystem.Models;

namespace VaccineTrackingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _service;
        private readonly ICustomerService _customerService;


        public AuthenticationController(IUserService userService, ICustomerService customerService)
        {
            _service = userService;
            _customerService = customerService;
        }

        [HttpGet]
        public ActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CustomerRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = await _customerService.RegisterCustomerAsync(model);
            return RedirectToAction("RegisterSuccess");
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _service.AuthenticateAsync(model.PhoneNumber, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid PhoneNumber or Password");
                    return View(model);
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else return Redirect("/");
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
