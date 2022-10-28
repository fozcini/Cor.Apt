using System.ComponentModel.DataAnnotations;

namespace Cor.Apt.Models
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage="Kullanıcı adı gereklidir.")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage="Parola gereklidir.")]
        public string Password { get; set; }
    }
}