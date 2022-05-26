using AutoMapper;
using Common.Enums;
using Google.Protobuf.WellKnownTypes;
using Network.API;
using Network.API.DTOs;
using Network.API.Entities;

namespace Network.API.Profiles
{
    public class AutoMapperStatusProfile : Profile
    {
        public AutoMapperStatusProfile()
        {
            CreateMap<DeviceDTO, Device>();

            CreateMap<Device, AvailableDeviceDTO>();

            //CreateMap<Device, GrpcItemModel>()
            //    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(d => d.Devicetype, opt => opt.MapFrom(src => src.DeviceType))
            //    .ForMember(d => d.Ip, opt => opt.MapFrom(src => src.IP))
            //    .ForMember(d => d.Mosquittousername, opt => opt.MapFrom(src => src.MosquittoUsername))
            //    .ForMember(d => d.Mosquittopassword, opt => opt.MapFrom(src => src.MosquittoPassword))
            //    .ForMember(d => d.Dateadded, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.DateAdded.ToUniversalTime())))
            //    .ForMember(d => d.Datemodified, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.DateModified.ToUniversalTime())));


            //CreateMap<Device_old, DeviceStatusResponseDTO>()
            //    .ForMember(d => d.UptimeInSeconds, opt => opt.MapFrom(src => src.State.UptimeSec))
            //    .ForMember(d => d.Status, opt => opt.MapFrom(src => System.Enum.GetName(typeof(DeviceStatus), src.DeviceStatus)));

        }
    }
}
