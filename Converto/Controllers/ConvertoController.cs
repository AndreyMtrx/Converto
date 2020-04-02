using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Converto.Controllers
{
    public class ConvertoController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
        public ViewResult About()
        {
            return View();
        }
    }
}