using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComputerGamesLibrary.Models
{
    public class UserModel
    {
        public int ID { get; set; }

        [Required]
        [MinLength(5, ErrorMessage ="Username must be greater than 5 characters")]
        [MaxLength(50, ErrorMessage ="Username must be shorter than 50 characters")]

        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage = "Password must be greater than 8 characters")]
        [MaxLength(50, ErrorMessage = "Password must be shorter than 50 characters")]
        public string Password { get; set; }
    }
}