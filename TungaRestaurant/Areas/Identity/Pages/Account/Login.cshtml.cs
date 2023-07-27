using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TungaRestaurant.Models;

namespace TungaRestaurant.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly string firstTimeLogin = "admin@admin.com";
        private readonly string firstTimeLoginPassword = "Admin000";
        public LoginModel(SignInManager<UserInfo> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<UserInfo> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if(await CheckFirstTimeLogin(Input.Email,Input.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(firstTimeLogin, firstTimeLoginPassword, false, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Invalid first time init attempt.");
                    return Page();
                }
                else
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Profile",new {area = ""});

                }
            }

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password,false,false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        private async Task<bool> CheckFirstTimeLogin(string email,string password)
        {
            if (_userManager.Users.Count() > 0)
            {
                return false;
            }
            if (email.Equals(firstTimeLogin) && password.Equals(firstTimeLoginPassword))
            {
                UserInfo firstUser = new UserInfo();
                firstUser.Email = firstTimeLogin;
                firstUser.UserName = firstTimeLogin;
                firstUser.DisplayName = "Admin";
                firstUser.PhoneNumber = "0000000000";
                firstUser.Address = "XXX";
                firstUser.Sex = Sex.OTHER;
                firstUser.Status = UserStatus.NORMAL;

                IdentityRole adminRole = new IdentityRole("Admin");
                IdentityRole branchRole = new IdentityRole("Branch Manager");
                IdentityRole customerRole = new IdentityRole("Customer");
                var adminRes = await _roleManager.CreateAsync(adminRole);
                var branchRes = await _roleManager.CreateAsync(branchRole);
                var customerRes = await _roleManager.CreateAsync(customerRole);

                var createRes = await _userManager.CreateAsync(firstUser, firstTimeLoginPassword);
                var addAdminRole = await _userManager.AddToRoleAsync(firstUser,"Admin");
                return true;
            }
            return false;
        }
    }
}
