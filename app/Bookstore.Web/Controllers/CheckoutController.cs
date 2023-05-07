using System.Collections.Generic;
using Bookstore.Domain.Addresses;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Orders;
using Bookstore.Web.Helpers;
using Bookstore.Web.ViewModel.Checkout;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IAddressService addressService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IOrderService orderService;

        public CheckoutController(IShoppingCartService shoppingCartService,
                                  IOrderService orderService,
                                  IAddressService addressService)
        {
            this.shoppingCartService = shoppingCartService;
            this.orderService = orderService;
            this.addressService = addressService;
        }

        public async Task<IActionResult> Index()
        {
            ShoppingCart shoppingCart = await shoppingCartService.GetShoppingCartAsync(HttpContext.GetShoppingCartCorrelationId());
            IEnumerable<Address> addresses = await addressService.GetAddressesAsync(User.GetSub());

            return View(new CheckoutIndexViewModel(shoppingCart, addresses));
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutIndexViewModel model)
        {
            CreateOrderDto dto = new CreateOrderDto(User.GetSub(), HttpContext.GetShoppingCartCorrelationId(), model.SelectedAddressId);

            int orderId = await orderService.CreateOrderAsync(dto);

            return RedirectToAction("Finished", new { orderId });
        }

        public async Task<IActionResult> Finished(int orderId)
        {
            Order order = await orderService.GetOrderAsync(orderId);

            return View(new CheckoutFinishedViewModel(order));
        }
    }
}
