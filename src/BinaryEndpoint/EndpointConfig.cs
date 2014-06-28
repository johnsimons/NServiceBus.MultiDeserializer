using System;
using Messages;
using NServiceBus;

namespace BinaryEndpoint
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(ConfigurationBuilder builder)
        {
            Console.Title = "Binary";
        }
    }

    internal class SetSerializer : INeedInitialization
    {
        public void Init(Configure config)
        {
            config.UseSerialization<Binary>();
        }
    }

    internal class Sender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Bus.Send(new MyRequest {ContentType = "binary"});
            }
        }

        public void Stop()
        {
        }
    }
}