using AutoMapper;
using Entities.DHT;
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

            CreateMap<GrpcItemModel, ItemDeviceDTO>();
        }
    }
}
