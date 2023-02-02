using Business.Interfaces;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using WebUI.ViewModels;
using static WebUI.Utilities.Extensions;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        public readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailService = mailService;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            AppUser appUser = new()
            {
                UserName = registerViewModel.Usrename,
                Fullname = registerViewModel.Fullname,
                Email = registerViewModel.Email,

            };
            var identityResult = await _userManager.CreateAsync(appUser, registerViewModel.Password);
            if (!identityResult.Succeeded)
            {
                string rslt = string.Empty;
                foreach (var error in identityResult.Errors)
                {
                    rslt += error.Description + ",";
                }
                ModelState.AddModelError("Password", rslt);
                return View(registerViewModel);
            }
            await _userManager.AddToRoleAsync(appUser, RoleType.Moderator.ToString());
            return Json(registerViewModel);
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var user = await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginViewModel.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("UsernameOrEmail", "Password or Username is incorrect");
                    return View(loginViewModel);
                }
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.rememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "many false tries");
                return View(loginViewModel);
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("UsernameOrEmail", "Password or Username is incorrect");
                return View(loginViewModel);
            }
            if (user.IsActive)
            {
                ModelState.AddModelError("", "not found");
                return View(loginViewModel);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRoles()
        {
            foreach (var type in Enum.GetValues(typeof(RoleType)))
            {
                if (!await _roleManager.RoleExistsAsync(type.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(type.ToString()));
                }
            }
            return Json("Ok");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email not found!");
                return View(forgotPasswordViewModel);
            }
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordViewModel);
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var test = new { userId = user.Id, token = token };
            string? link = Url.Action("ResetPassword", "Auth", test, Request.Scheme);
            await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = user.Email, Subject = "Reset password", Body = $"<a href={link}>click me</a>" });

            TempData["Message"] = " please check you gmail account .";

            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel, string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            var identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.NewPassword);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordViewModel);
            }
            return RedirectToAction(nameof(Login));
        }
    }
}
