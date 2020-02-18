using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace StateMachineDSL.FluentInterface
{
   
    public class Identifier
    {
        public string Name { get; set; }
        public StateMachineBuilder Builder { get; set; }

        public Identifier(string name, StateMachineBuilder builder)
        {
            Name = name;
            Builder = builder;
        }

        public static Identifier Create(Type type, string name, StateMachineBuilder builder)
        {
            ConstructorInfo ctor = type.GetConstructor(new Type[] { typeof(string), typeof(StateMachineBuilder) });
            return (Identifier) ctor.Invoke(new object[] { name, builder });
    }
    }
}
