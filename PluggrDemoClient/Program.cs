using Google.Protobuf;
using PluggrClient;
using PluggrShared.Helpers;
using PluggrShared.Protos;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PluggrDemoClient
{
    internal class Program
    {
        private static PluggrClient.PluggrClient _client;

        static void Main(string[] args)
        {
            _client = new PluggrClient.PluggrClient("DemoService", 3000);

            Task.Run(() => PluggrClient.PluggrClientService.Initalise(_client.Port));

            MethodInfo info = typeof(Program).GetMethod(nameof(RegisteredMethod));
            if (PluggrMessageManager.RegisterMethod("Method", null, info))
            {
                Console.WriteLine("Method successfully registered.");
            }

            Console.Read();
        }

        public static InvokeMethodResponse RegisteredMethod(byte[] data)
        {
            InvokeMethodResponse response = new InvokeMethodResponse();
            XElement xml = DataHelper.BytesToXElement(data);
            XElement? message = xml.Elements().FirstOrDefault();
            string responseString = $"Request message recieved: {message?.Value ?? "Valid XML was not given."}";

            response.Data = DataHelper.StringToBytes(responseString);

            return response;
        }
    }
}
