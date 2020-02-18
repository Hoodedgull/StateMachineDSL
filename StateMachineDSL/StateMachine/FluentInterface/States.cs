using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDSL.FluentInterface
{
    public class States : Identifier
    {
        public State BuiltState { get; set; }
        public List<KeyValuePair<Events, Transitions>> Transitions { get; set; }
        public States(string name, StateMachineBuilder builder) : base(name, builder)
        {
            Transitions = new List<KeyValuePair<Events, Transitions>>();
        }

        internal void AddTransition(Events ev, Transitions transitionBuilder)
        {
            Transitions.Add(new KeyValuePair<Events, Transitions>(ev, transitionBuilder));
        }
    }
}
