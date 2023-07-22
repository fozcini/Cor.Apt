using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public AnalysisController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Analysis> DataSource = _context.Analyses.Where(i => i.PatientId == pid).OrderByDescending(i => i.AnalysisDate).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Analysis>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Analysis> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Analysis _analysis = new Analysis();
            _analysis.Description = value.Value.Description;
            _analysis.AnalysisDate = value.Value.AnalysisDate;
            _analysis.PatientId = pid;
            _context.Analyses.Add(_analysis);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Analysis> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _analysis = _context.Analyses.Where(i => i.AnalysisId == value.Value.AnalysisId).FirstOrDefault();
            if (_analysis != null)
            {
                _analysis.Description = value.Value.Description;
                _analysis.AnalysisDate = value.Value.AnalysisDate;
                _analysis.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Analysis> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _analysis = _context.Analyses.Where(i => i.AnalysisId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_analysis);
            _context.SaveChanges();
            return Json(value);
        }
    }
}