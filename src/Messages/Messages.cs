using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace Messages
{
    [Serializable]
    public class MyName : ICommand
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class Thankyou : IMessage
    {
    }
}
