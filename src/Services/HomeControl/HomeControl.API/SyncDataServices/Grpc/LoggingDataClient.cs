using AutoMapper;
using Entities.DHT;
using Grpc.Net.Client;
using Logging.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace HomeControl.API.SyncDataServices.Grpc
{
    public class LoggingDataClient : ILoggingDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public LoggingDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }


        public List<DHT> ReturnAllDhts()
        {
            Console.WriteLine($"--> calling grpc service {_configuration["GrpcLogging"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcLogging"]);
            var client = new GrpcLogging.GrpcLoggingClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllLoggingValues(request);
                return _mapper.Map<List<DHT>>(reply.Dht);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not coll grpc service {ex.Message}");
                return null;
            }

        }
    }
}
