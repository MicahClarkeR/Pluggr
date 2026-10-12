using Google.Protobuf;
using PluggrClient;
using PluggrShared.Protos;
using System.Reflection;
using System.Text;
using System.Xml;
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
            using MemoryStream stream = new MemoryStream(data);
            XmlReader reader = XmlReader.Create(stream);
            InvokeMethodResponse response = new InvokeMethodResponse();

            while (reader.Read())
            {
                if (reader.Value != string.Empty)
                {
                    response.Data = Encoding.UTF8.GetBytes(reader.Value);
                }
            }

            return response;
        }
    }
}
