using StateMachineDSL;
using StateMachineDSL.FluentInterface;
using System;

namespace CookingHood
{
    class CookingHoodProgram
    {

        private const int MIN_POWER = 1;
        private const int MAX_POWER = 6;

        static void Main(string[] args)
        {
            var stateMachine = SetupCookingHood();
            Console.Write(stateMachine.CurrentState.Name + "     | ");
            var stateMachine2 = SetupCookingHoodFluent();
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

        public static StateMachine SetupCookingHoodFluent()
        {
            return new CookingHoodBuilder().BuildStateMachine();
        }
        class CookingHoodBuilder : StateMachineBuilder
        {

            public States off, waitingForPayment, brewingHotWater, brewingCoffee, brewingCocoa, waitingForDrinkSelection;
            public Events ON, PAID, WATER, COFFEE, COCOA, OFF, DONE, CUP, NOCUP;
            public Variables cupIsPlaced;

            public override StateMachine BuildStateMachine()
            {
                var stateMachine = InitialState(off)



                    .Build();
                return stateMachine;
            }




        }

        public static StateMachine SetupCookingHood()
        {
            var stateMachine = new StateMachine();
            var s1 = new State("PowerOff");
            var s2 = new State("PowerOn");
            var s3 = new State("MaxPower");

            var plus = new Event("PLUS");
            var minus = new Event("MINUS");

            s1.AddTransition(plus, new Transition
            {
                Target = s2,
                Action = () => stateMachine.SetVariable("power", MIN_POWER)
            });

            s2.AddTransition(plus, new Transition
            {
                Target = s3,
                Condition = () => stateMachine.GetVariable("power", typeof(int)) == MAX_POWER
            });
            s2.AddTransition(plus, new Transition
            {
                Condition = () => stateMachine.GetVariable("power", typeof(int)) < MAX_POWER,
                Action = () => stateMachine.SetVariable("power", stateMachine.GetVariable("power", typeof(int)) + 1)
            });

            s2.AddTransition(minus, new Transition
            {
                Target = s1,
                Condition = () => stateMachine.GetVariable("power", typeof(int)) == MIN_POWER
            });
            s2.AddTransition(minus, new Transition
            {
                Condition = () => stateMachine.GetVariable("power", typeof(int)) > MIN_POWER,
                Action = () => stateMachine.SetVariable("power", stateMachine.GetVariable("power", typeof(int)) - 1)
            });

            s3.AddTransition(minus, new Transition
            {
                Target = s2,
                Action = () => stateMachine.SetVariable("power", MAX_POWER)
            });



            stateMachine.CurrentState = s1;
            return stateMachine;
        }
    }
}
