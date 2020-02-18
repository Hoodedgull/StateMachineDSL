using System;

namespace StateMachineDSL
{
    public class Transition
    {
        public Transition() { }
        public Transition(State target)
        {
            Target = target;
        }

        public State Target { get; set; }
        public Func<bool> Condition { get; set; }
        public Action Action { get; set; }
    }
}