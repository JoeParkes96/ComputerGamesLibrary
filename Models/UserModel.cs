using System.ComponentModel.DataAnnotations;

namespace ComputerGamesLibrary.Models
{
    public class UserModel
    {
        public int ID { get; set; }

        [Required]
        [MinLength(Constants.MIN_USERNAME_LENGTH, ErrorMessage ="Username must be greater than 5 characters")]
        [MaxLength(Constants.MAX_USERNAME_LENGTH, ErrorMessage ="Username must be shorter than 50 characters")]

        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(Constants.MIN_PASSWORD_LENGTH, ErrorMessage = "Password must be greater than 8 characters")]
        [MaxLength(Constants.MAX_PASSWORD_LENGTH, ErrorMessage = "Password must be shorter than 50 characters")]
        public string Password { get; set; }
    }
}