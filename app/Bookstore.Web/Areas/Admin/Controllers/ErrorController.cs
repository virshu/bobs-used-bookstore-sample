﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class ErrorController : AdminAreaControllerBase
    {
        [Route("/Error/Index/{code:int}")]
        public IActionResult Index(int code)
        {
            IExceptionHandlerPathFeature exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewData["Path"] = exception?.Path;
            ViewData["StatusCode"] = code;
            return View();
        }

        [Route("/error")]
        public IActionResult Support()
        {
            IExceptionHandlerPathFeature exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewData["Path"] = exception?.Path;
            ObjectResult error = Problem();

            ViewData["StatusCode"] = error.StatusCode;
            return View("~/Views/Error/Index.cshtml");
        }
    }
}