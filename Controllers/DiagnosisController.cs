using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class DiagnosisController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public DiagnosisController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Diagnosis> DataSource = _context.Diagnoses.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Diagnosis>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Diagnosis> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            Diagnosis _diagnosis = new Diagnosis();
            _diagnosis.DiagnosisCode = value.Value.DiagnosisCode;
            _diagnosis.DiagnosisName = value.Value.DiagnosisName;
            _context.Diagnoses.Add(_diagnosis);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Diagnosis> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _diagnosis = _context.Diagnoses.Where(i => i.DiagnosisId == value.Value.DiagnosisId).FirstOrDefault();
            if (_diagnosis != null)
            {
                _diagnosis.DiagnosisCode = value.Value.DiagnosisCode;
                _diagnosis.DiagnosisName = value.Value.DiagnosisName;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Diagnosis> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _diagnosis = _context.Diagnoses.Where(i => i.DiagnosisId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_diagnosis);
            _context.SaveChanges();
            return Json(value);
        }
    }
}