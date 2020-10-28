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
            System.Console.WriteLine('\n'+horiz_line);
            System.Console.WriteLine(greeting);
            System.Console.WriteLine(horiz_line+'\n');
        }

        private static void SelectMode(List<Mode> modes)
        {
            PrintOptions();
            if (int.TryParse(Console.ReadLine(), out int x))
            {
                switch (x)
                {
                    case 1:
                        GenerateDeal();
                        break;
                    case 2:

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

        private static void GenerateDeal()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("SerializedScenarios.txt", FileMode.Create, FileAccess.Write, FileShare.None);
            for (int i = 0; i < 3; i++)
            {
                var d = new Deal();
                var s = new Scenario();
                formatter.Serialize(stream, s);
                formatter.Serialize(stream, d);
            }

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
