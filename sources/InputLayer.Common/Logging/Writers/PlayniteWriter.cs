namespace InputLayer.Common.Logging.Writers
{
    public class PlayniteWriter : ILogWriter
    {
        private readonly Playnite.SDK.ILogger _logger;

        public PlayniteWriter(Playnite.SDK.ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Write(string line)
        {
            _logger.Info(line);
        }
    }
}