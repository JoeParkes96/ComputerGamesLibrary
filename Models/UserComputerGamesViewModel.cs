using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

       [StringLength(100, ErrorMessage ="Search string can't exceed 100 characters")]
        public string SearchString { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
    }
}