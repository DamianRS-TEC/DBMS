
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
            string ver = "v0.1.7";
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
                    BC = new Regex(@"\b(borra campo)\s\S+\s\S+;"),
                    IC = new Regex(@"\b(inserta campos)\s\S+"),
                    EC = new Regex(@"\b(elimina en)\s\S+\s(donde)\s\S+\s(=)\s\S+;"),
                    MC = new Regex(@"\b(modifica en)\s\S+\s(donde)\s\S+\s(=)\s\S+"),
                    LT = new Regex(@"\b(lista)((\s\S+)+ de\s\S+(\sdonde\s\S+\s=\s\S+)?;)");

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
                    MuestraBase();
                else if (UB.IsMatch(UsrInput)) //Usa base
                    UsarBase(UsrInput);
                else if (CT.IsMatch(UsrInput)) //Crear tabla
                    CrearTabla(UsrInput);
                else if (MT.IsMatch(UsrInput)) //Muestra tablas
                    MuestraTabla();
                else if (BC.IsMatch(UsrInput)) //Borrar campo de una tabla
                    BorrarCampo(UsrInput);
                else if (IE.IsMatch(UsrInput)) //Insertar campo 
                    InsertarDato(UsrInput);
                else if (IC.IsMatch(UsrInput)) //Inserta campos
                    InsertarCampo(UsrInput);
                else if (EC.IsMatch(UsrInput)) //Elimina campo donde
                    EliminarCampoDonde(UsrInput);
                else if (MC.IsMatch(UsrInput)) //Modifica datos donde el campo sea igual
                    ModificaCampoDonde(UsrInput);
                else if (BT.IsMatch(UsrInput)) //Elimina una tabla, su .dat y su .est
                    EliminarTabla(UsrInput);
                else if (LT.IsMatch(UsrInput))
                    Mostrar(UsrInput);
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
                    "inserta en * - Inicia el proceso de insertar datos en la tabla *\n" +
                    "inserta campos * - Inicia el proceso de insertar campos en la tabla\n" +
                    "elimina en * donde ** - Elimina las entradas en la tabla * donde ** se cumpla\n" +
                    "modifica en * donde ** - Modifica las entradas en la tabla * donde ** se cumpla\n");
            }
            string Extraccion(string str, int loc) //Funcion para extraer una palabra de una cadena
            {
                string[] Ext = str.Split(' ');
                return Ext[loc].Trim(';');
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
            void MuestraBase() //Funcion para mostrar todas las bases de datos
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
            void MuestraTabla()
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
                string C = Extraccion(UsrI, 2), TB = Extraccion(UsrI, 3),
                    PATHDAT = "../BDs/" + BDUsing + "/" + TB + ".dat",
                    PATHEST = "../BDs/" + BDUsing + "/" + TB + ".est";
                if (BDUsing != null)
                {
                    if (File.Exists(@PATHEST))
                    {
                        string Mem = "";
                        string[] Arr = File.ReadAllLines(@PATHEST).ToArray();
                        int Pos = 0, AllDat, TamDat, Salto = 0, Entradas = 0;
                        bool Found = false;
                        Regex TestString = new Regex(@C);
                        do
                        {
                            if (TestString.IsMatch(Arr[Pos]))
                                Found = true;
                            else
                                Pos++;
                        }
                        while (!Found);
                        if (Found)
                        {
                            AllDat = ContarDato(Arr);
                            TamDat = ContarCampo(Arr[Pos]);
                            for (int i = 0; i < Pos; i++)
                                Salto += ContarCampo(Arr[i]);


                            using (FileStream Dat = new FileStream(@PATHDAT, FileMode.Open))
                            {
                                using (StreamReader SR = new StreamReader(Dat))
                                {
                                    string L = SR.ReadToEnd();
                                    Entradas = L.Length / AllDat;
                                }
                            }

                            using (FileStream Dat = new FileStream(@PATHDAT, FileMode.Open))
                            using (StreamReader SR = new StreamReader(Dat))
                            {
                                for (int i = 0; i < Entradas; i++)
                                {
                                    char Buffer;
                                    if (Salto == 0)
                                    {
                                        for (int m = 0; m < TamDat; m++)
                                            SR.Read();
                                        for (int m = TamDat; m < AllDat; m++)
                                        {
                                            Buffer = (char)SR.Read();
                                            Mem += Buffer.ToString();
                                        }
                                    }
                                    else
                                    {
                                        for (int m = 0; m < Salto; m++)
                                        {
                                            Buffer = (char)SR.Read();
                                            Mem += Buffer.ToString();
                                        }
                                        for (int m = 0; m < TamDat; m++)
                                            SR.Read();
                                        for (int m = TamDat + Salto; m < AllDat; m++)
                                        {
                                            Buffer = (char)SR.Read();
                                            Mem += Buffer.ToString();
                                        }
                                    }
                                }
                            }

                            using (FileStream Dat = new FileStream(@PATHDAT, FileMode.Create))
                            using (StreamWriter SW = new StreamWriter(Dat))
                                SW.Write(Mem);

                            List<string> MemEst = File.ReadAllLines(PATHEST).ToList();
                            MemEst.RemoveAt(Pos);
                            using (FileStream Dat = new FileStream(PATHEST, FileMode.Create))
                            using (StreamWriter SW = new StreamWriter(Dat))
                            {
                                foreach (string campo in MemEst)
                                    SW.WriteLine(campo);
                            }
                            Console.WriteLine("Campo eliminado");
                        }
                        else
                            Console.WriteLine("Campo no encontrado en tabla " + TB);
                    }
                }
                else
                    Console.WriteLine("No esta usando una base de datos");
            }
            void InsertarDato(string TB)
            {
                string[] Campos, CampoReferencia, CampoInsercion;
                int[] NumReferencia;
                string Tabla = Extraccion(TB, 2), In;
                DirectoryInfo DI = new DirectoryInfo(@"../BDs/" + BDUsing + "/");
                if (DI.GetFiles(Tabla + ".est").Any())
                {
                    bool cont = true, Correct;
                    int i = 0;
                    FileInfo[] FI = DI.GetFiles(Tabla + ".est");
                    FileInfo F = FI[0];
                    Campos = File.ReadAllLines(FI[0].FullName);
                    CampoReferencia = new string[Campos.Length];
                    NumReferencia = new int[Campos.Length];
                    CampoInsercion = new string[Campos.Length];

                    Console.WriteLine(F.Name);
                    foreach (string C in Campos)
                    {
                        Console.WriteLine("\t" + C);
                        NumReferencia[i] = ContarCampo(C);
                        CampoReferencia[i] = Extraccion(C, 0).Replace(",", "");
                        i++;
                    }
                    Console.WriteLine();
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
                                    if (item.Contains("entero") || item.Contains("fecha"))
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
                    StreamWriter SW = new StreamWriter(Dat);
                    for (int i1 = 0; i1 < CampoReferencia.Length; i1++)
                    {
                        string input = CampoInsercion[i1];
                        if (input == null)
                            for (int spaces = NumReferencia[i1]; spaces > 0; spaces--)
                                input += " ";
                        else
                            for (int spaces = NumReferencia[i1]; spaces > CampoInsercion[i1].Length; spaces--)
                                input += " ";
                        SW.Write(input);
                    }
                    SW.Close();
                    Dat.Close();
                }
                else
                    Console.WriteLine("La base de datos " + BDUsing + " no tiene la tabla " + Tabla);
            }
            void InsertarCampo(string TB)
            {
                List<string> Campos = new List<string>();
                string Reciente, NombreDeTabla = Extraccion(TB, 2);
                string DAT = @"../BDs/" + BDUsing + "/" + NombreDeTabla + ".dat",
                       EST = @"../BDs/" + BDUsing + "/" + NombreDeTabla + ".est";
                bool Exit = false, GoodInput = false;
                if (BDUsing != null)
                    if (File.Exists(@"../BDs/" + BDUsing + "/" + NombreDeTabla + ".est"))
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
                            int AllDat = ContarDato(File.ReadAllLines(EST).ToArray()), Entradas = 0;
                            string Mem = "";
                            char Buff;
                            File.AppendAllLines(EST, Campos);
                            int AllNewDat = ContarDato(File.ReadAllLines(EST).ToArray());
                            using (FileStream Dat = new FileStream(DAT, FileMode.Open))
                            {
                                using (StreamReader SR = new StreamReader(Dat))
                                {
                                    string L = SR.ReadToEnd();
                                    Entradas = L.Length / AllDat;
                                }
                            }
                            using (FileStream Dat = new FileStream(DAT, FileMode.Open))
                            using (StreamReader SR = new StreamReader(Dat))
                            {
                                for (int e = 0; e < Entradas; e++)
                                {
                                    for (int m = 0; m < AllDat; m++)
                                    {
                                        Buff = (char)SR.Read();
                                        Mem += Buff.ToString();
                                    }
                                    for (int i = AllDat; i < AllNewDat; i++)
                                        Mem += " ";
                                }
                            }
                            using (FileStream Dat = new FileStream(DAT, FileMode.Create))
                            using (StreamWriter SW = new StreamWriter(Dat))
                                SW.Write(Mem);
                            Console.WriteLine("Campo(s) insertados");
                        }
                    }
                    else
                        Console.WriteLine("La tabla " + NombreDeTabla + " no existe");
                else
                    Console.WriteLine("No esta usando una base de datos");
            }
            void EliminarCampoDonde(string UI)
            {
                string Tabla = Extraccion(UI, 2),
                    Campo = Extraccion(UI, 4),
                    Valor = Extraccion(UI, 6),
                    EST = @"../BDs/" + BDUsing + "/" + Tabla + ".est",
                    DAT = @"../BDs/" + BDUsing + "/" + Tabla + ".dat",
                    Mem = "",
                    MemBig = "",
                    Detector;
                char Buffer;


                if (File.Exists(EST))
                {
                    if (File.ReadAllText(EST).Contains(@Campo))
                    {
                        int AllDat = ContarDato(File.ReadAllLines(EST)), Entradas, BefDat = 0, Pos = 0;
                        string[] Arr = File.ReadAllLines(EST).ToArray();
                        bool Found = false;
                        Regex TestString = new Regex(@Campo);
                        do
                        {
                            if (TestString.IsMatch(Arr[Pos]))
                                Found = true;
                            else
                                Pos++;
                        }
                        while (!Found);
                        for (int i = 0; i < Pos; i++)
                            BefDat += ContarCampo(Arr[i]);

                        using (FileStream Dat = new FileStream(DAT, FileMode.Open))
                        {
                            using (StreamReader SR = new StreamReader(Dat))
                            {
                                string L = SR.ReadToEnd();
                                Entradas = L.Length / AllDat;
                            }
                        }
                        using (FileStream Dat = new FileStream(DAT, FileMode.Open))
                        using (StreamReader SR = new StreamReader(Dat))
                        {
                            for (int i = 0; i < Entradas; i++)
                            {
                                for (int m = 0; m < AllDat; m++)
                                {
                                    Buffer = (char)SR.Read();
                                    Mem += Buffer.ToString();
                                }
                                Detector = Mem.Substring(BefDat, ContarCampo(Arr[Pos]));
                                if (Detector.Trim(' ') == Valor)
                                    Mem = "";
                                MemBig += Mem;
                                Mem = "";
                            }
                        }
                        using (FileStream Dat = new FileStream(DAT, FileMode.Create))
                        using (StreamWriter SW = new StreamWriter(Dat))
                            SW.Write(MemBig);
                        Console.WriteLine("Accion completada");
                    }
                    else
                        Console.WriteLine("Campo " + Campo + " no existe");
                }
                else
                    Console.WriteLine("La tabla " + Tabla + " no existe");
            }
            void ModificaCampoDonde(string UI)
            {
                string Tabla = Extraccion(UI, 2),
                    Campo = Extraccion(UI, 4),
                    Valor = Extraccion(UI, 6),
                    EST = @"../BDs/" + BDUsing + "/" + Tabla + ".est",
                    DAT = @"../BDs/" + BDUsing + "/" + Tabla + ".dat",
                    Mem = "",
                    MemBig = "",
                    Detector,
                    Reciente;
                char Buffer;
                bool Exit, cont = true;
                string[] Campos, CampoReferencia, CampoInsercion;
                int[] NumReferencia;
                Campos = File.ReadAllLines(EST);
                CampoReferencia = new string[Campos.Length];
                NumReferencia = new int[Campos.Length];
                CampoInsercion = new string[Campos.Length];
                int e = 0;
                foreach (string C in Campos)
                {
                    Console.WriteLine("\t" + C);
                    NumReferencia[e] = ContarCampo(C);
                    CampoReferencia[e] = Extraccion(C, 0).Replace(",", "");
                    e++;
                }

                if (File.Exists(EST) && BDUsing != null)
                {
                    if (File.ReadAllText(EST).Contains(@Campo))
                    {
                        int AllDat = ContarDato(File.ReadAllLines(EST)), Entradas, BefDat = 0, Pos = 0;
                        string[] Arr = File.ReadAllLines(EST).ToArray();
                        int[] DataSpaces = new int[Arr.Length];
                        int x = 0;
                        bool Found = false;
                        foreach (string u in Arr)
                            if (Extraccion(u, 1) == "fecha")
                                DataSpaces[x] = 8;
                            else if (Extraccion(u, 1) == "decimal,")
                                DataSpaces[x] = int.Parse(Extraccion(u, 2).Replace(",", "")) + int.Parse(Extraccion(u, 3));
                            else
                                DataSpaces[x] = int.Parse(Extraccion(u, 2));


                        Regex Entrada = new Regex(@"\b(\S+\s=\s(\S|\d)+)(;)?");
                        do
                        {
                            Reciente = Console.ReadLine();
                            Exit = false;
                            for (int i1 = 0; i1 < CampoReferencia.Length; i1++)
                            {
                                string item = CampoReferencia[i1];
                                if (Entrada.Match(Reciente).Success && (Extraccion(Reciente, 2).Length <= NumReferencia[i1]))
                                {
                                    if (Reciente.Contains(item))
                                    {
                                        if (item.Contains("entero") || item.Contains("fecha"))
                                        {
                                            if (int.TryParse(Extraccion(Reciente, 2), out int result))
                                            {
                                                CampoInsercion[i1] = result.ToString();
                                                Exit = true;
                                            }
                                        }
                                        else if (item.Contains("decimal"))
                                        {
                                            if (int.TryParse((Extraccion(Reciente, 2) + Extraccion(Reciente, 3)), out int result))
                                            {
                                                CampoInsercion[i1] = result.ToString();
                                                Exit = true;
                                            }
                                        }
                                        else
                                        {
                                            CampoInsercion[i1] = Extraccion(Reciente, 2);
                                            Exit = true;
                                        }
                                    }
                                }
                            }
                            if (!Exit)
                                Console.WriteLine("Formato incorrecto");
                            else if (Reciente.Contains(";"))
                                cont = false;
                        } while (cont);



                        Regex TestString = new Regex(@Campo);
                        do
                        {
                            if (TestString.IsMatch(Arr[Pos]))
                                Found = true;
                            else
                                Pos++;
                        }
                        while (!Found);
                        for (int i = 0; i < Pos; i++)
                            BefDat += ContarCampo(Arr[i]);

                        using (FileStream Dat = new FileStream(DAT, FileMode.Open))
                        {
                            using (StreamReader SR = new StreamReader(Dat))
                            {
                                string L = SR.ReadToEnd();
                                Entradas = L.Length / AllDat;
                            }
                        }
                        using (FileStream Dat = new FileStream(DAT, FileMode.Open))
                        using (StreamReader SR = new StreamReader(Dat))
                        {
                            for (int i = 0; i < Entradas; i++)
                            {
                                for (int m = 0; m < AllDat; m++)
                                {
                                    Buffer = (char)SR.Read();
                                    Mem += Buffer.ToString();
                                }
                                Detector = Mem.Substring(BefDat, ContarCampo(Arr[Pos]));
                                if (Detector.Contains(Valor))
                                {
                                    Mem = "";
                                    for (int i1 = 0; i1 < CampoReferencia.Length; i1++)
                                    {
                                        if (CampoInsercion[i1] == null)
                                            CampoInsercion[i1] = "";
                                        Mem += CampoInsercion[i1];
                                        if (Mem == null)
                                            for (int spaces = NumReferencia[i1]; spaces > 0; spaces--)
                                                Mem += " ";
                                        else
                                            for (int spaces = NumReferencia[i1]; spaces > CampoInsercion[i1].Length; spaces--)
                                                Mem += " ";
                                    }
                                }
                                MemBig += Mem;
                                Mem = "";
                            }
                        }
                        using (FileStream Dat = new FileStream(DAT, FileMode.Create))
                        using (StreamWriter SW = new StreamWriter(Dat))
                            SW.Write(MemBig);
                        Console.WriteLine("Accion completada");
                    }
                    else
                        Console.WriteLine("Campo " + Campo + " no existe");
                }
                else
                    Console.WriteLine("La tabla " + Tabla + " no existe");
            }
            void EliminarTabla(string TB)
            {
                if (BDUsing != null)
                {
                    string Tabla = Extraccion(TB, 2),
                        PosTabla = @"../BDs/" + BDUsing + "/" + Tabla;
                    if (File.Exists(PosTabla + ".est"))
                    {
                        File.Delete(PosTabla + ".est");
                        File.Delete(PosTabla + ".dat");
                        Console.WriteLine("La tabla " + Tabla + " ha sido eliminada");
                    }
                    else
                        Console.WriteLine("La tabala " + Tabla + " no existe en la base de datos");
                }
                else
                    Console.WriteLine("No hay base de datos activa");
            }
            void Mostrar(string UsrI)
            {
                if (BDUsing != null)
                {
                    Regex A = new Regex("/*"),
                          B = new Regex("=");
                    bool TriggerAll = false, TriggerCond = false, CampoDetectado = false;
                    string TB = null;
                    int TBP = UsrI.Split(' ').Length - 5;
                    if (A.IsMatch(UsrI))
                        TriggerAll = true;
                    if (B.IsMatch(UsrI))
                        TriggerCond = true;
                    if (TriggerCond)
                        TB = Extraccion(UsrI, TBP);
                    else
                        TB = Extraccion(UsrI, UsrI.Split(' ').Length - 1);

                    string DAT = @"../BDs/" + BDUsing + "/" + TB + ".dat",
                           EST = @"../BDs/" + BDUsing + "/" + TB + ".est";

                    string[] Estructura = File.ReadAllLines(EST);
                    int Total = ContarDato(Estructura);
                    List<string> Datos = new List<string>();
                    string[] ADatos;

                    using (FileStream Dat = new FileStream(DAT, FileMode.Open))
                    using (StreamReader SR = new StreamReader(Dat))
                    {
                        while (SR.Peek() != -1)
                        {
                            Datos.Add(((char)SR.Read()).ToString());
                        }
                        ADatos = Datos.ToArray();
                    }
                    if (ADatos != null)
                    {
                        string[] Arr = File.ReadAllLines(EST).ToArray(), Nombres = new string[Arr.Length];
                        int[] DataSpaces = new int[Arr.Length];
                        string Mem = "";
                        string[] BigMemArr;
                        List<string> BigMem = new List<string>();
                        int x = 0, Entradas = Datos.Count / ContarDato(Arr);
                        foreach (string u in Arr)
                        {
                            if (Extraccion(u, 1) == "fecha")
                                DataSpaces[x] = 8;
                            else if (Extraccion(u, 1) == "decimal,")
                                DataSpaces[x] = int.Parse(Extraccion(u, 2).Replace(",", "")) + int.Parse(Extraccion(u, 3));
                            else
                                DataSpaces[x] = int.Parse(Extraccion(u, 2));
                            Nombres[x] = Extraccion(u, 0).Replace(",", "");
                            x++;
                        }

                        string[,] Data = new string[Entradas, Nombres.Length];
                        int CampoPos = 0, EntradaPos = 0, DatoPos = -1;
                        foreach (string i in ADatos)
                        {
                            DatoPos++;
                            Mem += i;
                            if (DatoPos == DataSpaces[CampoPos] - 1)
                            {
                                Data[EntradaPos, CampoPos] = Mem;
                                CampoPos++;
                                Mem = "";
                                DatoPos = -1;
                            }
                            if (CampoPos == DataSpaces.Length)
                            {
                                EntradaPos++;
                                CampoPos = 0;
                            }
                        }
                        if (TriggerCond)
                        {
                            string Cond = Extraccion(UsrI, UsrI.Split(' ').Length - 3);
                            string Val = Extraccion(UsrI, UsrI.Split(' ').Length - 1);
                            int s = 0, Good = 0;
                            foreach (string i in Nombres)
                                if (i == Cond)
                                    CampoDetectado = true;
                                else
                                    if (!CampoDetectado)
                                    s++;
                            if (CampoDetectado)
                            {
                                for (int f = 0; f < Entradas; f++)
                                    if (Val == Data[f, s])
                                            ImprimirDato(Data, Nombres, DataSpaces, f, Entradas);




                            }
                            else
                                Console.WriteLine("El campo introducido para la comparacion no existe en la tabla");

                        }
                    }
                    else
                        Console.WriteLine("No hay base de datos activa");
                }
            }
            void ImprimirDato(string[,] Dato, string[] Nombres, int[] Espacios, int Fila, int Entrada)
            {
                string[] NombresAjustados = Nombres;
                for (int i = 0; i < Nombres.Length; i++)                
                    if (NombresAjustados[i].Length <= Espacios[i]) 
                        for (int s = NombresAjustados[i].Length; s < Espacios[i]; s++)
                            NombresAjustados[i] += " ";
                foreach (string c in NombresAjustados)
                {
                    Console.Write(c + "|");
                }
                Console.WriteLine();
                for (int i = 0; i < NombresAjustados.Length; i++)
                {
                    if (Dato[Fila, i].Length < Espacios[i])
                        for (int s = Dato[Fila, i].Length; s < Espacios[i]; s++)
                            Dato[Fila, i] += " ";
                    Console.Write(Dato[Fila, i] + "|");
                }


                
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
        }
    }
}
