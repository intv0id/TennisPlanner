namespace TennisPlanner.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using TennisPlanner.Core.Contracts.Location;

    public class SearchModel
    {
        [Required]
        public DateTime? SelectedDate;

        public Address? SelectedAddressPlayer1;
        public Address? SelectedAddressPlayer2;

        public List<HourRangeSelectorModel> HourRangeSelectors = new List<HourRangeSelectorModel>();
    }
}
