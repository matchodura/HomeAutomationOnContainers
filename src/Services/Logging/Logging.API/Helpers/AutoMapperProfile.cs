using AutoMapper;
using Entities.DHT22;
using Logging.API.DTOs;

namespace Logging.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DHT22DTO, DHT22>()
                .ForMember(d => d.Humidity, opt => opt.MapFrom(s => s.StatusSNS.AM2301.Humidity))
                .ForMember(d => d.Temperature, opt => opt.MapFrom(s => s.StatusSNS.AM2301.Temperature))
                .ForMember(d => d.DewPoint, opt => opt.MapFrom(s => s.StatusSNS.AM2301.DewPoint))
                .ForMember(d => d.Time, opt => opt.MapFrom(s => s.StatusSNS.Time));
        }
    }
}
