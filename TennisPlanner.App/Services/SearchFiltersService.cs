using Blazored.LocalStorage;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using TennisPlanner.Shared.Extensions;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

public class SearchFiltersService : ISearchFiltersService
{
    public const string LocalStorageFiltersKey = "FiltersProfiles";

    public event EventHandler<HourRangeSelectorModel> AddHourRangeEvent;
    public event EventHandler<HourRangeSelectorModel> RemoveHourRangeEvent;
    public event EventHandler<AddressModel> AddPlayerAddressEvent;
    public event EventHandler<AddressModel> RemovePlayerAddressEvent;

    public List<HourRangeSelectorModel> HourRangeList { get; private set; } = new();
    public List<AddressModel> AddressesList { get; private set; } = new();
    public string ProfileName = string.Empty;
    public string StorageId = string.Empty;

    public readonly ILocalStorageService _localStorage;

    public SearchFiltersService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public void AddHourRange(HourRangeSelectorModel hourRangeSelectorModel)
    {
        HourRangeList.Add(hourRangeSelectorModel);
        AddHourRangeEvent.Invoke(this, hourRangeSelectorModel);
    }

    public void AddPlayerAddress(AddressModel playerAddress)
    {
        AddressesList.Add(playerAddress);
        AddPlayerAddressEvent.Invoke(this, playerAddress);
    }

    public void RemoveHourRange(HourRangeSelectorModel hourRangeSelectorModel)
    {
        HourRangeList.Remove(hourRangeSelectorModel);
        RemoveHourRangeEvent.Invoke(this, hourRangeSelectorModel);
    }

    public void RemovePlayerAddress(AddressModel playerAddress)
    {
        AddressesList.Remove(playerAddress);
        RemovePlayerAddressEvent.Invoke(this, playerAddress);
    }

    public bool TryLoadProfile(FiltersProfileDto filters)
    {
        try
        {
            ResetProfile();

            foreach (var newHourRange in filters.HourRangeList)
            {
                AddHourRange(newHourRange);
            }

            foreach (var newAddress in filters.AddressesList)
            {
                AddPlayerAddress(newAddress);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    public void ResetProfile()
    {
        foreach (var existingHourRange in HourRangeList)
        {
            RemoveHourRange(existingHourRange);
        }
        foreach (var existingAddress in AddressesList)
        {
            RemovePlayerAddress(existingAddress);
        }
    }

    public string ToBase64()
    {
        return this.ExportProfile().ToJson().ToBase64();
    }

    public bool TryLoadFromBase64(string encodedFilters)
    {
        try
        {
            var filters = encodedFilters.FromBase64().FromJson();
            return TryLoadProfile(filters);
        }
        catch 
        {
            return false;
        }
    }

    public FiltersProfileDto ExportProfile(bool includeId = false)
    {
        return new FiltersProfileDto(
            hourRangeList: HourRangeList,
            addressesList: AddressesList,
            profileName: ProfileName);
    }

    public async Task<bool> TryLoadProfileFromLocalStorageAsync(string id)
    {
        try
        {
            var filtersProfiles = await _localStorage.GetItemAsync<List<FiltersProfileDto>>(LocalStorageFiltersKey);
            var filterProfile = filtersProfiles.Single(x => x.Id == id);
            return TryLoadProfile(filterProfile);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> TrySaveProfileInLocalStorageAsync()
    {
        if (string.IsNullOrEmpty(StorageId))
        {
            StorageId = Guid.NewGuid().ToString();
        }

        try
        {
            await _localStorage.SetItemAsync("name", this.ExportProfile(includeId: true));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<FiltersProfileDto>> ListSavedProfileIdsInLocalStorageAsync()
    {
        return await _localStorage.GetItemAsync<List<FiltersProfileDto>>(LocalStorageFiltersKey);
    }
}
