using System;
using System.Collections.Generic; // List
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Euchre.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintGreeting();
            var modes = DefineModes();
            SelectMode(modes);
        }

        private static void PrintGreeting()
        {
            var horiz_line = new String('#', Console.WindowWidth);
            var greeting = "Welcome to Euchre.NET";
            Console.WriteLine('\n'+horiz_line);
            Console.WriteLine(greeting);
            Console.WriteLine(horiz_line+'\n');
            Console.Write(typeof(string).Assembly.ImageRuntimeVersion);
        }

        private static void SelectMode(List<Mode> modes)
        {
            PrintOptions();
            if (int.TryParse(Console.ReadLine(), out int x))
            {
                switch (x)
                {
                    case 1:
                        GenerateDeals();
                        break;
                    case 2:
                        ParseDeals();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.Write("Invalid Selection");
                Main(null);
            }
        }

        private static void PrintOptions()
        {
            Console.WriteLine("Deal a Hand");
            Console.WriteLine("Analyze a Hand");
            Console.Write("What would you like to do:  ");
        }

        private static List<Mode> DefineModes()
        {
            var modes = new List<Mode>();
            modes.Add(new Mode(1, "Deal a Hand"));
            modes.Add(new Mode(2, "Analyze a Hand"));
            return modes;
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
