using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class BodyAnalysisController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public BodyAnalysisController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse", "Secretary" })) return RedirectToAction("Index", "Auth");
            IEnumerable<BodyAnalysis> DataSource = _context.BodyAnalyses.Where(i => i.PatientId == pid).OrderByDescending(i => i.RecordDate).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<BodyAnalysis>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<BodyAnalysis> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            BodyAnalysis _bodyAnalysis = new BodyAnalysis();
            _bodyAnalysis.RecordDate = value.Value.RecordDate;
            _bodyAnalysis.Height = value.Value.Height;
            _bodyAnalysis.Weight = value.Value.Weight;
            _bodyAnalysis.Bmi = value.Value.Bmi;
            _bodyAnalysis.Fat = value.Value.Fat;
            _bodyAnalysis.Fm = value.Value.Fm;
            _bodyAnalysis.Mm = value.Value.Mm;
            _bodyAnalysis.Tbw = value.Value.Tbw;
            _bodyAnalysis.PatientId = pid;
            _context.BodyAnalyses.Add(_bodyAnalysis);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<BodyAnalysis> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            var _bodyAnalysis = _context.BodyAnalyses.Where(i => i.BodyAnalysisId == value.Value.BodyAnalysisId).FirstOrDefault();
            if (_bodyAnalysis != null)
            {
                _bodyAnalysis.RecordDate = value.Value.RecordDate;
                _bodyAnalysis.Height = value.Value.Height;
                _bodyAnalysis.Weight = value.Value.Weight;
                _bodyAnalysis.Bmi = value.Value.Bmi;
                _bodyAnalysis.Fat = value.Value.Fat;
                _bodyAnalysis.Fm = value.Value.Fm;
                _bodyAnalysis.Mm = value.Value.Mm;
                _bodyAnalysis.Tbw = value.Value.Tbw;
                _bodyAnalysis.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<BodyAnalysis> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            var _bodyAnalysis = _context.BodyAnalyses.Where(i => i.BodyAnalysisId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_bodyAnalysis);
            _context.SaveChanges();
            return Json(value);
        }
    }
}