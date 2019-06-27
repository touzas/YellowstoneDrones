using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YellowstoneDrones
{
    class Program
    {                
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("You must be indicate a file to load a commands");
                return;
            }
            var fi = new FileInfo(args[0]);
            if (!fi.Exists)
            {
                Console.WriteLine("You must be indicate a path of a file to load a commands");
                return;
            }
            var drone = new Drone();
            using(var str = new StreamReader(args[0]))
            {
                while (str.Peek() >= 0) 
                {
                    var line = str.ReadLine();
                    //Console.WriteLine(string.Format("Proccess line: {0}",line));
                    ProcessLine(line, drone);
                }
                drone.ShowPosition();
            }
        }
        static void ProcessLine(string data, Drone drone)
        {            
            var droneArea = new Regex("[0-9][ ][0-9]$");
            var droneStartAt = new Regex("[0-9][ ][0-9][ ][NSEW]$");
            var droneMovement = new Regex("[MRL]");
            
            if (droneArea.IsMatch(data))
            {                
                string[] area = data.Split(' ');
                var coord = ParseCoordinates(area[0], area[1]);
                if (coord.Item1 >= 0 && coord.Item2 >= 0)
                {
                    drone.SetArea(new DroneArea(coord.Item1, coord.Item2));
                }
            }                
            else if (droneStartAt.IsMatch(data))
            {
                drone.ShowPosition();
                string[] area = data.Split(' ');
                var coord = ParseCoordinates(area[0], area[1]);
                Direction dir = (Direction)Enum.Parse(typeof(Direction), area[2], true);
                drone.StartAt(coord.Item1, coord.Item2, dir);
            }
            else if (droneMovement.IsMatch(data))
            {
                foreach(char c in data)
                {
                    drone.Move(c);
                }
            }
        }
        static Tuple<int,int> ParseCoordinates(string xArea, string yArea)
        {
            int x = -1;
            int y = -1;
            int.TryParse(xArea, out x);
            int.TryParse(yArea, out y);
            return Tuple.Create(x,y);
        }
    }
    public class DroneArea
    {
        private int X;
        private int Y;
        public DroneArea(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public enum Direction
    {
        N,S,E,W
    }
    public enum Movement
    {
        M,L,R
    }
    public class Drone
    {
        private DroneArea m_area;
        private List<Tuple<int,int>> m_movements;
        private Direction m_direction;
        public void SetArea(DroneArea area)
        {
            m_area = area;
        }
        public void StartAt(int x, int y, Direction d)
        {
            if (m_movements == null)
                m_movements = new List<Tuple<int, int>>();
            m_movements.Add(Tuple.Create(x, y));
            m_direction = d;
        }
        public void Move(char c)
        {
            Movement move = (Movement)Enum.Parse(typeof(Movement), c.ToString(), true);
            var currentX = m_movements.Last().Item1;
            var currentY = m_movements.Last().Item2;
            switch(move)
            {
                case Movement.L:
                    if (m_direction == Direction.N)
                        m_direction = Direction.W;
                    else if (m_direction == Direction.S)
                        m_direction = Direction.E;
                    else if (m_direction == Direction.E)
                        m_direction = Direction.N;
                    else if (m_direction == Direction.W)
                        m_direction = Direction.S;                    
                break;
                case Movement.R:
                    if (m_direction == Direction.N)
                        m_direction = Direction.E;
                    else if (m_direction == Direction.S)
                        m_direction = Direction.W;
                    else if (m_direction == Direction.E)
                        m_direction = Direction.S;
                    else if (m_direction == Direction.W)
                        m_direction = Direction.N;
                break;
                case Movement.M:
                    if (m_direction == Direction.N)
                        m_movements.Add(Tuple.Create(currentX, currentY + 1));
                    else if (m_direction == Direction.S)
                        m_movements.Add(Tuple.Create(currentX, currentY - 1));
                    else if (m_direction == Direction.E)
                        m_movements.Add(Tuple.Create(currentX + 1, currentY));
                    else if (m_direction == Direction.W)
                        m_movements.Add(Tuple.Create(currentX - 1, currentY));
                break;
            }
        }
        public void ShowPosition()
        {
            if (m_movements == null || !m_movements.Any())
                return;
            var currentX = m_movements.Last().Item1;
            var currentY = m_movements.Last().Item2;
            Console.WriteLine(string.Format("{0} {1} {2}", currentX, currentY, m_direction));
        }
    }

}
