namespace PluggrShared
{
    public static class LoggerCreator
    {
        internal static LoggerFactory _factory = new LoggerFactory();

        public static ILogger<T> Create<T>()
        {
            return _factory.CreateLogger<T>();
        }
    }
}
