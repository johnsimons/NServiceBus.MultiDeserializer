using System;
using Messages;
using NServiceBus;

namespace BsonEndpoint
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(ConfigurationBuilder builder)
        {
        }
    }

    internal class SetSerializer : INeedInitialization
    {
        public void Init(Configure config)
        {
            config.UseSerialization<Bson>();
        }
    }

    internal class Sender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Bus.Send(new MyRequest {ContentType = "bson"});
            }
        }

        public void Stop()
        {
        }
    }
}