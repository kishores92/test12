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
    public class DataCache : IDataCache
    {
        //private readonly GreenPOSDBContext _ctx;
        //private readonly IRepository<Quote> _repository;

        //private readonly IRepository<User> _userRepository;
        //private readonly IRepository<Facade> _facadeRepository;
        //private readonly IRepository<HouseDesign> _homeDesignRepository;
        //private readonly IRepository<Promotion> _promotionRepository;
        //private readonly IRepository<Inclusion> _inclusionRepository;
        //private readonly IRepository<InclusionDetail> _inclusionDetailRepository;
        //private readonly IRepository<Package> _packageRepository;
        //private readonly IMapper _mapper;
        public DataCache()
        {
            //_ctx = ctx;
            //_repository = repository;

            //_userRepository = userRepository;
            //_facadeRepository = facadeRepository;
            //_promotionRepository = promotionRepository;
            //_homeDesignRepository = homeDesignRepository;
            //_inclusionRepository = inclusionRepository;
            //_packageRepository = packageRepository;
            //_inclusionDetailRepository = inclusionDetailRepository;
            //_mapper = mapper;
        }
        public async Task<List<FacadeViewModel>> GetFacadesAsync()
        {
            if (Facades != null)
                return Facades;
            //var facades = await _facadeRepository.FindByAsync(x => x.IsActive);
            //if (facades.Any())
            //{
            //    Facades = facades.Select(a => _mapper.Map<FacadeViewModel>(a)).ToList();
            //    return Facades;
            //}
            return Enumerable.Empty<FacadeViewModel>().ToList();
        }

        public async Task<List<PackageViewModel>> GetPackagesAsync()
        {
            return Enumerable.Empty<PackageViewModel>().ToList();
            /*
            var listPackages = new List<PackageViewModel>();
            var packages = await _packageRepository.FindByAsync(x => x.IsActive);
            if (packages.Any())
                return packages.Select(a => _mapper.Map<PackageViewModel>(a)).ToList();
            return Enumerable.Empty<PackageViewModel>().ToList();
*/
        }

        public async Task<List<InclusionViewModel>> GetInclusionsAsync()
        {
            if (Inclusions != null)
                return Inclusions;
            //var inclusions = await _inclusionRepository.FindByAsync(x => x.IsActive);
            //if (inclusions.Any())
            //{
            //    Inclusions = inclusions.Select(a => _mapper.Map<InclusionViewModel>(a)).ToList();

            //    return Inclusions;
            //}

            return Enumerable.Empty<InclusionViewModel>().ToList();

        }

        public async Task<List<InclusionDetailViewModel>> GetInclusionDetailsAsync()
        {
            if (InclusionDetails != null)
                return InclusionDetails;
            //var inclusions = await _inclusionDetailRepository.FindByAsync(x => x.IsActive);
            //if (inclusions.Any())
            //{
            //    InclusionDetails= inclusions.Select(a => _mapper.Map<InclusionDetailViewModel>(a)).ToList();
            //    return InclusionDetails;
            //}

            return Enumerable.Empty<InclusionDetailViewModel>().ToList();

        }

        public async Task<List<DesignViewModel>> GetHouseDesignsAsync()
        {
            if (HouseDesigns != null)
                return HouseDesigns;
            //var houseDesigns = await _homeDesignRepository.FindByAsync(x => x.IsActive);
            //if (houseDesigns.Any())
            //{
            //    HouseDesigns = houseDesigns.Select(a => _mapper.Map<DesignViewModel>(a)).ToList();
            //    return HouseDesigns;
            //}

            return Enumerable.Empty<DesignViewModel>().ToList();

        }

        public async Task<List<PromotionViewModel>> GetPromotionsAsync()
        {

            return Enumerable.Empty<PromotionViewModel>().ToList();
            //var promotions = await _promotionRepository.FindByAsync(x => x.IsActive);
            //if (promotions.Any())
            //    return promotions.Select(a => _mapper.Map<PromotionViewModel>(a)).ToList();

            //return Enumerable.Empty<PromotionViewModel>().ToList();

        }

        public List<FacadeViewModel> Facades { get; set; }
        public List<DesignViewModel> HouseDesigns { get; set; }
        public List<InclusionViewModel> Inclusions { get; set; }
        public List<InclusionDetailViewModel> InclusionDetails { get; set; }

        public List<PromotionViewModel> Promotions { get; set; }
        public List<PackageViewModel> Packages { get; set; }

    }

    public interface IDataCache
    {
        Task<List<FacadeViewModel>> GetFacadesAsync();

        Task<List<PackageViewModel>> GetPackagesAsync();

        Task<List<InclusionViewModel>> GetInclusionsAsync();

        Task<List<DesignViewModel>> GetHouseDesignsAsync();

        Task<List<PromotionViewModel>> GetPromotionsAsync();
        Task<List<InclusionDetailViewModel>> GetInclusionDetailsAsync();

        List<FacadeViewModel> Facades { get; set; }
        List<DesignViewModel> HouseDesigns { get; set; }
        List<InclusionViewModel> Inclusions { get; set; }
        List<InclusionDetailViewModel> InclusionDetails { get; set; }

        List<PromotionViewModel> Promotions { get; set; }
        List<PackageViewModel> Packages { get; set; }

    }
}
