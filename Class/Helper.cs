using System;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace YellowstoneDrones
{
    /// <summary>
    /// Valores reconocidos para poder interpretar un comando
    /// </summary>
    public enum Command
    {
        DronArea,
        StartPosition,
        Movement,
        Undefined,
        Quit
    }
    /// <summary>
    /// Estructura para procesar de la misma forma todos los comandos
    /// </summary>
    public class StringCommand
    {
        public Command cmd;
        public string args;
        public StringCommand()
        {
            cmd = Command.Undefined;
            args = "";
        }
    }
    public static class Helper
    {
        const bool DEBUG = false;
        const string RegexBoard = "[0-9][ ][0-9]$";
        const string RegexInitialPossition = "[0-9][ ][0-9][ ][NSEW]$";
        const string RegexMovement = "[MRL]";

        /// <summary>
        /// Convierte una cadena en un StringCommand.
        /// Devuelve los comandos válidos que haya en la cadena de string
        /// </summary>
        /// <param name="data">Cadena string</param>
        public static StringCommand GetCommandType(string data)
        {
            var res = new StringCommand();

            var rb = new Regex(RegexBoard);
            var rip = new Regex(RegexInitialPossition);
            var rm = new Regex(RegexMovement);

            MatchCollection matches = null;

            if (rb.IsMatch(data))
            {                
                matches = rb.Matches(data);
                res.cmd = Command.DronArea;
            }                
            else if (rip.IsMatch(data))
            {
                matches = rip.Matches(data);
                res.cmd = Command.StartPosition;
            }
            else if (rm.IsMatch(data))
            {
                matches = rm.Matches(data);
                res.cmd = Command.Movement;
            }
            else if (data == "Q")
            {
                res.cmd = Command.Quit;
            }
            if (matches != null)
            {
                foreach(Match m in matches)
                    res.args += m.Value.ToLowerInvariant();
            }

            return res;
        }
        /// <summary>
        /// Convierte una cadena en un DronDirection.
        /// Devuelve Null en caso de cadena inválida
        /// </summary>
        /// <param name="data">Cadena string</param>
        public static DronDirection GetDirection(string data)
        {
            switch(data)
            {
                case "n":
                    return DronDirection.North;
                case "s":
                    return DronDirection.South;
                case "e":
                    return DronDirection.East;
                case "w":
                    return DronDirection.West;
            }
            return DronDirection.Undefined;
        }
        /// <summary>
        /// Convierte una cadena en un DronMovement.
        /// Devuelve Null en caso de cadena inválida
        /// </summary>
        /// <param name="data">Cadena char</param>
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
        /// <summary>
        /// Convierte una cadena en un CoordinatesWithDirectionCommand.
        /// Devuelve Null en caso de cadena inválida
        /// </summary>
        /// <param name="data">Cadena string</param>
        public static CoordinatesWithDirectionCommand ParseStartPositionCommand(string data)
        {
            string[] area = data.Split(' ');
            if (area.Length != 3)
                return null;
            var coordinates = GetCoordinates(data);
            var movement = GetDirection(area[2]);
            return new CoordinatesWithDirectionCommand(){ X = coordinates.X, Y = coordinates.Y, Direction = movement};
        }
        /// <summary>
        /// Convierte una cadena en un CoordinatesCommand.
        /// Devuelve Null en caso de cadena inválida
        /// </summary>
        /// <param name="data">Cadena string</param>
        public static CoordinatesCommand GetCoordinates(string data)
        {
            int x = -1;
            int y = -1;
            string[] area = data.Split(' ');
            if (area.Length < 2)
            {
                ConsoleDebug("Flying area must be contains a X and Y positions");
                return null;
            }
            int.TryParse(area[0], out x);
            int.TryParse(area[1], out y);
            if (x <= 0 || y <= 0)
            {
                ConsoleDebug("Flying are is too much small");
                return null;
            }
            return new CoordinatesCommand(){ X = x, Y = y };
        }
        /// <summary>
        /// En el caso de que un enumerado tenga una descripción la devuelve, en caso contrario devuelve el enumerado en texto.
        /// </summary>
        /// <param name="value">Valor de enumerado</param>
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        /// <summary>
        /// En el caso de esté activo el modo DEBUG muestra por consola información.
        /// </summary>
        /// <param name="data">Valor de texto a mostrar por consola</param>
        public static void ConsoleDebug(string data, params object[] arg)
        {
            if (DEBUG)                
                Console.WriteLine(data, arg);
        }
        public static string StringFromConsole()
        {
            var cmd = Console.ReadLine();
            return cmd.Trim().ToUpperInvariant();
        }
    }
}