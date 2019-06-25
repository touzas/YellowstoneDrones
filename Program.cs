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
            bool Utest = false;
            Console.WriteLine("Yellowstone’s park control forest");
            Console.WriteLine("====================================");
            Console.WriteLine("Possible commands:");
            Console.WriteLine("t => Unitay test");
            Console.WriteLine("f=<file> => Drone Movements are in file");

            if (args != null && args.Length > 0 && args.First().ToLowerInvariant() == "t")
            {
                Console.WriteLine("Process a unitary test");
                lines = UnitaryTest();
                Utest = true;
            }
            else
            {                
                if (args != null && args.Length > 0 && args.First().ToLowerInvariant() == "f")
                {
                    var command = args.First().Split("=", StringSplitOptions.RemoveEmptyEntries);
                    var fi = new FileInfo(command[1]);
                    if (fi.Exists)
                    {
                        using(var str = new StreamReader(command[1]))
                        {
                            lines.AddRange(str.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList());
                        }
                    }
                }
            }
            var result = ProcessCommands(lines);
            if (Utest)
                Console.WriteLine("UnitaryTest is {0}", result.SequenceEqual(UnitaryTestResult()));

        }
        /// <summary>
        /// Procesa cada línea introducida en la lista de ordenes
        /// </summary>
        /// <param name="lines">Una Lista de comandos a procesar</param>
        static List<string> ProcessCommands(List<string> lines)
        {
            List<string> result = new List<string>();
            Drone drone = null;
            var command = new StringCommand();
            bool initialMovement = true;
            foreach(var line in lines)
            {
                command = Helper.GetCommandType(line);
                if (command.cmd == Command.DronArea)
                    drone = CreateDrone(command.args);
                
                if (drone == null)
                    continue;
                if (command.cmd == Command.StartPosition)
                {
                    if (!initialMovement)
                        result.Add(drone.ShowPossition());
                    drone.Start(command.args);
                }
                if (command.cmd == Command.Movement)
                {
                    drone.Move(command.args);
                    initialMovement = false;                    
                }
            }
            result.Add(drone.ShowPossition());
            return result;
        }
        /// <summary>
        /// Crea el Dron en el caso de que se hayan indicado unas coordenadas de forma correcta.
        /// </summary>
        /// <param name="lines">Commando que contiene unas coordenadas</param>
        static Drone CreateDrone(string data)
        {
            var flyingArea = Helper.GetCoordinates(data);
            if (flyingArea == null)
                return null;
            return new Drone(flyingArea);
        }
    }
}
