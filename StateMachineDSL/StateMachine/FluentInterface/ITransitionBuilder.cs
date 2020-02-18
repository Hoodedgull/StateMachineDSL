using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDSL.FluentInterface
{
    public interface ITransitionBuilder
    {

        IStateMachineBuilder TransitionTo(States target);
        ITransitionBuilder CheckThat(Variables var);
        ITransitionBuilder ModifyBooleanVariable(Variables variable);
        IStateMachineBuilder SetValue(dynamic value);
    }
}
