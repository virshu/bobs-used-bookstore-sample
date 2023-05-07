﻿using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        public static string GetShoppingCartCorrelationId(this HttpContext context)
        {
            string CookieKey = "ShoppingCartId";

            CookieOptions cookieOptions = new()
            {
                Expires = DateTime.Now.AddYears(1),
                Path = "/"
            };

            string shoppingCartClientId = context.Request.Cookies[CookieKey];

            if (string.IsNullOrWhiteSpace(shoppingCartClientId))
            {
                shoppingCartClientId = context.User.Identity!.IsAuthenticated ? context.User.GetSub() : Guid.NewGuid().ToString();
            }

            context.Response.Cookies.Append(CookieKey, shoppingCartClientId, cookieOptions);

            return shoppingCartClientId;
        }
    }
}