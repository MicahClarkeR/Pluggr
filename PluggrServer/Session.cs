using PluggrServer.Clients;
using PluggrShared;

namespace PluggrServer
{
    internal static class Session
    {
        internal static readonly ClientManager Clients = new ClientManager();

        internal static string HostAddress => $"http://localhost:{SharedConstants.ServerPort}/";
    }
}
