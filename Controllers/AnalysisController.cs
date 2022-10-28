using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting;

using Cor.Apt.Services.Interfaces;
using Cor.Apt.Entities;

namespace Cor.Apt.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly AppointmentContext _context;
        private readonly IAuthService _authService;
        private readonly IWebHostEnvironment _hostingEnv;

        public AnalysisController(IAuthService authService, AppointmentContext context, IWebHostEnvironment env)
        {
            _context = context;
            _authService = authService;
            _hostingEnv = env;
        }

        public JsonResult Get (int pid) {
            List<Analysis> _analysisRecords = _context.Analyses.Where(i => i.PatientId == pid).OrderBy(i => i.AnalysisDate).ToList();
            return Json(_analysisRecords);
        }
        // Upload method for chunk-upload and normal upload
        public void Save(IList<IFormFile> chunkFile, IList<IFormFile> UploadAnalysis, int pid)
        {
            long size = 0;
            try
            {
                // for chunk-upload
                foreach (var file in chunkFile)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filename = Guid.NewGuid().ToString() + Path.GetExtension(filename);
                    var filepath = _hostingEnv.WebRootPath + $@"\doc\{filename}";
                    size += file.Length;

                    if (AddAnalysisRecord(filename, pid))
                    {
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        using (FileStream fs = System.IO.File.Open(filename, FileMode.Append))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Dosya yükleme başarısız oldu";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }

            // for normal upload
            try
            {
                foreach (var file in UploadAnalysis)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filename = Guid.NewGuid().ToString() + Path.GetExtension(filename);
                    var filepath = _hostingEnv.WebRootPath + $@"\doc\{filename}";
                    size += file.Length;
                    if (AddAnalysisRecord(filename, pid))
                    {
                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Dosya yükleme başarısız oldu";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
        public JsonResult Remove(int analysisId){
            Analysis _analysis = _context.Analyses.Where(i => i.AnalysisId == analysisId).FirstOrDefault();
            if(_analysis != null)
            {
                try
                {
                    _context.Remove(_analysis);
                    _context.SaveChanges();

                    var filename = _hostingEnv.WebRootPath + $@"\doc\{_analysis.AnalysisFolderName}";
                    if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
                }
                catch (Exception e)
                {
                    Response.Clear();
                    Response.StatusCode = 200;
                    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Dosya kaldırma işlemi başarısız oldu";
                    Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
                }
            }
            return Json(analysisId);
        }
        private Boolean AddAnalysisRecord(string filename, int pid)
        {
            try
            {
                Analysis _analysis = new Analysis()
                {
                    AnalysisDate = DateTime.Now,
                    AnalysisFolderName = filename,
                    PatientId = pid
                };
                _context.Add(_analysis);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
    }
}