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
    public string ProfileName { get; set; }

    public void AddHourRange(HourRangeSelectorModel hourRangeSelectorModel);
    public void RemoveHourRange(HourRangeSelectorModel hourRangeSelectorModel);
    public void AddPlayerAddress(AddressModel playerAddress);
    public void RemovePlayerAddress(AddressModel playerAddress);

    public void ResetProfile();

    public bool TryLoadProfile(FiltersProfileDto filters);
    public FiltersProfileDto ExportProfile(bool includeId = false);

    public string ToBase64();
    public bool TryLoadFromBase64(string encodedFilters);

    public Task<bool> TryLoadProfileFromLocalStorageAsync(string id);
    public Task<bool> TrySaveProfileInLocalStorageAsync();
    public Task<List<FiltersProfileDto>> ListSavedProfileIdsInLocalStorageAsync();
}
