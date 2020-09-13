using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTWebAPI.DataAccess
{
    [Table("UserDetails")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
