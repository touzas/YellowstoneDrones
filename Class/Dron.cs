using System;
using System.Collections.Generic;
using System.Linq;

namespace Dron
{    
    public enum Command
    {
        Undefined,
        StartPosition,
        Movement,
        Quit
    }
    public enum DronDirection
    {
        Undefined,
        Nortn,
        South, 
        East,
        West
    }
    public enum DronMovement
    {
        Undefined,
        Left,
        Right,
        Advance    
    }
    public class Dron
    {
        private int[,] m_FlyingArea;
        private List<Tuple<int,int>> m_Start;
        private DronDirection m_Direction;
        public Dron(int x, int y)
        {
            Console.WriteLine("Create a Dron Area {0}x{1}", x, y);
            m_FlyingArea = new int[x,y];
        }
        public void Start(int x, int y, DronDirection dir)
        {
            Console.WriteLine("Start Dron position at {0}x{1} to direcction {2}", x, y, dir);
            if (m_Start == null)
                m_Start = new List<Tuple<int, int>>();
            m_Start.Add(Tuple.Create(x,y));
            m_Direction = dir;            
        }
        public void Move(DronMovement move)
        {
            Console.WriteLine("Dron move to {0}", move);
            var currentX = m_Start.Last().Item1;
            var currentY = m_Start.Last().Item2;
            switch(move)
            {
                case DronMovement.Advance:
                    if (m_Direction == DronDirection.Nortn)
                        m_Start.Add(Tuple.Create(currentX, currentY + 1));
                    else if (m_Direction == DronDirection.South)
                        m_Start.Add(Tuple.Create(currentX, currentY - 1));
                    else if (m_Direction == DronDirection.East)
                        m_Start.Add(Tuple.Create(currentX - 1, currentY));
                    else if (m_Direction == DronDirection.West)
                        m_Start.Add(Tuple.Create(currentX + 1, currentY));
                    break;
                case DronMovement.Left:
                    if (m_Direction == DronDirection.Nortn)
                        m_Direction = DronDirection.East;
                    else if (m_Direction == DronDirection.South)
                        m_Direction = DronDirection.West;
                    else if (m_Direction == DronDirection.East)
                        m_Direction = DronDirection.South;
                    else if (m_Direction == DronDirection.West)
                        m_Direction = DronDirection.Nortn;
                    break;
                case DronMovement.Right:
                    if (m_Direction == DronDirection.Nortn)
                        m_Direction = DronDirection.West;
                    else if (m_Direction == DronDirection.South)
                        m_Direction = DronDirection.East;
                    else if (m_Direction == DronDirection.East)
                        m_Direction = DronDirection.Nortn;
                    else if (m_Direction == DronDirection.West)
                        m_Direction = DronDirection.South;
                    break;
            }
            var afterMovementX = m_Start.Last().Item1;
            var afterMovementY = m_Start.Last().Item2;
            Console.WriteLine("Start Dron position at {0}x{1} to direcction {2}", afterMovementX, afterMovementY, m_Direction);
        }
    }
}