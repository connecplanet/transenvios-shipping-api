using System.Globalization;

namespace Transenvios.Shipping.Api.Domains.CatalogService
{
    public static class DateExtensions
    {
        public static DateTime Convert(this string dateValue)
        {
            DateTime date;
            var validFormats = new[] {
                "yyyyMMdd",
                "MM/dd/yyyy", "MM-dd-yyyy",
                "yyyy/MM/dd", "yyyy-MM-dd",
                "MM/dd/yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss",
                "MM/dd/yyyy hh:mm tt", "MM-dd-yyyy hh:mm tt",
                "yyyy-MM-dd HH:mm:ss, fff", "yyyy/MM/dd HH:mm:ss, fff"
            };

            var provider = new CultureInfo("en-US");

            try
            {
                date = DateTime.ParseExact(dateValue, validFormats, provider);
            }
            catch (FormatException)
            {
                date = DateTime.Today;
            }

            return date;
        }
    }
}
