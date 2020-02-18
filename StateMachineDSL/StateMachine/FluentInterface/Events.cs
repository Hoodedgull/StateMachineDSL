using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDSL.FluentInterface
{
    public class Events : Identifier
    {
        public Events(string name, StateMachineBuilder builder) : base(name, builder)
        {
        }

        public Event BuiltEvent { get; internal set; }
    }
}
