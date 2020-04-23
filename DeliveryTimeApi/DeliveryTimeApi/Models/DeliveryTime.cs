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

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeliveryType Type { get; set; }

        [RegularExpression(TimeOfDayExpression)]
        public string From { get; set; } = default!;

        [RegularExpression(TimeOfDayExpression)]
        public string To { get; set; } = default!;

        [RegularExpression(TimeOfDayExpression)]
        public int ClosesBeforeMinutes { get; set; }

        public bool ExistAt(DateTime currentDate, DateTime nextDate)
        {
            var isValid = nextDate >= Start && nextDate <= Finish;

            if (!isValid)
            {
                return false;
            }

            if (!DaysOfWeek.Any())
            {
                return false;
            }

            if (Type == DeliveryType.Urgent && currentDate.Date != nextDate.Date)
            {
                return false;
            }

            var isAvailableAt = _availableAtDays.Contains(nextDate.DayOfWeek);

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

        public bool IsAvailable(DateTime currentDate, DateTime nextDate)
        {
            if (!ExistAt(currentDate, nextDate))
            {
                return false;
            }

            switch (Type)
            {
                case DeliveryType.Regular:
                    return currentDate.AddMinutes(ClosesBeforeMinutes) <= nextDate;

                case DeliveryType.Urgent:
                    var nextTimeMinutes = nextDate.Hour * 60 + nextDate.Minute;
                    var fromMinutes = GetElapsedMinutes(From);
                    var toMinutes = GetElapsedMinutes(To);
                    return nextTimeMinutes >= fromMinutes && nextTimeMinutes <= toMinutes;

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

        private static int GetClosesMinutes(string time, int closesBeforeMinutes)
        {
            var array = time.Split(":");
            var hours = int.Parse(array[0]);
            var minutes = int.Parse(array[1]);

            return hours * 60 + minutes - closesBeforeMinutes;
        }
    }

    public enum DeliveryType
    {
        Unknown,
        Regular,
        Urgent
    }
}
