using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService.Services
{
    public class ChatService : Chat.ChatBase
    {
        private readonly ILogger<ChatService> _logger;

        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }

        public override async Task SendMessage(
            IAsyncStreamReader<RequestMessage> requestStream, 
            IServerStreamWriter<ResponseMessage> responseStream, 
            ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var message = requestStream.Current;
                _logger.LogInformation($"MESSAGE FROM CLIENT: {message.Text}");
            }

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new ResponseMessage()
                {
                    Text = "OK, I GOT YOU :)",
                    Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
                });
                await Task.Delay(1000);
            }
        }
    }
}