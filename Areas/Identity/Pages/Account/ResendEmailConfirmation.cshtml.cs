#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
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
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

      
        [BindProperty]
        public InputModel Input { get; set; }

      
        public class InputModel
        {
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

         public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Se ha enviado un correo electrónico de verificación. Por favor, revise su correo electrónico.");
                return Page();
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "El correo electrónico ya ha sido confirmado.");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            // Envío del correo asegurando que se usa UTF-8 y un mensaje descriptivo
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Confirmación de correo electrónico en CDR Worship",
                $"Hola {user.UserName},<br><br>Por favor, confirma tu cuenta haciendo clic en el siguiente enlace: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Confirmar mi cuenta</a>.<br><br>Gracias por unirte a CDR Worship.");

            ModelState.AddModelError(string.Empty, "Se ha enviado un correo electrónico de verificación. Por favor, revise su correo electrónico.");
            return Page();
        }
    }
}
