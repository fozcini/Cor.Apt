using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;
using Syncfusion.EJ2.Schedule;

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
            ViewBag.AppointmentTypes = _context.AppointmentTypes.ToList();
            ViewBag.Units = _context.Units.ToList();
            ViewBag.Resources = new string[] { "Units" };
            return View();
        }

        public IActionResult Patient()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.SocialSecurities = _context.SocialSecurities.ToList();
            ViewBag.DiscountTypes = _context.DiscountTypes.ToList();
            return View();
        }

        public IActionResult Details(int patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            Patient patient = _context.Patients.Include(i => i.DiscountType).Include(i => i.SocialSecurity).Where(i => i.PatientId == patientId).FirstOrDefault();
            if (_context.Anamnesises.Where(i => i.PatientId == patientId).Any()) ViewBag.Anamnesis = _context.Anamnesises.Where(i => i.PatientId == patientId).FirstOrDefault();
            else ViewBag.Anamnesis = new Anamnesis() { };
            
            ViewBag.PatientName = patient.FullName;
            ViewBag.IdentificationNumber = patient.IdentificationNumber;
            ViewBag.Age = (int) ((DateTime.Now - patient.BirthDate).TotalDays/365.242199);
            ViewBag.Phone = patient.Phone;

            ViewBag.RadiologyTypes = _context.RadiologyTypes.ToList();
            ViewBag.DecisionAndTracingTypes = _context.DecisionAndTracingTypes.ToList();
            ViewBag.Users = _context.Users.ToList();

            ViewBag.Analyses = _context.Analyses.Where(i => i.PatientId == patientId).OrderByDescending(i => i.AnalysisDate).ToList();
            ViewBag.AnalysisTypes = _context.AnalysisTypes.Where(i => i.Analysis.PatientId == patientId).ToList();
            ViewBag.Types = _context.Types.ToList();

            ViewBag.RadiologyRequests = _context.RadiologyRequests.Where(i => i.PatientId == patientId).OrderByDescending(i => i.RadiologyRequestDate).ToList();
            ViewBag.RadiologyRequestTypes = _context.RadiologyRequestTypes.Where(i => i.RadiologyRequest.PatientId == patientId).ToList();
            ViewBag.RadiologyRequestTypeLists = _context.RadiologyRequestTypeLists.ToList();

            ViewBag.SaleRecords = _context.SaleRecords.Where(i => i.PatientId == patientId).OrderByDescending(i => i.SaleRecordDate).ToList();
            ViewBag.SaleRecordLists = _context.SaleRecordLists.Include(i=> i.SaleRecord).Where(i => i.SaleRecord.PatientId == patientId).ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(patient);
        }
        
        public IActionResult Treatment()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.SocialSecurities = _context.SocialSecurities.ToList();
            ViewBag.DiscountTypes = _context.DiscountTypes.ToList();
            return View();
        }

        public IActionResult TreatmentDetails(int patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            Patient patient = _context.Patients.Include(i => i.DiscountType).Include(i => i.SocialSecurity).Where(i => i.PatientId == patientId).FirstOrDefault();
            if (_context.Anamnesises.Where(i => i.PatientId == patientId).Any()) ViewBag.Anamnesis = _context.Anamnesises.Where(i => i.PatientId == patientId).FirstOrDefault();
            else ViewBag.Anamnesis = new Anamnesis() { };
            
            ViewBag.PatientName = patient.FullName;
            ViewBag.IdentificationNumber = patient.IdentificationNumber;
            ViewBag.Age = (int) ((DateTime.Now - patient.BirthDate).TotalDays/365.242199);
            ViewBag.Phone = patient.Phone;
            
            return View(patient);
        }

        public IActionResult Users()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.Patients = _context.Patients.ToList();
            return View();
        }

        public IActionResult UserDetails(int userId)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            User user = _context.Users.Where(i => i.UserId == userId).FirstOrDefault();
            ViewBag.LeaveTypes = _context.LeaveTypes.ToList();
            ViewBag.SalaryPaymentTypes = _context.SalaryPaymentTypes.ToList();
            return View(user);
        }

        public IActionResult Applications()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.ApplicationTypes = _context.ApplicationTypes.ToList();
            return View();
        }

        public IActionResult Analysis(int? analysisId, int? patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.Types = _context.Types.ToList();
            if (analysisId == null && patientId != null)
            {
                Analysis _analysis = new Analysis() {
                    AnalysisDate = System.DateTime.Now,
                    PatientId = (int)patientId
                };
                _context.Add(_analysis);
                _context.SaveChanges();
                analysisId = _analysis.AnalysisId;
            }
            ViewBag.Analysis = _context.Analyses.Where(i => i.AnalysisId == analysisId).FirstOrDefault();
            return View(_context.AnalysisTypes.Where(i => i.AnalysisId == analysisId).ToList());
        }

        public IActionResult RadiologyRequest(int? radiologyRequestId, int? patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.RadiologyRequestTypeLists = _context.RadiologyRequestTypeLists.ToList();
            if (radiologyRequestId == null && patientId != null)
            {
                RadiologyRequest _radiologyRequest = new RadiologyRequest() {
                    RadiologyRequestDate = System.DateTime.Now,
                    PatientId = (int)patientId
                };
                _context.Add(_radiologyRequest);
                _context.SaveChanges();
                radiologyRequestId = _radiologyRequest.RadiologyRequestId;
            }
            ViewBag.RadiologyRequest = _context.RadiologyRequests.Where(i => i.RadiologyRequestId == radiologyRequestId).FirstOrDefault();
            return View(_context.RadiologyRequestTypes.Where(i => i.RadiologyRequestId == radiologyRequestId).ToList());
        }

        public IActionResult SaleRecord(int? saleRecordId, int? patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            ViewBag.Products = _context.Products.ToList();
            if (saleRecordId == null && patientId != null)
            {
                SaleRecord _saleRecord = new SaleRecord() {
                    SaleRecordDate = System.DateTime.Now,
                    PatientId = (int)patientId
                };
                _context.Add(_saleRecord);
                _context.SaveChanges();
                saleRecordId = _saleRecord.SaleRecordId;
            }
            ViewBag.PatientId = patientId;
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.SaleRecord = _context.SaleRecords.Where(i => i.SaleRecordId == saleRecordId).FirstOrDefault();
            return View();
        }

        public IActionResult Accounting()
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            return View();
        }
    }
}