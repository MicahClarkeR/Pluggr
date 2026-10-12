using Google.Protobuf;
using PluggrClient;
using PluggrShared.Protos;
using System.Reflection;
using System.Text;

namespace PluggrDemoSender
{
    internal class Program
    {
        private static PluggrClient.PluggrClient _senderClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Initialising...");

            _senderClient = new PluggrClient.PluggrClient("DemoSender", 3001);

            PluggrClient.PluggrClientService.Initalise(_senderClient.Port);

            GetClientInfoResponse info = _senderClient.ServerClient.GetClientInfo(new GetClientInfoRequest()
            {
                Service = "DemoService"
            });

            Thread.Sleep(1000);

            Console.WriteLine("Type message to send:");
            string message = Console.ReadLine() ?? string.Empty;

            SendMessageRequest request = new SendMessageRequest()
            {
                ClientGuid = info.ClientGuid.ToString(),
                Endpoint = "Method",
                Data = ByteString.CopyFromUtf8($"<Message>{message}</Message>")
            };
            
            SendMessageResponse response = _senderClient.ServerClient.SendMessage(request);

            Console.WriteLine($"Response message: {response.Data.ToStringUtf8()}");

            Console.ReadKey();

        }
    }
}