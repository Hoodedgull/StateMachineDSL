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
        public void ProcessEvent(Event ev){
            var transition = CurrentState.GetTransition(ev);
            if(transition == null)
            {
                return;
            }
            // Check conditions
            if (transition.Condition?.Invoke()==false)
                return;

            // Perform actions
            transition.Action?.Invoke();

            // Set new State
            if(transition.Target != null)
                CurrentState = transition.Target;
        }

        public void SetVariable<T>(string name, T value)
        {
            Variables[name] =  (typeof(T), value);
        }

        public T GetVariable<T>(string key)
        {
            var found = Variables.TryGetValue(key, out var result);
            if(found && typeof (T) == result.Item1)
            {
                return (T)result.Item2;
            }
            else
            {
                return default;
            }
        }


    }
}
