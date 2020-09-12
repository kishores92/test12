using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GreenPOS.Controllers
{
    public class HomeDesignController : BaseController
    {
        public HomeDesignController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateDesign()
        {
            return View();
        }
    }
}
