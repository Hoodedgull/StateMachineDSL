using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDSL.FluentInterface
{
    public interface IStateMachineBuilder
    {
        IStateMachineBuilder InitialState(States aState);
        IStateMachineBuilder State(States aState);
        ITransitionBuilder OnEvent(Events ev);
        Variables BooleanVariable(Variables variable);
        IStateMachineBuilder EveryState();

        StateMachine Build();
    }
}
