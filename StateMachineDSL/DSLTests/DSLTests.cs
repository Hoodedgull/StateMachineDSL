using CoffeeMaker;
using CookingHood;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineDSL;
using System;
using System.Collections.Generic;

namespace DSLTests
{
    [TestClass]
    public class DSLTests
    {
        [TestMethod]
        public void InitialStateIdentical()
        {
            var sm1 = CoffeeMakerProgram.SetupCoffeeMaker();
            var sm2 = CoffeeMakerProgram.SetupCoffeeMachineFluent();

            AssertThatStateMachinesAreEquivalent(sm1, sm2);
        }

        [TestMethod]
        public void InitialStateIdentical2()
        {
            var sm1 = CookingHoodProgram.SetupCookingHood();
            var sm2 = CookingHoodProgram.SetupCookingHoodFluent();

            AssertThatStateMachinesAreEquivalent(sm1, sm2);
        }


        [DataRow(new string[] {"ON", "WATER"})]
        [DataRow(new string[] {"ON", "PAID", "CUP", "WATER", "COFFEE"})]
        [DataTestMethod]
        public void TestCertainRowsOfEvents(string[] input)
        {
            var sm1 = CoffeeMakerProgram.SetupCoffeeMaker();
            var sm2 = CoffeeMakerProgram.SetupCoffeeMachineFluent();

            foreach(var ev in input)
            {
                sm1.ProcessEvent(new Event(ev));
                sm2.ProcessEvent(new Event(ev));
                AssertThatStateMachinesAreEquivalent(sm1, sm2);

            }
        }

        [TestMethod]
        public void RandomEventsStateMachinesEquivalent()
        {
            var sm1 = CoffeeMakerProgram.SetupCoffeeMaker();
            var sm2 = CoffeeMakerProgram.SetupCoffeeMachineFluent();

            var possibleEvents = new string[]
            {
                "ON",
                "OFF",
                "PAID",
                "WATER",
                "COFFEE",
                "COCOA",
                "CUP",
                "NOCUP"
            };

            var rand = new Random();

            var executedEvents = new List<string>();

            for (int i = 0; i < 100000; i++)
            {
                var ev = possibleEvents[rand.Next(0, possibleEvents.Length)];
                sm1.ProcessEvent(new Event(ev));
                sm2.ProcessEvent(new Event(ev));
                executedEvents.Add(ev);
                AssertThatStateMachinesAreEquivalent(sm1, sm2);

            }


        }


        [TestMethod]
        public void RandomEventsStateMachinesEquivalent2()
        {
            var sm1 = CookingHoodProgram.SetupCookingHood();
            var sm2 = CookingHoodProgram.SetupCookingHoodFluent();

            var possibleEvents = new string[]
            {
                "PLUS",
                "MINUS"
            };

            var rand = new Random();

            var executedEvents = new List<string>();

            for (int i = 0; i < 100000; i++)
            {
                var ev = possibleEvents[rand.Next(0, possibleEvents.Length)];
                sm1.ProcessEvent(new Event(ev));
                sm2.ProcessEvent(new Event(ev));
                executedEvents.Add(ev);
                AssertThatStateMachinesAreEquivalent(sm1, sm2);

            }




        }

        private void AssertThatStateMachinesAreEquivalent(StateMachine sm1, StateMachine sm2)
        {
            sm1.CurrentState.Name.Should().Be(sm2.CurrentState.Name);
            sm1.Variables.Should().BeEquivalentTo(sm2.Variables);
        }
    }
}
