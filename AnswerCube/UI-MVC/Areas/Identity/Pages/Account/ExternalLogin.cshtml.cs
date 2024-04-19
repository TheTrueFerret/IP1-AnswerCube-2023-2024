// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using AnswerCube.BL.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AnswerCube.UI.MVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<AnswerCubeUser> _signInManager;
        private readonly UserManager<AnswerCubeUser> _userManager;
        private readonly IUserStore<AnswerCubeUser> _userStore;
        private readonly IUserEmailStore<AnswerCubeUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ExternalLoginModel(
            SignInManager<AnswerCubeUser> signInManager,
            UserManager<AnswerCubeUser> userManager,
            IUserStore<AnswerCubeUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(25, ErrorMessage = "First Name must be less than 25 characters")]
            [Display(Name = "Firstname")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "First Name must be less than 50 characters")]
            public string LastName { get; set; }
        }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                    info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                if (!CheckName(user.FirstName, user.LastName))
                {
                    user.FirstName = Input.FirstName;
                    user.LastName = Input.LastName;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid name");
                    return Page();
                }

                //Adding Roles to the user
                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                {
                    if (role.Name != null) await _userManager.AddToRoleAsync(user, role.Name);
                }

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"<!DOCTYPE html> <html lang='en'><head>    <meta charset=\"UTF-8\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Registration Confirmation</title>\n    <style>\n        /* Styles for the email template */\n        body {{\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n            text-align: center;\n        }}\n\n        .container {{\n            max-width: 600px;\n            margin: 20px auto;\n            background-color: #fff;\n            padding: 20px;\n            border-radius: 8px;\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\n        }}\n\n        h1 {{\n            color: #333;\n        }}\n\n        p {{\n            color: #666;\n            margin-bottom: 20px;\n        }}\n\n        .btn {{\n            display: inline-block;\n            padding: 10px 20px;\n            background-color: #007bff;\n            color: #fff;\n            text-decoration: none;\n            border-radius: 5px;\n            transition: background-color 0.3s;\n        }}\n\n        .btn:hover {{\n            background-color: #0056b3;\n        }}\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Registration Confirmation</h1>\n        <p>Thank you for registering! To activate your account, please click the button below:</p>\n        <a href=\"{HtmlEncoder.Default.Encode(callbackUrl)}\" class=\"btn\">Confirm Account</a>\n    </div>\n</body>\n</html>\n");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private AnswerCubeUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AnswerCubeUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AnswerCubeUser)}'. " +
                                                    $"Ensure that '{nameof(AnswerCubeUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<AnswerCubeUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<AnswerCubeUser>)_userStore;
        }

        private Boolean CheckName(string name, string lastName)
        {
            //TODO: Add a check for faulty names like (cuss words, numbers, etc)
            return false;
        }
    }
}