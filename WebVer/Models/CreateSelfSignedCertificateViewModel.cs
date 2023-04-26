using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebVer.Models
{
    public class CreateSelfSignedCertificateViewModel
    {
        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public string OrganizationInn { get; set; }

        [Required]
        public DateTime ValidityPeriod { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}
