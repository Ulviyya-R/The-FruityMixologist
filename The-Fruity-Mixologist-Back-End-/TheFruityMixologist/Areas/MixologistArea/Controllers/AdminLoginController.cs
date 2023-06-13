using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheFruityMixologist.Entities;
using TheFruityMixologist.Utilities.Enum;
using TheFruityMixologist.Utilities.Extensions;
using TheFruityMixologist.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TheFruityMixologist.Areas.MixologistArea.Controllers
{
    [Area("MixologistArea")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminLoginController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public AdminLoginController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            
            if (!ModelState.IsValid) return View();
            User user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View();
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "User is blocked");
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(RoleType.SuperAdmin.ToString()) || roles.Contains(RoleType.Admin.ToString()))
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Due to overthing your account has been blocked for 5 minutes");
                        return View();
                    }
                    ModelState.AddModelError("", "Email or password is incorrect");
                    return View();
                }
            }

              
            return RedirectToAction("Dashboard", "Dashboard");

        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registeraccount)
        {
          
            if (!ModelState.IsValid) return View();
            if (!registeraccount.Terms)
            {
                ModelState.AddModelError("Terms", "Please click Terms button");
                return View();
            }
            if (!registeraccount.ProfilePicture.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Please choose image file");
                return View();
            }
            if (!registeraccount.ProfilePicture.IsValidLength(5))
            {
                ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 1MB");
                return View();
            }
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");

            string otp = "12345";
            User user = new User
            {
                UserName = registeraccount.Username,
                Fullname = String.Concat(registeraccount.Firstname, "", registeraccount.Lastname),
                Email = registeraccount.Email,
                Path = await registeraccount.ProfilePicture.CreateImage(imageFolderPath, "myAccount"),
                EmailConfirmed = true,
                OTP= otp,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registeraccount.Password);

            await _userManager.AddToRoleAsync(user, RoleType.Admin.ToString());

            if (!result.Succeeded)
            {
                foreach (IdentityError message in result.Errors)
                {
                    ModelState.AddModelError("", message.Description);
                }
                return View();
            }

            return RedirectToAction("Dashboard","Dashboard");
        }

    }
}
