using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Messages;
using NServiceBus;

namespace MyServer
{
    public class SayHello : IHandleMessages<Customer>
    {
        public void Handle(Customer message)
        {
            Console.Out.WriteLine("Hello from {0}", message.Name);
        }
    }
}
