using Microsoft.AspNetCore.Mvc;
using Transenvios.Shipping.Api.Adapters.UserService.AuthorizationEntity;

namespace Transenvios.Shipping.Api.Adapters.ShiopmentOrderService.ShipmentOrderPage
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
