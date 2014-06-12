using System;
using Messages;
using NServiceBus;

namespace BinaryEndpoint
{
    class HandleThankYou: IHandleMessages<Thankyou>
    {
        public void Handle(Thankyou message)
        {
            Console.Out.WriteLine("Got it");
        }
    }
}