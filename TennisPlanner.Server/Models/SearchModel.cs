namespace TennisPlanner.Server.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SearchModel
    {
        [Required]
        public DateTime? SelectedDate;

        public string? AdressPlayer1;
        public string? AdressPlayer2;
    }
}
