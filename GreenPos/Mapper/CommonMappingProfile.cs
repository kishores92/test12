using AutoMapper;
using GreenPOS.Entity;
using GreenPOS.Models;

namespace GreenPOS.Mapper
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
            CreateMap<ScreenPermission, ScreenPermissionViewModel>().ReverseMap();
            CreateMap<Contact, ContactViewModel>().ReverseMap();
            CreateMap<Notes, NotesViewModel>().ReverseMap();
            CreateMap<Quote, QuotesViewModel>().ReverseMap();

            CreateMap<Facade, FacadeViewModel>().ReverseMap();
            CreateMap<Promotion, PromotionViewModel>().ReverseMap();
            CreateMap<HouseDesign, DesignViewModel>().ReverseMap();
            CreateMap<Package, PackageViewModel>().ReverseMap();
            CreateMap<Inclusion, InclusionViewModel>().ReverseMap();
            CreateMap<InclusionDetail, InclusionDetailViewModel>().ReverseMap();
            CreateMap<Document, DocumentViewModel>().ReverseMap();
        }
    }
}
