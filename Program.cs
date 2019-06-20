using System;

namespace Dron
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Yellowstone’s park control forest");
            Console.WriteLine("Enter 'q' to Quit");
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
            return new Dron(flyingArea.Item1, flyingArea.Item2);
        }
        static bool DronMove(Dron data)
        {            
            var move = Helper.StringFromConsole();
            var command = Helper.GetCommandType(move);
            switch(command)
            {
                case Command.StartPosition:
                    var startPos = Helper.ParseStartPositionCommand(move);
                    data.Start(startPos.Item1, startPos.Item2, startPos.Item3);
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
