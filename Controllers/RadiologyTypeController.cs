using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class RadiologyTypeController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public RadiologyTypeController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<RadiologyType> DataSource = _context.RadiologyTypes.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<RadiologyType>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<RadiologyType> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            RadiologyType _radiologyType = new RadiologyType();
            _radiologyType.RadiologyTypeName = value.Value.RadiologyTypeName;
            _context.RadiologyTypes.Add(_radiologyType);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<RadiologyType> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _radiologyType = _context.RadiologyTypes.Where(i => i.RadiologyTypeId == value.Value.RadiologyTypeId).FirstOrDefault();
            if (_radiologyType != null)
            {
                _radiologyType.RadiologyTypeName = value.Value.RadiologyTypeName;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<RadiologyType> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _radiologyType = _context.RadiologyTypes.Where(i => i.RadiologyTypeId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_radiologyType);
            _context.SaveChanges();
            return Json(value);
        }
    }
}