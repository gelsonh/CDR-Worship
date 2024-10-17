// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using CDR_Worship.Models;
using CDR_Worship.Models.Enums;
using CDR_Worship.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace CDR_Worship.Areas.Identity.Pages.Account.Manage
{

    [Authorize(Policy = "NoDemoUserAccess")]
        public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IImageService _imageService;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IImageService imageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
             public string FirstName { get; set; }

             [Display(Name ="Last Name")]
             public string LastName { get; set; }

            [DataType(DataType.Upload)]
            [Display(Name = "Foto de Perfil")]
            public IFormFile ImageFormFile { get; set; }


        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = phoneNumber,
                ImageFormFile = null // Se inicializa en null, pero puede ser manejado en la vista        
            };
                
                // Aquí puedes cargar la imagen existente
                ViewData["Image"] = _imageService.ConvertByteArrayToFile(user.ImageFileData, user.ImageFileType, DefaultImage.UserImage);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null)
    {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
    }

    if (!ModelState.IsValid)
    {
        await LoadAsync(user);
        return Page();
    }

    // Actualización del número de teléfono
    var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
    if (Input.PhoneNumber != phoneNumber)
    {
        var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
        if (!setPhoneResult.Succeeded)
        {
            StatusMessage = "Unexpected error when trying to set phone number.";
            return RedirectToPage();
        }
    }

    // Actualización de nombre
    if (Input.FirstName != user.FirstName || Input.LastName != user.LastName)
    {
        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;
    }

    // Manejar la subida de la imagen
    if (Input.ImageFormFile != null && Input.ImageFormFile.Length > 0)
    {
        user.ImageFileData = await _imageService.ConvertFileToByteArrayAsync(Input.ImageFormFile);
        user.ImageFileType = Path.GetExtension(Input.ImageFormFile.FileName);

           // Depurar para verificar que la imagen se ha guardado correctamente
    Console.WriteLine($"Image uploaded: {user.ImageFileData?.Length} bytes");
    }

    // Guardar cambios en el usuario
    var updateResult = await _userManager.UpdateAsync(user);
    if (!updateResult.Succeeded)
    {
        StatusMessage = "Unexpected error when trying to update profile.";
        return RedirectToPage();
    }

    // Refrescar sesión del usuario
    await _signInManager.RefreshSignInAsync(user);
    StatusMessage = "Your profile has been updated";
    return RedirectToPage();
}
    }
}
