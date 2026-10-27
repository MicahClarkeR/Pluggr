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
        /// <summary>
        /// Internal ID of this Pluggr Client once registered.
        /// </summary>
        internal readonly Guid ID;
        
        /// <summary>
        /// Name of the registered service this Client represents, attained from Pluggr Service it registers itself.
        /// </summary>
        internal readonly string ServiceName;

        /// <summary>
        /// Internal address for the Pluggr Client that was registered.
        /// </summary>
        internal readonly string Address;

        /// <summary>
        /// Underlying localhost port for this Client.
        /// </summary>
        public readonly uint Port;

        /// <summary>
        /// Private Client Service that allows this Client to send messages to the Pluggr Client.
        /// </summary>
        private readonly PluggrClientServiceClient _client;

        /// <summary>
        /// Internal GRPC channel used for the Client.
        /// </summary>
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

        /// <summary>
        /// Send a message to the underlying Pluggr Client.
        /// </summary>
        /// <param name="request">Inbound request to send to client..</param>
        /// <param name="options">Options for calls made to client../param>
        /// <returns></returns>
        internal SendClientMessageResponse SendMessage(SendClientMessageRequest request, CallOptions? options = null) => _client.SendClientMessage(request, options ?? new CallOptions());

        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}
