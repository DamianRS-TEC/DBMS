
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DBMS
{
    class Program
    {
        static void Main()
        {
            string ver = "v0.1.4";
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
                    HE = new Regex(@"help;"),
                    EX = new Regex(@"exit;"),
                    CL = new Regex(@"clear;"),
                    CB = new Regex(@"\b(crea base)\s\S+;"),
                    BB = new Regex(@"\b(borrar base)\s\S+;"),
                    MB = new Regex(@"\b(muestra bases);"),
                    UB = new Regex(@"\b(usa base)\s\S+;"),
                    CT = new Regex(@"\b(crea tabla)\s\S+"),
                    MT = new Regex(@"\b(muestra tablas);"),
                    IE = new Regex(@"\b(inserta en)\s\S+"),
                    BT = new Regex(@"\b(borra tabla)\s\S+;"),
                    BC = new Regex(@"\b(borra campo)\s\S+\s\S+;");

            Regex   //Tipos de datos
                    INT = new Regex(@"\b(\S+,)\s(entero,)\s\d(\d)*(\z|;)"),
                    CAR = new Regex(@"\b(\S+,)\s(caracter,)\s\d(\d)*(\z|;)"),
                    DEC = new Regex(@"\b(\S+,)\s(decimal,)\s(\d(\d)*,\s\d(\d)?)(\z|;)"),
                    FEC = new Regex(@"\b(\S+,)\s(fecha)(\z|;)");


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
                if (HE.IsMatch(UsrInput))      //Mostrar los comandos
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
                else if (MT.IsMatch(UsrInput)) //Muestra tablas
                    MuestraTabla(UsrInput);
                else if (BC.IsMatch(UsrInput)) //Borrar campo de una tabla
                    BorrarCampo(UsrInput);
                else if (IE.IsMatch(UsrInput)) //Insertar campo 
                    InsertarDato(UsrInput);
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
                    "crea tabla * - Inicia el proceso de creacion de una tabla en la base de datos en uso\n" +
                    "muestra tablas - Muestra las tablas de la base de datos seleccionada\n" +
                    "borra tabla * - Borra la tabla asignada de la base de datos en uso\n" +
                    "borra campo * ** - Borra el campo * de la tabla ** en la base de datos en uso\n" +
                    "inserta en * - Inicia el proceso de insertar campos en la tabla *\n");
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
                        Console.WriteLine(a);

                }
                else
                    Console.WriteLine("No existen bases de datos");
            }
            void UsarBase(string BD) //Funcion para asignar una base de datos a trabajar
            {
                string NomBD = Extraccion(BD, 2);
                if (System.IO.Directory.Exists(@"../BDs/" + NomBD))
                    BDUsing = NomBD;
                else
                    Console.WriteLine("No existe la base de datos");
            }
            void CrearTabla(string TB) //Funcion para crear una tabla, primero se tiene que seleccionar una base de datos
            {
                List<string> Campos = new List<string>();
                string Reciente, NombreDeTabla = Extraccion(TB, 2);
                string DAT = @"../BDs/" + BDUsing + "/" + NombreDeTabla + ".dat",
                       EST = @"../BDs/" + BDUsing + "/" + NombreDeTabla + ".est";

                bool Exit = false, GoodInput = false;
                if (BDUsing != null)
                    if (!File.Exists(@"../BDs/" + BDUsing + "/" + NombreDeTabla + ".est"))
                    {
                        do
                        {
                            Reciente = Console.ReadLine();
                            if (INT.IsMatch(Reciente) |
                                CAR.IsMatch(Reciente) |
                                DEC.IsMatch(Reciente) |
                                FEC.IsMatch(Reciente))
                            {

                                if (Reciente.Contains(";"))
                                {
                                    Exit = true;
                                    GoodInput = true;
                                    Reciente = Reciente.Replace(";", "");
                                }
                                Campos.Add(Reciente);
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
                            File.Create(DAT).Close();
                            File.Create(EST).Close();
                            File.AppendAllLines(EST, Campos);
                        }
                    }
                    else
                        Console.WriteLine("La tabla con ese nombre ya existe");
                else
                    Console.WriteLine("No esta usando una base de datos");
            }
            void MuestraTabla(string TB)
            {
                DirectoryInfo DI = new DirectoryInfo(@"../BDs/" + BDUsing + "/");
                if (DI.GetFiles("*.*").Any())
                {
                    FileInfo[] FI = DI.GetFiles("*.est");
                    foreach (FileInfo F in FI)
                    {
                        Console.WriteLine(F.Name);
                        string[] Campos = File.ReadAllLines(F.FullName);
                        foreach (string C in Campos)
                            Console.WriteLine("\t" + C);
                        Console.WriteLine();
                    }
                }
                else
                    Console.WriteLine("La base de datos " + BDUsing + " no tiene tablas");
            }
            void BorrarCampo(string UsrI) //Fucnion para borrar campo, requiere de una base de datos en uso, el campo y la tabla
            {
                string C = Extraccion(UsrI, 2), TB = Extraccion(UsrI, 3);
                if (BDUsing != null)
                {
                    if (File.Exists(@"../BDs/" + BDUsing + "/" + TB + ".est"))
                    {
                        string[] Arr = File.ReadAllLines(@"../BDs/" + BDUsing + "/" + TB + ".est").ToArray();
                        int Pos = 0, PosDat = 0, AllDat = 0;
                        bool Found = false;
                        Regex TestString = new Regex(@C);
                        foreach (string S in Arr)
                        {
                            if (TestString.IsMatch(S))
                                Found = true;
                            else
                                Pos++;

                        }
                        if (Found)
                        {
                            AllDat = ContarDato(Arr);
                            PosDat = ContarCampo(Arr[Pos]);
                            //AQUI ME QUEDE

                            Console.WriteLine("Campo eliminado");
                        }
                        else
                            Console.WriteLine("Campo no encontrado en tabla " + TB);
                    }
                }
                else
                    Console.WriteLine("No esta usando una base de datos");
            }
            int ContarDato(string[] ArrayOfData)
            {
                int Count = 0;
                foreach (string C in ArrayOfData)
                {
                    if (C.Contains("caracter") | C.Contains("entero"))
                        Count += int.Parse(Extraccion(C, 2));
                    else if (C.Contains("decimal"))
                        Count += int.Parse(Extraccion(C, 2).Replace(",", "")) + int.Parse(Extraccion(C, 3));
                    else
                        Count += 8;
                }
                return Count;
            }
            int ContarCampo(string Data)
            {
                if (Data.Contains("caracter") | Data.Contains("entero"))
                    return int.Parse(Extraccion(Data, 2));
                else if (Data.Contains("decimal"))
                    return int.Parse(Extraccion(Data, 2).Replace(",", "")) + int.Parse(Extraccion(Data, 3));
                else
                    return 8;
            }
            void InsertarDato(string TB)
            {
                string[] Campos, CampoReferencia, CampoInsercion;
                int[] NumReferencia;
                string Tabla = Extraccion(TB, 2), In;
                DirectoryInfo DI = new DirectoryInfo(@"../BDs/" + BDUsing + "/");
                if (DI.GetFiles(Tabla+ ".est").Any())
                {
                    bool cont = true, Correct = false;
                    int i = 0;
                    List<string> CInput = new List<string>();
                    FileInfo[] FI = DI.GetFiles(Tabla + ".est");
                    Campos = File.ReadAllLines(FI[0].FullName);
                    CampoReferencia = new string[Campos.Length];
                    NumReferencia = new int[Campos.Length];
                    CampoInsercion = new string[Campos.Length];

                    foreach (FileInfo F in FI)
                    {
                        Console.WriteLine(F.Name);
                        foreach (string C in Campos)
                        {
                            Console.WriteLine("\t" + C);
                            NumReferencia[i] = ContarCampo(C);
                            CampoReferencia[i] = Extraccion(C, 0).Replace(",", "");
                            i++;
                        }
                        Console.WriteLine();

                    }
                    Console.WriteLine("Inserta los valores con el formato * = ** donde * es el campo y ** es el valor");
                    Regex Entrada = new Regex(@"\b(\S+\s=\s(\S|\d)+)(;)?");
                    do
                    {
                        In = Console.ReadLine();
                        Correct = false;
                        for (int i1 = 0; i1 < CampoReferencia.Length; i1++)
                        {
                            string item = CampoReferencia[i1];
                            if (Entrada.Match(In).Success && (Extraccion(In, 2).Length <= NumReferencia[i1]))
                            {
                                if (In.Contains(item))
                                {
                                    if (item.Contains("entero")|| item.Contains("fecha"))
                                    {
                                        if (int.TryParse(Extraccion(In, 2), out int result))
                                        {
                                            CampoInsercion[i1] = result.ToString();
                                            Correct = true;
                                        }
                                    }
                                    else if (item.Contains("decimal"))
                                    {
                                        if (int.TryParse((Extraccion(In, 2) + Extraccion(In, 3)), out int result))
                                        {
                                            CampoInsercion[i1] = result.ToString();
                                            Correct = true;
                                        }
                                    }
                                    else
                                    {
                                        CampoInsercion[i1] = Extraccion(In, 2);
                                        Correct = true;
                                    }
                                }
                            }
                        }
                        if (!Correct)
                            Console.WriteLine("Formato incorrecto");
                        if (In.Contains(";"))
                            cont = false;
                    } while (cont);


                    FileStream Dat = new FileStream(@"../BDs/" + BDUsing + "/" + Tabla + ".dat", FileMode.Append);
                    for(int i1 = 0; i1 < CampoReferencia.Length; i1++)
                    {
                        string input = CampoInsercion[i1];
                        if (input == null)
                            for (int spaces = NumReferencia[i1]; spaces > 0; spaces--)
                                input = input + " ";
                        else
                            for (int spaces = NumReferencia[i1]; spaces > CampoInsercion[i1].Length; spaces--)
                                input = input + " ";
                    }
                }
                else
                    Console.WriteLine("La base de datos " + BDUsing + " no tiene la tabla " + Tabla);
            }
        }
    }
}
