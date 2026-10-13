using Google.Protobuf;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace PluggrShared.Helpers
{
    /// <summary>
    /// Helper class to assist in converting between commonly used data types in Pluggr.
    /// </summary>
    public static class DataHelper
    {
        public static byte[] StringToBytes(string value) => Encoding.UTF8.GetBytes(value);
        public static string BytesToString(byte[] bytes) => Encoding.UTF8.GetString(bytes);

        public static XElement BytesToXElement(byte[] bytes)
        {
            using MemoryStream stream = new MemoryStream(bytes);
            XElement xml = XElement.Load(stream);

            return xml;
        }

        public static byte[] XElementToBytes(XElement element)
        {
            string xmlString = element.ToString();
            byte[] data = Encoding.UTF8.GetBytes(xmlString);

            return data;
        }

        public static ByteString XElementToByteString(XElement element)
        {
            byte[] bytes = XElementToBytes(element);
            return ByteString.CopyFrom(bytes);
        }

        public static XElement ByteStringToXElement(ByteString bytes)
        {
            byte[] data = bytes.Memory.ToArray();
            return BytesToXElement(data);
        }

        public static JsonDocument BytesToJson(byte[] bytes)
        {
            string jsonString = Encoding.UTF8.GetString(bytes);
            JsonDocument json = JsonDocument.Parse(jsonString);

            return json;
        }

        public static byte[] JsonToBytes(JsonDocument json)
        {
            string jsonString = json.RootElement.ToString();
            byte[] data = Encoding.UTF8.GetBytes(jsonString);

            return data;
        }

        public static ByteString JsonToByteString(JsonDocument json)
        {
            byte[] bytes = JsonToBytes(json);
            return ByteString.CopyFrom(bytes);
        }

        public static JsonDocument ByteStringToJson(ByteString bytes)
        {
            byte[] data = bytes.Memory.ToArray();
            return BytesToJson(data);
        }

        public static ByteString StringToByteString(string str) => ByteString.CopyFromUtf8(str);
    }
}
