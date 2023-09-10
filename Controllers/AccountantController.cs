using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;

namespace Cor.Apt.Controllers
{
    public class AccountantController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AccountantController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        public IActionResult Index()
        {
            if (!_authService.UserIsValid(new List<string> { "Accountant" })) return RedirectToAction("Index", "Auth");
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View();
        }
    }
}