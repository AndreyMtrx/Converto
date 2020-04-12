using ConvertApiDotNet;
using ConvertApiDotNet.Model;
using Converto.Data;
using Converto.Data.Models;
using Converto.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Converto.Controllers
{
    public class ConvertController : Controller
    {
        private ApplicationDbContext dbContext;
        private IWebHostEnvironment _environment;

        private string secret = Startup.Configuration["Secret"];
        private ConvertApi convertApi => new ConvertApi(secret);

        public ConvertController(IWebHostEnvironment environment,ApplicationDbContext context)
        {
            _environment = environment;
            dbContext = context;
        }
        public PhysicalFileResult GetFile(string fileName, string conversionGuid)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, $"wwwroot\\Files\\{conversionGuid}\\Converted\\{fileName}");
            string mimeType = MimeTypes.GetMimeType(filePath);
            return PhysicalFile(filePath, mimeType, fileName);
        }
        [HttpPost]
        public PartialViewResult FilesInfo(IFormFileCollection files)
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
            return PartialView("_FilesInfo", fileViewList);
        }

        public PartialViewResult ConversionProcess()
        {
            return PartialView("_ConversionProcess");
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
        private List<FileViewModel> GetFilesViewModel(string conversionGuid)
        {
            List<FileViewModel> filesVM = new List<FileViewModel>();
            string convertedFilesPath = Path.Combine(_environment.ContentRootPath, $"wwwroot\\Files\\{conversionGuid}\\Converted");
            foreach (string filePath in Directory.GetFiles(convertedFilesPath))
            {
                filesVM.Add(new FileViewModel()
                {
                    FileName = Path.GetFileName(filePath),
                    ConversionGuid = conversionGuid
                });
            }
            return filesVM;
        }
        private async Task SaveConversionInfo(IFormFileCollection files, string conversionGuid, string fromFormat, string toFormat)
        {
            foreach (IFormFile file in files)
            {
                Conversion conversion = new Conversion()
                {
                    ConversionGuid = conversionGuid,
                    FileName = file.FileName,
                    FromFormat = fromFormat,
                    ToFormat = toFormat
                };
                dbContext.Add(conversion);
            }
            await dbContext.SaveChangesAsync();
        }
        [HttpGet]
        public ViewResult WordToPdf()
        {
            ViewBag.ToFormat = "PDF";
            return View("WordTo");
        }

        [HttpPost]
        public async Task<PartialViewResult> WordToPdf(IFormFileCollection files)
        {
            string conversionGuid = Guid.NewGuid().ToString();

            await UploadFiles(files, conversionGuid);
            await ConvertFiles("docx", "pdf", conversionGuid);

            List<FileViewModel> filesVM = GetFilesViewModel(conversionGuid);

            await SaveConversionInfo(files, conversionGuid, "docx", "pdf");

            return PartialView("_ConversionResult", filesVM);
        }
        [HttpGet]
        public ViewResult WordToJpg()
        {
            ViewBag.ToFormat = "JPG";
            return View("WordTo");
        }

        [HttpPost]
        public async Task<PartialViewResult> WordToJpg(IFormFileCollection files)
        {
            string conversionGuid = Guid.NewGuid().ToString();

            await UploadFiles(files, conversionGuid);
            await ConvertFiles("docx", "jpg", conversionGuid);

            List<FileViewModel> filesVM = GetFilesViewModel(conversionGuid);

            await SaveConversionInfo(files, conversionGuid, "docx", "jpg");

            return PartialView("_ConversionResult", filesVM);
        }

        [HttpGet]
        public ViewResult WordToTxt()
        {
            ViewBag.ToFormat = "TXT";
            return View("WordTo");
        }

        [HttpPost]
        public async Task<PartialViewResult> WordToTxt(IFormFileCollection files)
        {
            string conversionGuid = Guid.NewGuid().ToString();

            await UploadFiles(files, conversionGuid);
            await ConvertFiles("docx", "txt", conversionGuid);

            List<FileViewModel> filesVM = GetFilesViewModel(conversionGuid);

            await SaveConversionInfo(files, conversionGuid, "docx", "txt");

            return PartialView("_ConversionResult", filesVM);
        }
    }
}