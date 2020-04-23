using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryTimeApi.Models;
using DeliveryTimeApi.Repositories;
using DeliveryTimeApi.Services;
using FluentAssertions;
using Xunit;

namespace DeliveryTimeApi.UnitTests
{
    public class DeliveryTimeServiceBulkTests
    {
        private readonly DateTime _startDate = new DateTime(2020, 4, 1);
        private readonly DateTime _finishDate = new DateTime(2020, 4, 30);

        [Fact]
        public async Task Given_RegularDeliveryTime_WhenDeliveryTimeIsOpen_ShouldReturnSingleAvailableItem()
        {
            // Arrange
            var currentTime = new DateTime(2020, 4, 18, 15, 0, 0);
            var actualDeliveryTimes = CreateDeliveryTime();

            var repository = new DeliveryTimeInMemoryRepository();

            foreach (var actualDeliveryTime in actualDeliveryTimes)
            {
                await repository.Add(actualDeliveryTime);
            }

            var sut = new DeliveryTimeService(repository);

            // Act
            var result = await sut.Get(currentTime, 3);

            // Assert
            var expectedDeliveryTimesDto = CreateExpectedDeliveryTimeDto();
            result.Should().BeEquivalentTo(expectedDeliveryTimesDto);
        }

        private static IEnumerable<DeliveryTimeDto> CreateExpectedDeliveryTimeDto()
        {
            return new[]
            {
                new DeliveryTimeDto
                {
                    Name = "Срочная доставка",
                    Description = "Доставка за 1-2 часа",
                    Start = new DateTime(2020, 4, 18, 15, 0, 0),
                    Finish = new DateTime(2020, 4, 18, 17, 0, 0),
                    Type = DeliveryType.Urgent,
                    Price = 99,
                    Available = true
                },
                new DeliveryTimeDto
                {
                    Name = "14:00 - 18:00",
                    Description = "Доставка 14:00 - 18:00",
                    Start = new DateTime(2020, 4, 18, 14, 0, 0),
                    Finish = new DateTime(2020, 4, 18, 18, 0, 0),
                    Type = DeliveryType.Regular,
                    Price = 59,
                    Available = false
                },
                new DeliveryTimeDto
                {
                    Name = "18:00 - 23:00",
                    Description = "Доставка 18:00 - 23:00",
                    Start = new DateTime(2020, 4, 18, 18, 0, 0),
                    Finish = new DateTime(2020, 4, 18, 23, 0, 0),
                    Type = DeliveryType.Regular,
                    Price = 59,
                    Available = false
                },
                new DeliveryTimeDto
                {
                    Name = "10:00 - 18:00",
                    Description = "Доставка 10:00 - 18:00",
                    Start = new DateTime(2020, 4, 19, 10, 0, 0),
                    Finish = new DateTime(2020, 4, 19, 18, 0, 0),
                    Type = DeliveryType.Regular,
                    Price = 59,
                    Available = true
                },
                new DeliveryTimeDto
                {
                    Name = "10:00 - 12:00",
                    Description = "Доставка 10:00 - 12:00",
                    Start = new DateTime(2020, 4, 20, 10, 0, 0),
                    Finish = new DateTime(2020, 4, 20, 12, 0, 0),
                    Type = DeliveryType.Regular,
                    Price = 49,
                    Available = true
                },
                new DeliveryTimeDto
                {
                    Name = "12:00 - 18:00",
                    Description = "Доставка 12:00 - 18:00",
                    Start = new DateTime(2020, 4, 20, 12, 0, 0),
                    Finish = new DateTime(2020, 4, 20, 18, 0, 0),
                    Type = DeliveryType.Regular,
                    Price = 49,
                    Available = true
                },
                new DeliveryTimeDto
                {
                    Name = "10:00 - 12:00",
                    Description = "Доставка 10:00 - 12:00",
                    Start = new DateTime(2020, 4, 21, 10, 0, 0),
                    Finish = new DateTime(2020, 4, 21, 12, 0, 0),
                    Type = DeliveryType.Regular,
                    Price = 49,
                    Available = true
                },
                new DeliveryTimeDto
                {
                    Name = "12:00 - 18:00",
                    Description = "Доставка 12:00 - 18:00",
                    Start = new DateTime(2020, 4, 21, 12, 0, 0),
                    Finish = new DateTime(2020, 4, 21, 18, 0, 0),
                    Type = DeliveryType.Regular,
                    Price = 49,
                    Available = true
                }
            };
        }

        private IEnumerable<DeliveryTime> CreateDeliveryTime()
        {
            return new[]
            {
                new DeliveryTime
                {
                    Name = "Срочная доставка",
                    Description = "Доставка за 1-2 часа",
                    Start = _startDate,
                    Finish = _finishDate,
                    Type = DeliveryType.Urgent,
                    Price = 99,
                    From = "15:00",
                    To = "17:00",
                    DaysOfWeek = "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday"
                },
                new DeliveryTime
                {
                    Name = "10:00 - 12:00",
                    Description = "Доставка 10:00 - 12:00",
                    Start = _startDate,
                    Finish = _finishDate,
                    Type = DeliveryType.Regular,
                    Price = 49,
                    From = "10:00",
                    To = "12:00",
                    DaysOfWeek = "Monday,Tuesday,Wednesday,Thursday,Friday",
                    ClosesBeforeMinutes = 840
                },
                new DeliveryTime
                {
                    Name = "12:00 - 18:00",
                    Description = "Доставка 12:00 - 18:00",
                    Start = _startDate,
                    Finish = _finishDate,
                    Type = DeliveryType.Regular,
                    Price = 49,
                    From = "12:00",
                    To = "18:00",
                    DaysOfWeek = "Monday,Tuesday,Wednesday,Thursday,Friday",
                    ClosesBeforeMinutes = 960
                },
                new DeliveryTime
                {
                    Name = "14:00 - 18:00",
                    Description = "Доставка 14:00 - 18:00",
                    Start = _startDate,
                    Finish = _finishDate,
                    Type = DeliveryType.Regular,
                    Price = 59,
                    From = "14:00",
                    To = "18:00",
                    DaysOfWeek = "Saturday",
                    ClosesBeforeMinutes = 960
                },
                new DeliveryTime
                {
                    Name = "18:00 - 23:00",
                    Description = "Доставка 18:00 - 23:00",
                    Start = _startDate,
                    Finish = _finishDate,
                    Type = DeliveryType.Regular,
                    Price = 59,
                    From = "18:00",
                    To = "23:00",
                    DaysOfWeek = "Saturday",
                    ClosesBeforeMinutes = 1320
                },     
                new DeliveryTime
                {
                    Name = "10:00 - 18:00",
                    Description = "Доставка 10:00 - 18:00",
                    Start = _startDate,
                    Finish = _finishDate,
                    Type = DeliveryType.Regular,
                    Price = 59,
                    From = "10:00",
                    To = "18:00",
                    DaysOfWeek = "Sunday",
                    ClosesBeforeMinutes = 840
                },
            };
        }
    }
}
