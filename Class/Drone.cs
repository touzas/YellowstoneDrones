using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace YellowstoneDrones
{   
    /// <summary>
    /// Dirección que puede realizar un Dron
    /// </summary>
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
    /// <summary>
    /// Movimiento que puede realizar un Dron
    /// </summary>
    public enum DronMovement
    {
        Undefined,
        Left,
        Right,
        Advance    
    }
    /// <summary>
    /// Clase básica para no trabajar con array's y sí con objectos
    /// </summary>
    public class CoordinatesCommand
    {
        public int X;
        public int Y;
    }
    /// <summary>
    /// Clase básica para no trabajar con array's y sí con objectos
    /// Extiende a CoordinatesCommand ya que es igual pero además incluye la dirección
    /// </summary>
    public class CoordinatesWithDirectionCommand : CoordinatesCommand
    {
        public DronDirection Direction;
    }
    /// <summary>
    /// Clase para gestionar el Dron
    /// </summary>
    public class Drone
    {
        /// <summary>
        /// Difinición del area de vuelo
        /// </summary>
        private CoordinatesCommand m_FlyingArea;
        /// <summary>
        /// Lista de movimientos
        /// </summary>
        private List<Tuple<int,int>> m_Start;
        /// <summary>
        /// Dirección actual del Dron
        /// </summary>
        private DronDirection m_Direction;
        /// <summary>
        /// Creación del objecto Dron
        /// </summary>
        /// <param name="coord">CoordinatesCommand: Coordenadas válidas que define el tablero</param>
        public Drone(CoordinatesCommand coord)
        {
            Helper.ConsoleDebug("Create a Dron Area {0}x{1}", coord.X, coord.Y);
            m_FlyingArea = coord;
        }
        /// <summary>
        /// Inicialización del Dron
        /// </summary>
        /// <param name="data">String: Comprueba que el parámetro es un commando válido y lo procesa</param>
        public void Start(string data)
        {
            var startPos = Helper.ParseStartPositionCommand(data);
            StartAt(startPos.X, startPos.Y, startPos.Direction);
        }
        /// <summary>
        /// Inicialización del Dron
        /// Sitúa el Dron en unas coordenadas y dirección dentro del tablero
        /// </summary>
        /// <param name="x">Int: Coordenada X</param>
        /// <param name="y">Int: Coordenada Y</param>
        /// <param name="dir">DronDirection: Dirección válidas que definen el punto de inicio del dron dentro del tablero</param>
        private void StartAt(int x, int y, DronDirection dir)
        {
            Helper.ConsoleDebug("Start {0} {1} {2}", x, y, dir);
            if (m_Start == null)
                m_Start = new List<Tuple<int, int>>();
            m_Start.Add(Tuple.Create(x,y));
            m_Direction = dir;            
        }
        /// <summary>
        /// Movimiento del Dron
        /// </summary>
        /// <param name="data">String: Comprueba que el parámetro es un commando válido y lo procesa</param>
        public void Move(string data)
        {
            foreach(var ch in data)
            {
                var movement = Helper.GetMovement(ch);
                MoveTo(movement);
            }
        }
        /// <summary>
        /// Movimiento del Dron
        /// </summary>
        /// <param name="data">DronMovement: Tipo de movimiento hacia donde debe dirigirse el Dron</param>
        private void MoveTo(DronMovement move)
        {
            Helper.ConsoleDebug("Dron move to {0}", move);
            var currentX = m_Start.Last().Item1;
            var currentY = m_Start.Last().Item2;
            switch(move)
            {
                case DronMovement.Advance:
                    if (m_Direction == DronDirection.North)
                    {
                        if (currentY + 1 > m_FlyingArea.Y )
                            throw new Exception("Invalid Drone movement. It goes outside the defined flight area.");
                        m_Start.Add(Tuple.Create(currentX, currentY + 1));
                    }
                    else if (m_Direction == DronDirection.South)
                    {
                        if (currentY - 1 < 0 )
                            throw new Exception("Invalid Drone movement. It goes outside the defined flight area.");
                        m_Start.Add(Tuple.Create(currentX, currentY - 1));
                    }
                    else if (m_Direction == DronDirection.East)
                    {
                        if (currentX + 1 > m_FlyingArea.X )
                            throw new Exception("Invalid Drone movement. It goes outside the defined flight area.");
                        m_Start.Add(Tuple.Create(currentX + 1, currentY));
                    }
                    else if (m_Direction == DronDirection.West)
                    {
                        if (currentX - 1 < 0 )
                            throw new Exception("Invalid Drone movement. It goes outside the defined flight area.");
                        m_Start.Add(Tuple.Create(currentX - 1, currentY));
                    }
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
        /// <summary>
        /// Muestra la posición actual del último Dron creado
        /// </summary>
        public string ShowPossition()
        {
            var currentX = m_Start.Last().Item1;
            var currentY = m_Start.Last().Item2;
            Helper.ConsoleDebug("Drone at position {0}x{1} Direction {2}", currentX, currentY, m_Direction);
            return string.Format("{0} {1} {2}", currentX, currentY, Helper.GetDescriptionFromEnumValue(m_Direction));            
        }
    }    
}