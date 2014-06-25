
using System;

namespace JsonEndpoint
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(ConfigurationBuilder builder)
        { }
    }

    class SetSerializer : INeedInitialization
    {
        public void Init(Configure config)
        {
            config.UseSerialization<Json>();
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
