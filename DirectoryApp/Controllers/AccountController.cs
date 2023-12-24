using AutoMapper;
using DirectoryApp.Models;
using DirectoryApp.Models.Identity;
using DirectoryApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;

namespace DirectoryApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            CheckRoles();
        }
        private void CheckRoles()
        {
            foreach (var roleName in RoleNames.Roles)
            {
                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    var result = _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = roleName,
                    }).Result;
                }
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                registerViewModel.Password = string.Empty;
                registerViewModel.ConfirmPassword = string.Empty;
                return View(registerViewModel);
            }
            var data = registerViewModel;

            var user = await _userManager.FindByNameAsync(registerViewModel.UserName);
            if (user != null)
            {
                ModelState.AddModelError(nameof(registerViewModel.UserName), "Bu kullanıcı adı daha önce sisteme kayıt edilmiştir");
                return View(registerViewModel);
            }
            var email = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (email != null)
            {
                ModelState.AddModelError(nameof(registerViewModel.Email), "Bu email daha önce sisteme kayıt edilmiştir");
                return View(registerViewModel);
            }
            user = new ApplicationUser()
            {
                UserName = data.UserName,
                Email = data.Email,
                SurName = data.Surname,
                Name = data.Name,
            };
            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                //kullanıcıya rol atama
                var count = _userManager.Users.Count();
                result = await _userManager.AddToRoleAsync(user, count == 1 ? RoleNames.Admin : RoleNames.User);
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage)));
                return View(nameof(Login), data);
            }

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Method = "login";
                return View(nameof(Login), loginViewModel);
            }
            var data = loginViewModel;
            var result = await _signInManager.PasswordSignInAsync(data.UserName, data.Password, data.RememberMe, true);
            var user = await _userManager.FindByNameAsync(data.UserName);
            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, RoleNames.User))
                {
                    return RedirectToAction("Index", "Home");

                }
                else if (await _userManager.IsInRoleAsync(user, RoleNames.Passive))
                {
                    return RedirectToAction("Register", "Account");
                }
                else if (await _userManager.IsInRoleAsync(user, RoleNames.Admin))
                {
                    return RedirectToAction("Index", "Home");
                }
                return BadRequest();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                ViewBag.Method = "login";
                return View(nameof(Login), loginViewModel);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}
