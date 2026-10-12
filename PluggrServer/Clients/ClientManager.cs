using System.Xml.Linq;
using PluggrShared;

namespace PluggrServer.Clients
{
    /// <summary>
    /// Central manager class for all clients in this Pluggr Server.
    /// </summary>
    internal class ClientManager
    {
        private readonly List<Client> _clients = new List<Client>();
        private readonly ILogger _logger;

        internal ClientManager()
        {
            _logger = LoggerCreator.Create<ClientManager>();
        }

        /// <summary>
        /// Attempts to register a client by the name of its service, which will be used for its associated path.
        /// </summary>
        /// <param name="name">ServiceName or path of the client to be registered.</param>
        /// <param name="message">Out parameter populated if an error occurs (if `null` is returned).</param>
        /// <returns>Guid of the registered client, null if an error occured.</returns>
        internal Guid? RegisterClient(string name, uint port, out string? message)
        {
            message = null;
            Guid id = default;

            _logger.LogInformation("Registering client {name}.", name);

            if (!CheckClientExists(name))
            {
                id = Guid.NewGuid();
                Client client = new Client(id, name, port);

                _clients.Add(client);
            }
            else
            {
                message = $"Client '{name}' already registered.";
            }

            if (id == default)
                _logger.LogError(message);



            return id;
        }

        /// <summary>
        /// Attempts to deregister a Client.
        /// </summary>
        /// <param name="guid">GUID of Client to deregister.</param>
        /// <param name="message">Optional message noting the reason for failure.</param>
        /// <returns>If Client was found and removed.</returns>
        internal bool DeregisterClient(string guid, out string? message)
        {
            _logger.LogInformation($"Request to deregister client received, {guid}.");

            message = null;
            if (Guid.TryParse(guid, out Guid id))
            {

                if (FindClientById(id, out Client? found) && found != null)
                {
                    _logger.LogInformation($"{found.ServiceName} ({found.ID}) found and removed.");

                    _clients.Remove(found);
                    found.Dispose();
                }
                else
                    message = "Client not found.";

            }
            else
            {
                message = $"{guid} is not a valid guid.";
            }

            if (message != null)
            {
                _logger.LogError(message);
            }

            return message != null;
        }

        /// <summary>
        /// Check is a client exists by the name of its service.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal bool CheckClientExists(string name) => FindClientByName(name, out Client? _);

        internal bool FindClientByName(string name, out Client? found)
        {
            found = null;

            foreach(Client client in _clients)
            {
                if (client.ServiceName == name)
                {
                    found = client;
                    break;
                }
            }

            return found != null;
        }

        internal bool FindClientById(Guid id, out Client found)
        {
            found = null;

            foreach (Client client in _clients)
            {
                if (client.ID == id)
                {
                    found = client;
                    break;
                }
            }

            return found != null;
        }

        internal static string CreateClientAddress(uint port) => $"http://localhost:{port}/";
    }
}
