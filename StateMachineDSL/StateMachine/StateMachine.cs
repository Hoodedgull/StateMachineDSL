using System;
using System.Collections.Generic;

namespace StateMachineDSL
{
    public class StateMachine
    {
        public State CurrentState { get; set; }

        public Dictionary<string, (Type, dynamic)> Variables { get; set; }

        public StateMachine()
        {
            Variables = new Dictionary<string, (Type, dynamic)>();
        }
        public void ProcessEvent(Event ev)
        {
            var transitions = CurrentState.GetTransitions(ev);
            if (transitions == null)
            {
                return;
            }

            foreach (var transition in transitions)
            {

                // Check conditions
                if (transition.Condition?.Invoke() == false)
                    continue;

                // Perform actions
                transition.Action?.Invoke();

                // Set new State
                if (transition.Target != null)
                    CurrentState = transition.Target;
            }
        }

        public void SetVariable<T>(string name, T value)
        {
            Variables[name] = (typeof(T), value);
        }

        public dynamic GetVariable(string key, Type type)
        {
            var found = Variables.TryGetValue(key, out var result);
            if (found && type == result.Item1)
            {
                return result.Item2;
            }
            else
            {
                return default;
            }
        }


    }
}
