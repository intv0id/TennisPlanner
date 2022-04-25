using System.Text.Json;
using TennisPlanner.Shared.Models;

namespace TennisPlanner.App.Services;

public class SearchFiltersService : ISearchFiltersService
{
    public event EventHandler<HourRangeSelectorModel> AddHourRangeEvent;
    public event EventHandler<HourRangeSelectorModel> RemoveHourRangeEvent;
    public event EventHandler<AddressModel> AddPlayerAddressEvent;
    public event EventHandler<AddressModel> RemovePlayerAddressEvent;

    public List<HourRangeSelectorModel> HourRangeList { get; private set; } = new();
    public List<AddressModel> AddressesList { get; private set; } = new();

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

    public string ToJson()
    {
        return JsonSerializer.Serialize(new FiltersDto
        {
            HourRangeList = HourRangeList,
            AddressesList = AddressesList
        });
    }

    public bool TryLoadFromJson(string encodedFilters)
    {
        try
        {
            var filters = JsonSerializer.Deserialize<FiltersDto>(encodedFilters);

            if (filters == null)
            {
                return false;
            }

            foreach (var existingHourRange in HourRangeList)
            {
                RemoveHourRange(existingHourRange);
            }

            foreach (var newHourRange in filters.HourRangeList)
            {
                AddHourRange(newHourRange);
            }

            foreach (var existingAddress in filters.AddressesList)
            {
                RemovePlayerAddress(existingAddress);
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
}
