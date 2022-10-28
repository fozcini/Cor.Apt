using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Cor.Apt.Helpers;
using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;

namespace Cor.Apt.Controllers
{
    public class UserController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public UserController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        public IActionResult Index()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            return View();
        }

        public IActionResult Appointment()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.Patients = _context.Patients.ToList();
            return View();
        }
        public IActionResult Andulation()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.Patients = _context.Patients.ToList();
            return View();
        }

        public IActionResult Patient()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            return View();
        }

        public IActionResult Details(int patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            Patient patient = _context.Patients.Where(i => i.PatientId == patientId).FirstOrDefault();
            if (_context.Anamnesises.Where(i => i.PatientId == patientId).Any()) ViewBag.Anamnesis = _context.Anamnesises.Where(i => i.PatientId == patientId).FirstOrDefault();
            else ViewBag.Anamnesis = new Anamnesis() { };
            ViewBag.Analyses = _context.Analyses.Where(i => i.PatientId == patientId).ToList();
            ViewBag.WeightRecords = _context.WeightRecords.Where(i => i.PatientId == patientId).ToList();
            return View(patient);
        }
        public IActionResult Ozone()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.Patients = _context.Patients.ToList();
            return View();
        }
    }
}