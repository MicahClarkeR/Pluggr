using PluggrShared;
using PluggrShared.Protos;
using System.Reflection;

namespace PluggrClient
{
    public class PluggrMessageManager
    {
        private static Dictionary<string, RegisteredMethod> _registeredMethods = new Dictionary<string, RegisteredMethod>(); // Endpoint and Registered Method.
        private static ILogger _logger;

        static PluggrMessageManager()
        {
            _logger = LoggerCreator.Create<PluggrMessageManager>();
        }

        public static bool RegisterMethod(string endpoint, object? objectToInvoke, MethodInfo method)
        {
            if (_registeredMethods.ContainsKey(endpoint))
                return false;

            RegisteredMethod registration = new RegisteredMethod()
            {
                Object = objectToInvoke,
                MethodInfo = method
            };
            _registeredMethods.Add(endpoint, registration);
            return true;
        }

        public static bool InvokeMethod(SendClientMessageRequest request, out InvokeMethodResponse response)
        {
            string endpoint = request.Endpoint;
            bool result;

            _logger.LogDebug($"Received request to invoke method '{endpoint}'.");

            if (_registeredMethods.TryGetValue(endpoint, out RegisteredMethod method))
            {
                object? responseObject = method.MethodInfo.Invoke(method.Object, [request.Data.ToByteArray()]);

                if (responseObject != null && responseObject is InvokeMethodResponse methodResponse)
                {
                    _logger.LogDebug($"Method successfully invoked. Message returned: {methodResponse.Message ?? "null"}");
                    response = methodResponse;
                }
                else
                {
                    response = new InvokeMethodResponse();
                    _logger.LogError($"Return type from method '{method.MethodInfo.Name}' did not return an InvokeMethodResponse struct. {responseObject?.GetType().Name ?? "null"} was returned.");
                }

                result = true;
            }
            else
            {
                response = new InvokeMethodResponse()
                {
                    Data = null,
                    Message = $"Method '{endpoint}' has not been registered."
                };

                _logger.LogError(response.Message);

                result = false;
            }

            return result;
        }

        private struct RegisteredMethod
        {
            public object? Object;
            public MethodInfo MethodInfo;
        }

    }

    public struct InvokeMethodResponse
    {
        public byte[]? Data;
        public string? Message;

    }

}
