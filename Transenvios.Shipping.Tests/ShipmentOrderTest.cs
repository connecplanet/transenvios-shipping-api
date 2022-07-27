using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Mediators.ShipmentOrderService.ShipmentOrderPage;
using Xunit;

namespace Transenvios.Shipping.Tests;

public class ShipmentOrderTest
{
    [Fact]
    public void CalculatePriceByWeightTest()
    {
        // Arrange
        var route = new ShipmentRoute
        {
            FromCityCode = "MTR",
            ToCityCode = "MDE",
            InitialKiloPrice = 15000M,
            AdditionalKiloPrice = 1500M
        };
        const decimal weight = 20M;
        const decimal expectedPriceByWeight = 43500M;

        // Act
        ICalculateShipmentPrice mediator = new ShipmentOrderMediator();
        var resultPrice = mediator.CalculatePriceByWeight(route, weight);

        // Assert
        Assert.Equal(resultPrice, expectedPriceByWeight);
    }

    [Fact]
    public void CalculatePriceByVolumeTest()
    {
        // Arrange
        var route = new ShipmentRoute
        {
            FromCityCode = "MTR",
            ToCityCode = "MDE",
            PriceCm3 = 0.3M
        };
        const decimal height = 100M;
        const decimal length = 20M;
        const decimal weight = 20M;
        const decimal expectedPriceByVolume = 12000M;

        // Act
        ICalculateShipmentPrice mediator = new ShipmentOrderMediator();
        var resultPrice = mediator.CalculatePriceByVolume(route, height, length, weight);

        // Assert
        Assert.Equal(resultPrice, expectedPriceByVolume);
    }
}