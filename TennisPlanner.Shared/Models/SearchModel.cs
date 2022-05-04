using System.ComponentModel.DataAnnotations;

namespace TennisPlanner.Shared.Models;

public class SearchModel
{
    [Required]
    public DateTime? SelectedDate;
}
