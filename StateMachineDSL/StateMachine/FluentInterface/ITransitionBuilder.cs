using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDSL.FluentInterface
{
    public interface ITransitionBuilder
    {

        IStateMachineBuilder TransitionTo(States target);
        ITransitionBuilder CheckThat(Func<bool> condition);
        IVariableBuilder<T> ModifyVariable<T>(Variables<T> variable);
    }

    public interface IVariableBuilder<T>
    {
        IStateMachineBuilder SetValue(T value);

        IStateMachineBuilder Add(T value);
        IStateMachineBuilder Subtract(T value);


    }
}
