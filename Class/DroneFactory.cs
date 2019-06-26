using System;
using System.Linq;
using System.Collections.Generic;

namespace YellowstoneDrones
{
    public class DroneFactory
    {
        /// <summary>
        /// Objecto Dron
        /// </summary>
        private Drone m_Drone;
        /// <summary>
        /// Boolean que nos indica si es el primer movimiento y cuando entra otro dron en acción
        /// </summary>
        private bool m_initialMovement;
        /// <summary>
        /// Histórico de Movimientos
        /// </summary>
        private List<string> m_DroneMovements;
        /// <summary>
        /// Boolean que nos si debemos lanzar un error cuando se sale del área de vuelo
        /// </summary>
        private bool m_ErrorOutside;
        /// <summary>
        /// Boolean que nos indica si estamos en modo juego
        /// </summary>
        private bool m_Game;

        /// <summary>
        /// Creación de la factoria de Drones
        /// </summary>
        /// <param name="isGaming">Bool: Modo Juego</param>
        public DroneFactory(bool errorOutside = true, bool isGaming = false)
        {
            m_ErrorOutside = errorOutside;
            m_initialMovement = true;
            m_DroneMovements = new List<string>();
            m_Game = isGaming;
        }
        /// <summary>
        /// Procesamiento de cada línea
        /// </summary>
        /// <param name="data">Línea de comandos</param>
        public void ProcessLine(string data)
        {
            var command = new StringCommand();            
            command = Helper.GetCommandType(data);
            if (command.cmd == Command.DronArea)
                CreateDrone(command.args);
            
            if (m_Drone != null)
            {
                if (command.cmd == Command.StartPosition)
                {
                    if (!m_initialMovement)
                        m_DroneMovements.Add(m_Drone.ShowPossition());
                    m_Drone.Start(command.args);
                    if (m_Game)
                        m_DroneMovements.Add(m_Drone.ShowPossition());
                }
                if (command.cmd == Command.Movement)
                {
                    try
                    {
                        m_Drone.Move(command.args);
                    }
                    catch(Exception ex)
                    {
                        if (m_ErrorOutside)
                        {
                            Console.WriteLine(ex.Message);
                            throw;
                        }
                    }                    
                    m_initialMovement = false;
                    if (m_Game)
                        m_DroneMovements.Add(m_Drone.ShowPossition());
                }
                if (command.cmd == Command.Quit)
                    throw new Exception("Game end");
            }
        }
        /// <summary>
        /// Devuelve el histórico de movimientos
        /// </summary>
        public List<string> GetMovements()
        {
            m_DroneMovements.Add(m_Drone.ShowPossition());
            return m_DroneMovements;
        }
        /// <summary>
        /// Devuelve el último movimiento del dron
        /// </summary>
        public string GetMovement()
        {
            if (m_DroneMovements != null && m_DroneMovements.Any())
            {
                return m_DroneMovements.Last();
            }
            return "";
        }
        /// <summary>
        /// Crea el Dron en el caso de que se hayan indicado unas coordenadas de forma correcta.
        /// </summary>
        /// <param name="lines">Commando que contiene unas coordenadas</param>
        private void CreateDrone(string data)
        {
            var flyingArea = Helper.GetCoordinates(data);
            if (flyingArea == null)
            {
                AddMovementToHistory(string.Format("Create a Drone Area Invalid Command: {0}", data));
                return;
            }
            m_Drone = new Drone(flyingArea);
            AddMovementToHistory(string.Format("Create a Drone Area {0}x{1}", flyingArea.X, flyingArea.Y));
        }
        /// <summary>
        /// En caso de estar en modo juego, añadimo todos los movimientos a la historia
        /// </summary>
        private void AddMovementToHistory(string data)
        {
            if (m_Game)
                m_DroneMovements.Add(data);
        }
    }
}