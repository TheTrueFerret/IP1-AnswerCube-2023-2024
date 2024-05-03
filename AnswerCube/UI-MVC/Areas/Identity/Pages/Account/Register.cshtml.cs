// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using AnswerCube.BL;
using Microsoft.AspNetCore.Authentication;
using AnswerCube.BL.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using NuGet.Versioning;

namespace AnswerCube.UI.MVC.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AnswerCubeUser> _signInManager;
        private readonly UserManager<AnswerCubeUser> _userManager;
        private readonly IUserStore<AnswerCubeUser> _userStore;
        private readonly IUserEmailStore<AnswerCubeUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IManager _manager;

        public RegisterModel(
            UserManager<AnswerCubeUser> userManager,
            IUserStore<AnswerCubeUser> userStore,
            SignInManager<AnswerCubeUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager, IManager manager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _manager = manager;
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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //Adds the first and last name fields to the registration form
            [Required]
            [StringLength(25, ErrorMessage = "First Name must be less than 25 characters")]
            [Display(Name = "Firstname")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "First Name must be less than 50 characters")]
            public string LastName { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null, string email = null)
        {
            // Extract the email from the URL query parameters
            email = Request.Query["email"];

            // Pass the email to the view
            ViewData["Email"] = email;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                if (!CheckName(user.FirstName, user.LastName))
                {
                    user.FirstName = Input.FirstName;
                    user.LastName = Input.LastName;
                    user.TypeUser = TypeUser.USER;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid name");
                    return Page();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //Adding Roles to the user
                    var role = await _roleManager.FindByNameAsync("User");
                    if (role != null)
                    {
                        if (role.Name != null) await _userManager.AddToRoleAsync(user, role.Name);
                    }

                    if (IsDeelplatformBeheerder(user))
                    {
                        user.TypeUser = TypeUser.DEELPLATFORMMANAGER;
                        role = await _roleManager.FindByNameAsync("DeelplatformBeheerder");
                        if (role != null)
                        {
                            if (role.Name != null) await _userManager.AddToRoleAsync(user, role.Name);
                        }

                        _manager.AddUserToOrganization(user);
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"<!DOCTYPE html> <html lang='en'><head>    <meta charset=\"UTF-8\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Registration Confirmation</title>\n    <style>\n body {{\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n            text-align: center;\n        }}\n\n        .container {{\n            max-width: 600px;\n            margin: 20px auto;\n            background-color: #fff;\n            padding: 20px;\n            border-radius: 8px;\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\n        }}\n\n        h1 {{\n            color: #333;\n        }}\n\n        p {{\n            color: #666;\n            margin-bottom: 20px;\n        }}\n\n        .btn {{\n            display: inline-block;\n            padding: 10px 20px;\n            background-color: #007bff;\n            color: #000;\n            text-decoration: none;\n            border-radius: 5px;\n            transition: background-color 0.3s;\n        }}\n\n        .btn:hover {{\n            background-color: #0056b3;\n        }}\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Registration Confirmation</h1>\n        <p>Thank you for registering! To activate your account, please click the button below:</p>\n        <a href=\"{HtmlEncoder.Default.Encode(callbackUrl)}\" class=\"btn\">Confirm Account</a>\n    </div>\n</body>\n</html>\n");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation",
                            new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private bool IsDeelplatformBeheerder(AnswerCubeUser user)
        {
            return _manager.GetDeelplatformBeheerderByEmail(user.Email);
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
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
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