using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODBCITOFW
{
    internal class Program
    {
       

        static void Main(string[] args)
        {
            const string registryPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ODBC\ODBC.INI\";
            const string dsnName = "SISC";
            string ActualServer = Registry.GetValue($"{registryPath}{dsnName}", "ServerName", null).ToString();
            string tipo; 
            String ServerName = "";
            List<string> serversNames;
            List<string> ListaMostrar;
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(exePath);
            string fileName = "servers.txt";
            string path = System.IO.Path.Combine(directory, fileName);
            string ff(int queOpcion)
            {

                Console.WriteLine("");
                if (queOpcion == 1)
                {
                    Console.WriteLine("Tipo de Servidor (d = DESARROLLO, p= PRODUCCION): ");
                }
                else
                {
                    Console.WriteLine("Ingrese el número del servidor:");

                }

                ConsoleKeyInfo keyInfo1 = Console.ReadKey();


                var go = keyInfo1.KeyChar.ToString();
                if (go.ToLower() == "w")
                {
                    Console.Clear();
                    init();
                }

                if (go.ToLower() == "q")
                {
                    Console.WriteLine("  Finalizado");
                }
                else
                {
                    go = go + Console.ReadLine();


                }
                return go;

            }
            void init()
            {
                if (File.Exists(path))
                {
                    serversNames = new List<string>(File.ReadAllLines(path));
                    ListaMostrar = new List<string>();
                    int posicion = 0;
                    ActualServer = Registry.GetValue($"{registryPath}{dsnName}", "ServerName", null).ToString();
                    tipo = Registry.GetValue($"{registryPath}{dsnName}", "Host", null)?.ToString() ?? "";

                    Console.WriteLine("EL SERVIDOR ACTUAL ES: " + ActualServer + " -" + tipo);
                    Console.WriteLine("");


                    var opcion = "f";
                    string principio = "";
                    string fin = "";
                    while (opcion.ToString().ToLower() != "d" & opcion.ToString().ToLower() != "p")
                    {
                        opcion = ff(1);
                    }

                    if (opcion.ToString().ToLower() == "d")
                    {
                        principio = "[DEV]";
                        fin = "[PROD]";
                        //Console.WriteLine($"[{posicion}] ->  {item}");
                        //posicion++;
                    }
                    else if (opcion.ToString().ToLower() == "p")
                    {
                        principio = "[PROD]";
                        fin = "[DEV]";
                    }



                    bool insertar = false;
                    foreach (var item in serversNames)
                    {
                        if (item.ToString() == principio)
                        {
                            insertar = true;
                        }
                        if (item.ToString() == fin)
                        {
                            insertar = false;
                        }

                        if (insertar & item.ToString() != principio)
                        {
                            //Console.WriteLine($"[{posicion}] ->  {item}");               
                            ListaMostrar.Add(item.ToString());
                        }
                    }

                    foreach (var item in ListaMostrar)
                    {
                        string[] split = new string[3];
                        string[] textoServer = item.Split(',');

                        if (textoServer.Length <= 2)
                        {
                            split[0] = textoServer[0];
                            if (textoServer.Length == 1)
                            {
                                split[1] = "[N/D]";
                            }
                            else
                            {
                                split[1] = textoServer[1];
                            }

                            split[2] = "[N/D]";
                        }
                        else
                        {
                            split[0] = string.IsNullOrWhiteSpace(textoServer[0]) ? "[N/D]" : textoServer[0];
                            split[1] = string.IsNullOrWhiteSpace(textoServer[1]) ? "[N/D]" : textoServer[1]; ;
                            split[2] = string.IsNullOrWhiteSpace(textoServer[2]) ? "[N/D]" : textoServer[2]; ;

                        }
                        if (opcion.ToString().ToLower() == "p")
                        {
                            if (split[0] != "[N/D]" && split[1] != "[N/D]" && split[2] != "[N/D]")
                            {
                                Console.WriteLine($"[{posicion}] ->  {split[0]}             ->{split[2]}, [{split[1]}]");
                            }
                        }
                        if (opcion.ToString().ToLower() == "d")
                        {
                            if (split[0] != "[N/D]")
                            {
                                Console.WriteLine($"[{posicion}] ->  {split[0]}             ->{split[2]}, [{split[1]}]");
                            }
                        }

                        posicion++;
                    }



                    if (ListaMostrar.Count > 0)
                    {
                        ServerName = ff(2);

                        while (ServerName.ToString() != "q" && ServerName.ToString() != "Q" && ServerName != null)
                        {

                            if (Int32.TryParse(ServerName, out int index) && index >= 0 && index < ListaMostrar.Count)
                            {
                                var serverValue = ListaMostrar[Int32.Parse(ServerName)].ToString();
                                var databaseValue = ListaMostrar[Int32.Parse(ServerName)].ToString();

                                string[] splitterServer = serverValue.Split(',');
                                string nombreServer = splitterServer[0].ToString();
                                string HostServer = splitterServer.Length > 1 ? (splitterServer.Length > 1 ? (opcion.ToString().ToLower() == "p" ? splitterServer[1].ToString() : "") : "") : "";


                                try
                                {


                                    Registry.SetValue($"{registryPath}{dsnName}", "ServerName", nombreServer);
                                    Registry.SetValue($"{registryPath}{dsnName}", "DatabaseName", nombreServer);
                                    Registry.SetValue($"{registryPath}{dsnName}", "Host", HostServer);
                                }
                                finally
                                {
                                    Console.WriteLine($"  Servidor en ODBC Cambio a {serverValue}");
                                    ServerName = ff(2);
                                }
                            }
                            else
                            {
                                Console.WriteLine("   Valor no pertenece a ningun servidor");

                                ServerName = ff(2);


                                //break;
                            }
                        }


                        Console.ReadKey();


                    }
                }
                else
                {
                    Console.WriteLine("No hay servers enlistados");
                    Console.ReadKey();
                }
            }


            try
            {



                //string path = "servers.txt";
                //String ServerName = "";
                //List<string> serversNames;
                //List<string> ListaMostrar;

                init();
                Console.ReadKey();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la configuración: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
    }
}
