using AutoMapper;
using Entities.DHT;
using Grpc.Net.Client;
using HomeControl.API.DTOs;
using Logging.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace HomeControl.API.SyncDataServices.Grpc
{
    public class GrpcClient : IGrpcClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GrpcClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public List<ItemDeviceDTO> GetAllDevicesFromStatusAPI(string deviceName)
        {
            throw new NotImplementedException();
        }

        public ItemDeviceDTO GetDeviceFromStatusAPI(string deviceName)
        {
            Console.WriteLine($"--> calling grpc service {_configuration["GrpcStatus"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcStatus"]);
            //var client = new GrpcLogging.GrpcLoggingClient(channel);
            var client = new GrpcItem.GrpcItemClient(channel);
            var request = new GetItemRequest() { DeviceName = deviceName };

            try
            {
                var reply = client.GetItemFromStatus(request);

                var response = _mapper.Map<ItemDeviceDTO>(reply.Item);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not call grpc service {ex.Message}");
                return null;
            }
        }

        public List<DHT> ReturnAllDhts()
        {
            Console.WriteLine($"--> calling grpc service {_configuration["GrpcLogging"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcLogging"]);
            var client = new GrpcLogging.GrpcLoggingClient(channel);
            var request = new GetAllRequest();

            List<DHT> dhts = new List<DHT>();
            try
            {
                var reply = client.GetAllLoggingValues(request);

                foreach (var item in reply.Dht)
                {
                    dhts.Add(_mapper.Map<DHT>(item));
                }


                //return _mapper.Map<List<DHT>>(reply.Dht);
                return dhts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not call grpc service {ex.Message}");
                return null;
            }

        }


    }
}
