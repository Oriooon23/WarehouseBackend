using AutoMapper;
using Warehouse.Common.DTOs;
using Warehouse.Common.DTOs.UserDTO;
using Warehouse.Data.Models;

namespace Warehouse.Common.BaseProfile
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Categories, CategoryDTO>().ReverseMap();

            CreateMap<Products, ProductDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.Name))
                .ReverseMap();

            CreateMap<Orders, OrderDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom (src => src.Status.Name))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap();

            CreateMap<Users, UserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Role))
                .ReverseMap();

            CreateMap<OrderStatuses, OrderStatusDTO>().ReverseMap();

            CreateMap<Suppliers, SupplierDTO>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ReverseMap();

            CreateMap<Cities, CityDTO>().ReverseMap();

            CreateMap<Roles, RoleDTO>().ReverseMap();

            CreateMap<Permissions, PermissionDTO>().ReverseMap();

            CreateMap<OrderProducts, OrderProductDTO>().ReverseMap();

            CreateMap<RolePermissions, RolePermissionDTO>().ReverseMap();
        }
    }
}