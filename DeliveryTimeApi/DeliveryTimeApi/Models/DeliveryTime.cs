using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace DeliveryTimeApi.Models
{
    public class DeliveryTime
    {
        private const string TimeOfDayExpression = @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";
        private const string DaysOfWeekExpression = @"^[(Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday),]+$";
        private const string ZeroOrPositiveExpression = @"^(0|[1-9][0-9]{0,9})$";

        private IEnumerable<DayOfWeek> _availableAtDays = default!;
        private string _daysOfWeek = default!;

        [Required]
        [MinLength(1)]
        public string Name { get; set; } = default!;

        [Required]
        [MinLength(1)]
        public string Description { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Finish { get; set; }

        [Required]
        [RegularExpression(DaysOfWeekExpression)]
        public string DaysOfWeek
        {
            get => _daysOfWeek;
            set
            {
                _availableAtDays = value
                    .Split(",")
                    .Select(x => Enum.Parse(typeof(DayOfWeek), x))
                    .Cast<DayOfWeek>()
                    .ToList();
                _daysOfWeek = value;
            }
        }

        [RegularExpression(ZeroOrPositiveExpression)]
        public decimal Price { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeliveryType Type { get; set; }

        [RegularExpression(TimeOfDayExpression)]
        public string From { get; set; } = default!;

        [RegularExpression(TimeOfDayExpression)]
        public string To { get; set; } = default!;

        [RegularExpression(TimeOfDayExpression)]
        public string? ClosesAt { get; set; }

        public bool ExistAt(DateTime dateTime)
        {
            var isValid = dateTime >= Start && dateTime <= Finish;

            if (!isValid)
            {
                return false;
            }

            if (!DaysOfWeek.Any())
            {
                return false;
            }

            var isAvailableAt = _availableAtDays.Contains(dateTime.DayOfWeek);

            return isAvailableAt;
        }

        public DateTime CalculateStart(DateTime dateTime)
        {
            return dateTime.Date.AddMinutes(GetElapsedMinutes(From));
        }

        public DateTime CalculateFinish(DateTime dateTime)
        {
            return dateTime.Date.AddMinutes(GetElapsedMinutes(To));
        }

        public bool IsAvailable(DateTime dateTime)
        {
            if (!ExistAt(dateTime))
            {
                return false;
            }

            var currentTimeMinutes = dateTime.Hour * 60 + dateTime.Minute;

            switch (Type)
            {
                case DeliveryType.Regular:
                    var closesAtMinutes = GetElapsedMinutes(ClosesAt!);
                    return currentTimeMinutes <= closesAtMinutes;

                case DeliveryType.Urgent:
                    var fromMinutes = GetElapsedMinutes(From);
                    var toMinutes = GetElapsedMinutes(To);
                    return currentTimeMinutes >= fromMinutes && currentTimeMinutes <= toMinutes;

                default:
                    return false;
            }
        }
        
        private static int GetElapsedMinutes(string time)
        {
            var array = time.Split(":");
            var hours = int.Parse(array[0]);
            var minutes = int.Parse(array[1]);

            return hours * 60 + minutes;
        }
    }

    public enum DeliveryType
    {
        Unknown,
        Regular,
        Urgent
    }
}
