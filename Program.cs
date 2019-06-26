using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace YellowstoneDrones
{
    class Program
    {
        /// <summary>
        /// Devuelve una serie de líneas que definen la entrada de ordenes hacia los drons
        /// </summary>
        static List<string> UnitaryTest()
        {
            var lines = new List<string>();
            lines.Add("5 5");
            lines.Add("3 3 E");
            lines.Add("L");
            lines.Add("3 3 E");
            lines.Add("MMRMMRMRRM");
            lines.Add("1 2 N");
            lines.Add("LMLMLMLMMLMLMLMLMM");
            return lines;
        }
        /// <summary>
        /// Devuelve una serie de líneas que son el resultado esperado
        /// </summary>
        static List<string> UnitaryTestResult()
        {
            var lines = new List<string>();
            lines.Add("3 3 N");
            lines.Add("5 1 E");
            lines.Add("1 4 N");
            return lines;
        }
        /// <summary>
        /// Método principal de la aplicación
        /// </summary>
        /// <param name="args">Una Lista de argumentos que indican que se va a procesar</param>
        static void Main(string[] args)
        {
            List<string> lines = new List<string>();
            List<string> dronMovements = new List<string>();
            var df = new DroneFactory(true, false);
            bool Utest = false;
            Console.WriteLine("Yellowstone’s park control forest");
            Console.WriteLine("====================================");
            Console.WriteLine("Possible commands:");
            Console.WriteLine("t        => Unitay test");
            Console.WriteLine("f=<file> => Drone Movements are in file");
            Console.WriteLine("g        => Join the game");
            Console.WriteLine("q        => Quit the game");
            Console.WriteLine(string.Join(";", args));

            if (args != null && args.Length > 0 && args.First().ToLowerInvariant() == "t")
            {
                Console.WriteLine("Process a unitary test");
                foreach(var line in UnitaryTest())
                {
                    df.ProcessLine(line);
                }
                
                Utest = true;
            }
            else if (args != null && args.Length > 0 && args.First().ToLowerInvariant().StartsWith("f="))
            {
                var command = args.First().Split("=", StringSplitOptions.RemoveEmptyEntries);
                var fi = new FileInfo(command[1]);
                if (fi.Exists)
                {
                    using(var str = new StreamReader(command[1]))
                    {
                        while (str.Peek() >= 0) 
                        {
                            var line = str.ReadLine();
                            Console.WriteLine(string.Format("Proccess line: {0}",line));
                            df.ProcessLine(line);
                        }
                    }
                }
            }
            else if (args != null && args.Length > 0 && args.First().ToLowerInvariant() == "g")
            {
                df = new DroneFactory(false, true);
                do
                {
                    Console.WriteLine("Please, insert a start position or a movement");
                }
                while(GameRunning(df));
            }
            dronMovements = df.GetMovements();
            if (Utest)
            {
                Console.WriteLine(string.Join("\n", dronMovements));
                Console.WriteLine("UnitaryTest is {0}", dronMovements.SequenceEqual(UnitaryTestResult()));
            }
        }
        static bool GameRunning(DroneFactory df)
        {
            var move = Helper.StringFromConsole();
            try
            {
                df.ProcessLine(move);
                Console.WriteLine(df.GetMovement());
            }
            catch
            {
                return false;
            }            
            return true;
        }
    }
}
