using ConvertApiDotNet;
using ConvertApiDotNet.Model;
using Converto.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            string conversionGuid = Guid.NewGuid().ToString();
            if (files.Count > 0)
            {
                await UploadFiles(files, conversionGuid);
                await ConvertFiles("docx", "pdf", conversionGuid);
            }
            return PartialView("_WordToPdf");
        }

        [HttpPost]
        public PartialViewResult WordFilesInfo(IFormFileCollection files)
        {
            List<FileViewModel> fileViewList = new List<FileViewModel>();
            foreach (IFormFile file in files)
            {
                fileViewList.Add(new FileViewModel()
                {
                    FileName = file.FileName,
                    FileSize = file.Length
                });
            }

            ViewBag.fileCount = fileViewList.Count();
            return PartialView("_WordFilesInfo", fileViewList);
        }

        private async Task UploadFiles(IFormFileCollection files, string conversionGuid)
        {
            string directoryPath = Path.Combine(_environment.ContentRootPath, $"wwwroot\\Files\\{conversionGuid}");
            string convertedPath = Path.Combine(directoryPath, "Converted");
            Directory.CreateDirectory(directoryPath);
            Directory.CreateDirectory(convertedPath);

            foreach (IFormFile file in files)
            {
                string fileUploadpath = Path.Combine(directoryPath, file.FileName);
                using (FileStream stream = new FileStream(fileUploadpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }

        private async Task ConvertFiles(string fromFormat, string toFormat, string conversionGuid)

        {
            string filesToConvertPath = Path.Combine(_environment.ContentRootPath, $"wwwroot\\Files\\{conversionGuid}");
            string convertedFilesPath = Path.Combine(filesToConvertPath, "Converted");
            string[] filePaths = Directory.GetFiles(filesToConvertPath);

            foreach (string filePath in filePaths)
            {
                ConvertApiResponse convertResponse = await convertApi.ConvertAsync(fromFormat, toFormat,
                    new ConvertApiFileParam("File", filePath),
                    new ConvertApiParam("FileName", Path.GetFileNameWithoutExtension(filePath))
                );

                await convertResponse.SaveFilesAsync(convertedFilesPath);
            }
        }
    }
}