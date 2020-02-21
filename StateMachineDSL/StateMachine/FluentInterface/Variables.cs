using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDSL.FluentInterface
{
    public class Variables<T> : Identifier, IVariableBuilder<T>
    {
        public Variables(string name, StateMachineBuilder builder) : base(name, builder)
        {
        }


        public T Value { get; set; }

        public Func<bool> IsEqualTo(T other)
        {
            return () => Builder.stateMachine.GetVariable<T>(this.Name).Equals(other);
        }
        public Func<bool> IsntEqualTo(T  other)
        {
            return () => !Builder.stateMachine.GetVariable<T>(this.Name).Equals(other);
        }
        public Func<bool> IsGreaterThan(T other)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(float) || typeof(T) == typeof(double))
                return () => (dynamic)Builder.stateMachine.GetVariable<T>(this.Name) > (other);
            else
                throw new ArgumentException("Must be number in greater than");
        }

        public Func<bool> IsLessThan(T other)
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(float) || typeof(T) == typeof(double))
                return () => (dynamic)Builder.stateMachine.GetVariable<T>(this.Name) < (other);
            else
                throw new ArgumentException("Must be number in greater than");
        }
        public IStateMachineBuilder Add(T value)
        {
            this.Builder.SetAddAction(this, value);
            return Builder;
        }

        public IStateMachineBuilder Subtract(T value)
        {
            this.Builder.SetSubtractAction(this, value);
            return Builder;
        }

        IStateMachineBuilder IVariableBuilder<T>.SetValue(T value)
        {
            this.Builder.SetAssignmentAction(this, value);
            return Builder;
        }




    }



}
