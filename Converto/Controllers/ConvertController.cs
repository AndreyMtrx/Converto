using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConvertApiDotNet;
using Converto.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Converto.Controllers
{
    public class ConvertController : Controller
    {
        private IWebHostEnvironment _environment;

        private string secret = Startup.Configuration["Secret"];
        private ConvertApi convertApi => new ConvertApi(secret);

        public ConvertController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpGet]
        public ViewResult WordToPdf()
        {
            return View();
        }
        [HttpPost]
        public async Task<PartialViewResult> WordToPdf(IFormFileCollection files)
        {
            if (files.Count > 0)
            {
                string userGuid = Guid.NewGuid().ToString();
                foreach (IFormFile item in files)
                {
                    string path = Path.Combine(_environment.WebRootPath, $"//Files//{userGuid}//{item.Name}");
                }
            }
            return PartialView("_WordToPdf");
        }
        [HttpPost]
        public PartialViewResult WordFilesInfo(IFormFileCollection files)
        {
            List<FileViewModel> fileViewList = new List<FileViewModel>();
            foreach(IFormFile file in files)
            {
                fileViewList.Add(new FileViewModel() { 
                    FileName = file.FileName,
                    FileSize = file.Length
                });
            }
            ViewBag.fileCount = fileViewList.Count();
            return PartialView("_WordFilesInfo", fileViewList);
        }
    }
}