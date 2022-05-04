using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

public interface ISearchFiltersService
{
    public event EventHandler<HourRangeSelectorModel> AddHourRangeEvent;
    public event EventHandler<HourRangeSelectorModel> RemoveHourRangeEvent;
    public event EventHandler<AddressModel> AddPlayerAddressEvent;
    public event EventHandler<AddressModel> RemovePlayerAddressEvent;

    public List<HourRangeSelectorModel> HourRangeList { get; }
    public List<AddressModel> AddressesList { get; }

    public void AddHourRange(HourRangeSelectorModel hourRangeSelectorModel);
    public void RemoveHourRange(HourRangeSelectorModel hourRangeSelectorModel);
    public void AddPlayerAddress(AddressModel playerAddress);
    public void RemovePlayerAddress(AddressModel playerAddress);

    public string ToJson();
    public bool TryLoadFromJson(string encodedFilters);
}
