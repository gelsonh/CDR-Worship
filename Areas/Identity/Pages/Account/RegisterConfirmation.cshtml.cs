using System.Text;
using Microsoft.AspNetCore.Authorization;
using CDR_Worship.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace CDR_Worship.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<AppUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string? Email { get; set; } = string.Empty; // Valor predeterminado
        public bool DisplayConfirmAccountLink { get; set; }
        public string? EmailConfirmationUrl { get; set; } = string.Empty; // Valor predeterminado


        public async Task<IActionResult> OnGetAsync(string email, string returnUrl )
        {
            if (email == null)
    {
        return RedirectToPage("/Index");
    }
    returnUrl = returnUrl ?? Url.Content("~/");

    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
        return NotFound($"No se puede cargar el usuario con correo electrónico '{email}'.");
    }

    Email = email; // El compilador ahora entiende que email no es null aquí
    DisplayConfirmAccountLink = false;

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await _sender.SendEmailAsync(
                email,
                "Confirma tu correo electrónico",
                $"Por favor, confirme su cuenta mediante <a href='{EmailConfirmationUrl}'>clicking here</a>.");

            return Page();
        }
    }
}
