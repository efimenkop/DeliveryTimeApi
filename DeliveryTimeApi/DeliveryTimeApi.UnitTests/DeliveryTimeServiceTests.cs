using System;
using System.Linq;
using System.Threading.Tasks;
using DeliveryTimeApi.Models;
using DeliveryTimeApi.Repositories;
using DeliveryTimeApi.Services;
using FluentAssertions;
using Xunit;

namespace DeliveryTimeApi.UnitTests
{
    public class DeliveryTimeServiceTests
    {
        private readonly DateTime _startDate = new DateTime(2020, 4, 17);
        private readonly DateTime _finishDate = new DateTime(2020, 4, 24);
        private readonly DateTime _expectedStart = new DateTime(2020, 4, 17, 14, 0, 0);
        private readonly DateTime _expectedFinish = new DateTime(2020, 4, 17, 18, 0, 0);
        private const int DeliveryPrice = 123;

        [Fact]
        public async Task Given_RegularDeliveryTime_WhenDeliveryTimeIsOpen_ShouldReturnSingleAvailableItem()
        {
            // Arrange
            var currentTime = new DateTime(2020, 4, 17, 8, 0,0);
            var expectedDto = CreateDeliveryTimeDto(DeliveryType.Regular, _expectedStart, _expectedFinish, true);
            var sut = new DeliveryTimeService(new DeliveryTimeInMemoryRepository());
            await sut.Add(CreateRegularDeliveryTime("14:00", "18:00", 0));

            // Act
            var result = await sut.Get(currentTime, 0);

            // Assert
            result.Should().ContainSingle();
            result.Single().Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task Given_RegularDeliveryTime_WhenDeliveryTimeIsClosed_ShouldReturnSingleUnavailableItem()
        {
            var currentTime = new DateTime(2020, 4, 17, 12, 0, 0);
            var expectedDto = CreateDeliveryTimeDto(DeliveryType.Regular, _expectedStart, _expectedFinish, false);
            var sut = new DeliveryTimeService(new DeliveryTimeInMemoryRepository());
            await sut.Add(CreateRegularDeliveryTime("14:00", "18:00", 840));

            // Act
            var result = await sut.Get(currentTime, 0);

            // Assert
            result.Should().ContainSingle();
            result.Single().Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task Given_UrgentDeliveryTime_WhenDeliveryTimeIsOpened_ShouldReturnSingleAvailableItem()
        {
            var currentTime = new DateTime(2020, 4, 17, 17, 59, 0);
            var expectedDto = CreateDeliveryTimeDto(DeliveryType.Urgent, _expectedStart, _expectedFinish, true);
            var sut = new DeliveryTimeService(new DeliveryTimeInMemoryRepository());
            await sut.Add(CreateUrgentDeliveryTime("14:00", "18:00"));

            // Act
            var result = await sut.Get(currentTime, 0);

            // Assert
            result.Should().ContainSingle();
            result.Single().Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task Given_UrgentDeliveryTime_WhenDeliveryTimeIsClosed_ShouldReturnSingleUnavailableItem()
        {
            var currentTime = new DateTime(2020, 4, 17, 19, 00, 0);
            var expectedDto = CreateDeliveryTimeDto(DeliveryType.Urgent, _expectedStart, _expectedFinish, false);
            var sut = new DeliveryTimeService(new DeliveryTimeInMemoryRepository());
            await sut.Add(CreateUrgentDeliveryTime("14:00", "18:00"));

            // Act
            var result = await sut.Get(currentTime, 0);

            // Assert
            result.Should().ContainSingle();
            result.Single().Should().BeEquivalentTo(expectedDto);
        }

        private static DeliveryTimeDto CreateDeliveryTimeDto(DeliveryType deliveryType, DateTime start, DateTime finish, bool isAvailable)
        {
            return new DeliveryTimeDto
            {
                Name = "Lunch delivery",
                Description = "Lunch delivery",
                Start = start,
                Finish = finish,
                Type = deliveryType,
                Price = DeliveryPrice,
                Available = isAvailable
            };
        }

        private DeliveryTime CreateUrgentDeliveryTime(string from, string to)
        {
            return new DeliveryTime
            {
                Name = "Lunch delivery",
                Description = "Lunch delivery",
                Start = _startDate,
                Finish = _finishDate,
                Type = DeliveryType.Urgent,
                Price = DeliveryPrice,
                From = from,
                To = to,
                DaysOfWeek = "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday"
            };
        }

        private DeliveryTime CreateRegularDeliveryTime(string from, string to, int closesBeforeMinutes)
        {
            return new DeliveryTime
            {
                Name = "Lunch delivery",
                Description = "Lunch delivery",
                Start = _startDate,
                Finish = _finishDate,
                Type = DeliveryType.Regular,
                Price = DeliveryPrice,
                From = from,
                To = to,
                DaysOfWeek = "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday",
                ClosesBeforeMinutes = closesBeforeMinutes
            };
        }
    }
}
