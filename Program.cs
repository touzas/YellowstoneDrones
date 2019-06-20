using System;
using System.Collections.Generic;

namespace YellowstoneDrones
{
    class Program
    {
        static string[] UnitaryTest()
        {
            var lines = new List<string>();
            lines.Add("5 5");
            lines.Add("3 3 E");
            lines.Add("L");
            lines.Add("3 3 E");
            lines.Add("MMRMMRMRRM");
            lines.Add("1 2 N");
            lines.Add("LMLMLMLMMLMLMLMLMM");
            return lines.ToArray();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Yellowstone’s park control forest");

            var lines = UnitaryTest();            
            var command = new StringCommand();
            Dron dron = null;
            foreach(var line in lines)
            {
                command = Helper.GetCommandType(line);
                if (command.cmd == Command.DronArea)
                    dron = CreateDron(command.args);
                
                if (dron == null)
                    continue;
                if (command.cmd == Command.StartPosition)
                    dron.Start(command.args);
                if (command.cmd == Command.Movement)
                    dron.Move(command.args);
            }
        }
        static Dron CreateDron(string data)
        {
            var flyingArea = Helper.GetCoordinates(data);
            if (flyingArea == null)
                return null;
            return new Dron(flyingArea);
        }
    }
}
