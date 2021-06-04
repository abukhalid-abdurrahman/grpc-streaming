using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GrpcService.Services
{
    public class WeatherService : Weather.WeatherBase
    {
        public override Task<WeatherResponse> GetCurrentWeather(WeatherRequest request, ServerCallContext context)
        {
            return Task.FromResult(new WeatherResponse()
            {
                FeelsLike = 2.60,
                Temp = 45.5,
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
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
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
            });
        }
    }
}