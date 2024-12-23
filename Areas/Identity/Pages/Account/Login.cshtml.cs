﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Configuration;
using CDR_Worship.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace CDR_Worship.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger, IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null, string demoLoginEmail = null)
        {
            returnUrl ??= Url.Content("~/");

            // Asegúrate de que _signInManager está correctamente inicializado
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!string.IsNullOrEmpty(demoLoginEmail))
            {
                string email = _configuration[demoLoginEmail] ?? Environment.GetEnvironmentVariable(demoLoginEmail);
                string password = _configuration["DemoLoginPassword"] ?? Environment.GetEnvironmentVariable("DemoLoginPassword");
                var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento de inicio de sesión de demostración no válido.");
                    return Page();
                }
            }

            if (ModelState.IsValid)
            {
                // Verifica si el correo está registrado
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    // El correo no existe
                    ModelState.AddModelError("Input.Email", "El correo electrónico no está registrado.");
                    return Page();
                }

                // Intenta iniciar sesión si el correo es válido
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Cuenta de usuario bloqueada.");
                    return RedirectToPage("./Lockout");
                }

                // Contraseña incorrecta
                ModelState.AddModelError("Input.Password", "La contraseña es incorrecta. Intenta de nuevo.");
                return Page();
            }

            // Si llegamos hasta aquí, algo falló, redisplay form
            return Page();
        }
    }
}
