
using System;

namespace BinaryEndpoint
{
    using NServiceBus;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
	    public Configure Init()
	    {
	        return Configure.With().UseSerialization<Binary>();
	    }
    }

    class Sender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Bus.Send(new Messages.Customer { Name = "binary" });
            }
        }

        public void Stop()
        {
        }
    }
}
