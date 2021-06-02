using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcService.Services
{
    public class WeatherService : Weather.WeatherBase
    {
        public override Task<WeatherResponse> GetCurrentWeather(WeatherRequest request, ServerCallContext context)
        {
            return Task.FromResult(new WeatherResponse() {});
        }
    }
}