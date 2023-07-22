using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class AndulationController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AndulationController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Andulation> DataSource = _context.Andulations.Where(i => i.PatientId == pid).OrderByDescending(i => i.RecordDate).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Andulation>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Andulation> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Andulation _andulation = new Andulation();
            _andulation.RecordDate = value.Value.RecordDate;
            _andulation.Session = value.Value.Session;
            _andulation.Program = value.Value.Program;
            _andulation.IsDone = false;
            _andulation.PatientId = pid;
            _context.Andulations.Add(_andulation);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Andulation> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _andulation = _context.Andulations.Where(i => i.AndulationId == value.Value.AndulationId).FirstOrDefault();
            if (_andulation != null)
            {
                _andulation.RecordDate = value.Value.RecordDate;
                _andulation.Session = value.Value.Session;
                _andulation.Program = value.Value.Program;
                _andulation.IsDone = value.Value.IsDone;
                _andulation.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Andulation> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _andulation = _context.Andulations.Where(i => i.AndulationId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_andulation);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult UpdateAndulationDescription() // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _patient = _context.Patients.Where(i => i.PatientId == Convert.ToInt32(HttpContext.Request.Form["PatientId"])).FirstOrDefault();
            if (_patient != null)
            {
                _patient.AndulationDescription = HttpContext.Request.Form["AndulationDescription"];
            }
            _context.SaveChanges();
            return RedirectToAction("TreatmentDetails", "User", new { patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]) });
        }
    }
}