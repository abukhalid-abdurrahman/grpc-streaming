using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService.Services
{
    public class WeatherService : Weather.WeatherBase
    {
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }
        public override Task<WeatherResponse> GetCurrentWeather(WeatherRequest request, ServerCallContext context)
        {
            return Task.FromResult(new WeatherResponse()
            {
                FeelsLike = 2.60,
                Temp = 45.5,
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                City = "Tajikistan",
                Units = Units.Imperial
            });
        }
        
        public override async Task GetCurrentWeatherStream(WeatherRequest request, 
            IServerStreamWriter<WeatherResponse> streamResponse,  
            ServerCallContext context)
        {
            if (context.CancellationToken.IsCancellationRequested)
                return;
            
            await streamResponse.WriteAsync(new WeatherResponse()
            {
                FeelsLike = 2.60,
                Temp = 45.5,
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                City = "Tajikistan",
                Units = Units.Imperial
            });
        }

        public override async Task<MultiWeatherResponse> GetMultiCurrentWeatherStream(
            IAsyncStreamReader<WeatherRequest> requestStream, 
            ServerCallContext context)
        {
            var response = new MultiWeatherResponse()
            {
                Weather = { }
            };
            await foreach (var request in requestStream.ReadAllAsync())
            {
                response.Weather.Add(new WeatherResponse()
                {
                    FeelsLike = 2.60,
                    Temp = 45.5,
                    Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                    City = "Tajikistan",
                    Units = Units.Imperial
                });   
            }
            return response;
        }

        public override async Task<Empty> PrintStream(
            IAsyncStreamReader<PrintRequest> requestStream, 
            ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                _logger.LogInformation($"REQUEST: {request.Message}");
            }
            return new Empty();
        }
    }
}