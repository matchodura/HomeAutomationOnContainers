using AutoMapper;
using Entities.Enums;
using Google.Protobuf.WellKnownTypes;
using HomeControl.API.DTOs;
using HomeControl.API.DTOs.LoggingAPI;
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

            CreateMap<Room, RoomDTO>();
            CreateMap<RoomValue, SensorValueDTO>();
            CreateMap<HomeLayoutDTO, HomeLayout>();


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
                .ForMember(d => d.DateAdded, opt => opt.MapFrom(src => src.Dateadded.ToDateTime().ToUniversalTime()))
                .ForMember(d => d.DateModified, opt => opt.MapFrom(src => src.Datemodified.ToDateTime().ToUniversalTime()));

            CreateMap<GrpcSensorModel, SensorValueDTO>()
                .ForMember(d => d.Topic, opt => opt.MapFrom(src => src.Topic))
                .ForMember(d => d.Temperature, opt => opt.MapFrom(src => src.Temperature))
                .ForMember(d => d.Humidity, opt => opt.MapFrom(src => src.Humidity))
                .ForMember(d => d.DewPoint, opt => opt.MapFrom(src => src.Dewpoint))
                .ForMember(d => d.TimePolled, opt => opt.MapFrom(src => src.Time.ToDateTime().ToUniversalTime()));

            CreateMap<RoomItem, RoomItemDTO>();

        }
    }
}
