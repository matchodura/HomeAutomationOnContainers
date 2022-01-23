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

            List<DHT> dhts = new List<DHT>();
            try
            {
                var reply = client.GetAllLoggingValues(request);

                foreach(var item in reply.Dht)
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
