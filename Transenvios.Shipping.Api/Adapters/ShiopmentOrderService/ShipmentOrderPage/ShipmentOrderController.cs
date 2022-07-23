using Microsoft.AspNetCore.Mvc;

namespace Transenvios.Shipping.Api.Adapters.ShiopmentOrderService.ShipmentOrderPage
{
    public class ShipmentOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
