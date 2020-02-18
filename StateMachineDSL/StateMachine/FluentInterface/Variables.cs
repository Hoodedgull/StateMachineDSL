using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDSL.FluentInterface
{
    public class Variables:Identifier
    {
        public Variables(string name, StateMachineBuilder builder) : base(name, builder)
        {
        }

        public Type Type { get; set; }

        public dynamic Value { get; set; }
        
        public StateMachineBuilder SetValue(dynamic value)
        {
            if(value.GetType() != Type)
            {
                throw new Exception("Type mismatch");
            }
            this.Value = value;
            return Builder;
        }
    }
}
