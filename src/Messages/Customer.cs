using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace Messages
{
    [Serializable]
    public class Customer : ICommand
    {
        public string Name { get; set; }
    }
}
