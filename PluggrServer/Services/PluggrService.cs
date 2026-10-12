using Grpc.Core;
using Microsoft.AspNetCore.Identity.Data;
using PluggrServer.Clients;
using PluggrShared.Protos;
using PluggrShared;
using static PluggrShared.Protos.PluggrClientService;

namespace PluggrServer.Services
{
    public class PluggrService : PluggrShared.Protos.PluggrService.PluggrServiceBase
    {
        private readonly ILogger _logger;
        private static ClientManager ClientManager => Session.Clients;
        
        public PluggrService(ILogger? logger = null)
        {
            _logger = logger ?? LoggerCreator.Create<PluggrService>();
        }

        /// <summary>
        /// Attempts to register a Client with the internal Client Manager.
        /// </summary>
        /// <param name="request">Request received from the pending Client.</param>
        /// <param name="context">Context.</param>
        /// <returns>Response to be returned to the calling Client.</returns>
        public override Task<RegisterClientResponse> RegisterClient(RegisterClientRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Received register request from {request.Service}");

            Guid? guid = ClientManager.RegisterClient(request.Service, request.Port, out string? message);
            uint status = (guid != null) ? Statuses.Server.RegisterSuccess : Statuses.Server.RegisterFailure;

            _logger.LogInformation((guid != null) ? "Client was successfully registered." : $"Client registration failure: {message}");

            return Task.FromResult(new RegisterClientResponse
            {
                Status = status,
                Guid = guid?.ToString(),
                Message = message ?? string.Empty
            });
        }

        public override Task<DeregisterClientResponse> DeregisterClient(DeregisterClientRequest request, ServerCallContext context)
        {
            bool success = ClientManager.DeregisterClient(request.Guid, out string? message);

            return Task.FromResult(new DeregisterClientResponse
            {
                Status = (success) ? Statuses.Server.DeregisterSuccess : Statuses.Server.DeregisterFailure,
                Message = message ?? string.Empty
            });
        }

        public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
        {
            SendMessageResponse messageResponse;

            if (Guid.TryParse(request.ClientGuid, out Guid clientId))
            {
                if (ClientManager.FindClientById(clientId, out Client client))
                {
                    SendClientMessageRequest message = new SendClientMessageRequest()
                    {
                        Data = request.Data,
                        Endpoint = request.Endpoint
                    };

                    SendClientMessageResponse response = client.SendMessage(message);

                    messageResponse = new SendMessageResponse
                    {
                        Data = response.Data,
                        Message = response.Message,
                        Status = Statuses.Server.SendMessageSuccess,
                        MessageStatus = response.Status
                    };
                }
                else
                {
                    messageResponse = new SendMessageResponse()
                    {
                        Status = Statuses.Server.SendMessageFailure,
                        Message = $"Client {clientId} has not been registered and does not exist."
                    };
                }
            }
            else
            {
                messageResponse = new SendMessageResponse()
                {
                    Status = Statuses.Server.SendMessageFailure,
                    Message = $"{clientId} is not a valid GUID."
                };
            }

            return Task.FromResult(messageResponse);
        }

        public override Task<GetClientInfoResponse> GetClientInfo(GetClientInfoRequest request, ServerCallContext context)
        {
            GetClientInfoResponse response;

            if (ClientManager.FindClientByName(request.Service, out Client client))
            {
                response = new GetClientInfoResponse()
                {
                    Status = Statuses.Server.SendMessageSuccess,
                    ClientGuid = client.ID.ToString(),
                    Message = string.Empty
                };
            }
            else
            {
                response = new GetClientInfoResponse()
                {
                    Status = Statuses.Server.SendMessageFailure,
                    Message = "Service does not exist."
                };
            }

            return Task.FromResult(response);
        }
    }
}
