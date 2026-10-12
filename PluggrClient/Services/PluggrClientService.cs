using Google.Protobuf;
using Grpc.Core;
using PluggrShared;
using PluggrShared.Protos;

namespace PluggrClient.Services
{
    public class PluggrClientService : PluggrShared.Protos.PluggrClientService.PluggrClientServiceBase
    {
        private readonly ILogger _logger;

        public PluggrClientService()
        {
            _logger = LoggerCreator.Create<PluggrClientService>();
        }

        public override Task<SendClientMessageResponse> SendClientMessage(SendClientMessageRequest request, ServerCallContext context)
        {
            bool result = PluggrMessageManager.InvokeMethod(request, out InvokeMethodResponse methodResponse);
            using MemoryStream? stream = (methodResponse.Data != null) ? new MemoryStream(methodResponse.Data) : null;
            SendClientMessageResponse response = new SendClientMessageResponse()
            {
                Data = (stream != null) ? ByteString.FromStream(stream) : ByteString.Empty,
                Status = (result) ? Statuses.Client.SendMessageSuccess : Statuses.Client.SendMessageFailure,
                Message = (methodResponse.Message != null) ? methodResponse.Message : string.Empty
            };

            return Task.FromResult(response);
        }
    }
}
