using System;
using Messages;
using NServiceBus;

namespace BinaryEndpoint
{
    internal class HandleResponse : IHandleMessages<MyResponse>
    {
        public void Handle(MyResponse message)
        {
            Console.Out.WriteLine(message.Message);
        }
    }
}