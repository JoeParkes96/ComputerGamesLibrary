using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ComputerGamesLibrary.Models
{
    public class UserComputerGamesViewModel
    {
        public List<UserComputerGame> Games { get; set; }
        public SelectList Genres { get; set; }
        public SelectList Years { get; set; }
        public string GameGenre { get; set; }
        public string SelectedYear { get; set; }

       [StringLength(Constants.MAX_NAME_LENGTH, ErrorMessage ="Search string can't exceed 100 characters")]
        public string SearchString { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FromPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ToPrice { get; set; }
    }
}