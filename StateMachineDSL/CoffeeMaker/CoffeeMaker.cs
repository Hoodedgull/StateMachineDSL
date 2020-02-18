using System;
using StateMachineDSL;
using StateMachineDSL.FluentInterface;

namespace CoffeeMaker
{
    class CoffeeMaker
    {
        static void Main(string[] args)
        {
            var stateMachine = SetupCoffeeMaker();
            Console.Write(stateMachine.CurrentState.Name + "     | ");
            var stateMachine2 = SetupCoffeeMachineFluent();
            Console.WriteLine(stateMachine2.CurrentState.Name);


            while (true)
            {
                var input = Console.ReadLine();
                var inputEvent = new Event(input);
                stateMachine.ProcessEvent(inputEvent);
                stateMachine2.ProcessEvent(inputEvent);
                Console.Write(stateMachine.CurrentState.Name + "     | ");
                Console.WriteLine(stateMachine2.CurrentState.Name);
            }
        }

        public static StateMachine SetupCoffeeMachineFluent()
        {
            return new CoffeeMachineBuilder().BuildStateMachine();
        }
        class CoffeeMachineBuilder : StateMachineBuilder
        {
            public States off, waitingForPayment, brewingHotWater, brewingCoffee, brewingCocoa, waitingForDrinkSelection;
            public Events ON, PAID, WATER, COFFEE, COCOA, OFF, DONE, CUP, NOCUP;
            public Variables cupIsPlaced;

            public override StateMachine BuildStateMachine()
            {

                BooleanVariable(cupIsPlaced).SetValue(false);
                var stateMachine = InitialState(off)
                    .OnEvent(ON)
                        .TransitionTo(waitingForPayment)
                .State(waitingForPayment)
                    .OnEvent(PAID)
                        .TransitionTo(waitingForDrinkSelection)
                    .OnEvent(WATER) // Water is free. Does not need payment.
                        .CheckThat(cupIsPlaced)
                        .TransitionTo(brewingHotWater)
                .State(waitingForDrinkSelection)
                    .OnEvent(COFFEE)
                        .CheckThat(cupIsPlaced)
                        .TransitionTo(brewingCoffee)
                    .OnEvent(COCOA)
                        .CheckThat(cupIsPlaced)
                        .TransitionTo(brewingCocoa)
                .State(brewingHotWater)
                    .OnEvent(DONE)
                        .TransitionTo(waitingForPayment)
                .State(brewingCocoa)
                    .OnEvent(DONE)
                        .TransitionTo(waitingForPayment)
                .State(brewingCoffee)
                    .OnEvent(DONE)
                        .TransitionTo(waitingForPayment)
                .EveryState() // Add the following transitions to all states
                    .OnEvent(OFF)
                        .TransitionTo(off)
                   .OnEvent(CUP)
                        .ModifyBooleanVariable(cupIsPlaced).SetValue(true)
                    .OnEvent(NOCUP)
                        .ModifyBooleanVariable(cupIsPlaced).SetValue(false)
                .Build();

               return stateMachine;
            }




        }

        public static StateMachine SetupCoffeeMaker()
        {
            var stateMachine = new StateMachine();
            var off = new State("off");
            var on = new State("waitingForPayment");
            var payed = new State("waitingForDrinkSelection");
            var brewingHotWater = new State("brewingHotWater");
            var brewingCoffee = new State("brewingCoffe");
            var brewingCocoa = new State("brewingCocoa");
            stateMachine.CurrentState = off;
            var turnOn = new Event("ON");
            var turnOff = new Event("OFF");
            var brewWater = new Event("WATER");
            var brewCoffee = new Event("COFFEE");
            var brewCocoa = new Event("COCOA");
            var paymentRecieved = new Event("PAID");
            var doneBrewing = new Event("DONE");
            var cupPlaced = new Event("CUP");
            var cupRemoved = new Event("NOCUP");

            var t1 = new Transition
            {
                Target = on
            };
            var t2 = new Transition
            {
                Target = brewingHotWater,
                Condition = () => true == stateMachine.GetVariable("cupIsPlaced", typeof(bool?))
            };
            var t3 = new Transition
            {
                Target = payed
            };
            var t4 = new Transition
            {
                Target = brewingCocoa,
                Condition = () => true == stateMachine.GetVariable("cupIsPlaced", typeof(bool?))
            };
            var t5 = new Transition
            {
                Target = brewingCoffee,
                Condition = () => true == stateMachine.GetVariable("cupIsPlaced", typeof(bool?))
            };
            var t6 = new Transition
            {
                Target = on
            };
            var t7 = new Transition
            {
                Target = on
            };
            var t8 = new Transition
            {
                Target = on
            };

            off.AddTransition(turnOn, t1);
            on.AddTransition(brewWater, t2);
            on.AddTransition(paymentRecieved, t3);
            payed.AddTransition(brewCocoa, t4);
            payed.AddTransition(brewCoffee, t5);
            brewingHotWater.AddTransition(doneBrewing, t6);
            brewingCoffee.AddTransition(doneBrewing, t7);
            brewingCocoa.AddTransition(doneBrewing, t8);

            var t10 = new Transition
            {
                Target = off
            };
            var t11 = new Transition
            {
                Target = off
            };
            var t12 = new Transition
            {
                Target = off
            };
            var t13 = new Transition
            {
                Target = off
            };
            var t14 = new Transition
            {
                Target = off
            };
            on.AddTransition(turnOff, t10);
            payed.AddTransition(turnOff, t11);
            brewingCocoa.AddTransition(turnOff, t12);
            brewingCoffee.AddTransition(turnOff, t13);
            brewingHotWater.AddTransition(turnOff, t14);

            off.AddTransition(cupPlaced, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", true) });
            on.AddTransition(cupPlaced, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", true) });
            payed.AddTransition(cupPlaced, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", true) });
            brewingCocoa.AddTransition(cupPlaced, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", true) });
            brewingCoffee.AddTransition(cupPlaced, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", true) });
            brewingHotWater.AddTransition(cupPlaced, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", true) });

            off.AddTransition(cupRemoved, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", false) });
            on.AddTransition(cupRemoved, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", false) });
            payed.AddTransition(cupRemoved, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", false) });
            brewingCocoa.AddTransition(cupRemoved, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", false) });
            brewingCoffee.AddTransition(cupRemoved, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", false) });
            brewingHotWater.AddTransition(cupRemoved, new Transition { Action = () => stateMachine.SetVariable<bool?>("cupIsPlaced", false) });











            return stateMachine;
        }
    }
}
