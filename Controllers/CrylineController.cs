using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class CrylineController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public CrylineController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Cryline> DataSource = _context.Crylines.Where(i => i.PatientId == pid).OrderByDescending(i => i.RecordDate).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Cryline>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Cryline> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Cryline _cryline = new Cryline();
            _cryline.RecordDate = value.Value.RecordDate;
            _cryline.Area = value.Value.Area;
            _cryline.Seans = value.Value.Seans;
            _cryline.MeasureOnFoot = value.Value.MeasureOnFoot;
            _cryline.MeasureOnBed = value.Value.MeasureOnBed;
            _cryline.PatientId = pid;
            _context.Crylines.Add(_cryline);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Cryline> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _cryline = _context.Crylines.Where(i => i.CrylineId == value.Value.CrylineId).FirstOrDefault();
            if (_cryline != null)
            {
                _cryline.RecordDate = value.Value.RecordDate;
                _cryline.Area = value.Value.Area;
                _cryline.Seans = value.Value.Seans;
                _cryline.MeasureOnFoot = value.Value.MeasureOnFoot;
                _cryline.MeasureOnBed = value.Value.MeasureOnBed;
                _cryline.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Cryline> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _cryline = _context.Crylines.Where(i => i.CrylineId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_cryline);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult UpdateCrylineDescription() // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _patient = _context.Patients.Where(i => i.PatientId == Convert.ToInt32(HttpContext.Request.Form["PatientId"])).FirstOrDefault();
            if (_patient != null)
            {
                _patient.CrylineDescription = HttpContext.Request.Form["CrylineDescription"];
            }
            _context.SaveChanges();
            return RedirectToAction("TreatmentDetails", "User", new { patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]) });
        }
    }
}