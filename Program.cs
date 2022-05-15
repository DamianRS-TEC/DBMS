
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace DBMS
{
    class Program
    {
        static void Main()
        {
            string ver = "v0.1.3";
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
             * Tipos de datos
             * caracter
             * int
             * decimal
             * fecha
             *  
             *  Notes:
             *  tbl1.est | structure file
             *  tbl1.dat | data file
             */
            Regex   //Palabras clave de comandos
                    H = new Regex(@"help;"),
                    EX = new Regex(@"exit;"),
                    CL = new Regex(@"clear;"),
                    CB = new Regex(@"\b(crea base)\s\S+;"),
                    BB = new Regex(@"\b(borrar base)\s\S+;"),
                    MB = new Regex(@"\b(muestra bases);"),
                    UB = new Regex(@"\b(usa base)\s\S+;"),
                    CT = new Regex(@"\b(crea tabla)\s\S+"),
                    BT = new Regex(@"\b(borrar tabla)\s\S+;"),
                    BC = new Regex(@"\b(borrar campo)\s\S+;");

            Regex   //Tipos de datos
                    INT = new Regex(@"\b(\S+,)\s(entero,)\s\d(\d)?(\z|;)"),
                    CAR = new Regex(@"\b(\S+,)\s(caracter,)\s\d(\d)?(\z|;)"),
                    DEC = new Regex(@"\b(\S+,)\s(decimal,)\s\d(\d)?(\z|;)"),
                    FEC = new Regex(@"\b(\S+,)\s(fecha,)(\z|;)");


            string UsrInput, BDUsing = null;
            bool Cont = true;


            Info();
            do
            {
                Console.Write("\n" + BDUsing + "> ");
                Lectura(); //Aqui el programa espera al usuario a que introdusca algo
            } while (Cont);
            void Info() //Aqui se despliega el menu por primera vez
            {
                Console.WriteLine("DBMS " + ver);
                Console.WriteLine("Proyecto final de administracion de bases de datos");
                Console.WriteLine("Creado por Robledo Sanchez Damian 19211719");
            }
            void Lectura() //Deteccion de la entrada del usuario
            {
                UsrInput = Console.ReadLine();
                if (H.IsMatch(UsrInput))       //Mostrar los comandos
                    Help();
                else if (CL.IsMatch(UsrInput)) //Limpiar consola
                    Clear();
                else if (EX.IsMatch(UsrInput)) //Salir del programa
                    Cont = false;
                else if (CB.IsMatch(UsrInput)) //Crear base
                    CreaBase(UsrInput);
                else if (BB.IsMatch(UsrInput)) //Borrar base
                    BorrarBase(UsrInput);
                else if (MB.IsMatch(UsrInput)) //Mostrar bases
                    MuestraBase(UsrInput);
                else if (UB.IsMatch(UsrInput)) //Usa base
                    UsarBase(UsrInput);
                else if (CT.IsMatch(UsrInput)) //Crear tabla
                    CrearTabla(UsrInput);
            }
            string Extraccion(string str, int loc) //Funcion para extraer una palabra de una cadena
            {
                string[] Ext = str.Split(' ');
                return Ext[loc].Trim(';');
            }
            void Help() //Despliegue de comandos para usarse
            {
                Console.Write(
                    "help - Muestra este texto\n" +
                    "exit - Salir del programa\n" +
                    "crear base * - Crea una base de datos con el nombre asignado\n" +
                    "borrar base * - Borra la base de datos con el nombre asignado, si existe\n" +
                    "muestra bases - Lista todas las bases de datos existentes\n" +
                    "usa base * - Asigna la base de datos a trabajar\n" +
                    "crea tabla * - Inicia el proceso de creacion de una tabla en la base de datos en uso");
            }
            void Clear() //Limpia la consola
            {
                Console.Clear();
                Info();
            }
            void CreaBase(string BD) //Funcion para crear la carpeta que actua como base de datos
            {
                string NomBD = Extraccion(BD, 2);
                if (!System.IO.Directory.Exists(@"../BDs/" + NomBD))
                {
                    System.IO.Directory.CreateDirectory(@"../BDs/" + NomBD);
                    Console.Write("Base de datos " + NomBD + " creada.");
                }
                else
                    Console.Write("Error! Base de datos ya existe");
            }
            void BorrarBase(string BD) //Funcion para eliminar una carpeta base de datos
            {
                string NomBD = Extraccion(BD, 2);
                if (System.IO.Directory.Exists(@"../BDs/" + NomBD))
                {
                    System.IO.Directory.Delete(@"../BDs/" + NomBD, true);
                    Console.Write("Base de datos " + NomBD + " eliminada.");
                }
                else
                    Console.Write("Error! Base de datos no existe");
            }
            void MuestraBase(string BD) //Funcion para mostrar todas las bases de datos
            {
                if (System.IO.Directory.Exists(@"../BDs/"))
                {
                    string[] BasesDeDatos = System.IO.Directory.GetDirectories(@"../BDs/");
                    foreach (string a in BasesDeDatos)
                    {
                        Console.WriteLine(a);
                    }
                }
                else
                    Console.WriteLine("No existen bases de datos");
            }
            void UsarBase(string BD) //Funcion para asignar una base de datos a trabajar
            {
                string NomBD = Extraccion(BD, 2);
                if (System.IO.Directory.Exists(@"../BDs/" + NomBD))
                {
                    BDUsing = NomBD;
                }
                else
                    Console.WriteLine("No existe la base de datos");
            }
            void CrearTabla(string TB) //Funcion para crear una tabla, primero se tiene que seleccionar una base de datos
            {
                List<string> Campos = new List<string>();
                string Reciente;
                bool Exit = false, GoodInput = false;
                if (!System.IO.File.Exists(@"../BDs/" + BDUsing + "/" + Extraccion(TB,2) + ".est"))
                {
                    if (BDUsing != null)
                    {
                        do
                        {
                            Reciente = Console.ReadLine();
                            if (INT.IsMatch(Reciente) |
                                CAR.IsMatch(Reciente) |
                                DEC.IsMatch(Reciente) |
                                FEC.IsMatch(Reciente))
                            {
                                Campos.Add(Reciente);
                                if (Reciente.Contains(";"))
                                {
                                    Exit = true;
                                    GoodInput = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error en la entrada de campos");
                                Exit = true;
                            }
                        }
                        while (!Exit);
                        if (GoodInput)
                        {
                            System.IO.File.Create(@"../BDs/" + BDUsing + "/" + TB + ".dat");
                            System.IO.File.Create(@"../BDs/" + BDUsing + "/" + TB + ".est");
                            File.AppendAllLines(@"../BDs/" + BDUsing + "/" + TB + ".dat", Campos);
 
                        }
                    }
                    else
                        Console.WriteLine("No esta usando una base de datos");
                }
                else
                    Console.WriteLine("La tabla con ese nombre ya existe");
            }
        }
    }
}
