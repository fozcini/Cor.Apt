using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;
using Syncfusion.EJ2.Base;

namespace Cor.Apt.Controllers
{
    public class SurveyController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;

        public SurveyController(IAuthService authService, AppointmentContext context)
        {
            _context = context;
            _authService = authService;
        }

        public IActionResult Index()
        {
            if (!_authService.UserIsValid(new List<string> { "Survey" })) return RedirectToAction("Index", "Auth");
            return View();
        }

        [HttpPost]
        public IActionResult Insert()
        {
            if (!_authService.UserIsValid(new List<string> { "Survey", "User" })) return RedirectToAction("Index", "Auth");
            Survey survey = new Survey()
            {
                SurveyDate = DateTime.Now,
                AnswerOne = HttpContext.Request.Form["AnswerOne"],
                AnswerTwo = HttpContext.Request.Form["AnswerTwo"],
                AnswerThree = HttpContext.Request.Form["AnswerThree"],
                AnswerFour = HttpContext.Request.Form["AnswerFour"],
                AnswerFive = HttpContext.Request.Form["AnswerFive"],
                AnswerSix = HttpContext.Request.Form["AnswerSix"],
                AnswerSeven = HttpContext.Request.Form["AnswerSeven"],
                AnswerEight = HttpContext.Request.Form["AnswerEight"],
                AnswerNine = HttpContext.Request.Form["AnswerNine"],
                AnswerTen = HttpContext.Request.Form["AnswerTen"],
                IsFirstTime = HttpContext.Request.Form["AnswerEleven"],
                Gender = HttpContext.Request.Form["Gender"],
                DateYear = HttpContext.Request.Form["Age"],
                IsMarried = HttpContext.Request.Form["IsMarried"],
                Profession = HttpContext.Request.Form["Profession"],
                Description = HttpContext.Request.Form["Description"]
            };
            _context.Surveys.Add(survey);
            _context.SaveChanges();
            return RedirectToAction("Index", "Survey");
        }

        public IActionResult Get([FromBody] DataManagerRequest dm)
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            IEnumerable<Survey> DataSource = _context.Surveys.ToList();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0) DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            if (dm.Sorted != null && dm.Sorted.Count > 0) DataSource = operation.PerformSorting(DataSource, dm.Sorted); //Sorting
            if (dm.Where != null && dm.Where.Count > 0) DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator); //Filtering
            int count = DataSource.Cast<Survey>().Count();
            if (dm.Skip != 0) DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            if (dm.Take != 0) DataSource = operation.PerformTake(DataSource, dm.Take);
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public ActionResult Remove([FromBody] CRUDModel<Survey> value) // Remove record 
        {
            if (!_authService.UserIsValid(new List<string> { "User" })) return RedirectToAction("Index", "Auth");
            var _survey = _context.Surveys.Where(i => i.SurveyId == int.Parse(value.Key.ToString())).FirstOrDefault();
            _context.Remove(_survey);
            _context.SaveChanges();
            return Json(value);
        }
    }
}