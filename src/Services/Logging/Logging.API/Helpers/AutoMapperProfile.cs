using AutoMapper;
using Entities.DHT;
using Logging.API.DTOs;

namespace Logging.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DHTDTO, DHT>()
                .ForMember(d => d.Humidity, opt => opt.MapFrom(s => s.StatusSNS.Values.Humidity))
                .ForMember(d => d.Temperature, opt => opt.MapFrom(s => s.StatusSNS.Values.Temperature))
                .ForMember(d => d.DewPoint, opt => opt.MapFrom(s => s.StatusSNS.Values.DewPoint))
                .ForMember(d => d.Time, opt => opt.MapFrom(s => s.StatusSNS.Time));


        }
    }
}
