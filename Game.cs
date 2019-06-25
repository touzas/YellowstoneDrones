/*
using System;
using System.Collections.Generic;

namespace YellowstoneDrones
{
    class GameProgram
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
            Console.WriteLine("Command Options:");
            Console.WriteLine("tu: Unitary test");
            Console.WriteLine("Number<space>Number: Defines board");
            Console.WriteLine("Number<space>Number<space>Direction: Initial possition of drone");
            Console.WriteLine("M/L/R: Dron movements:");
            Console.WriteLine("      M: Move");
            Console.WriteLine("      L: Turn Left");
            Console.WriteLine("      R: Turn Right");
            Console.WriteLine("q: Quit");
            Console.WriteLine("Please, enter a command:");
            var option = Helper.StringFromConsole();
            var command = Helper.GetCommandType(option);
            if (command == Command.Quit)
                return;
            if (command == Command.UnitaryTest)
            {
                var lines = UnitaryTest();
                foreach(var line in lines)
                {

                }
            }

            var dron = ReadFlyingArea();
            if (dron == null)
                return;
            
            while(DronMove(dron))
            {
                Console.WriteLine("Please, insert a start position or a movement");
            }
            Console.WriteLine("Dron stop!");
        }
        static Dron ReadFlyingArea()
        {
            Console.WriteLine("Please insert a flying area:");
            var data = Helper.StringFromConsole();
            var flyingArea = Helper.GetCoordinates(data);
            if (flyingArea == null)
                return null;
            return new Dron(flyingArea.X, flyingArea.Y);
        }
        static bool DronMove(Dron data)
        {            
            var move = Helper.StringFromConsole();
            var command = Helper.GetCommandType(move);
            switch(command)
            {
                case Command.StartPosition:
                    var startPos = Helper.ParseStartPositionCommand(move);
                    data.Start(startPos.X, startPos.Y, startPos.Direction);
                    break;
                case Command.Movement:
                    foreach(var ch in move)
                    {
                        var movement = Helper.GetMovement(ch);
                        data.Move(movement);
                    }
                    break;
                case Command.Quit:
                    return false;
            }
            return true;
        }
    }
}
*/