using GreenPOS.Common;
using GreenPOS.Interfaces;
using GreenPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenPOS.Entity;
using AutoMapper;
using GreenPOS.Context;
using Microsoft.Extensions.Options;

namespace GreenPOS.Service
{
    public class QuoteService : IQuoteService
    {
        private readonly GreenPOSDBContext _ctx;
        private readonly IRepository<Quote> _repository;

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Facade> _facadeRepository;
        private readonly IRepository<HouseDesign> _homeDesignRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<Inclusion> _inclusionRepository;
        private readonly IRepository<InclusionDetail> _inclusionDetailRepository;
        private readonly IRepository<Package> _packageRepository;
        private readonly IRepository<Document> _documentRepository;
        private readonly IMapper _mapper;

        private const string imageUrlPrefix = "/images/";

        private readonly IDataCache _dataCache;

        public QuoteService(GreenPOSDBContext ctx, IRepository<Quote> repository
            , IRepository<User> userRepository,
            IMapper mapper,
            IRepository<Facade> facadeRepository
            , IRepository<HouseDesign> homeDesignRepository,
             IRepository<Promotion> promotionRepository,
              IRepository<Inclusion> inclusionRepository,
               IRepository<Package> packageRepository,
                 IRepository<InclusionDetail> inclusionDetailRepository,
                 IRepository<Document> documentRepository,
                 IDataCache dataCache
            )
        {
            _ctx = ctx;
            _repository = repository;

            _userRepository = userRepository;
            _facadeRepository = facadeRepository;
            _promotionRepository = promotionRepository;
            _homeDesignRepository = homeDesignRepository;
            _inclusionRepository = inclusionRepository;
            _packageRepository = packageRepository;
            _inclusionDetailRepository = inclusionDetailRepository;
            _documentRepository = documentRepository;
            _mapper = mapper;
            _dataCache = dataCache;
        }
        public async Task<QuotesViewModel> GetQuoteByIdAsync(long id)
        {
            var m = await _repository.FirstOrDefaultAsync(a => a.Id == id);

            if (m != null)
            {
                var vm = _mapper.Map<QuotesViewModel>(m);
                var facades = await GetFacadesAsync();
                var promotions = await GetPromotionsAsync();
                var packages = await GetPackagesAsync();
                var inclusions = await GetInclusionsAsync();
                var houseDesigns = await GetHouseDesignsAsync();
                var inclusionDetails = await GetInclusionDetailsAsync();
                var documents = await GetDocuments(m.Id);
                var masterdata = await GetContractDetails();
                var additionalInclusionDetails = await GetAdditionalInclusionDetailsAsync(id);
                vm.TableData = masterdata;
                vm.Documents = documents;

                vm.SetMasterData(facades, promotions, houseDesigns, packages, inclusions, inclusionDetails, additionalInclusionDetails);

                if (vm.HouseDesignId.HasValue)
                {
                    var index = vm.HouseDesigns.IndexOf(vm.HouseDesigns.First(x => x.Id == vm.HouseDesignId));
                    vm.HouseDesigns[index].IsSelected = true;

                }
                if (vm.InclusionId.HasValue)
                {
                    var index = vm.Inclusions.IndexOf(vm.Inclusions.First(x => x.Id == vm.InclusionId));
                    vm.Inclusions[index].IsSelected = true;

                }
                if (vm.FacadeId.HasValue)
                {
                    var index = vm.Facades.IndexOf(vm.Facades.First(x => x.Id == vm.FacadeId));
                    vm.Facades[index].IsSelected = true;

                }
                return vm;
            }


            return null;
        }

        public async Task<long> SaveCustomPdfAsync(QuotesViewModel vm)
        {
            var m = _mapper.Map<Quote>(vm);
            var current = await _repository.FirstOrDefaultAsync(a => a.Id == m.Id);
            current.PdfUrl = vm.PdfUrl;
            await _repository.UpdateAsync(current, current.Id);
            return m.Id;
        }

        public async Task<long> DeleteCustomPdfAsync(QuotesViewModel vm)
        {
            var m = _mapper.Map<Quote>(vm);
            var current = await _repository.FirstOrDefaultAsync(a => a.Id == m.Id);
            current.PdfUrl = string.Empty;
            await _repository.UpdateAsync(current, current.Id);
            return m.Id;
        }

        public async Task<long> SaveAsync(QuotesViewModel vm, long loggedinUserId)
        {
            var m = _mapper.Map<Quote>(vm);

            if (m.Id > 0)
            {
                var current = await _repository.FirstOrDefaultAsync(a => a.Id == m.Id);
                current.ModifiedBy = loggedinUserId;
                current.ModifiedOn = DateTime.UtcNow;
                current.Status = vm.Status;
                current.Notes = vm.Notes;
                current.CustomerAddress = vm.CustomerAddress;
                current.CustomerFirstName = vm.CustomerFirstName;
                current.CustomerEmail = vm.CustomerEmail;
                current.CustomerLastName = vm.CustomerLastName;
                current.CustomerProjectAddress = vm.CustomerProjectAddress;
                current.CustomerContact = vm.CustomerContact;
                current.FacadeId = vm.FacadeId;
                current.HouseDesignId = vm.HouseDesignId;
                current.InclusionId = vm.InclusionId;
                current.IsTender = vm.IsTender;
                current.ValidDays = vm.ValidDays;
                current.GarageHand = vm.GarageHand;
                current.FrontAlignment = vm.FrontAlignment;
                //                current.SupervisorContact
                //current.CompanyName = vm.CompanyName;
                //current.Email = vm.Email;
                //current.UserName = vm.UserName;
                await _repository.UpdateAsync(current, current.Id);
            }
            else
            {
                m.CreatedBy = loggedinUserId;
                m.CreatedOn = DateTime.UtcNow;
                m.IsActive = true;
                await _repository.AddAsync(m);
            }

            return m.Id;
        }


        public async Task<long> SaveVariationInclusionAsync(InclusionDetailViewModel vm)
        {
            var m = _mapper.Map<InclusionDetail>(vm);
            await _inclusionDetailRepository.AddAsync(m);
            return m.Id;
        }

        public async Task<IEnumerable<QuotesViewModel>> SearchAsync()
        {
            var mList = await _repository.FindByAsync(a => a.IsActive);

            if (mList.Any())
                return mList.Select(a => _mapper.Map<QuotesViewModel>(a));

            return Enumerable.Empty<QuotesViewModel>();
        }

        public async Task<IEnumerable<QuotesViewModel>> SearchAsync(bool isTender)
        {
            var mList = await _repository.FindByAsync(a => a.IsActive && (a.IsTender ?? false) == isTender);

            //var mList = await _repository.FindByAsync(a => a.IsActive );
            if (mList.Any())
                return mList.Select(a => _mapper.Map<QuotesViewModel>(a));

            return Enumerable.Empty<QuotesViewModel>();
        }

        public async Task<IEnumerable<QuotesViewModel>> SearchTenderAsync()
        {
            var mList = await _repository.FindByAsync(a => a.IsActive && (a.IsTender ?? false) == true);

            if (mList.Any())
                return mList.Select(a => _mapper.Map<QuotesViewModel>(a));

            return Enumerable.Empty<QuotesViewModel>();
        }

        public async Task<long> DeleteQuoteAsync(long id, long loggedinUserId)
        {
            var result = -1;
            var m = await _repository.FirstOrDefaultAsync(a => a.Id == id);
            if (m != null)
            {
                m.ModifiedBy = loggedinUserId;
                m.ModifiedOn = CommonHelper.CurrentDateTime;
                m.IsActive = false;
                result = await _repository.UpdateAsync(m, m.Id);
            }
            return result >= 0 ? m.Id : result;
        }

        public async Task<List<FacadeViewModel>> GetFacadesAsync()
        {
            //if (_dataCache.Facades != null)
            //    return _dataCache.Facades;
            var facades = await _facadeRepository.FindByAsync(x => x.IsActive);
            if (facades.Any())
            {
                _dataCache.Facades = facades.Select(a => _mapper.Map<FacadeViewModel>(a)).ToList();
                return _dataCache.Facades;
            }
            return Enumerable.Empty<FacadeViewModel>().ToList();
        }

        public async Task<List<PackageViewModel>> GetPackagesAsync()
        {

            //var listPackages = new List<PackageViewModel>();
            //var packages = await _packageRepository.FindByAsync(x => x.IsActive);
            //if (packages.Any())
            //    return packages.Select(a => _mapper.Map<PackageViewModel>(a)).ToList();
            return Enumerable.Empty<PackageViewModel>().ToList();

        }

        public async Task<List<InclusionViewModel>> GetInclusionsAsync()
        {
            if (_dataCache.Inclusions != null)
                return _dataCache.Inclusions;
            var inclusions = await _inclusionRepository.FindByAsync(x => x.IsActive);
            if (inclusions.Any())
            {
                _dataCache.Inclusions = inclusions.Select(a => _mapper.Map<InclusionViewModel>(a)).ToList();
                return _dataCache.Inclusions;
            }

            return Enumerable.Empty<InclusionViewModel>().ToList();

        }

        public async Task<List<InclusionDetailViewModel>> GetInclusionDetailsAsync()
        {
            //   return await _dataCache.GetInclusionDetailsAsync();
            var inclusions = await _inclusionDetailRepository.FindByAsync(x => x.IsActive && (x.QuoteId == 0));
            if (inclusions.Any())
                return inclusions.Select(a => _mapper.Map<InclusionDetailViewModel>(a)).ToList();

            return Enumerable.Empty<InclusionDetailViewModel>().ToList();

        }

        public async Task<List<InclusionDetailViewModel>> GetAdditionalInclusionDetailsAsync(long quoteId)
        {
            //   return await _dataCache.GetInclusionDetailsAsync();
            var inclusions = await _inclusionDetailRepository.FindByAsync(x => x.IsActive && (x.QuoteId == quoteId));
            if (inclusions.Any())
                return inclusions.Select(a => _mapper.Map<InclusionDetailViewModel>(a)).ToList();

            return Enumerable.Empty<InclusionDetailViewModel>().ToList();

        }

        public async Task<List<InclusionDetailViewModel>> GetInclusionDetailsAsync(long quoteId)
        {
            //   return await _dataCache.GetInclusionDetailsAsync();
            var inclusions = await _inclusionDetailRepository.FindByAsync(x => x.IsActive && (x.QuoteId == quoteId));
            if (inclusions.Any())
                return inclusions.Select(a => _mapper.Map<InclusionDetailViewModel>(a)).ToList();

            return Enumerable.Empty<InclusionDetailViewModel>().ToList();

        }

        public async Task<List<DesignViewModel>> GetHouseDesignsAsync()
        {
            //if (_dataCache.HouseDesigns != null)
            //    return _dataCache.HouseDesigns;

            var houseDesigns = await _homeDesignRepository.FindByAsync(x => x.IsActive);
            if (houseDesigns.Any())
            {
                _dataCache.HouseDesigns = houseDesigns.Select(a => _mapper.Map<DesignViewModel>(a)).ToList();
                return _dataCache.HouseDesigns;
            }

            return Enumerable.Empty<DesignViewModel>().ToList();

        }

        public async Task<List<PromotionViewModel>> GetPromotionsAsync()
        {
            return await _dataCache.GetPromotionsAsync();
            //var promotions = await _promotionRepository.FindByAsync(x => x.IsActive);
            //if (promotions.Any())
            //    return promotions.Select(a => _mapper.Map<PromotionViewModel>(a)).ToList();

            //return Enumerable.Empty<PromotionViewModel>().ToList();

        }

        public async Task<List<DocumentViewModel>> GetDocuments(long QuoteId)
        {
            var documents = await _documentRepository.FindByAsync(x => x.IsActive && x.QuoteId == QuoteId);

            //var promotions = await _promotionRepository.FindByAsync(x => x.IsActive);
            if (documents.Any())
            {
                var docs = documents.OrderByDescending(x => x.CreatedOn).Select(a => _mapper.Map<DocumentViewModel>(a)).ToList();
                var tenderVersion = documents.OrderByDescending(x => x.CreatedOn).Where(x => x.Title.ToLower().Contains("tender")).ToList().Count;
                var quoteVersion = documents.OrderByDescending(x => x.CreatedOn).Where(x => x.Title.ToLower().Contains("quote")).ToList().Count;
                TimeZoneInfo timezoneInfo;
                try
                {
                    timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                }
                catch
                {                   
                    timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
                }

                for (int i = 0; i < docs.Count; i++)
                {

                    docs[i].Title = docs[i].Title.ToLower().Contains("quote") ? docs[i].Title + $"_v{quoteVersion--}" : docs[i].Title + $"_v{tenderVersion--}"; ;
                    docs[i].CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(docs[i].CreatedOn, timezoneInfo);
                }

                return docs;
            }

            return Enumerable.Empty<DocumentViewModel>().ToList();

        }

        public async Task<long> SaveDocumentAsync(DocumentViewModel vm, long loggedinUserId)
        {
            var m = _mapper.Map<Document>(vm);
            m.CreatedBy = loggedinUserId;
            m.CreatedOn = DateTime.UtcNow;
            m.IsActive = true;
            await _documentRepository.AddAsync(m);


            return m.Id;
        }

        public async Task<bool> DeleteDocumentAsync(DocumentViewModel vm)
        {
            var m = _mapper.Map<Document>(vm);
            await _documentRepository.DeleteAsync(m);
            return true;
        }

        public async Task<bool> DeleteInclustionAsync(long id)
        {
            var m = await _inclusionDetailRepository.FirstOrDefaultAsync(a => a.Id == id);
            await _inclusionDetailRepository.DeleteAsync(m);
            return true;
        }

        public async Task<List<TableData>> GetContractDetails()
        {

            var data = new List<TableData>();
            var css = "header-text-bold";
            data.Add(new TableData { RowNumber = "1.00", Description = "Site Costs", Price = "", CssClass = css });
            data.Add(new TableData { RowNumber = "", Description = "Standard Site Costs", Price = "", CssClass = css });
            for (int i = 1; i < 70; i++)
            {
                var numbering = i < 10 ? $"1.0{i}" : $"1.{i}";
                data.Add(new TableData { RowNumber = numbering, Description = "Provide 450mm wide eaves to all Ground Floor roof areas as per.Provide 450mm wide eaves to all Ground Floor roof areas as per developer requirements.<BR>NOTE: Driveway crossover to be plain concrete as per developer", Price = "INCLUDED" });
                data.Add(new TableData());
                //data.Add(new TableData { RowNumber = "", Description = "developer requirements.", Price = "" });
                //data.Add(new TableData { RowNumber = "", Description = "NOTE: Driveway crossover to be plain concrete as per developer", Price = "" });
                //data.Add(new TableData { RowNumber = "", Description = "requirements.", Price = "" });
            }
            data.Add(new TableData());
            data.Add(new TableData { RowNumber = "", Description = "Sub Total", Price = "$2,356", CssClass = css, SubTotalCss = "header-text-bold-right " });
            data.Add(new TableData());

            data.Add(new TableData { RowNumber = "", Description = "Grand Total", Price = "$222,356", CssClass = css, SubTotalCss = "header-text-bold-right " });
            data.Add(new TableData());
            return data;
        }
    }

    public interface IQuoteService
    {
        //Task<long> DeleteRoleByIdAsync(long id, long userId);
        Task<QuotesViewModel> GetQuoteByIdAsync(long id);
        Task<long> SaveAsync(QuotesViewModel vm, long userId);
        Task<IEnumerable<QuotesViewModel>> SearchAsync();

        //  Task<IEnumerable<SelectListItem>> GetRolePermissionsAsync(long userId);

        Task<long> DeleteQuoteAsync(long id, long loggedinUserId);

        Task<List<FacadeViewModel>> GetFacadesAsync();

        Task<List<PackageViewModel>> GetPackagesAsync();

        Task<List<InclusionViewModel>> GetInclusionsAsync();

        Task<List<DesignViewModel>> GetHouseDesignsAsync();

        Task<List<PromotionViewModel>> GetPromotionsAsync();
        Task<List<InclusionDetailViewModel>> GetInclusionDetailsAsync();

        Task<List<DocumentViewModel>> GetDocuments(long QuoteId);

        Task<long> SaveDocumentAsync(DocumentViewModel vm, long loggedinUserId);

        Task<List<TableData>> GetContractDetails();
        Task<bool> DeleteDocumentAsync(DocumentViewModel vm);
        Task<long> SaveVariationInclusionAsync(InclusionDetailViewModel vm);
        Task<List<InclusionDetailViewModel>> GetAdditionalInclusionDetailsAsync(long quoteId);

        Task<IEnumerable<QuotesViewModel>> SearchTenderAsync();
        Task<bool> DeleteInclustionAsync(long id);

        Task<IEnumerable<QuotesViewModel>> SearchAsync(bool isTender);
        Task<long> SaveCustomPdfAsync(QuotesViewModel vm);

        Task<long> DeleteCustomPdfAsync(QuotesViewModel vm);
    }
}
