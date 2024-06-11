#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using AnswerCube.BL;
using Microsoft.AspNetCore.Authentication;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace AnswerCube.UI.MVC.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AnswerCubeUser> _signInManager;
        private readonly UserManager<AnswerCubeUser> _userManager;
        private readonly IUserStore<AnswerCubeUser> _userStore;
        private readonly IUserEmailStore<AnswerCubeUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMailManager _mailManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly UnitOfWork _uow;

        public RegisterModel(
            UserManager<AnswerCubeUser> userManager,
            IUserStore<AnswerCubeUser> userStore,
            SignInManager<AnswerCubeUser> signInManager,
            ILogger<RegisterModel> logger,
            IMailManager mailManager,
            RoleManager<IdentityRole> roleManager, IOrganizationManager manager, UnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _mailManager = mailManager;
            _roleManager = roleManager;
            _organizationManager = manager;
            _uow = unitOfWork;
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
                if (!CheckName(Input.FirstName, Input.LastName))
                {
                    user.FirstName = Input.FirstName;
                    user.LastName = Input.LastName;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Your name contains a bad word. Please try again.");
                    return Page();
                }

                _uow.BeginTransaction();
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _uow.CommitAsync();
                
                if (result.Succeeded)
                {
                    //Adding Roles to the user
                    var role = await _roleManager.FindByNameAsync("Gebruiker");
                    if (role != null) await _userManager.AddToRoleAsync(user, role.Name);

                    if (IsDeelplatformBeheerder(user))
                    {
                        _uow.BeginTransaction();
                        await _organizationManager.AddUserToOrganization(user);
                        await _uow.CommitAsync();
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    
                    await _mailManager.SendConfirmationEmail(Input.Email, userId, code, returnUrl);
                    
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
            return _organizationManager.GetDeelplatformBeheerderByEmail(user.Email);
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
            var badwords = System.IO.File.ReadLines(@"Areas\Identity\Data\BadWords\en.txt")
                .Select(word => word.Trim().ToLower())
                .ToArray();
            if (name != null && (badwords.Equals(name.ToLower()) || name.Any(char.IsDigit) ||
                                 name.Any(ch => !char.IsLetter(ch))))
            {
                return true;
            }

            if (lastName != null && (badwords.Equals(lastName.ToLower()) || lastName.Any(char.IsDigit) ||
                                     lastName.Any(ch => !char.IsLetter(ch))))
            {
                return true;
            }

            return false;
        }
    }
}