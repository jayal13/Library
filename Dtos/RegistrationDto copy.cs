using System.ComponentModel.DataAnnotations;

namespace Library.Dtos
{
    public partial class RegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }

        public RegistrationDto()
        {
            Email ??= "";
            Password ??= "";
            PasswordConfirmation ??= "";
        }
    }
}