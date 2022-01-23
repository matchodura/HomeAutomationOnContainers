using AutoMapper;
using Entities.DHT;
using Google.Protobuf.WellKnownTypes;
using Logging.API.DTOs;

namespace Logging.API.Profiles
{
    public class AutoMapperLoggingProfile : Profile
    {
        public AutoMapperLoggingProfile()
        {
            CreateMap<DHTDTO, DHT>()
                .ForMember(d => d.Humidity, opt => opt.MapFrom(s => s.StatusSNS.Values.Humidity))
                .ForMember(d => d.Temperature, opt => opt.MapFrom(s => s.StatusSNS.Values.Temperature))
                .ForMember(d => d.DewPoint, opt => opt.MapFrom(s => s.StatusSNS.Values.DewPoint))
                .ForMember(d => d.Time, opt => opt.MapFrom(s => s.StatusSNS.Time));

            //TODO fix mapping google protobuf time to datetime
            CreateMap<DHT, GrpcDHTModel>()
                //.ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                //.ForMember(d => d.Sensorname, opt => opt.MapFrom(src => src.SensorName))
                //.ForMember(d => d.Temperature, opt => opt.MapFrom(src => src.Temperature))
                //.ForMember(d => d.Humidity, opt => opt.MapFrom(src => src.Humidity))
                //.ForMember(d => d.Dewpoint, opt => opt.MapFrom(src => src.DewPoint))
                .ForMember(d => d.Time, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.Time.ToUniversalTime())));

        }
    }
}
