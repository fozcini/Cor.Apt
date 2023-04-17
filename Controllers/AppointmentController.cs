using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AppointmentController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public ActionResult Load()  // Here we get the Start and End Date and based on that can filter the data and return to Scheduler
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            return Json(_context.Appointments.Include(i => i.AppointmentType).Include(i => i.Unit).ToList());
        }
        [HttpPost]
        public ActionResult Update([FromBody] EditParams param)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            if (param.action == "insert" || (param.action == "batch" && param.added.Count > 0)) // this block of code will execute while inserting the appointments
            {
                var value = (param.action == "insert") ? param.value : param.added[0];
                DateTime startTime = Convert.ToDateTime(value.StartTime);
                DateTime endTime = Convert.ToDateTime(value.EndTime);
                Appointment appointment = new Appointment()
                {
                    PatientName = value.PatientName,
                    StartTime = startTime.AddHours(3),
                    EndTime = endTime.AddHours(3),
                    Description = value.Description,
                    AppointmentTypeId = value.AppointmentTypeId,
                    UnitId = value.UnitId
                };
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
            }
            if (param.action == "update" || (param.action == "batch" && param.changed.Count > 0)) // this block of code will execute while updating the appointment
            {
                var value = (param.action == "update") ? param.value : param.changed[0];
                var filterData = _context.Appointments.Where(c => c.Id == Convert.ToInt32(value.Id));
                if (filterData.Count() > 0)
                {
                    DateTime startTime = Convert.ToDateTime(value.StartTime);
                    DateTime endTime = Convert.ToDateTime(value.EndTime);
                    Appointment appointment = _context.Appointments.Single(A => A.Id == Convert.ToInt32(value.Id));
                    appointment.PatientName = value.PatientName;
                    appointment.StartTime = startTime.AddHours(3);
                    appointment.EndTime = endTime.AddHours(3);
                    appointment.Description = value.Description;
                    appointment.AppointmentTypeId = value.AppointmentTypeId;
                    appointment.UnitId = value.UnitId;
                }
                _context.SaveChanges();
            }
            if (param.action == "remove" || (param.action == "batch" && param.deleted.Count > 0)) // this block of code will execute while removing the appointment
            {
                if (param.action == "remove")
                {
                    int key = Convert.ToInt32(param.key);
                    Appointment appointment = _context.Appointments.Where(c => c.Id == key).FirstOrDefault();
                    if (appointment != null) _context.Appointments.Remove(appointment);
                }
                else
                {
                    foreach (var apps in param.deleted)
                    {
                        Appointment appointment = _context.Appointments.Where(c => c.Id == apps.Id).FirstOrDefault();
                        if (appointment != null) _context.Appointments.Remove(appointment);
                    }
                }
                _context.SaveChanges();
            }
            var data = _context.Appointments.ToList();
            return Json(data);
        }

        public class EditParams
        {
            public string key { get; set; }
            public string action { get; set; }
            public List<Appointment> added { get; set; }
            public List<Appointment> changed { get; set; }
            public List<Appointment> deleted { get; set; }
            public Appointment value { get; set; }
        }
    }
}