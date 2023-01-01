using Microsoft.Extensions.Options;
using Moq;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Mediators.ShipmentOrderService;
using Xunit;

namespace Transenvios.Shipping.Tests;

public class ShipmentOrderTest
{
    const decimal ROUTE_INITIAL_KILO_PRICE = 15000M;
    const decimal ROUTE_ADDITIONAL_KILO_PRICE = 1500M;
    const decimal ROUTE_PRICE_CM3 = 0.3M;
    const decimal PACKAGE_WEIGHT = 20.0M;
    const decimal PACKAGE_HEIGHT = 100.0M;
    const decimal PACKAGE_LENGTH = 20.0M;
    const decimal PACKAGE_WIDTH = 20.0M;
    DataContext context;
    Mock<IGetUser> _getUserMock;

    public ShipmentOrderTest()
    {
        _getUserMock = new Mock<IGetUser>();
    }

    [Fact]
    public void Calculate_Payment_Return_WeightCharge()
    {
        // Arrange
        var route = MockShipmentRoute(ROUTE_INITIAL_KILO_PRICE, ROUTE_ADDITIONAL_KILO_PRICE, ROUTE_PRICE_CM3);
        var order = MockShipmentOrderItem(PACKAGE_WEIGHT, PACKAGE_HEIGHT, PACKAGE_LENGTH, PACKAGE_WIDTH);
        
        const decimal expected = 43500M;
        ICalculateShipmentCharges mediator = new ShipmentOrderMediator(AppSettingsMock(), context, _getUserMock.Object);

        // Act
        var actual = mediator.CalculateChargeByWeight(route, order.Weight??0);

        // Assert
        Assert.Equal(actual, expected);
    }

    [Fact]
    public void Calculate_Payment_Return_VolumeCharge()
    {
        var route = MockShipmentRoute(ROUTE_INITIAL_KILO_PRICE, ROUTE_ADDITIONAL_KILO_PRICE, ROUTE_PRICE_CM3);
        var order = MockShipmentOrderItem(PACKAGE_WEIGHT, PACKAGE_HEIGHT, PACKAGE_LENGTH, PACKAGE_WIDTH);
        const decimal expected = 12000M;
        ICalculateShipmentCharges mediator = new ShipmentOrderMediator(AppSettingsMock(), context, _getUserMock.Object);

        var actual = mediator.CalculateChargeByVolume(
            route, order.Height??0, order.Length??0, order.Width??0);

        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData(1, 20, 100, 20, 20, 43500)]
    [InlineData(2, 1, 100, 50, 20, 30000)]
    public void Calculate_Payment_Return_HighestValue(
        int testId, decimal weight, decimal height, decimal length, decimal width, decimal expected)
    {
        var route = MockShipmentRoute(ROUTE_INITIAL_KILO_PRICE, ROUTE_ADDITIONAL_KILO_PRICE, ROUTE_PRICE_CM3);
        var order = MockShipmentOrderItem(weight, height, length, width);
        ICalculateShipmentCharges mediator = new ShipmentOrderMediator(AppSettingsMock(), context, _getUserMock.Object);

        var actual = mediator.CalculateInitialPayment(route, order);

        Assert.True(actual == expected, $"T{testId} Expected/Actual: {expected} / {actual}");
    }

    [Theory]
    [InlineData(1, 0, false, false, 43500, 8265, 51765)]
    [InlineData(2, 0, true, false, 65250, 12397.5, 77647.5)]
    [InlineData(3, 0, false, true, 52200, 9918, 62118)]
    [InlineData(4, 0, true, true, 73950, 14050.5, 88000.5)]
    [InlineData(5, 10000, false, false, 43600, 8284, 51884)]
    [InlineData(6, 10000, true, false, 65350, 12416.5, 77766.5)]
    [InlineData(7, 10000, false, true, 52300, 9937, 62237)]
    [InlineData(8, 10000, true, true, 74050, 14069.5, 88119.5)]
    public void Calculate_Payment_Return_ServiceCost(
        int testId, decimal InsuredValue, bool IsUrgent, bool IsFragile, 
        decimal expectedPrice, decimal expectedTaxes, decimal expectedTotal)
    {
        var route = MockShipmentRoute(ROUTE_INITIAL_KILO_PRICE, ROUTE_ADDITIONAL_KILO_PRICE, ROUTE_PRICE_CM3);
        var order = MockShipmentOrder(PACKAGE_WEIGHT, PACKAGE_HEIGHT, PACKAGE_LENGTH, PACKAGE_WIDTH);
        ICalculateShipmentCharges mediator = new ShipmentOrderMediator(AppSettingsMock(), context, _getUserMock.Object);

        if (order.Items != null)
        {
            order.Items[0].InsuredAmount = InsuredValue;
            order.Items[0].IsUrgent = IsUrgent;
            order.Items[0].IsFragile = IsFragile;
        }
        
        var actual = mediator.CalculateShipmentCharges(route, order);

        Assert.True(actual.BasePrice == expectedPrice, $"T{testId} Price Exp/Act: {expectedPrice} / {actual.BasePrice}");
        Assert.True(actual.Taxes == expectedTaxes, $"T{testId} Taxes Exp/Act: {expectedTaxes} / {actual.Taxes}");
        Assert.True(actual.Total == expectedTotal, $"T{testId} Total Exp/Act: {expectedTotal} / {actual.Total}");
    }

    private static ShipmentRoute MockShipmentRoute(
        decimal? initialKiloPrice = null, decimal? additionalKiloPrice = null, decimal? priceCm3 = null)
    {
        return new ShipmentRoute
        {
            FromCityCode = "MTR",
            ToCityCode = "MDE",
            InitialKiloPrice = initialKiloPrice,
            AdditionalKiloPrice = additionalKiloPrice,
            PriceCm3 = priceCm3
        };
    }

    private static ShipmentOrderItem MockShipmentOrderItem(
        decimal? weight = null, decimal? height = null, decimal? length = null, decimal? width = null) 
    {
        return new ShipmentOrderItem
        {
            Weight = weight,
            Height = height,
            Length = length,
            Width = width
        };
    }

    private static ShipmentOrderRequest MockShipmentOrder(
        decimal? weight = null, decimal? height = null, decimal? length = null, decimal? width = null)
    {
        return new ShipmentOrderRequest
        {
            Items = new ShipmentOrderItem[]
            {
                new ShipmentOrderItem
                {
                    Weight = weight,
                    Height = height,
                    Length = length,
                    Width = width
                }
            }
        };
    }

    private static IOptions<AppSettings> AppSettingsMock()
    {
        IOptions<AppSettings> settings = Options.Create(new AppSettings()
        {
            Shipment = new ShipmentSettings
            {
                InsuredAmountRatio = 1.0M,
                UrgentAmountRatio = 50.0M,
                FragileAmountRatio = 20.0M,
                TaxAmountRatio = 19.0M
            }
        });
        return settings;
    }
}