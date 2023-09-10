using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;

namespace Cor.Apt.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AdminController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        public IActionResult Index()
        {
            if (!_authService.UserIsValid(new List<string> { "Admin" })) return RedirectToAction("Index", "Auth");
            return View();
        }
        public IActionResult Analysis()
        {
            if (!_authService.UserIsValid(new List<string> { "Admin" })) return RedirectToAction("Index", "Auth");
            return View();
        }
        public IActionResult Radiology()
        {
            if (!_authService.UserIsValid(new List<string> { "Admin" })) return RedirectToAction("Index", "Auth");
            return View();
        }
        public IActionResult Appointment()
        {
            if (!_authService.UserIsValid(new List<string> { "Admin" })) return RedirectToAction("Index", "Auth");
            return View(_context.AppointmentTypes.ToList());
        }
    }
}