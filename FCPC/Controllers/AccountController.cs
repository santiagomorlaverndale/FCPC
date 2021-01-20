using System.Threading.Tasks;
using FCPC.CustomProvider;
using FCPC.Models;
using FCPC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FCPC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user != null)
            {
                var loggedUser = new ApplicationUser
                {
                    UserName = user.UserId,
                    Email = user.EmailAddress
                };
                _signInManager.SignInAsync(loggedUser, true);
            }
            else
            {
                ModelState.AddModelError( nameof(model.Password), "Usuario no encontrado!");
            }

            return View();
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
