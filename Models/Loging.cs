using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public partial class Auth
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { set; get; }
        public Auth()
        {
            Email ??= "";
            PasswordHash ??= [];
            PasswordSalt ??= [];
        }
    }
}