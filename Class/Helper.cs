using System;
namespace Dron
{
    public static class Helper
    {
        public static DronDirection GetDirection(char data)
        {
            switch(data)
            {
                case 'n':
                    return DronDirection.Nortn;
                case 's':
                    return DronDirection.South;
                case 'e':
                    return DronDirection.East;
                case 'w':
                    return DronDirection.West;
            }
            return DronDirection.Undefined;
        }
        public static DronMovement GetMovement(char data)
        {
            switch(data)
            {
                case 'l':
                    return DronMovement.Left;
                case 'r':
                    return DronMovement.Right;
                case 'm':
                    return DronMovement.Advance;
            }
            return DronMovement.Undefined;
        }
        public static Command GetCommandType(string data)
        {
            if (string.IsNullOrEmpty(data))
                return Command.Undefined;
            if (data[0] == 'q')
                return Command.Quit;
            if (char.IsDigit(data[0]))
                return Command.StartPosition;
            else 
                return Command.Movement;
        }
        public static Tuple<int, int, DronDirection> ParseStartPositionCommand(string data)
        {
            string[] area = data.Split(' ');
            if (area.Length != 3)
                return null;
            var coordinates = GetCoordinates(data);
            var movement = GetDirection(area[2][0]);
            return Tuple.Create(coordinates.Item1, coordinates.Item2, movement);
        }
        public static Tuple<int, int> GetCoordinates(string data)
        {
            int x = -1;
            int y = -1;
            string[] area = data.Split(' ');
            if (area.Length < 2)
            {
                Console.WriteLine("Flying area must be contains a X and Y positions");
                return null;
            }
            int.TryParse(area[0], out x);
            int.TryParse(area[1], out y);
            if (x <= 0 || y <= 0)
            {
                Console.WriteLine("Flying are is too much small");
                return null;
            }
            return Tuple.Create(x, y);
        }
        public static string StringFromConsole()
        {
            var data = Console.ReadLine();
            return data.Trim().ToLowerInvariant();
        }
    }
}