using System.ComponentModel.DataAnnotations;

namespace ComputerGamesLibrary.Models
{
    public class UserModel
    {
        public int ID { get; set; }

        [Required]
        [MinLength(Constants.MIN_USERNAME_LENGTH, ErrorMessage ="Username must be 5 characters or more")]
        [MaxLength(Constants.MAX_USERNAME_LENGTH, ErrorMessage ="Username must be 50 characters or less")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [MinLength(Constants.MIN_PASSWORD_LENGTH, ErrorMessage = "Password must be 8 characters or more")]
        [MaxLength(Constants.MAX_PASSWORD_LENGTH, ErrorMessage = "Password must be 50 characters or less")]
        public string Password { get; set; }
    }
}