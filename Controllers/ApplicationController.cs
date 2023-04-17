using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public ApplicationController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Application> DataSource = _context.Applications.Include(i => i.ApplicationType).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Application>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Application> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Application _application = new Application();
            _application.FullName = value.Value.FullName;
            _application.BirthDate = value.Value.BirthDate;
            _application.Phone = value.Value.Phone;
            _application.Cv = value.Value.Cv;
            _application.PhotoLink = "";
            _application.ApplicationTypeId = value.Value.ApplicationTypeId;
            _context.Applications.Add(_application);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Application> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _application = _context.Applications.Include(i => i.ApplicationType).FirstOrDefault();
            if (_application != null)
            {
                _application.FullName = value.Value.FullName;
                _application.BirthDate = value.Value.BirthDate;
                _application.Phone = value.Value.Phone;
                _application.Cv = value.Value.Cv;
                _application.PhotoLink = "";
                _application.ApplicationTypeId = value.Value.ApplicationTypeId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<Application> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _application = _context.Applications.Where(i => i.ApplicationId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_application);
            _context.SaveChanges();
            return Json(value);
        }
    }
}