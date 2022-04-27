using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMS
{
    class Program
    {
        static void Main(string[] args)
        {
            string ver = "v0.1.1";
            /* Keywords:
             * Crea base %name
             * Borrar base %name
             * Muestra bases
             * Usa base %name
             * Crear tabla %name
             * %name %type %size
             * 
             * Borrar tabla %name
             * Borrar campo %name %name
             *  
             *  Notes:
             *  tbl1.est | structure file
             *  tbl1.dat | data file
             */
            Regex CB = new Regex(@"\b(crea base)\s\S+;"),
                    BB = new Regex(@"\b(borrar base)\s\S+;"),
                    MB = new Regex(@"\b(muestra base)\s\S+;"),
                    UB = new Regex(@"\b(usa base)\s\S+;"),
                    CT = new Regex(@"\b(crear tabla)\s\S+;"),
                    BT = new Regex(@"\b(borrar tabla)\s\S+;"),
                    BC = new Regex(@"\b(borrar campo)\s\S+;");

            string UsrInput;

            Info();
            Lectura();

            void Info()
            {
                Console.WriteLine("DBMS " + ver);
                Console.WriteLine("Proyecto final de administracion de bases de datos");
                Console.WriteLine("Creado por Robledo Sanchez Damian 19211719");
                Console.Write("> ");
            }
            string Extraccion(string ex, int loc)
            {
                string[] exA = ex.Split(' ');
                return exA[loc].Trim(';');
            }
            void Lectura()
            {
                UsrInput = Console.ReadLine();
                if (CB.IsMatch(UsrInput))
                {
                    CreaBase(UsrInput);
                    Console.ReadKey();
                }
            }
            void CreaBase(string BD)
            {
                string NomBD = Extraccion(BD, 2);
                
            }
        }
    }
}
