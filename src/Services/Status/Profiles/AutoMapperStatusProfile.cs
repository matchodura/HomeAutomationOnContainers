using AutoMapper;
using Status.API;
using Status.API.DTOs;
using Status.API.Entities;

namespace Status.API.Profiles
{
    public class AutoMapperStatusProfile : Profile
    {
        public AutoMapperStatusProfile()
        {
            CreateMap<DeviceDTO, Device>();
            CreateMap<Device, AvailableDeviceDTO>();
            CreateMap<Device, GrpcItemModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Devicetype, opt => opt.MapFrom(src => src.DeviceType))
                .ForMember(d => d.Ip, opt => opt.MapFrom(src => src.IP))
                .ForMember(d => d.Mosquittousername, opt => opt.MapFrom(src => src.MosquittoUsername))
                .ForMember(d => d.Mosquittopassword, opt => opt.MapFrom(src => src.MosquittoPassword))
                .ForMember(d => d.Dateadded, opt => opt.MapFrom(src => src.DateAdded));
           
        }
    }
}
