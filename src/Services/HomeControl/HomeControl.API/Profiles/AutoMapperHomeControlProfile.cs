using AutoMapper;
using Entities.DHT;
using Google.Protobuf.WellKnownTypes;
using HomeControl.API.DTOs;
using HomeControl.API.Entities;

namespace HomeControl.API.Profiles
{
    public class AutoMapperHomeControlProfile : Profile
    {    
        public AutoMapperHomeControlProfile()
        {
            CreateMap<RoomDTO, Room>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(d => d.RoomType, opt => opt.MapFrom(src => src.RoomType))
                .ForMember(d => d.Topic, opt => opt.MapFrom(src => src.Topic));

            CreateMap<ItemDeviceDTO, RoomItem>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.DeviceType, opt => opt.MapFrom(src => src.DeviceType))
                .ForMember(d => d.Topic, opt => opt.MapFrom(src => src.Topic));

            CreateMap<GrpcItemModel, ItemDeviceDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.DeviceType, opt => opt.MapFrom(src => src.Devicetype))
                .ForMember(d => d.Topic, opt => opt.MapFrom(src => src.Topic))
                .ForMember(d => d.IP, opt => opt.MapFrom(src => src.Ip))
                .ForMember(d => d.MosquittoUsername, opt => opt.MapFrom(src => src.Mosquittousername))
                .ForMember(d => d.MosquittoPassword, opt => opt.MapFrom(src => src.Mosquittopassword))
                .ForMember(d => d.DateAdded, opt => opt.MapFrom(src => src.Dateadded.ToDateTime().ToUniversalTime()));

        }
    }
}
