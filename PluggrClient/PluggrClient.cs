using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using PluggrShared;
using PluggrShared.Protos;
using static PluggrShared.Protos.PluggrClientService;
using static PluggrShared.Protos.PluggrService;

namespace PluggrClient
{
    public sealed class PluggrClient
    {
        public readonly Guid ID;
        public readonly Uri ClientUri;
        public readonly string ServiceName;
        public readonly uint Port;

        public PluggrServiceClient ServerClient => _serverClient;

        private readonly GrpcChannel _channel, _serverChannel;
        private readonly PluggrServiceClient _serverClient;
        private readonly ILogger _logger;

        public PluggrClient(string serviceName, uint port)
        {

            Uri serverUri = new Uri($"http://localhost:{SharedConstants.ServerPort}");
            
            ClientUri = new Uri($"http://localhost:{port}/");
            ServiceName = serviceName;
            Port = port;

            _logger = LoggerCreator.Create<PluggrClient>();
            _channel = GrpcChannel.ForAddress(ClientUri);
            _serverChannel = GrpcChannel.ForAddress(serverUri);
            _serverClient = new PluggrServiceClient(_serverChannel);

            ID = Register();
        }

        private Guid Register()
        {
            _logger.LogDebug("Attempting to register client.");

            RegisterClientRequest request = new RegisterClientRequest()
            {
                Service = ServiceName,
                Port = Port
            };

            RegisterClientResponse response = _serverClient.RegisterClient(request, new CallOptions());

            _logger.LogDebug($"Register Response");
            _logger.LogDebug($"-----");
            _logger.LogDebug($"Status: {response.Status}");
            _logger.LogDebug($"Message: {response.Message}");
            _logger.LogDebug($"Guid: {response.Guid}");

            if (response.Guid == null)
            {
                throw new Exception($"Client registration failed due to the following error: {response.Message}");
            }

            return Guid.Parse(response.Guid);
        }

        public void RegisterMethod()
        {
            
        }
    }
}
