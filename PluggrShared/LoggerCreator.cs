namespace PluggrShared
{
    /// <summary>
    /// Helper class to make creating Loggers more simple.
    /// </summary>
    public static class LoggerCreator
    {
        internal static LoggerFactory _factory = new LoggerFactory();

        public static ILogger<T> Create<T>()
        {
            return _factory.CreateLogger<T>();
        }
    }
}
