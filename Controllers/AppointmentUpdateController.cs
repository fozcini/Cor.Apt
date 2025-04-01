using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class AppointmentUpdateController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AppointmentUpdateController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse", "Secretary" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Appointment> DataSource = _context.Appointments.Where(i => i.StartTime >= DateTime.Now && i.AppointmentTypeId != 8).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Appointment>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Update([FromBody] CRUDModel<Appointment> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Secretary" })) return RedirectToAction("Index", "Auth");
            var _appointment = _context.Appointments.Where(i => i.Id == value.Value.Id).FirstOrDefault();
            if (_appointment != null)
            {
                _appointment.PatientName = value.Value.PatientName;
                _appointment.Phone = value.Value.Phone;
                _appointment.Description = value.Value.Description;
                _appointment.StartTime = value.Value.StartTime.AddHours(3);
                _appointment.EndTime = value.Value.EndTime.AddHours(3);
                _appointment.UnitId = value.Value.UnitId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
    }
}