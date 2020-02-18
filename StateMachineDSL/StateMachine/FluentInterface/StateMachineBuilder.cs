using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace StateMachineDSL.FluentInterface
{
    public abstract class StateMachineBuilder : IStateMachineBuilder, ITransitionBuilder
    {
        private List<States> states;
        private List<Variables> variables;
        private States initialState;
        private List<States> currentStates;
        private Events currentEvent;
        private Variables currentTransitionConditionVariable;
        private Operator currentTransitionConditionOperator;
        private dynamic currentTransitionConditionValue;
        private Variables currentTransitionActionVariable;
        private dynamic currentTransitionActionValue;
        private bool shouldAddAnotherState = false;

        public StateMachineBuilder()
        {
            InitializeFields();
            states = new List<States>();
            variables = new List<Variables>();
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

        public IStateMachineBuilder State(States aState)
        {
            if (!shouldAddAnotherState)
                currentStates.Clear();

            currentStates.Add(aState);
            states.Add(aState);
            return this;
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

        public ITransitionBuilder CheckThat(Variables var)
        {
            currentTransitionConditionVariable = var;
            currentTransitionConditionOperator = Operator.EQUALS;
            currentTransitionConditionValue = true;
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

                if (currentTransitionConditionVariable != null)
                {
                    transition.Condition = (currentTransitionConditionOperator, currentTransitionConditionVariable, currentTransitionConditionValue);
                }

                if (currentTransitionActionVariable != null)
                {
                    transition.Action = (currentTransitionActionVariable, currentTransitionActionValue);
                }

                currentState.AddTransition(currentEvent, transition);
            }
            currentEvent = null;
            currentTransitionConditionVariable = null;
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

        public Variables BooleanVariable(Variables variable)
        {
            variable.Type = typeof(bool);
            variables.Add(variable);
            return variable;
        }

        ITransitionBuilder ITransitionBuilder.ModifyBooleanVariable(Variables variable)
        {
            variable.Type = typeof(bool);
            currentTransitionActionVariable = variable;
            return this;
        }

        IStateMachineBuilder ITransitionBuilder.SetValue(dynamic value)
        {
            currentTransitionActionValue = value;
            return this;
        }

        public StateMachine Build()
        {
            // A self-transition is still pending
            if(currentEvent != null)
            {
                TransitionTo(null);
            }

            // Then build
            var stateMachine = new StateMachine();
            foreach (var var in variables)
            {
                stateMachine.SetVariable(var.Name, var.Value);
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

                    if (!transition.Condition.Equals(default))
                    {
                        switch (transition.Condition.op)
                        {
                            case Operator.EQUALS:
                                t.Condition = () => stateMachine.GetVariable(transition.Condition.var.Name, transition.Condition.var.Type) == transition.Condition.val;
                                break;
                            case Operator.NotEQUALS:
                                t.Condition = () => stateMachine.GetVariable(transition.Condition.var.Name, transition.Condition.var.Type) != transition.Condition.val;
                                break;
                            case Operator.GreaterThan:
                                t.Condition = () => stateMachine.GetVariable(transition.Condition.var.Name, transition.Condition.var.Type) > transition.Condition.val;
                                break;
                            case Operator.LessThan:
                                t.Condition = () => stateMachine.GetVariable(transition.Condition.var.Name, transition.Condition.var.Type) < transition.Condition.val;
                                break;
                            case Operator.GreaterThanOrEqual:
                                t.Condition = () => stateMachine.GetVariable(transition.Condition.var.Name, transition.Condition.var.Type) >= transition.Condition.val;
                                break;
                            case Operator.LessThanOrEqual:
                                t.Condition = () => stateMachine.GetVariable(transition.Condition.var.Name, transition.Condition.var.Type) <= transition.Condition.val;
                                break;
                        }
                    }


                    if (!transition.Action.Equals(default))
                    {
                        t.Action = () => stateMachine.SetVariable(transition.Action.var.Name, transition.Action.val);
                    }

                    stat.AddTransition(kvp.Key.BuiltEvent, t);
                }




            }
            return stateMachine;

        }
    }
}