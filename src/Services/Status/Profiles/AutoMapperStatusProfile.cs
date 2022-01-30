using AutoMapper;
using Status.API.DTOs;
using Status.API.Entities;

namespace Logging.API.Profiles
{
    public class AutoMapperStatusProfile : Profile
    {
        public AutoMapperStatusProfile()
        {
            CreateMap<DeviceDTO, Device>();
        
        }
    }
}
