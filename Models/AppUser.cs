using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDR_Worship.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Display(Name = "FirstName")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2}  and max {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2}  and max {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [NotMapped]
        public IFormFile? ImageFormFile { get; set; }

        public byte[]? ImageFileData { get; set; }

        public string? ImageFileType { get; set; }

        // Nueva propiedad para almacenar la ruta o base64 de la imagen procesada
        [NotMapped]
        public string? ImageFilePath { get; set; }

        // Constructor sin parámetros
        public AppUser()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

    }
}