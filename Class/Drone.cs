using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace YellowstoneDrones
{            
    public enum DronDirection
    {
        Undefined,
        [Description("N")]
        North,
        [Description("S")]
        South, 
        [Description("E")]
        East,
        [Description("W")]
        West
    }
    public enum DronMovement
    {
        Undefined,
        Left,
        Right,
        Advance    
    }
    public class CoordinatesCommand
    {
        public int X;
        public int Y;
    }
    public class CoordinatesWithDirectionCommand : CoordinatesCommand
    {
        public DronDirection Direction;
    }
    public class Drone
    {
        private CoordinatesCommand m_FlyingArea;
        private List<Tuple<int,int>> m_Start;
        private DronDirection m_Direction;
        public Drone(CoordinatesCommand coord)
        {
            Helper.ConsoleDebug("Create a Dron Area {0}x{1}", coord.X, coord.Y);
            m_FlyingArea = coord;
        }
        public void Start(string data)
        {
            var startPos = Helper.ParseStartPositionCommand(data);
            StartAt(startPos.X, startPos.Y, startPos.Direction);
        }
        private void StartAt(int x, int y, DronDirection dir)
        {
            Helper.ConsoleDebug("Start {0} {1} {2}", x, y, dir);
            if (m_Start == null)
                m_Start = new List<Tuple<int, int>>();
            m_Start.Add(Tuple.Create(x,y));
            m_Direction = dir;            
        }
        public void Move(string data)
        {
            foreach(var ch in data)
            {
                var movement = Helper.GetMovement(ch);
                MoveTo(movement);
            }
        }
        private void MoveTo(DronMovement move)
        {
            Helper.ConsoleDebug("Dron move to {0}", move);
            var currentX = m_Start.Last().Item1;
            var currentY = m_Start.Last().Item2;
            switch(move)
            {
                case DronMovement.Advance:
                    if (m_Direction == DronDirection.North)
                        m_Start.Add(Tuple.Create(currentX, currentY + 1));
                    else if (m_Direction == DronDirection.South)
                        m_Start.Add(Tuple.Create(currentX, currentY - 1));
                    else if (m_Direction == DronDirection.East)
                        m_Start.Add(Tuple.Create(currentX + 1, currentY));
                    else if (m_Direction == DronDirection.West)
                        m_Start.Add(Tuple.Create(currentX - 1, currentY));
                    break;
                case DronMovement.Left:
                    if (m_Direction == DronDirection.North)
                        m_Direction = DronDirection.West;
                    else if (m_Direction == DronDirection.South)
                        m_Direction = DronDirection.East;
                    else if (m_Direction == DronDirection.East)
                        m_Direction = DronDirection.North;
                    else if (m_Direction == DronDirection.West)
                        m_Direction = DronDirection.South;
                    break;
                case DronMovement.Right:
                    if (m_Direction == DronDirection.North)
                        m_Direction = DronDirection.East;
                    else if (m_Direction == DronDirection.South)
                        m_Direction = DronDirection.West;
                    else if (m_Direction == DronDirection.East)
                        m_Direction = DronDirection.South;
                    else if (m_Direction == DronDirection.West)
                        m_Direction = DronDirection.North;
                    break;
            }
            var afterMovementX = m_Start.Last().Item1;
            var afterMovementY = m_Start.Last().Item2;
            Helper.ConsoleDebug("{0}x{1} to direcction {2}", afterMovementX, afterMovementY, m_Direction);
        }
        public string ShowPossition()
        {
            var currentX = m_Start.Last().Item1;
            var currentY = m_Start.Last().Item2;
            Helper.ConsoleDebug("Drone at position {0}x{1} Direction {2}", currentX, currentY, m_Direction);
            return string.Format("{0} {1} {2}", currentX, currentY, Helper.GetDescriptionFromEnumValue(m_Direction));            
        }
    }    
}