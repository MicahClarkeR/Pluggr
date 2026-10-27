namespace PluggrShared
{
    /// <summary>
    /// All possible statuses for the Pluggr Server and Clients.
    /// </summary>
    public static class Statuses
    {
        /// <summary>
        /// Pluggr Server response codes.
        /// </summary>
        public static class Server
        {
            public const uint RegisterSuccess = 11001;
            public const uint RegisterFailure = 11002;
            public const uint DeregisterSuccess = 11003;
            public const uint DeregisterFailure = 11004;
            public const uint SendMessageSuccess = 11005;
            public const uint SendMessageFailure = 11006;
        }

        /// <summary>
        /// Response codes for Pluggr Clients.
        /// </summary>
        public static class Client
        {
            public const uint SendMessageSuccess = 21001;
            public const uint SendMessageFailure = 21001;
        }
    }
}
