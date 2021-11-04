using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace TenmoServer.Models
{
    public class User
    {
        [Required(ErrorMessage = "User id can not be blank.")]
        [Range(1,double.PositiveInfinity)]
        public int UserId { get; set; }
        [Required]
        [RegularExpression(@"^(\w+)$",ErrorMessage = "Username may only have letters and numbers.")]
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        [RegularExpression("^(.+)(@)(.+)$",ErrorMessage = "Invalid email format.")]

        public string Email { get; set; }
    }

    /// <summary>
    /// Model to return upon successful login
    /// </summary>
    public class ReturnUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }        
        public string Token { get; set; }
    }

    /// <summary>
    /// Model to accept login parameters
    /// </summary>
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
