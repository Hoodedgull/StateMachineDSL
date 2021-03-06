﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineDSL
{
    public class State
    {
        public string Name { get; set; }

        List<KeyValuePair<Event,Transition>> Transitions { get; set; }

   
        public State(string name)
        {
            this.Name = name;
            Transitions = new List<KeyValuePair<Event, Transition>>();
        }

        internal IEnumerable<Transition> GetTransitions(Event ev)
        {
            return Transitions.Where(kvp => kvp.Key.Code.Equals(ev.Code, StringComparison.InvariantCultureIgnoreCase)).Select(kvp => kvp.Value);
            
        }

     
        public void AddTransition(Event trigger, Transition transition)
        {
            Transitions.Add(new KeyValuePair<Event, Transition>(trigger, transition));
        }
    }
}