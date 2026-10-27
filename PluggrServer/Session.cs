using PluggrServer.Clients;
using PluggrShared;

namespace PluggrServer
{
    /// <summary>
    /// Static class to contain all current session information. I.e. Clients and Host Address.
    /// </summary>
    internal static class Session
    {
        internal static readonly ClientManager Clients = new ClientManager();

        internal static string HostAddress => $"http://localhost:{SharedConstants.ServerPort}/";
    }
}
