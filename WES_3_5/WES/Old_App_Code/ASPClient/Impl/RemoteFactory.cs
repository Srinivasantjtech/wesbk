namespace TradingBell.Common.Impl
{
    using TradingBell.Common;
    using System;

    public class RemoteFactory
    {
        public static IRemoteEasyAsk create(string hostName, int port, string dictionary)
        {
            return new RemoteEasyAsk(hostName, port, dictionary);
        }
    }
}

