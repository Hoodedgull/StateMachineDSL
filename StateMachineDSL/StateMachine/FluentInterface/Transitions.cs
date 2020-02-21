using System;

namespace StateMachineDSL.FluentInterface
{
    public class Transitions
    {
        public States Target { get; set; }
        public Func<bool> Condition { get; set; }

        public Action Action { get; set; }

        public Transitions()
        {

        }


        public Transitions(States target)
        {
            this.Target = target;
        }
    }
}