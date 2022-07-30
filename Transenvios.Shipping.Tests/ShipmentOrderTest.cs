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
        var resultPrice = mediator.CalculateChargesByWeight(route, order.Weight??0);

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
        var resultPrice = mediator.CalculateChargesByVolume(
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
        var resultPrice = mediator.CalculateBasePrice(route, order);

        Assert.Equal(resultPrice, expectedInitialPrice);
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
    public void CalculatePriceServiceTest(
        int testId, decimal InsuredValue, bool IsUrgent, bool IsFragile, 
        decimal expectedBasePrice, decimal expectedTaxes, decimal expectedTotal)
    {
        var route = ShipmentRouteMock(15000M, 1500M, 0.3M);
        var order = ShipmentOrderMock(20.0M, 100.0M, 20.0M, 20.0M);
        
        if (order.Items != null)
        {
            order.Items[0].InsuredAmount = InsuredValue;
            order.Items[0].IsUrgent = IsUrgent;
            order.Items[0].IsFragile = IsFragile;
        }

        ICalculateShipmentPrice mediator = new ShipmentOrderMediator(AppSettingsMock());
        var result = mediator.CalculatePriceService(route, order);

        Assert.True(result.BasePrice == expectedBasePrice, 
            $"T{testId} Base price Exp/Act: {expectedBasePrice} / {result.BasePrice}");
        Assert.True(result.Taxes == expectedTaxes,
            $"T{testId} Taxes Exp/Act: {expectedTaxes} / {result.Taxes}");
        Assert.True(result.Total == expectedTotal,
            $"T{testId} Total Exp/Act: {expectedTotal} / {result.Total}");
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