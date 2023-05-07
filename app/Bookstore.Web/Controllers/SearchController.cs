﻿using System.Threading.Tasks;
using Bookstore.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Web.Helpers;
using Bookstore.Domain.Books;
using Bookstore.Domain.Carts;
using Bookstore.Web.ViewModel.Search;

namespace Bookstore.Web.Controllers;

[AllowAnonymous]
public class SearchController : Controller
{
    private readonly IBookService inventoryService;
    private readonly IShoppingCartService shoppingCartService;

    public SearchController(IBookService inventoryService, IShoppingCartService shoppingCartService)
    {
        this.inventoryService = inventoryService;
        this.shoppingCartService = shoppingCartService;
    }

    public async Task<IActionResult> Index(string searchString, string sortBy = "Name", int pageIndex = 1, int pageSize = 10)
    {
        IPaginatedList<Book> books = await inventoryService.GetBooksAsync(searchString, sortBy, pageIndex, pageSize);

        return View(new SearchIndexViewModel(books));
    }

    public async Task<IActionResult> Details(int id)
    {
        Book book = await inventoryService.GetBookAsync(id);

        return View(new SearchDetailsViewModel(book));
    }

    public async Task<IActionResult> AddItemToShoppingCart(int bookId)
    {
        AddToShoppingCartDto dto = new(HttpContext.GetShoppingCartCorrelationId(), bookId, 1);

        await shoppingCartService.AddToShoppingCartAsync(dto);

        this.SetNotification("Item added to shopping cart");

        return RedirectToAction("Index", "Search");
    }

    public async Task<IActionResult> AddItemToWishlist(int bookId)
    {
        AddToWishlistDto dto = new(HttpContext.GetShoppingCartCorrelationId(), bookId);

        await shoppingCartService.AddToWishlistAsync(dto);

        this.SetNotification("Item added to wishlist");

        return RedirectToAction("Index", "Search");
    }
}