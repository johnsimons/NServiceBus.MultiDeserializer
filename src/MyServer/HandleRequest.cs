using System;
using Messages;
using NServiceBus;

namespace MyServer
{
    public class HandleRequest : IHandleMessages<MyRequest>
    {
        public IBus Bus { get; set; }

        public void Handle(MyRequest message)
        {
            Console.Out.WriteLine("Received message with content type of '{0}'", message.ContentType);

            Bus.Reply(new MyResponse
            {
                Message = string.Format("Received message with content type of '{0}'", message.ContentType)
            });
        }
    }
}