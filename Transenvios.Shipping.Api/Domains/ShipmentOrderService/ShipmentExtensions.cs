using System.ComponentModel;

namespace Transenvios.Shipping.Api.Domains.ShipmentOrderService
{
    public static class ShipmentExtensions
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
    }
}
