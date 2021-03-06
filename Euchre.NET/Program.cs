﻿using System;
using System.Collections.Generic; // List
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Euchre.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintGreeting();
            SelectMode();
        }

        private static void PrintGreeting()
        {
            var horiz_line = new String('#', Console.WindowWidth);
            var greeting = "Welcome to Euchre.NET";
            Console.WriteLine('\n' + horiz_line);
            Console.WriteLine(greeting);
            Console.WriteLine(horiz_line + '\n');
            Console.WriteLine(typeof(string).Assembly.ImageRuntimeVersion);
        }

        private static void SelectMode()
        {
            Console.WriteLine("1. Generate Scenarios");
            Console.Write("What would you like to do:  ");
            if (int.TryParse(Console.ReadLine(), out int x))
            {
                switch (x)
                {
                    case 1:
                        EnterAHand();
                        break;
                    case 2:
                        EnterDefaultHand();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.Write("Invalid Selection");
            }
        }

        private static void GenerateDeals()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("SerializedScenarios.txt", FileMode.Create, FileAccess.Write, FileShare.None);
            for (int i = 0; i < 50000; i++)
            {
                var s = new Scenario();
                formatter.Serialize(stream, s);
            }

            stream.Close();
        }

        private static void ParseDeals()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("SerializedScenarios.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            List<Scenario> s = (List<Scenario>)formatter.Deserialize(stream);
            stream.Close();
        }

        private static void EnterAHand()
        {
            Console.Write("Enter your seat number (1, 2, 3, or 0): ");
            if (!int.TryParse(Console.ReadLine(), out int seat))
                Console.Write("Invalid Seat");

            var hand = new List<Card>();
            Console.Write("Enter your hand (ten characters): ");
            var handString = Console.ReadLine();
            if (handString.Length != 10)
                Console.Write("Invalid Hand");

            for (int i = 0; i < 5; i++)
                hand.Add(new Card(handString[2 * i], handString[2 * i + 1]));

            Console.Write("Enter the upcard (two characters): ");
            var upcardString = Console.ReadLine();
            if (upcardString.Length != 2)
                Console.Write("Invalid Upcard");

            var upcard = new Card(upcardString[0], upcardString[1]);

            Console.Write("Enter the caller (1, 2, 3, or 0): ");
            var callerString = Console.ReadLine();
            if (!int.TryParse(callerString, out int caller))
                Console.Write("Invalid Caller");

            Console.Write("Enter the trump suit (H, D, S, or C): ");
            var trumpString = Console.ReadLine();
            if (!char.TryParse(trumpString, out char trump))
                Console.Write("Invalid Trump");

            var bank = new ScenarioBank(seat, hand, upcard, caller, trump);
            foreach (Scenario scen in bank._relevantScenarios)
                Console.WriteLine(scen.Serialize());

            var round = new List<Card>();
            Console.Write("Enter the first round of cards (eight characters): ");
            var roundString = Console.ReadLine();

            for (int i = 0; i < 4; i++)
                round.Add(new Card(roundString[2 * i], roundString[2 * i + 1]));

            bank.RevealRound(1, round);
            foreach (Scenario scen in bank._relevantScenarios)
                Console.WriteLine(scen.Serialize());

            PrintGreeting();
            SelectMode();
        }

        private static void EnterDefaultHand()
        {
            int seat = 1;
            var hand = new List<Card>()
            {
                new Card('A','S'),
                new Card('K','S'),
                new Card('Q','S'),
                new Card('J','S'),
                new Card('T','S'),
            };
            var upcard = new Card('K','D');
            int caller = 0;
            char trump = 'D';

            var bank = new ScenarioBank(seat, hand, upcard, caller, trump);
            foreach (Scenario scen in bank._relevantScenarios)
                Console.WriteLine(scen.Serialize());

            var round = new List<Card>();
            Console.Write("Enter the first round of cards (eight characters): ");
            var roundString = Console.ReadLine();

            for (int i = 0; i < 4; i++)
                round.Add(new Card(roundString[2 * i], roundString[2 * i + 1]));

            bank.RevealRound(1, round);
            foreach (Scenario scen in bank._relevantScenarios)
                Console.WriteLine(scen.Serialize());

            PrintGreeting();
            SelectMode();
        }
    }

    public struct Mode
    {
        public int ID;
        public string Name;

        public Mode(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
