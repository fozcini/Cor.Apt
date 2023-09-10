using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cor.Apt.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public ProductController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }
        public IActionResult Get([FromBody] DataManagerRequest dm, int pid)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Product> DataSource = _context.Products.Include(i => i.ProductType).ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Product>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Product> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant" })) return RedirectToAction("Index", "Auth");
            Product _product = new Product
            {
                ProductName = value.Value.ProductName,
                Piece = value.Value.Piece,
                ProductTypeId = value.Value.ProductTypeId
            };
            _context.Products.Add(_product);
            _context.SaveChanges();
            return Json(value);
        }
        public IActionResult Update([FromBody] CRUDModel<Product> value) // Update record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant" })) return RedirectToAction("Index", "Auth");
            var _product = _context.Products.Where(i => i.ProductId == value.Value.ProductId).FirstOrDefault();
            if (_product != null)
            {
                _product.ProductName = value.Value.ProductName;
                _product.Piece = value.Value.Piece;
                _product.ProductTypeId = value.Value.ProductTypeId;
            }
            _context.SaveChanges();
            return Json(value.Value);
        }
        // public ActionResult Remove([FromBody] CRUDModel<Type> value) // Remove record 
        // {
        //     if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Accountant" })) return RedirectToAction("Index", "Auth");
        //     var _type = _context.Types.Where(i => i.TypeId == int.Parse(value.Key.ToString())).FirstOrDefault();
        //     _context.Remove(_type);
        //     _context.SaveChanges();
        //     return Json(value);
        // }
    }
}