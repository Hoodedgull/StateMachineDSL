﻿using StateMachineDSL;
using StateMachineDSL.FluentInterface;
using System;

namespace CookingHood
{
    public class CookingHoodProgram
    {

        private const int MIN_POWER = 1;
        private const int MAX_POWER = 6;

        static void Main(string[] args)
        {
            Console.WriteLine("StateMachine implemented Traditionally | StateMachine Implemented with DSL");
            Console.WriteLine("");
            var stateMachine = SetupCookingHood();
            Console.Write(stateMachine.CurrentState.Name.PadRight(39)+"| ");
            var stateMachine2 = SetupCookingHoodFluent();
            Console.WriteLine(stateMachine2.CurrentState.Name);


            while (true)
            {
                var input = Console.ReadLine();
                var inputEvent = new Event(input);
                stateMachine.ProcessEvent(inputEvent);
                stateMachine2.ProcessEvent(inputEvent);
                Console.Write((stateMachine.CurrentState.Name + " - " + stateMachine.GetVariable<int>("power")).PadRight(39) + "| ");
                Console.WriteLine((stateMachine2.CurrentState.Name + " - " + stateMachine2.GetVariable<int>("power")).PadRight(39));
            }
        }

        public static StateMachine SetupCookingHoodFluent()
        {
            return new CookingHoodBuilder().BuildStateMachine();
        }
        class CookingHoodBuilder : StateMachineBuilder
        {

            public States PowerOff, PowerOn, MaxPower;
            public Events PLUS, MINUS;
            public Variables<int> power;

            public override StateMachine BuildStateMachine()
            {
                var stateMachine = 
                    InitialState(PowerOff)
                        .OnEvent(PLUS)
                            .ModifyVariable(power).SetValue(MIN_POWER)
                            .And().TransitionTo(PowerOn)
                    .State(PowerOn)
                        .OnEvent(MINUS)
                            .CheckThat(power.IsEqualTo(MIN_POWER))
                                .TransitionTo(PowerOff)
                        .OnEvent(MINUS)
                            .CheckThat(power.IsGreaterThan(MIN_POWER))
                                .ModifyVariable(power).Subtract(1)
                        .OnEvent(PLUS)
                            .CheckThat(power.IsLessThan(MAX_POWER))
                                .ModifyVariable(power).Add(1)
                        .OnEvent(PLUS)
                            .CheckThat(power.IsEqualTo(MAX_POWER))
                                .TransitionTo(MaxPower)
                    .State(MaxPower)
                        .OnEvent(MINUS)
                            .ModifyVariable(power).SetValue(MAX_POWER)
                            .And().TransitionTo(PowerOn)
                           
                    



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
                Condition = () => stateMachine.GetVariable<int>("power") == MAX_POWER
            });
            s2.AddTransition(plus, new Transition
            {
                Condition = () => stateMachine.GetVariable<int>("power") < MAX_POWER,
                Action = () => stateMachine.SetVariable("power", stateMachine.GetVariable<int>("power") + 1)
            });

            s2.AddTransition(minus, new Transition
            {
                Target = s1,
                Condition = () => stateMachine.GetVariable<int>("power") == MIN_POWER
            });
            s2.AddTransition(minus, new Transition
            {
                Condition = () => stateMachine.GetVariable<int>("power") > MIN_POWER,
                Action = () => stateMachine.SetVariable("power", stateMachine.GetVariable<int>("power") - 1)
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
