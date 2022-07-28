using Microsoft.Extensions.Options;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Mediators.ShipmentOrderService.ShipmentOrderPage;
using Xunit;

namespace Transenvios.Shipping.Tests;

public class ShipmentOrderTest
{
    [Fact]
    public void CalculatePriceByWeightTest()
    {
        // Arrange
        var route = ShipmentRouteMock(15000M, 1500M, 0.3M);
        var order = ShipmentOrderItemMock(20.0M, 100.0M, 20.0M, 20.0M);
        const decimal expectedPriceByWeight = 43500M;
        
        // Act
        ICalculateShipmentPrice mediator = new ShipmentOrderMediator(AppSettingsMock());
        var resultPrice = mediator.CalculatePriceByWeight(route, order.Weight??0);

        // Assert
        Assert.Equal(resultPrice, expectedPriceByWeight);
    }

    [Fact]
    public void CalculatePriceByVolumeTest()
    {
        var route = ShipmentRouteMock(15000M, 1500M, 0.3M);
        var order = ShipmentOrderItemMock(20.0M, 100.0M, 20.0M, 20.0M);
        const decimal expectedPriceByVolume = 12000M;

        ICalculateShipmentPrice mediator = new ShipmentOrderMediator(AppSettingsMock());
        var resultPrice = mediator.CalculatePriceByVolume(
            route, order.Height??0, order.Length??0, order.Width??0);

        Assert.Equal(resultPrice, expectedPriceByVolume);
    }

    [Fact]
    public void CalculateInitialPriceTest()
    {
        var route = ShipmentRouteMock(15000M, 1500M, 0.3M);
        var order = ShipmentOrderItemMock(20.0M, 100.0M, 20.0M, 20.0M);
        const decimal expectedInitialPrice = 43500M;

        ICalculateShipmentPrice mediator = new ShipmentOrderMediator(AppSettingsMock());
        var resultPrice = mediator.CalculateInitialPrice(route, order);

        Assert.Equal(resultPrice, expectedInitialPrice);
    }

    [Theory]
    // TODO: [InlineData(null, false, false)]
    // TODO: [InlineData(25000M, true, false)]
    [InlineData(25000, true, true, 161200, 30628, 191828)]
    public void CalculatePriceServiceTest(
        decimal InsuredValue, bool IsUrgent, bool IsFragile, 
        decimal expectedInitialPrice, decimal expectedTaxes, decimal expectedTotalPrice)
    {
        var route = ShipmentRouteMock(15000M, 1500M, 0.3M);
        var order = ShipmentOrderMock(20.0M, 100.0M, 20.0M, 20.0M);
        
        if (order.Items != null)
        {
            order.Items[0].InsuredValue = InsuredValue;
            order.Items[0].IsUrgent = IsUrgent;
            order.Items[0].IsFragile = IsFragile;
        }

        ICalculateShipmentPrice mediator = new ShipmentOrderMediator(AppSettingsMock());
        var resultPrice = mediator.CalculatePriceService(route, order);

        Assert.Equal(resultPrice.InitialPrice, expectedInitialPrice);
        Assert.Equal(resultPrice.Taxes, expectedTaxes);
        Assert.Equal(resultPrice.Total, expectedTotalPrice);
    }

    private static ShipmentRoute ShipmentRouteMock(
        decimal? initialPrice = null, decimal? additionPrice = null, decimal? priceCm3 = null)
    {
        return new ShipmentRoute
        {
            FromCityCode = "MTR",
            ToCityCode = "MDE",
            InitialKiloPrice = initialPrice,
            AdditionalKiloPrice = additionPrice,
            PriceCm3 = priceCm3
        };
    }

    private static ShipmentOrderItem ShipmentOrderItemMock(
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

    private static ShipmentOrderRequest ShipmentOrderMock(
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