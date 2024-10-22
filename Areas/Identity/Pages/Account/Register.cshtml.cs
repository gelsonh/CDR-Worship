#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using CDR_Worship.Services.Interfaces;
using SendGrid.Helpers.Mail;

namespace CDR_Worship.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IImageService _imageService;

        public RegisterModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, IImageService imageService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _imageService = imageService;
        }

       
        [BindProperty]
        public InputModel Input { get; set; }

       
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "Nombre")]
            [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 2)]

            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Apellido")]
            [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 2)]

            public string LastName { get; set; }
        
            [Required]
            [EmailAddress]
            [Display(Name = "Correo Electronico")]
            public string Email { get; set; }

         
            [Required]
            [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Por favor, sube una foto de perfil.")]
            [DataType(DataType.Upload)]
            [Display(Name = "Foto de Perfil")]
            public IFormFile ImageFormFile { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

       public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl ??= Url.Content("~/");
    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

    if (ModelState.IsValid)
    {
        // Crear el usuario con las propiedades adicionales
        var user = new AppUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            FirstName = Input.FirstName,
            LastName = Input.LastName
        };

        _logger.LogInformation($"El correo electrónico proporcionado es: {Input.Email}");

        // Procesar la imagen subida
        if (Input.ImageFormFile != null && Input.ImageFormFile.Length > 0)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(Input.ImageFormFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("Input.ImageFormFile", "Tipo de archivo no permitido. Solo se permiten imágenes en formato JPG, JPEG, PNG o GIF.");
                return Page();
            }

            if (Input.ImageFormFile.Length > 2 * 1024 * 1024) // 2 MB
            {
                ModelState.AddModelError("Input.ImageFormFile", "El tamaño del archivo no debe exceder 2 MB.");
                return Page();
            }

            user.ImageFileData = await _imageService.ConvertFileToByteArrayAsync(Input.ImageFormFile);
            user.ImageFileType = extension;
        }

        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("El usuario ha creado una nueva cuenta con contraseña.");

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code, returnUrl },
                protocol: Request.Scheme);

            try
            {
                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Confirma tu correo electrónico",
                    $"Por favor confirma tu cuenta haciendo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clic aquí</a>.");
            }
            catch (Exception ex)
            {
                // Agregar error al ModelState si falla el envío del correo
                ModelState.AddModelError(string.Empty, "Hubo un error al enviar el correo de confirmación. Por favor intenta de nuevo más tarde.");
                _logger.LogError(ex, "Error enviando el correo de confirmación.");
                return Page(); // Vuelve a mostrar la página con el error.
            }

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
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

    // Si llegamos hasta aquí, algo falló; volvemos a mostrar el formulario
    return Page();
}

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }
    }
}
