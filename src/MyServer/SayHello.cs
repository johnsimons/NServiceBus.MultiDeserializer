using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Messages;
using NServiceBus;

namespace MyServer
{
    public class SayHello : IHandleMessages<MyName>
    {
        public IBus Bus { get; set; }

        public void Handle(MyName message)
        {
            Console.Out.WriteLine("Hello from {0}", message.Name);

            Bus.Reply(new Thankyou());
        }
    }
}
