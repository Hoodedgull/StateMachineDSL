using System;
using StateMachineDSL;

namespace CoffeeMaker
{
    class CoffeeMaker
    {
        static void Main(string[] args)
        {
            var stateMachine = SetupCoffeeMaker();
            Console.WriteLine(stateMachine.CurrentState.Name);

            ;
            stateMachine.SetVariable("aKey", true);
            bool result = stateMachine.GetVariable<bool>("aKey");

            stateMachine.SetVariable("aKey2", 2);
            int result2 = stateMachine.GetVariable<int>("aKey2");

            Console.WriteLine(result);
            Console.WriteLine(result2);

            while (true)
            {
                var input = Console.ReadLine();
                var inputEvent = new Event(input);
                stateMachine.ProcessEvent(inputEvent);
                Console.WriteLine(stateMachine.CurrentState.Name);
            }
        }

        static StateMachine SetupCoffeeMaker()
        {
            var stateMachine = new StateMachine();
            stateMachine.SetVariable("CupPlaced", false);
            var off = new State("Off");
            var on = new State("On");
            var payed = new State("payed");
            var brewingHotWater = new State("brewingHotWater");
            var brewingCoffee = new State("brewingCoffe");
            var brewingCocoa = new State("brewingCocoa");
            stateMachine.CurrentState = off;
            var turnOn = new Event("ON");
            var turnOff = new Event("OFF");
            var brewWater = new Event("WTR");
            var brewCoffee = new Event("CFE");
            var brewCocoa = new Event("COC");
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
                Condition = () => true == stateMachine.GetVariable<bool?>("cupIsPlaced")
            };
            var t3 = new Transition
            {
                Target = payed
            };
            var t4 = new Transition
            {
                Target = brewingCocoa,
                Condition = () => true == stateMachine.GetVariable<bool?>("cupIsPlaced")
            };
            var t5 = new Transition
            {
                Target = brewingCoffee,
                Condition = () => true == stateMachine.GetVariable<bool?>("cupIsPlaced")
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
