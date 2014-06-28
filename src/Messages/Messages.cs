using System;
using NServiceBus;

namespace Messages
{
    [Serializable]
    public class MyRequest : ICommand
    {
        public string ContentType { get; set; }
    }

    [Serializable]
    public class MyResponse : IMessage
    {
        public string Message { get; set; }
    }
}