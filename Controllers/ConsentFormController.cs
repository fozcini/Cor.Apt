using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class ConsentFormController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public ConsentFormController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            IEnumerable<ConsentForm> DataSource = _context.ConsentForms.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<ConsentForm>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<ConsentForm> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            ConsentForm _consentForm = new ConsentForm
            {
                RecordDate = value.Value.RecordDate,
                ConsentFormName = value.Value.ConsentFormName,
                Details = value.Value.Details
            };
            _context.ConsentForms.Add(_consentForm);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<ConsentForm> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _consentForm = _context.ConsentForms.Where(i => i.ConsentFormId == value.Value.ConsentFormId).FirstOrDefault();
            if (_consentForm != null)
            {
                _consentForm.RecordDate = value.Value.RecordDate;
                _consentForm.ConsentFormName = value.Value.ConsentFormName;
                _consentForm.Details = value.Value.Details;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<ConsentForm> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _consentForm = _context.ConsentForms.Where(i => i.ConsentFormId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_consentForm);
            _context.SaveChanges();
            return Json(value);
        }
    }
}