using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cor.Apt.Controllers
{
    public class SaleRecordController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public SaleRecordController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Update()
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            int _saleRecordId = Convert.ToInt32(HttpContext.Request.Form["SaleRecordId"]);
            int _patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]);
            List<Product> _products = _context.Products.ToList();
            if (!_context.SaleRecordLists.Where(i => i.SaleRecordId == _saleRecordId).Any())
            {
                if (HttpContext.Request.Form["productId"].Count == 0)
                {
                    return RedirectToAction("Remove", "SaleRecord", new { saleRecordId = _saleRecordId, patientId = _patientId });
                }
                else
                {
                    List<SaleRecordList> _saleRecordList = new List<SaleRecordList>();
                    var productIds = HttpContext.Request.Form["productId"];
                    var productPieces = HttpContext.Request.Form["productPiece"];
                    for (int i = 0; i < productIds.Count(); i++)
                    {
                        int _productId = Convert.ToInt32(productIds[i]);
                        SaleRecordList _saleRecordListItem = new SaleRecordList
                        {
                            IsDone = false,
                            IsEndoklinikTest = "Endoklinik",
                            Piece = Convert.ToInt32(productPieces[i]),
                            ProductId = _productId,
                            SaleRecordId = _saleRecordId,
                            Price = _products.Where(i => i.ProductId == _productId).FirstOrDefault().SalePrice
                        };
                        _saleRecordList.Add(_saleRecordListItem);
                    }
                    _context.AddRange(_saleRecordList);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Details", "User", new { patientId = _patientId });
        }
        [HttpPost]
        public IActionResult Set()
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            int _analysisId = Convert.ToInt32(HttpContext.Request.Form["AnalysisId"]);
            int _patientId = Convert.ToInt32(HttpContext.Request.Form["PatientId"]);
            List<AnalysisType> analysisTypes = _context.AnalysisTypes.Where(i => i.AnalysisId == _analysisId).ToList();
            Analysis _analysis = _context.Analyses.Where(i => i.AnalysisId == _analysisId).FirstOrDefault();
            _analysis.Description = HttpContext.Request.Form["Description"];
            foreach (var analysisType in analysisTypes)
            {
                analysisType.IsEndoklinikTest = HttpContext.Request.Form[analysisType.AnalysisTypeId.ToString()].ToString();
            }
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = _patientId });
        }

        public IActionResult Remove(int saleRecordId, int patientId)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master", "Specialist", "Nurse" })) return RedirectToAction("Index", "Auth");
            if (_context.SaleRecords.Any(i => i.SaleRecordId == saleRecordId))
            {
                List<SaleRecordList> _saleRecordList = _context.SaleRecordLists.Where(i => i.SaleRecordId == saleRecordId).ToList();
                _context.RemoveRange(_saleRecordList);
                _context.SaveChanges();
            }
            SaleRecord _saleRecord = _context.SaleRecords.Where(i => i.SaleRecordId == saleRecordId).FirstOrDefault();
            _context.Remove(_saleRecord);
            _context.SaveChanges();
            return RedirectToAction("Details", "User", new { patientId = patientId });
        }
    }
}