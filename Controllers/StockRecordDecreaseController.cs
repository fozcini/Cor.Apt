using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cor.Apt.Controllers
{
    public class StockRecordDecreaseController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public StockRecordDecreaseController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant", "Specialist" })) return RedirectToAction("Index", "Auth");
            IEnumerable<StockRecordDecrease> DataSource = _context.StockRecordDecreases.Include(i => i.Product).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<StockRecordDecrease>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<StockRecordDecrease> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant", "Specialist" })) return RedirectToAction("Index", "Auth");
            StockRecordDecrease _stockRecordDecrease = new StockRecordDecrease
            {
                Details = value.Value.Details,
                RecordDate = value.Value.RecordDate,
                Piece = value.Value.Piece,
                ProductId = value.Value.ProductId
            };
            Product _product = _context.Products.Where(i => i.ProductId == _stockRecordDecrease.ProductId).FirstOrDefault();
            _product.Piece -= _stockRecordDecrease.Piece;
            _context.StockRecordDecreases.Add(_stockRecordDecrease);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<StockRecordDecrease> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _stockRecordDecrease = _context.StockRecordDecreases.Where(i => i.StockRecordDecreaseId == value.Value.StockRecordDecreaseId).FirstOrDefault();
            Product _product = _context.Products.Where(i => i.ProductId == _stockRecordDecrease.ProductId).FirstOrDefault();
            _product.Piece += _stockRecordDecrease.Piece - value.Value.Piece;
            if (_stockRecordDecrease != null)
            {
                _stockRecordDecrease.Details = value.Value.Details;
                _stockRecordDecrease.RecordDate = value.Value.RecordDate;
                _stockRecordDecrease.Piece = value.Value.Piece;
                _stockRecordDecrease.ProductId = value.Value.ProductId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        public ActionResult Remove([FromBody] CRUDModel<StockRecordDecrease> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant", "Specialist" })) return RedirectToAction("Index", "Auth");
            var _stockRecordDecrease = _context.StockRecordDecreases.Where(i => i.StockRecordDecreaseId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_stockRecordDecrease);
            _context.SaveChanges();
            return Json(value);
        }
    }
}