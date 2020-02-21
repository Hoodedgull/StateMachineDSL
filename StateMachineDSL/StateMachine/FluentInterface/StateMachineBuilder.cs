using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace StateMachineDSL.FluentInterface
{
    public abstract class StateMachineBuilder : IStateMachineBuilder, ITransitionBuilder
    {
        private List<States> states;
        private States initialState;
        private List<States> currentStates;
        private Events currentEvent;
        private Func<bool> currentTransitionCondition;
        private Action currentTransitionAction;
        internal StateMachine stateMachine = new StateMachine();

        public StateMachineBuilder()
        {
            InitializeFields();
            states = new List<States>();
            currentStates = new List<States>();
        }

        public IStateMachineBuilder InitialState(States aState)
        {
            initialState = aState;
            currentStates.Clear();
            currentStates.Add(aState);
            states.Add(aState);
            return this;
        }

        internal void SetSubtractAction<T>(Variables<T> var, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(float) || typeof(T) == typeof(double))
                this.currentTransitionAction = () => stateMachine.SetVariable(var.Name, (dynamic)stateMachine.GetVariable<T>(var.Name) - (dynamic)value);
            else
                throw new ArgumentException("The type used in Add Should be a number");
        }

        internal void SetAddAction<T>(Variables<T> var, T value)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(float) || typeof(T) == typeof(double))
                this.currentTransitionAction = () => stateMachine.SetVariable(var.Name, (dynamic)stateMachine.GetVariable<T>(var.Name) + (dynamic)value);
            else
                throw new ArgumentException("The type used in Add Should be a number");
        }

        public IStateMachineBuilder State(States aState)
        {
            
            currentStates.Clear();
            currentStates.Add(aState);
            states.Add(aState);
            return this;
        }

        internal void SetAssignmentAction<T>(Variables<T> var, T value)
        {
            this.currentTransitionAction =  () => stateMachine.SetVariable(var.Name, value);
        }

        public ITransitionBuilder OnEvent(Events ev)
        {
            if (currentEvent == null)
            {
                currentEvent = ev;
                return this;
            }

            // This event is a self transition (action)
            TransitionTo(null);

            // Set event to next transition
            currentEvent = ev;
            return this;
        }

        public ITransitionBuilder CheckThat(Func<bool> condition)
        {

            currentTransitionCondition = condition;
            return this;
        }

        public IStateMachineBuilder TransitionTo(States target)
        {
            if (currentStates == null || currentStates.Count == 0)
                throw new Exception("Select a state before making a transition");
            if (currentEvent == null)
                throw new Exception("Select a triggering event before making a transition");

            foreach (var currentState in currentStates)
            {
                var transition = new Transitions(target);

                if (currentTransitionCondition != null)
                {
                    transition.Condition = currentTransitionCondition;
                }

                if (currentTransitionAction != null)
                {
                    transition.Action = currentTransitionAction;
                }

                currentState.AddTransition(currentEvent, transition);
            }
            currentEvent = null;
            currentTransitionCondition = null;
            currentTransitionAction = null;
            return this;
        }

        public IStateMachineBuilder EveryState()
        {
            currentStates.Clear();
            currentStates.AddRange(states);
            return this;
        }

        /// <summary>
        /// Initialize fields in the class symbol table as Fowler does.
        /// Adapted to C#
        /// </summary>
        private void InitializeFields()
        {
            foreach (FieldInfo f in this.GetType().GetFields())
            {

                f.SetValue(this, Identifier.Create(f.FieldType, f.Name, this));


            }
        }

        public abstract StateMachine BuildStateMachine();



        IVariableBuilder<T> ITransitionBuilder.ModifyVariable<T>(Variables<T> variable)
        {

            return variable;
        }

        //IStateMachineBuilder ITransitionBuilder.SetValue(dynamic value)
        //{
        //    currentTransitionActionValue = value;
        //    return this;
        //}

        public StateMachine Build()
        {
            // A self-transition is still pending
            if (currentEvent != null)
            {
                TransitionTo(null);
            }

            // 1st pass. Create states & events
            foreach (var state in states)
            {
                var stat = new State(state.Name);
                if (state == initialState)
                    stateMachine.CurrentState = stat;

                foreach (var kvp in state.Transitions)
                {
                    if (kvp.Key.BuiltEvent == null)
                        kvp.Key.BuiltEvent = new Event(kvp.Key.Name);
                }

                state.BuiltState = stat;
            }

            // 2nd pass. Assign transitions
            foreach (var state in states)
            {
                var stat = state.BuiltState;
                foreach (var kvp in state.Transitions)
                {
                    var transition = kvp.Value;
                    var t = new Transition
                    {
                        Target = transition.Target?.BuiltState
                    };

                    if (transition.Condition != null)
                    {
                        t.Condition = transition.Condition;
                    }


                    if (transition.Action != null)
                    {
                        t.Action = transition.Action;
                    }

                    stat.AddTransition(kvp.Key.BuiltEvent, t);
                }




            }
            return stateMachine;

        }

        public ITransitionBuilder And()
        {
            return this;
        }
    }
}