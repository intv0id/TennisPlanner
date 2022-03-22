using Microsoft.AspNetCore.Components;
using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Contracts;
using TennisPlanner.Core.Extensions;
using TennisPlanner.Server.Models;
using TennisPlanner.Server.Services;
using TennisPlanner.Shared.Services.Logging;

namespace TennisPlanner.Server.Pages
{
    public partial class Search
    {
        [Inject]
        ILoggerService LoggerService { get; set; }
        [Inject]
        INotificationService NotificationService { get; set; }
        [Inject]
        ITennisClient TennisClient { get; set; }
        [Inject]
        ITransportClient TransportClient { get; set; }

        SearchModel searchModel = new();

        bool IsSearchDisplayed;
        bool IsSearchEnabled;
        IEnumerable<TimeSlot>? Availabilities;

        string CollapseSearch => collapseSearch ? "collapse" : string.Empty;
        bool collapseSearch = false;

        protected override async Task OnInitializedAsync()
        {
            await Task.Yield();
            searchModel.SelectedDate = DateTime.Today;
        }

        private void ResetSearchResults()
        {
            Availabilities = null;
        }

        private void ExpandSearch()
        {
            collapseSearch = false;
            ResetSearchResults();
        }

        private void AddHourRangeSelector()
        {
            searchModel.HourRangeSelectors.Add(new HourRangeSelectorModel());
        }

        private void RemoveHourRangeSelector(HourRangeSelectorModel hourRangeSelector)
        {
            searchModel.HourRangeSelectors.Remove(hourRangeSelector);
        }

        private async Task HandleSearch()
        {
            if (searchModel.SelectedDate == null)
            {
                LoggerService.Log(logLevel: LogLevel.Error, operationName: $"{nameof(Search)}.{nameof(this.HandleSearch)}", message: $"{nameof(searchModel.SelectedDate)} is null.");
                NotificationService.Display(level: LogLevel.Error, message: "A error occured.");
                return;
            }

            Availabilities = await fetchAvailabilitiesAsync(date: searchModel.SelectedDate.Value, transportSearch: IsTransportSearchEnabled);
            collapseSearch = true;
        }

        private async Task<IEnumerable<TimeSlot>> fetchAvailabilitiesAsync(DateTime date, bool transportSearch)
        {
            var courts = await TennisClient.GetTennisCourtsListAsync();
            var availabilities = await Task.WhenAll(courts.Select(async court => await TennisClient.GetTimeSlotListAsync(court, date)));
            var tennisSlots = availabilities.SelectMany(x => x);
            tennisSlots = filterSlots(tennisSlots);
            if (transportSearch)
            {
                tennisSlots = await Task.WhenAll(tennisSlots.Select(GetTransportationTimesAsync));
            }

            tennisSlots = orderSlots(tennisSlots);
            return tennisSlots;
        }

        private async Task<TimeSlot> GetTransportationTimesAsync(TimeSlot timeSlot)
        {
            if (searchModel.SelectedAddressPlayer1 == null || searchModel.SelectedAddressPlayer2 == null)
            {
                throw new ArgumentException("Selected address is null.");
            }

            if (searchModel.SelectedDate == null)
            {
                throw new ArgumentException("Selected date is null.");
            }

            var transportationArrivalDate = new DateTime(year: searchModel.SelectedDate.Value.Year, month: searchModel.SelectedDate.Value.Month, day: searchModel.SelectedDate.Value.Day, hour: timeSlot.TimeRange.StartHour, minute: 0, second: 0);
            var transportationTime1 = await TransportClient.GetTransportationTimeInMinutesAsync(arrivalTime: transportationArrivalDate, fromGeoCoordinates: searchModel.SelectedAddressPlayer1.Geometry.Coordinates.ToGeoCoordinates(), toGeoCoordinates: timeSlot.CourtInfo.Facility.Coordinates);
            var transportationTime2 = await TransportClient.GetTransportationTimeInMinutesAsync(arrivalTime: transportationArrivalDate, fromGeoCoordinates: searchModel.SelectedAddressPlayer2.Geometry.Coordinates.ToGeoCoordinates(), toGeoCoordinates: timeSlot.CourtInfo.Facility.Coordinates);
            timeSlot.TravelInfo.JourneyDurationFromAdress1 = transportationTime1;
            timeSlot.TravelInfo.JourneyDurationFromAdress2 = transportationTime2;
            return timeSlot;
        }

        private IEnumerable<TimeSlot> filterSlots(IEnumerable<TimeSlot> slots)
        {
            if (searchModel.HourRangeSelectors.Count == 0)
            {
                return slots;
            }

            return slots.Where(slot => searchModel.HourRangeSelectors.Any(rs => 
            rs.Min <= slot.TimeRange.StartHour 
            && rs.Max >= slot.TimeRange.EndHour));
        }

        private IEnumerable<TimeSlot> orderSlots(IEnumerable<TimeSlot> slots)
        {
            var orderedByTimeSlots = slots.OrderBy(s => s.TimeRange.StartHour);
            if (IsTransportSearchEnabled)
            {
                return orderedByTimeSlots.ThenBy(s => travelTimeMetric(
                    s.TravelInfo.JourneyDurationFromAdress1?.TotalDurationInSeconds, 
                    s.TravelInfo.JourneyDurationFromAdress1?.TotalDurationInSeconds));
            }

            return orderedByTimeSlots;
        }

        private bool ValidateFields()
        {
            var selectedDate = searchModel.SelectedDate;
            if (selectedDate == null || selectedDate < DateTime.Today || selectedDate > DateTime.Today.AddDays(7))
            {
                return false;
            }

            return true;
        }

        private bool IsTransportSearchEnabled =>
            searchModel?.SelectedAddressPlayer1 != null 
            && searchModel?.SelectedAddressPlayer2 != null;

        private double travelTimeMetric(int? duration1, int? duration2)
        {
            if (duration1 == null || duration2 == null)
            {
                return double.PositiveInfinity;
            }

            return Math.Pow((int)duration1, 2) + Math.Pow((int)duration2, 2);
        }
    }
}