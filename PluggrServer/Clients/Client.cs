using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using PluggrShared.Protos;
using static PluggrShared.Protos.PluggrClientService;

namespace PluggrServer.Clients
{
    /// <summary>
    /// Pluggr Client to send messages to registered clients.
    /// </summary>
    internal class Client : IDisposable
    {
        internal readonly Guid ID;
        internal readonly string ServiceName, Address;
        public readonly uint Port;
        private readonly PluggrClientServiceClient _client;
        private readonly GrpcChannel _channel;
        
        /// <summary>
        /// Create a Client with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <param name="serviceName"></param>
        internal Client(Guid id, string serviceName, uint port)
        {
            ID = id;
            ServiceName = serviceName;
            Port = port;
            Address = ClientManager.CreateClientAddress(Port);

            _channel = GrpcChannel.ForAddress(Address);
            _client = new PluggrClientServiceClient(_channel);
        }

        internal SendClientMessageResponse SendMessage(SendClientMessageRequest request, CallOptions? options = null) => _client.SendClientMessage(request, options ?? new CallOptions());

        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}
