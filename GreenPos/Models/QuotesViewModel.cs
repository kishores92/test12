using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class QuotesViewModel : BaseViewModel<long>
    {

        private const string visibleCss = "visible";
        private const string hiddenCss = "hidden";
        private const string baseUrl = @"https://dhdocgen.blob.core.windows.net/quotes/";

        public QuotesViewModel()
        {

            Statuses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<string>() { "Draft", "Deposit Paid", "Expired" });
            Status = "Draft";
            Documents = new List<DocumentViewModel>();
            TableData = new List<TableData>();
        }
        public QuotesViewModel(List<FacadeViewModel> facades, List<PromotionViewModel> promotions, List<DesignViewModel> houseDesigns,
            List<PackageViewModel> packages, List<InclusionViewModel> inclusions,
            List<InclusionDetailViewModel> inclusionDetails, List<DocumentViewModel> documents, string requestType = "Quote")
        {
            Facades = facades;
            Promotions = promotions;
            HouseDesigns = houseDesigns;
            Packages = packages;
            Inclusions = GetInclusionPacks(inclusions);
            InclusionDetails = inclusionDetails;
            AddOns = GetAddOns(inclusions);
            PackageCss = Packages.Count > 0 ? visibleCss : hiddenCss;
            PromotionCss = Packages.Count > 0 ? visibleCss : hiddenCss;
            Documents = documents ?? new List<DocumentViewModel>();
            AdditionalInclusions = new List<InclusionDetailViewModel>();
            //  RequestType = requestType;

            Statuses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<string>() { "Draft", "Deposit Paid", "Expired" });
            // Status = "Draft";

        }


        public void SetMasterData(List<FacadeViewModel> facades, List<PromotionViewModel> promotions, List<DesignViewModel> houseDesigns,
            List<PackageViewModel> packages, List<InclusionViewModel> inclusions,
            List<InclusionDetailViewModel> inclusionDetails, List<InclusionDetailViewModel> additionalinclusionDetails = null)
        {
            Facades = facades;
            Promotions = promotions;
            HouseDesigns = houseDesigns;
            Packages = packages;
            Inclusions = GetInclusionPacks(inclusions);
            InclusionDetails = inclusionDetails;
            AddOns = GetAddOns(inclusions);
            PackageCss = Packages.Count > 0 ? visibleCss : hiddenCss;
            PromotionCss = Packages.Count > 0 ? visibleCss : hiddenCss;
            Statuses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(new List<string>() { "Draft", "Deposit Paid", "Expired" });
            //Status = "Draft";
            AdditionalInclusions = additionalinclusionDetails ?? new List<InclusionDetailViewModel>();

        }

        public string CustomerFirstName { get; set; }


        public string CustomerLastName { get; set; }


        public string CustomerContact { get; set; }


        public string CustomerEmail { get; set; }

        public string CustomerAddress { get; set; }


        public string CustomerProjectAddress { get; set; }


        public string Budget { get; set; }


        public string Supervisor { get; set; }

        public string FloorPlan { get; set; }

        public string Facade { get; set; }
        public string Status { get; set; }
        public string SupervisorContact { get; set; }
        public string Notes { get; set; }

        public int ValidDays { get; set; }

        public DateTime? DepositDate { get; set; }

        public DateTime? ValidUntil { get; set; }

        public long? DepositAmount { get; set; }
        [BindProperty]
        public List<FacadeViewModel> Facades { get; set; }

        public List<PromotionViewModel> Promotions { get; set; }

        [BindProperty]
        public List<DesignViewModel> HouseDesigns { get; set; }
        [BindProperty]
        public List<InclusionViewModel> Inclusions { get; set; }

        public List<PackageViewModel> Packages { get; set; }

        public List<InclusionDetailViewModel> InclusionDetails { get; set; }

        public List<InclusionViewModel> AddOns { get; set; }


        public string PackageCss { get; set; }

        public string PromotionCss { get; set; }

        private List<InclusionViewModel> GetInclusionPacks(List<InclusionViewModel> inclusions)
        {
            return inclusions.Where(x => x.Name.ToLower() != "addon").ToList();
        }

        private List<InclusionViewModel> GetAddOns(List<InclusionViewModel> inclusions)
        {
            return inclusions.Where(x => x.Name.ToLower() == "addon").ToList();
        }

        public string Name
        {
            get
            {
                return (CustomerFirstName ?? "") + " " + (CustomerLastName ?? "");
            }
        }

        public string FileNameOfAttachment { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectList Statuses { get; set; }

        public SelectListItem SelectedStatus { get; set; }
        public string SelectedDesignID { get; set; }
        //public string SelectedDesignID { get; set; }

        public string Email { get; set; }

        //[Required]
        //[StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        //[DataType(DataType.Text)]
        public string Address { get; set; }

        [EmailAddress]
        public string EmailCC { get; set; }

        public string JobNumber
        {
            get
            {
                return $"EH{Id}";
            }
        }

        public string BuildPrice { get; set; }

        public string GarageHand { get; set; }

        public string FrontAlignment { get; set; }

        public long BasePrice { get; set; }

        public string FacadeCost { get; set; }

        public long Total
        {
            get
            {
                return BasePrice + 21173;
            }
        }

        public string DocUrl { get; set; }

        public string PdfUrl { get; set; }

        public long? InclusionId { get; set; }

        public int? HouseDesignId { get; set; }
        public int? FacadeId { get; set; }

        //public string RequestType { get; set; }

        public List<DocumentViewModel> Documents { get; set; }

        public string FacadeName { get; set; }

        public string InclusionName { get; set; }

        public string HouseDesignName { get; set; }

        public string BaseDisplayPrice
        {
            get
            {
                return BasePrice.ToString("C0");
            }
        }
        public string TotalDisplayPrice
        {
            get
            {
                return Total.ToString("C0");
            }
        }

        public List<TableData> TableData { get; set; }

        public List<InclusionDetailViewModel> AdditionalInclusions { get; set; }

        public IFormFile FileUpload { get; set; }

        public bool? IsTender { get; set; }

        public string RequestTypeLower
        {
            get
            {
                return (RequestType ?? "Quote").ToLower();
            }
        }

        public string RequestType
        {
            get
            {
                if (IsTender ?? false)
                    return "Tender";
                return "Quote";
            }
        }

        public string CanConvertCss
        {
            get
            {
                if (RequestTypeLower == "quote" && Id > 0)
                    return visibleCss;
                return hiddenCss;
            }
        }

        public string CustomPdfUrl
        {
            get
            {
                return baseUrl + PdfUrl;
            }
        }
    }
}
