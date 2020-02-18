namespace StateMachineDSL.FluentInterface
{
    public class Transitions
    {
        public States Target { get; set; }
        public (Operator op, Variables var, dynamic val) Condition { get; set; }

        public (Variables var, dynamic val) Action { get; set; }

        public Transitions()
        {

        }


        public Transitions(States target)
        {
            this.Target = target;
        }
    }
}