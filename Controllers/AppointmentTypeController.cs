using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;
using System;

namespace Cor.Apt.Controllers
{
    public class AppointmentTypeController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AppointmentTypeController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Update(AppointmentType value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            if (value.AppointmentTypeId == 0)
            {
                AppointmentType _appointmentType = new AppointmentType();
                _appointmentType.AppointmentTypeName = value.AppointmentTypeName;
                _appointmentType.Color = value.Color;
                _context.AppointmentTypes.Add(_appointmentType);
            }
            else
            {
                AppointmentType _appointmentType = _context.AppointmentTypes.Where(i => i.AppointmentTypeId == value.AppointmentTypeId).FirstOrDefault();
                _appointmentType.AppointmentTypeName = value.AppointmentTypeName;
                _appointmentType.Color = value.Color;
            }
            _context.SaveChanges();
            return RedirectToAction("Appointment", "Admin");
        }
    }
}