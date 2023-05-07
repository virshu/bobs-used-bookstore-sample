﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Web.Helpers;
using Bookstore.Domain.Customers;
using Bookstore.Domain.Carts;
using Bookstore.Web.ViewModel.Wishlist;

namespace Bookstore.Web.Controllers;

[AllowAnonymous]
public class WishlistController : Controller
{
    private readonly ICustomerService customerService;
    private readonly IShoppingCartService shoppingCartService;

    public WishlistController(ICustomerService customerService, IShoppingCartService shoppingCartService)
    {
        this.customerService = customerService;
        this.shoppingCartService = shoppingCartService;
    }

    public async Task<IActionResult> Index()
    {
        ShoppingCart shoppingCart = await shoppingCartService.GetShoppingCartAsync(HttpContext.GetShoppingCartCorrelationId());

        return View(new WishlistIndexViewModel(shoppingCart));
    }

    [HttpPost]
    public async Task<IActionResult> MoveToShoppingCart(int shoppingCartItemId)
    {
        MoveWishlistItemToShoppingCartDto dto = new(HttpContext.GetShoppingCartCorrelationId(), shoppingCartItemId);

        await shoppingCartService.MoveWishlistItemToShoppingCartAsync(dto);

        this.SetNotification("Item moved to shopping cart");

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> MoveAllItemsToShoppingCart()
    {
        MoveAllWishlistItemsToShoppingCartDto dto = new(HttpContext.GetShoppingCartCorrelationId());

        await shoppingCartService.MoveAllWishlistItemsToShoppingCartAsync(dto);

        this.SetNotification("All items moved to shopping cart");

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int shoppingCartItemId)
    {
        DeleteShoppingCartItemDto dto = new(HttpContext.GetShoppingCartCorrelationId(), shoppingCartItemId);

        await shoppingCartService.DeleteShoppingCartItemAsync(dto);

        this.SetNotification("Item removed from wishlist");

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error()
    {
        return View();
    }
}