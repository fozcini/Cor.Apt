using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class DecisionAndTracingController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public DecisionAndTracingController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<DecisionAndTracing> DataSource = _context.DecisionAndTracings.Where(i => i.PatientId == pid).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<DecisionAndTracing>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<DecisionAndTracing> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            DecisionAndTracing _decisionAndTracing = new DecisionAndTracing();
            _decisionAndTracing.Description = value.Value.Description;
            _decisionAndTracing.RecordDate = value.Value.RecordDate;
            _decisionAndTracing.UserId = value.Value.UserId;
            _decisionAndTracing.DecisionAndTracingTypeId = value.Value.DecisionAndTracingTypeId;
            _decisionAndTracing.PatientId = pid;
            _context.DecisionAndTracings.Add(_decisionAndTracing);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<DecisionAndTracing> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _decisionAndTracing = _context.DecisionAndTracings.Where(i => i.DecisionAndTracingId == value.Value.DecisionAndTracingId).FirstOrDefault();
            if (_decisionAndTracing != null)
            {
                _decisionAndTracing.RecordDate = value.Value.RecordDate;
                _decisionAndTracing.Description = value.Value.Description;
                _decisionAndTracing.UserId = value.Value.UserId;
                _decisionAndTracing.DecisionAndTracingTypeId = value.Value.DecisionAndTracingTypeId;
                _decisionAndTracing.PatientId = value.Value.PatientId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<DecisionAndTracing> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _decisionAndTracing = _context.DecisionAndTracings.Where(i => i.DecisionAndTracingId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_decisionAndTracing);
            _context.SaveChanges();
            return Json(value);
        }
    }
}