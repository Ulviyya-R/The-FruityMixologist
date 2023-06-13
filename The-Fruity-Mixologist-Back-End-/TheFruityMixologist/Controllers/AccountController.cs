using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TheFruityMixologist.Utilities.Extensions;

using NuGet.Common;
using System.Data;
using System.Net;
using System.Net.Mail;
using TheFruityMixologist.Entities;
using TheFruityMixologist.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using static System.Net.WebRequestMethods;
using TheFruityMixologist.Utilities.Enum;
using TheFruityMixologist.Services.Interfaces;

namespace TheFruityMixologist.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
            _emailService = emailService;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registeraccount)
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();
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

           

            string otp = GenerateOTP(); 
            User user = new User
            {
                UserName = registeraccount.Username,
                OTP = otp,
				Fullname = String.Concat(registeraccount.Firstname, "", registeraccount.Lastname),
                Email = registeraccount.Email,
                Path = await registeraccount.ProfilePicture.CreateImage(imageFolderPath, "myAccount")
            };


            IdentityResult result = await _userManager.CreateAsync(user,registeraccount.Password);
           
            await _userManager.AddToRoleAsync(user, RoleType.Member.ToString());

            // E-post send
            string templatePath = Path.Combine("wwwroot/assets/template/EmailTemplate.html");
            string emailTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

            string subject = "Confirm Email";
            string body = emailTemplate.Replace("{SUBJECT}", subject).Replace("{otp}", otp);

            _emailService.Send(user.Email, subject, body);  
            
            
            if (!result.Succeeded)
            {
                foreach (IdentityError message in result.Errors)
                {
                    ModelState.AddModelError("", message.Description);
                }
                return View();
            }

            return RedirectToAction("CheckMessage", "Account", new { email = user.Email });
        }

        private static string GenerateOTP()
        {
            Random random = new Random();
            int otpNumber = random.Next(100000, 999999);
            return otpNumber.ToString();
        }

     
        [HttpGet]
        public async Task<IActionResult> CheckMessage(string email)
        {
            ValidateVM validate = new ValidateVM
            {
                Email = email
            };
            return View(validate);
        }

        [HttpPost]
        public async Task<IActionResult> ResultOpt(ValidateVM validate)
        {

            if (string.IsNullOrEmpty(validate.Opt) || string.IsNullOrEmpty(validate.Opt))
            {
                return RedirectToAction(nameof(CheckMessage));
            }

            User user = await _userManager.FindByEmailAsync(validate.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email.");
                return View();
            }

            if (user.OTP != validate.Opt)
            {
                return RedirectToAction(nameof(CheckMessage), new { email = validate.Email });
            }
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, isPersistent: false);
            

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public  IActionResult ForgotPassword(string email)
        {
            MyAccountVM account = new MyAccountVM
            {
                Email = email
            };
            return View(account);
        }


        [HttpPost]
        public async Task<IActionResult> Passowrd(MyAccountVM account)
        {
            User user = await _userManager.FindByEmailAsync(account.Email);

            if (user == null) BadRequest("User Not Found");

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
             string templatePath = Path.Combine("wwwroot/assets/template/forgotPassword.html");
            string emailTemplate = await System.IO.File.ReadAllTextAsync(templatePath);
           

            string subject = "Reset Password";

            string link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            string body = emailTemplate.Replace("{SUBJECT}", subject).Replace("{link}", link);

            _emailService.Send(user.Email, subject, body);


            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult>  ResetPassword([FromQuery] string email,string token)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null) BadRequest();

            MyAccountVM account = new MyAccountVM
            {
                Email = email,
                Token = token
            };
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(MyAccountVM account)
        {
            User user = await _userManager.FindByEmailAsync(account.Email);
            if(user is null) return BadRequest();

            MyAccountVM accountModel = new()
            {
                User = user,
                Password = user.PasswordHash,
                Token = account.Token
            };

            if (await _userManager.CheckPasswordAsync(user, account.Password))
            {
                ModelState.AddModelError("", "This password already exist!");

                return View(account);
            }

            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
            }
            var result = await _userManager.ResetPasswordAsync(user, account.Token, account.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(account);
            }
        }





        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();
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
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<ActionResult> MyAccount()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }
            UserVM userVM = new()
            {
                Email = user.Email,
                Username = user.UserName,
                Fullname = user.Fullname,
                Path = user.Path
                
            };
            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }
            UserVM userVM = new()
            {
                Email = user.Email,
                Username = user.UserName,
                Fullname = user.Fullname,
                Path = user.Path
            };

            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Settings(UserVM editedUserVM)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");

            if (editedUserVM.ProfilePicture is not null)
            {
                if (!editedUserVM.ProfilePicture.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Please choose an image file");
                    return View();
                }

                if (!editedUserVM.ProfilePicture.IsValidLength(5))
                {
                    ModelState.AddModelError(string.Empty, "Please choose an image with a maximum size of 2MB");
                    return View();
                }
                if (!string.IsNullOrEmpty(user.Path))
                {
                    string imagePath = Path.Combine(imageFolderPath, user.Path);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                string newImageFileName = await editedUserVM.ProfilePicture.CreateImage(imageFolderPath, "myAccount");
                user.Path = newImageFileName;
            }
            user.UserName = editedUserVM.Username;
            user.Fullname = editedUserVM.Fullname;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(editedUserVM);
            }
            
        }


        ////public async Task<IActionResult> CreateRoles()
        ////{
        ////    foreach (var roleType in Enum.GetValues(typeof(RoleType)))
        ////    {
        ////        if (!await _roleManager.RoleExistsAsync(roleType.ToString()))
        ////            await _roleManager.CreateAsync(new IdentityRole { Name = roleType.ToString() });
        ////    }

        ////    return RedirectToAction("Index", "Home");
        ////}



        private async Task AdjustUserPhoto(bool? Ismain, IFormFile image, User user)
        {
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "photos");
            string filePath = Path.Combine(imageFolderPath, "myAccount", user.Path);
            FileUpload.DeleteImage(filePath);
            user.Path = await image.CreateImage(imageFolderPath, "myAccount");
        }

    }
}
