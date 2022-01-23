using AutoMapper;
using Entities.DHT;

namespace HomeControl.API.Profiles
{
    public class AutoMapperHomeControlProfile : Profile
    {    
        public AutoMapperHomeControlProfile()
        {
            CreateMap<GrpcDHTModel, DHT>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.SensorName, opt => opt.MapFrom(src => src.Sensorname))
                .ForMember(d => d.Temperature, opt => opt.MapFrom(src => src.Temperature))
                .ForMember(d => d.Humidity, opt => opt.MapFrom(src => src.Humidity))
                .ForMember(d => d.DewPoint, opt => opt.MapFrom(src => src.Dewpoint))
                .ForMember(d => d.Time, opt => opt.MapFrom(src => src.Time.ToDateTime().ToUniversalTime()));

        }
    }
}
