using System.ComponentModel.DataAnnotations;

namespace TennisPlanner.Server.Models;

public class SearchModel
{
    [Required]
    public DateTime? SelectedDate;
}
