using System;
using System.Collections.Generic; // List

namespace Euchre.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintGreeting();
            var modes = DefineModes();
            var mode = SelectMode(modes);

            Main(null);
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
