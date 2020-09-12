using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using GreenPOS.Interfaces;
using GreenPOS.Models;
using GreenPOS.Service;
using IronPdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using ControllerContext = Microsoft.AspNetCore.Mvc.ControllerContext;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using ViewEngineResult = Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult;
using GreenPOS.Mapper;
using Microsoft.AspNetCore.Http;
using System.Web;
using GreenPOS.Common;
using Microsoft.Extensions.Options;

namespace GreenPOS.Controllers
{
    public class QuotesController : BaseController
    {
        private IQuoteService _service;
        private IContactService _contactService;
        // private readonly IDocGeneration _docGeneration;
        private readonly IEmailSender _emailSender;

        private IServiceProvider _serviceProvider;
        private IWebHostEnvironment _env;
        private readonly IAzureStorageService _azureStorageService;
        private readonly AppSettings _appSettings;


        public QuotesController(
            IQuoteService service,
            // IDocGeneration docGeneration,
            IEmailSender emailSender,
            IServiceProvider serviceProvider,
            IWebHostEnvironment env,
            IAzureStorageService azureStorageService,
            IContactService contactService,
            IOptions<AppSettings> appSettings)
        {
            _service = service;
            // _docGeneration = docGeneration;
            _emailSender = emailSender;
            _serviceProvider = serviceProvider;
            _env = env;
            _azureStorageService = azureStorageService;
            _contactService = contactService;
            _appSettings = appSettings.Value;
        }
        public async Task<IActionResult> Index(string type)
        {
            //type = "tender";
            //string? ttype = "notender";
            bool isTender = (type == "tender");
            var result = await _service.SearchAsync(isTender);
           

            ViewBag.quotes = result;
            ViewBag.requestType = type;
            ViewBag.CreateQuoteCss = isTender ? "hidden" : "visible";
            return View();
        }
        //  [Route("Quotes/TenderSearchAsync")]
        public async Task<IActionResult> TenderSearc()
        {

            var result = await _service.SearchTenderAsync();
            ViewBag.quotes = result;
            return View();
        }

        public async Task<IActionResult> CreateQuote()
        {
            var facades = await _service.GetFacadesAsync();
            var promotions = await _service.GetPromotionsAsync();
            var packages = await _service.GetPackagesAsync();
            var inclusions = await _service.GetInclusionsAsync();
            var houseDesigns = await _service.GetHouseDesignsAsync();
            var inclusionDetails = await _service.GetInclusionDetailsAsync();

            return View(new QuotesViewModel(facades, promotions, houseDesigns, packages, inclusions, inclusionDetails, new List<DocumentViewModel>()));
        }

        [HttpGet]
        [Route("Quotes/GetQuotesAsync/{id}")]
        public async Task<IActionResult> GetQuotesAsync(long id)
        {
            var result = await _service.GetQuoteByIdAsync(id);
            return View("CreateQuote", result);
        }


        [HttpGet]
        [Route("Quotes/CreateQuoteForContactAsync/{id}")]
        public async Task<IActionResult> CreateQuoteForContactAsync(long id)
        {
            var vm = await _contactService.GetContactAsync(id, UserId);
            var facades = await _service.GetFacadesAsync();
            var promotions = await _service.GetPromotionsAsync();
            var packages = await _service.GetPackagesAsync();
            var inclusions = await _service.GetInclusionsAsync();
            var houseDesigns = await _service.GetHouseDesignsAsync();
            var inclusionDetails = await _service.GetInclusionDetailsAsync();
            var quoteModel = new QuotesViewModel(facades, promotions, houseDesigns, packages, inclusions, inclusionDetails, new List<DocumentViewModel>());
            quoteModel.CustomerFirstName = vm.FirstName;
            quoteModel.CustomerLastName = vm.LastName;
            quoteModel.CustomerEmail = vm.Email;
            quoteModel.CustomerAddress = $"{vm.MailingStreet} {vm.City} {vm.MailingState} {vm.MailingZip} {vm.State}";
            quoteModel.CustomerContact = vm.Phone;
            var newQuoteID = await _service.SaveAsync(quoteModel, UserId);
            return RedirectToAction("GetQuotesAsync", new { id = newQuoteID });
        }

        [HttpGet]
        [Route("Quotes/DeleteQuoteAsync/{id}")]
        public async Task<IActionResult> DeleteQuoteAsync(long id)
        {
            var result = await _service.DeleteQuoteAsync(id, UserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Quotes/SaveQuoteAsync")]
        public async Task<IActionResult> SaveQuoteAsync(QuotesViewModel vm, IEnumerable<DesignViewModel> dvm)
        {
            var _dvm = dvm;
            await _service.SaveAsync(vm, UserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Quotes/ConvertQuoteAsync")]
        public async Task<IActionResult> ConvertQuoteAsync(QuotesViewModel vm)
        {
            var data = await _service.GetQuoteByIdAsync(vm.Id);
            data.IsTender = true;
            await _service.SaveAsync(data, UserId);
            return RedirectToAction("Index", new { type = "tender" });
        }

        [HttpPost]
        [Route("Quotes/PartialDeleteQuote")]
        public async Task<IActionResult> PartialDeleteQuote(QuotesViewModel quoteModel)
        {
            var documentName = Request.Form["deleteID"][0];
            quoteModel.Documents = await _service.GetDocuments(quoteModel.Id);
            var documentToDelete = quoteModel.Documents.Where(d => d.Name == documentName).FirstOrDefault();
            await _azureStorageService.DeleteDocAsync(documentToDelete.Name);
            await _service.DeleteDocumentAsync(documentToDelete);
            quoteModel.Documents = await _service.GetDocuments(quoteModel.Id);
            return PartialView("_quoteEmailPartial", quoteModel);
        }

        [HttpPost]
        [Route("Quotes/PartialDeleteVariation")]
        public async Task<IActionResult> PartialDeleteVariation(QuotesViewModel quoteModel)
        {
            try
            {
                var inclusionID = Request.Form["deleteID"][0];
                await _service.DeleteInclustionAsync(Convert.ToInt32(inclusionID));
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            var result = await _service.GetQuoteByIdAsync(quoteModel.Id);
            return PartialView("_inclusionVariationPartial", result);
        }

        [HttpPost]
        [Route("Quotes/PartialStandardVariation")]
        public async Task<IActionResult> PartialStandardVariation(QuotesViewModel quoteModel)
        {
            await SaveQuoteInternalAsync(quoteModel);
            var subCategoryId = Request.Form["SubCategoryId"][0];
            var description = Request.Form["VariationDescription"][0];
            var category = Request.Form["VariationCategory"][0];
            var costVariation = Request.Form["CostVariation"][0];
            InclusionDetailViewModel inclusionViewDetail = null;
            if (!string.IsNullOrEmpty(subCategoryId)
                && !string.IsNullOrEmpty(description)
                && !string.IsNullOrEmpty(subCategoryId))
            {
                inclusionViewDetail = new InclusionDetailViewModel
                {
                    Category = category,
                    SubCategory = subCategoryId,
                    Description = description,
                    Cost = string.IsNullOrEmpty(costVariation) ? 0 : Convert.ToDecimal(costVariation),
                    QuoteId = Convert.ToInt32(quoteModel.Id)
                };
                if (quoteModel.AdditionalInclusions == null)
                {
                    quoteModel.AdditionalInclusions = new List<InclusionDetailViewModel>();
                }
                quoteModel.AdditionalInclusions.Add(inclusionViewDetail);
            }
            await _service.SaveVariationInclusionAsync(inclusionViewDetail);
            var result = await _service.GetQuoteByIdAsync(quoteModel.Id);
            return PartialView("_inclusionVariationPartial", result);
        }

        [HttpPost]
        [Route("Quotes/PartialUploadCustomPdf")]
        public async Task<IActionResult> PartialUploadCustomPdf()
        {
            var dict = HttpUtility.ParseQueryString(Request.Form["quoteModel"][0]);
            string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
            var quoteModel = JsonConvert.DeserializeObject<QuotesViewModel>(json);
            var pdfFile = Request.Form.Files[0];
            if (Path.GetExtension(pdfFile.FileName).ToLower() == ".pdf"
                && pdfFile.Length <= (20 * 1048576))
            {
                string fileName = quoteModel.JobNumber + "-custom-" + Guid.NewGuid().ToString() + ".pdf";
                await _azureStorageService.UploadDocAsync(pdfFile.OpenReadStream(), fileName);
                quoteModel.PdfUrl = fileName;
                await _service.SaveCustomPdfAsync(quoteModel);
                var result = await _service.GetQuoteByIdAsync(quoteModel.Id);
                return PartialView("_inclusionVariationPartial", result);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error .. file type should be pdf and size less than 20M ..");
            }

        }

        [HttpPost]
        [Route("Quotes/PartialDeleteCustomPdf")]
        public async Task<IActionResult> PartialDeleteCustomPdf(QuotesViewModel quoteModel)
        {
            try
            {
                var fileName = Request.Form["deleteID"][0];
                await _azureStorageService.DeleteDocAsync(fileName);
                await _service.DeleteCustomPdfAsync(quoteModel);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            var result = await _service.GetQuoteByIdAsync(quoteModel.Id);
            return PartialView("_inclusionVariationPartial", result);
        }

        [HttpPost]
        [Route("Quotes/PartialEnquiryEmail")]
        public async Task<IActionResult> PartialEnquiryEmail(QuotesViewModel quoteModel)
        {
            var facades = await _service.GetFacadesAsync();
            var promotions = await _service.GetPromotionsAsync();
            var packages = await _service.GetPackagesAsync();
            var inclusions = await _service.GetInclusionsAsync();
            var houseDesigns = await _service.GetHouseDesignsAsync();
            var inclusionDetails = await _service.GetInclusionDetailsAsync();

            quoteModel.EmailCC = Request.Form["EmailCC"][0];
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error .. Please check email format ..");
            }


            quoteModel.SetMasterData(facades, promotions, houseDesigns, packages, inclusions, inclusionDetails);
            DesignViewModel houseDesign = quoteModel.HouseDesignId.HasValue ? quoteModel.HouseDesigns.First(x => x.Id == quoteModel.HouseDesignId.Value) : quoteModel.HouseDesigns.First();
            FacadeViewModel facade = quoteModel.FacadeId.HasValue ? quoteModel.Facades.First(x => x.Id == quoteModel.FacadeId.Value) : quoteModel.Facades.First();
            InclusionViewModel inclusion = quoteModel.InclusionId.HasValue ? quoteModel.Inclusions.First(x => x.Id == quoteModel.InclusionId.Value) : quoteModel.Inclusions.First();

            quoteModel.HouseDesignName = houseDesign.Name;
            quoteModel.InclusionName = inclusion.Name;
            quoteModel.FacadeName = facade.Name;
            quoteModel.BasePrice = houseDesign.Cost;
            quoteModel.Documents = await _service.GetDocuments(quoteModel.Id);
            if (quoteModel.Documents == null)
            {
                quoteModel.Documents = new List<DocumentViewModel>();
            }
            await _emailSender.SendEmailAsync(quoteModel);
            TempData["SuccessMessage"] = "Your enquiry has been sent successfully.";
            return PartialView("_quoteEmailPartial", quoteModel);
        }

        [HttpPost]
        [Route("Quotes/PartialGenerateQuote")]
        public async Task<IActionResult> PartialGenerateQuote(QuotesViewModel quoteModel)
        {

            quoteModel.Email = quoteModel.CustomerEmail;
            await SaveQuoteInternalAsync(quoteModel);

            quoteModel.Documents = await _service.GetDocuments(quoteModel.Id);

            var docsFounds = quoteModel.Documents.Where(x => x.Title.ToLower().Contains(quoteModel.RequestTypeLower)).ToList();
            if (docsFounds != null && docsFounds.Count >= 2)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json($"Max limit reached: maximum of 2 {quoteModel.RequestType} allowed per contact, please delete an existing {quoteModel.RequestType}  to create new one..");
            }
            var vmReturn = await GeneratePdfDocument(quoteModel);
            TempData["SuccessMessage"] = "Your {quoteModel.RequestType}  has been generated successfully.";
            vmReturn.Documents = await _service.GetDocuments(vmReturn.Id);
            vmReturn.Email = vmReturn.CustomerEmail;
            return PartialView("_quoteEmailPartial", vmReturn);
        }

        private IActionResult Json(object p, object allowGet)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> SendEnquiryEmail(QuotesViewModel quoteModel)
        {

            if (Request.Form["SendEnquiryEmail"].Count > 0)
            {
                var facades = await _service.GetFacadesAsync();
                var promotions = await _service.GetPromotionsAsync();
                var packages = await _service.GetPackagesAsync();
                var inclusions = await _service.GetInclusionsAsync();
                var houseDesigns = await _service.GetHouseDesignsAsync();
                var inclusionDetails = await _service.GetInclusionDetailsAsync();

                quoteModel.SetMasterData(facades, promotions, houseDesigns, packages, inclusions, inclusionDetails);


                DesignViewModel houseDesign = quoteModel.HouseDesignId.HasValue ? quoteModel.HouseDesigns.First(x => x.Id == quoteModel.HouseDesignId.Value) : quoteModel.HouseDesigns.First();
                FacadeViewModel facade = quoteModel.FacadeId.HasValue ? quoteModel.Facades.First(x => x.Id == quoteModel.FacadeId.Value) : quoteModel.Facades.First();
                InclusionViewModel inclusion = quoteModel.InclusionId.HasValue ? quoteModel.Inclusions.First(x => x.Id == quoteModel.InclusionId.Value) : quoteModel.Inclusions.First();

                quoteModel.HouseDesignName = houseDesign.Name;
                quoteModel.InclusionName = inclusion.Name;
                quoteModel.FacadeName = facade.Name;
                quoteModel.BasePrice = houseDesign.Cost;
                quoteModel.Documents = await _service.GetDocuments(quoteModel.Id);
                if (quoteModel.Documents == null)
                {
                    quoteModel.Documents = new List<DocumentViewModel>();
                }
                await _emailSender.SendEmailAsync(quoteModel);
                TempData["SuccessMessage"] = "Your enquiry has been sent successfully.";

                return View("CreateQuote", quoteModel);
            }
            else
            {
                var vmReturn = await GeneratePdfDocument(quoteModel);
                TempData["SuccessMessage"] = "Your quote has been generated successfully.";
                return View("CreateQuote", vmReturn);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDoc(QuotesViewModel vm)
        {
            var vmReturn = await GeneratePdfDocument(vm);
            return View("CreateQuote", vmReturn);
        }

        private async Task<QuotesViewModel> GeneratePdfDocument(QuotesViewModel vm)
        {
            //var doc = @"https://docs.google.com/document/d/1Lm4WZMynzfOwjsFTFF7SLDvKYfb2AG6YRfpc9tFcepc";
            // Response.Redirect($"{doc}");
            //if (!ModelState.IsValid)
            //{
            //    return PartialView("_quoteEmailPartial", quoteModel);
            //}



            var facades = await _service.GetFacadesAsync();
            var promotions = await _service.GetPromotionsAsync();
            var packages = await _service.GetPackagesAsync();
            var inclusions = await _service.GetInclusionsAsync();
            var houseDesigns = await _service.GetHouseDesignsAsync();
            var inclusionDetails = await _service.GetInclusionDetailsAsync();
            var additionalInclusionDetails = await _service.GetAdditionalInclusionDetailsAsync(vm.Id);

            vm.SetMasterData(facades, promotions, houseDesigns, packages, inclusions, inclusionDetails, additionalInclusionDetails);


            DesignViewModel houseDesign = vm.HouseDesignId.HasValue ? vm.HouseDesigns.First(x => x.Id == vm.HouseDesignId.Value) : vm.HouseDesigns.First();
            FacadeViewModel facade = vm.FacadeId.HasValue ? vm.Facades.First(x => x.Id == vm.FacadeId.Value) : vm.Facades.First();
            InclusionViewModel inclusion = vm.InclusionId.HasValue ? vm.Inclusions.First(x => x.Id == vm.InclusionId.Value) : vm.Inclusions.First();

            vm.HouseDesignName = houseDesign.Name;
            vm.InclusionName = inclusion.Name;
            vm.FacadeName = facade.Name;
            vm.BasePrice = houseDesign.Cost;
            vm.FacadeCost = facade.Cost == 0 ? "Included" : facade.Cost.ToString("C0");
            var fileName = vm.JobNumber + "-" + Guid.NewGuid().ToString() + ".pdf";

            var masterdata = inclusionDetails.ToTableData(vm.BasePrice, facade.Cost, vm.AdditionalInclusions); // await _service.GetContractDetails();
            vm.TableData = masterdata;


            var htmmlString = RenderPartialViewToString("QuotePdfTemplate", vm);
            IronPdf.License.LicenseKey = _appSettings.IronPdfKey;
            var Renderer = new IronPdf.HtmlToPdf();
            // Build a footer using html to style the text
            // mergable fields are:
            // {page} {total-pages} {url} {date} {time} {html-title} & {pdf-title}

            Renderer.PrintOptions.Footer = new HtmlHeaderFooter()
            {
                Height = 30,
                HtmlFragment = "<center><div class=\"stl_01\" style=\"top: 64.9321em;white-space:nowrap;left:25.3123em;\"><span class=\"stl_20\">{page}</span></div></center>" +
                "<center><div class=\"stl_01\" style=\"top: 66.1712em; left: 13.3647em; \"><span class=\"stl_21 stl_10\" style=\"word - spacing:0.02em; \">(Test Company Pty Ltd) T/A   Pty Ltd | ABN 12 345 678 900|</span><span class=\"stl_21 stl_10\" style=\"word-spacing:0.03em;\">company address</span>" +
                "<span class=\"stl_21 stl_10\" style=\"word-spacing:0.05em;\"></span></div></center>" +
                "<center><div class=\"stl_01\" style=\"top: 67.0469em; left:18.9945em;z-index:801;\"><span class=\"stl_21 stl_10\" style=\"word-spacing:0.27em;\">Ph: (02)</span>" +
                "<span class=\"stl_21 stl_10\" style=\"word-spacing:0.02em;\">&nbsp;8630</span><span class=\"stl_21 stl_10\" style=\"word-spacing:0.02em;\">&nbsp;8996 |</span>" +
                "<span class=\"stl_21 stl_10\" style=\"word-spacing:0em;\">&nbsp;</span><a href = \"company url\" target=\"_blank\"><span class=\"stl_22\" style=\"word-spacing:0em;\">​</span></a>" +
                "<a href = \"company url\" target=\"_blank\"><span class=\"stl_23 stl_10\" style=\"word-spacing:0em;\">www.testcompany<span class=\"stl_14\">.com.au &nbsp;</span></span></a></div></center>" +
                "<div class=\"stl_01\" style=\"top: 67.7975em; margin-left:3.7308em;\"><span class=\"stl_21 stl_14\" style=\"word-spacing:0em;\">Client Initial........../<span class=\"stl_10\">.......... &nbsp;</span></span></div>" +
                "<div class=\"stl_01\" style=\"top: 68.6107em; margin-left:3.7308em;\"><span class=\"stl_21 stl_10\" style=\"word-spacing:0.03em;\">Test Company - © 2020 &nbsp;</span></div>",
                DrawDividerLine = true
            };

            // Build a header using an image asset
            // Note the use of BaseUrl to set a relative path to the assets
            //if cover page un comment below line
            Renderer.PrintOptions.FirstPageNumber = 2;

            Renderer.PrintOptions.Header = new HtmlHeaderFooter()
            {
                Height = 30,
                HtmlFragment = System.IO.File.ReadAllText($"{_env.WebRootPath}/templates/header.html"),
                BaseUrl = $"{_env.WebRootPath}/templates",
                Spacing = 5,
                DrawDividerLine = true,
                LoadStylesAndCSSFromMainHtmlDocument = true
            };

            Renderer.PrintOptions.Zoom = 100;
            Renderer.PrintOptions.FitToPaperWidth = true;
            Renderer.PrintOptions.MarginTop = 0; //millimenters
            Renderer.PrintOptions.MarginLeft = 0; //millimenters
            Renderer.PrintOptions.MarginRight = 0; //millimenters
            Renderer.PrintOptions.MarginBottom = 50;
           

            var coverPageRenderer = new IronPdf.HtmlToPdf();
            coverPageRenderer.PrintOptions.Zoom = 100;
            coverPageRenderer.PrintOptions.FitToPaperWidth = true;
            coverPageRenderer.PrintOptions.MarginTop = 0; //millimenters
            coverPageRenderer.PrintOptions.MarginLeft = 0; //millimenters
            coverPageRenderer.PrintOptions.MarginRight = 0; //millimenters
            Renderer.PrintOptions.MarginBottom = 0;
            var coverpageHtml = System.IO.File.ReadAllText($"{_env.WebRootPath}/templates/coverpage1.html").Replace("{{Name}}", vm.Name).Replace("{{JobNumber}}", vm.JobNumber).Replace("{{RequestType}}", vm.RequestType);
            var coverpage = coverPageRenderer.RenderHtmlAsPdf(coverpageHtml, $"{_env.WebRootPath}/templates");

            //Renderer.RenderHtmlAsPdf(htmmlString).PrependPdf(coverPage).SaveAs("html-string.pdf");

            //Renderer.RenderHtmlAsPdf(htmmlString).SaveAs("html-string.pdf");

            var quoteDoc = Renderer.RenderHtmlAsPdf(htmmlString).PrependPdf(coverpage);
            //.Stream;
            //var stream = coverpage.Stream;
            //  .SaveAs("html-string.pdf");
            //var inclusionDoc = PdfDocument.FromFile($"{_env.WebRootPath}{inclusion.FileUrl}");

            //quoteDoc = quoteDoc.AppendPdf(inclusionDoc);
            var quoteFromDb = await _service.GetQuoteByIdAsync(vm.Id);
            if (quoteFromDb.IsTender.HasValue 
                && quoteFromDb.IsTender.Value 
                && !string.IsNullOrEmpty(quoteFromDb.PdfUrl))
            {
                // read file from Azure Syore
                using (MemoryStream mem = new MemoryStream())
                {
                    ConvertToStream(quoteFromDb.CustomPdfUrl, mem);
                    var customPdfDoc = new PdfDocument(mem);
                    quoteDoc = quoteDoc.AppendPdf(customPdfDoc);
                }               
               
            }
            var stream = quoteDoc.Stream;
            //.SaveAs("html-string.pdf");
            //await _azureStorageService.UploadDocAsync("html-string.pdf", vm.JobNumber +"-"+ Guid.NewGuid().ToString()+".pdf");
            await _azureStorageService.UploadDocAsync(stream, fileName);

            var docVM = new DocumentViewModel { Name = fileName, Title = $"{vm.JobNumber} {vm.RequestType}", QuoteId = vm.Id };

            await _service.SaveDocumentAsync(docVM, UserId);

            var docs = await _service.GetDocuments(vm.Id);
            vm.Documents = docs;
            //var context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "DinkToPdf.dll"));
            //var converter = new SynchronizedConverter(new PdfTools());


            //converter.Convert(pdf);

            //var basePath = $"https://docs.google.com/document/d/";
            //var id =_docGeneration.GenerateDoc(vm);
            //vm.DocUrl = $"{basePath}{id}";
            //TempData["SuccessMessage"] = "The document has been generated successfully!!";

            return vm;
        }

        private static void ConvertToStream(string fileUrl, Stream stream)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                Stream response_stream = response.GetResponseStream();

                response_stream.CopyTo(stream, 4096);
            }
            finally
            {
                response.Close();
            }
        }

        private async Task SaveQuoteInternalAsync(QuotesViewModel vm)
        {
            //if (vm.Id == 0)
            {
                vm.Id = await _service.SaveAsync(vm, UserId).ConfigureAwait(false);
            }
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

    }
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}