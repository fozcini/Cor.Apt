using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Controllers
{
    public class SalaryPaymentController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public SalaryPaymentController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<SalaryPayment> DataSource = _context.SalaryPayments.Include(i => i.SalaryPaymentType).Where(i => i.UserId == pid).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<SalaryPayment>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<SalaryPayment> value, int pid) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            SalaryPayment _salaryPayment = new SalaryPayment();
            _salaryPayment.RecordDate = value.Value.RecordDate;
            _salaryPayment.Amount = value.Value.Amount;
            _salaryPayment.SalaryPaymentTypeId = value.Value.SalaryPaymentTypeId;
            _salaryPayment.UserId = pid;
            _context.SalaryPayments.Add(_salaryPayment);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<SalaryPayment> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _salaryPayment = _context.SalaryPayments.Include(i => i.SalaryPaymentType).Where(i => i.SalaryPaymentId == value.Value.SalaryPaymentId).FirstOrDefault();
            if (_salaryPayment != null)
            {
                _salaryPayment.RecordDate = value.Value.RecordDate;
                _salaryPayment.Amount = value.Value.Amount;
                _salaryPayment.SalaryPaymentTypeId = value.Value.SalaryPaymentTypeId;
                _salaryPayment.UserId = value.Value.UserId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<SalaryPayment> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _salaryPayment = _context.SalaryPayments.Where(i => i.SalaryPaymentId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_salaryPayment);
            _context.SaveChanges();
            return Json(value);
        }
    }
}