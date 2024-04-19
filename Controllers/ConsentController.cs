using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.EJ2.Base;
using System.Text;
using Microsoft.Extensions.Options;
using Cor.Apt.Helpers;
using System.Net.Http;

namespace Cor.Apt.Controllers
{
    public class ConsentController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ConsentController(IAuthService authService, AppointmentContext context, IOptions<AppSettings> appSettings, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _authService = authService;
            _appSettings = appSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult U(int cid)
        {
            ViewBag.ConsentForms = _context.ConsentForms.ToList();
            Consent _consent = _context.Consents.Where(i => i.ConsentId == cid).FirstOrDefault();
            if(_consent.IsApproved) return RedirectToAction("ApproveO");
            else return View(_consent);
        }
        public IActionResult Approve()
        {
            Consent _consent = _context.Consents.Where(i => i.ConsentId == Convert.ToInt32(Request.Form["ConsentId"])).FirstOrDefault();
            if (_consent != null)
            {
                _consent.ApprovedDate = DateTime.Now;
                _consent.IsApproved = true;
                _consent.Log = "SESID:" + HttpContext.Session.Id.ToString() + "_IP:" + HttpContext.Connection.RemoteIpAddress.ToString() + "_PORT:" + HttpContext.Connection.RemotePort.ToString();
                _context.SaveChanges();
            }
            return View();
        }
        public IActionResult ApproveO()
        {
            return View();
        }
        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Consent> DataSource = _context.Consents.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Consent>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult Insert([FromBody] CRUDModel<Consent> value) // Insert the new record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            Consent _consent = new Consent
            {
                RecordDate = value.Value.RecordDate,
                IdentificationNumber = value.Value.IdentificationNumber,
                FullName = value.Value.FullName,
                Phone = value.Value.Phone,
                ConsentFormId = value.Value.ConsentFormId,
                ApprovedDate = value.Value.RecordDate,
                IsApproved = false
            };
            _context.Consents.Add(_consent);
            _context.SaveChanges();
            string link = Request.Scheme + "://" + HttpContext.Request.Host.Value + "/consent/u?cid=" + _consent.ConsentId.ToString();
            if (!SendMessage(value.Value.Phone, link))
            {
                _context.Remove(_consent);
                _context.SaveChanges();
            }

            return Json(_consent);
        }
        public ActionResult Remove([FromBody] CRUDModel<Consent> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User", "Admin", "Master" })) return RedirectToAction("Index", "Auth");
            var _consent = _context.Consents.Where(i => i.ConsentId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_consent);
            _context.SaveChanges();
            return Json(value);
        }
        private bool SendMessage(string phone, string link)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.vatansms.net/");
            var text = "{\"api_id\": \"" + _appSettings.ApiId + "\",\"api_key\": \"" + _appSettings.ApiKey + "\",\"sender\": \"" + _appSettings.Sender + "\",\"message_type\": \"" + _appSettings.MessageType + "\",\"message\":\""+ _appSettings.Message + link +"\",\"message_content_type\":\"" + _appSettings.MessageContentType + "\",\"phones\": [\"" + phone + "\"]}";
            StringContent stringContent = new StringContent(text, Encoding.UTF8, "application/json");

            var result = httpClient.PostAsync("api/v1/1toN", stringContent).Result;

            if ((int)result.StatusCode == 200) return true;
            else return false;
        }
    }
}