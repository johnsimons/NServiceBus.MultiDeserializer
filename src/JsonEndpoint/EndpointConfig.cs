
using System;

namespace JsonEndpoint
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public Configure Init()
        {
            return Configure.With().UseSerialization<Json>();
        }
    }

    class Sender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Bus.Send(new Messages.MyName { Name = "json" });
            }
        }

        public void Stop()
        {
        }
    }
}
