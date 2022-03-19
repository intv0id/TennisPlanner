namespace TennisPlanner.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using TennisPlanner.Core.Contracts.Location;

    public class SearchModel
    {
        [Required]
        public DateTime? SelectedDate;

        public string? PartialAddressPlayer1;
        public string? PartialAddressPlayer2;

        public Address? SelectedAddressPlayer1;
        public Address? SelectedAddressPlayer2;

        public IEnumerable<Address>? AddressSuggestionsPlayer1 { get; set; }
        public IEnumerable<Address>? AddressSuggestionsPlayer2 { get; set; }
    }
}
