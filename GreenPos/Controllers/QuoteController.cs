using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GreenPOS.Interfaces;
using GreenPOS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenPOS.Controllers
{
    [Route("[controller]/[action]")]
    public class QuoteController : Controller
    {
        private readonly IEmailSender _emailSender;
        private IServiceProvider _serviceProvider;
        private IWebHostEnvironment _env;


        public QuoteController(IEmailSender emailSender,
             IServiceProvider serviceProvider,
            IWebHostEnvironment env)
        {
            _emailSender = emailSender;
            _serviceProvider = serviceProvider;
            _env = env;
        }

        // GET: /<controller>/
        public IActionResult Index(QuotesViewModel vm)
        {
            var quoteModel = new QuoteViewModel { Name = "TestQuote", FileNameOfAttachment = "Test Quote.pdf" };
            var list = new List<FacadeViewModel>();
            list.Add(new FacadeViewModel
            {
                Name = "Test A/c name",
                Cost = 24900,
                ImageUrl = "image url"
            });
            list.Add(new FacadeViewModel
            {
                Name = "Test A/c name2",
                Cost = 25100,
                ImageUrl = "image url"
            });
            list.Add(new FacadeViewModel
            {
                Name = "Test A/c name3",
                Cost = 26900,
                ImageUrl = "image url"
            });
            list.Add(new FacadeViewModel
            {
                Name = "Test A/c name4",
                Cost = 29900,
                ImageUrl = "image url"
            });
            list.Add(new FacadeViewModel
            {
                Name = "Test A/c name5",
                Cost = 24900,
                ImageUrl = "image url"
            });
            list.Add(new FacadeViewModel
            {
                Name = "Test A/c name6",
                Cost = 25100,
                ImageUrl = "image url"
            });

            quoteModel.Facades = list;


            var listPackages = new List<PackageViewModel>();
            listPackages.Add(new PackageViewModel
            {
                Name = "Test A/c name",
                Cost = 634900,
                ImageUrl = "image url"
            });
            listPackages.Add(new PackageViewModel
            {
                Name = "Test A/c name2",
                Cost = 614990,
                ImageUrl = "image url"
            });
            listPackages.Add(new PackageViewModel
            {
                Name = "Test A/c name3",
                Cost = 849000,
                ImageUrl = "image url"
            });
            listPackages.Add(new PackageViewModel       
            {
                Name = "Test A/c name4",
                Cost = 611900,
                ImageUrl = "image url"
            });
            listPackages.Add(new PackageViewModel
            {
                Name = "Test A/c name5",
                Cost = 598990,
                ImageUrl = "image url"
            });
            listPackages.Add(new PackageViewModel
            {
                Name = "Test A/c name6",
                Cost = 799000,
                ImageUrl = "image url"
            });

            var listInclusions = new List<InclusionViewModel>();
            listInclusions.Add(new InclusionViewModel
            {
                Name = "Standard Inclusions",
                Cost = 0,
                ImageUrl = "/images/standard.jpg"
            });
            listInclusions.Add(new InclusionViewModel
            {
                Name = "Gold Inclusions",
                Cost = 61499,
                ImageUrl = "/images/gold.jpg"
            });
            listInclusions.Add(new InclusionViewModel
            {
                Name = "Platinum Inclusions",
                Cost = 71499,
                ImageUrl = "/images/platinum.jpg"
            });

            var listDesigns = new List<DesignViewModel>();
            listDesigns.Add(new DesignViewModel
            {
                Name = "Test A/c name",
                Cost = 634900,
                ImageUrl = "image url"
            });
            listDesigns.Add(new DesignViewModel
            {
                Name = "Test A/c name2",
                Cost = 614990,
                ImageUrl = "image url"
            });
            listDesigns.Add(new DesignViewModel
            {
                Name = "Test A/c name3",
                Cost = 849000,
                ImageUrl = "image url"
            });
            listDesigns.Add(new DesignViewModel
            {
                Name = "Test A/c name4",
                Cost = 611900,
                ImageUrl = "image url"
            });
            listDesigns.Add(new DesignViewModel
            {
                Name = "Test A/c name5",
                Cost = 598990,
                ImageUrl = "image url"
            });

            var listPromotions = new List<PromotionViewModel>();
            listPromotions.Add(new PromotionViewModel
            {
                Name = "Break Free Promotion",
                Cost = -1850,
                ImageUrl = "/images/promo1.jpg"
            });
            listPromotions.Add(new PromotionViewModel
            {
                Name = "Spring Promotion",
                Cost =  -2500,
                ImageUrl = "/images/promo2.jpg"
            });

            quoteModel.Packages = listPackages;
            quoteModel.Inclusions = listInclusions;
            quoteModel.Designs = listDesigns;
            quoteModel.Promotions = listPromotions;

           // var htmmlString = RenderPartialViewToString("QuotePdfTemplate", vm);

            return View("QuotePdfTemplate", vm);
        }


        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.DisplayName;

            var pngBinaryData = System.IO.File.ReadAllBytes($"{_env.WebRootPath}/images/background01.jpg");
            var ImgDataURI = @"data:image/png;base64," + Convert.ToBase64String(pngBinaryData);

            ViewData.Model = model;
            ViewData["MainImagePath"] = ImgDataURI;
            using (StringWriter sw = new StringWriter())
            {
                var engine = _serviceProvider.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine; // Resolver.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = engine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions() //Added this parameter in
                );

                //Everything is async now!
                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();

                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendEnquiryEmail(QuotesViewModel quoteModel)
        {
            if (quoteModel.Documents == null)
            {
                quoteModel.Documents = new List<DocumentViewModel>();
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_quoteEmailPartial", quoteModel);
            }

            await _emailSender.SendEmailAsync(quoteModel);
            TempData["SuccessMessage"] = "Your enquiry has been sent successfully.";
            return PartialView("_quoteEmailPartial", quoteModel);
        }
    }
}
