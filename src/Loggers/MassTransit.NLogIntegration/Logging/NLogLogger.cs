namespace MassTransit.NLogIntegration.Logging
{
    using MassTransit.Logging;
    using NLog;

    public class NLogLogger :
        ILogger
    {
        readonly LogFactory _factory = new LogFactory();

        public ILog Get(string name)
        {
            return new NLogLog(_factory.GetLogger(name), name);
        }
    }
}