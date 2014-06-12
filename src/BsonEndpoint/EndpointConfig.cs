
using System;

namespace BsonEndpoint
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public Configure Init()
        {
            return Configure.With().UseSerialization<Bson>();
        }
    }

    class Sender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Bus.Send(new Messages.Customer { Name = "bson" });
            }
        }

        public void Stop()
        {
        }
    }
}
