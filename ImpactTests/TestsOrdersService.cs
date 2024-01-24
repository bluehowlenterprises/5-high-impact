using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Common;
using Impact.Data;
using Impact.Models;
using Impact.Services;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace ImpactTests
{
    public class TestsOrdersService
    {
        private readonly Fixture fixture;
        private readonly Mock<IDataContext> dataContext;
        private readonly OrdersService ordersService;

        public TestsOrdersService()
        {
            fixture = new Fixture();
            this.dataContext = new Mock<IDataContext>();
            this.ordersService = new OrdersService(this.dataContext.Object);
        }

        [Fact]
        public async Task TestGetOrders()
        {
            // Arrange
            var orders = fixture.CreateMany<Orders>()
                                .AsQueryable()
                                .BuildMockDbSet();

            this.dataContext.Setup(m => m.Orders).Returns(() => orders.Object);

            // Act
            var results = await this.ordersService.GetOrders();

            // Assert
            results.Should().NotBeEmpty();
        }

        [Theory, AutoData]
        public async Task TestGetOrdersById(int id)
        {
            // Arrange
            var orders = fixture.Build<Orders>()
                                .With(c => c.OrdersId, id)
                                .CreateMany()
                                .AsQueryable()
                                .BuildMockDbSet();

            this.dataContext.Setup(m => m.Orders).Returns(() => orders.Object);

            // Act
            var results = await this.ordersService.GetOrdersById(id);

            // Assert
            results.Should().NotBeEmpty();
        }

        [Fact]
        public async Task TestCreateOrder()
        {
            // Arrange
            var order = fixture.Create<Orders>();
            this.dataContext.Setup(m => m.Orders.Add(order));

            // Act
            await ordersService.CreateOrder(order);

            // Assert
            this.dataContext.Verify(m => m.Orders.Add(order), Times.Once);
            this.dataContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task TestUpdateOrder()
        {
            // Arrange
            var order = fixture.Create<Orders>();
            this.dataContext.Setup(m => m.Orders.Update(order));

            // Act
            await ordersService.UpdateOrder(order);

            // Assert
            this.dataContext.Verify(m => m.Orders.Update(order), Times.Once);
            this.dataContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task TestDeleteOrder(int id)
        {
            // Arrange
            var dbEmployee = fixture.Build<Orders>()
                                    .With(c => c.OrdersId, id)
                                    .CreateMany(1)
                                    .AsQueryable()
                                    .BuildMockDbSet();

            dataContext.Setup(m => m.Orders).Returns(() => dbEmployee.Object);

            // Act
            await ordersService.DeleteOrder(id);

            // Assert
            dataContext.Verify(m => m.Orders.Remove(It.IsAny<Orders>()), Times.Once);
            dataContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}