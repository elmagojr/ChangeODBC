// See https://aka.ms/new-console-template for more information
using Microsoft.Win32;
using System.IO;

const string registryPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ODBC\ODBC.INI\";
const string dsnName = "SISC";

static string ff()
{
    Console.WriteLine("");
    Console.WriteLine("Ingrese el numero del servisor:");

    ConsoleKeyInfo keyInfo1 = Console.ReadKey();

    var go = keyInfo1.KeyChar.ToString();
    if (go =="Q" | go =="q")
    {
        Console.WriteLine("  Fianlizado");
    
    }
    else
    {
        go = go+ Console.ReadLine();
   
    }
    return go;

}


try
{
    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
    string directory = Path.GetDirectoryName(exePath);
    string fileName = "servers.txt";
    string path = System.IO.Path.Combine(directory, fileName);


    //string path = "servers.txt";
    String ServerName = "";
    List<string> serversNames;
    if (File.Exists(path))
    {
        serversNames = new List<string>(File.ReadAllLines(path));
        int posicion = 0;
        foreach (var item in serversNames)
        {
            Console.WriteLine($"[{posicion}] ->  {item}");
            posicion++;
        }
        if (serversNames.Count > 0)
        {           
            ServerName = ff();
     
            while (ServerName.ToString() != "q" && ServerName.ToString() != "Q" && ServerName != null)
            {                       
                
                if (Int32.TryParse(ServerName, out int index) && index >= 0 && index  < serversNames.Count )
                {

                    var serverValue = serversNames[Int32.Parse(ServerName)].ToString();
                    var databaseValue = serversNames[Int32.Parse(ServerName)].ToString();
                    try
                    {
                        Registry.SetValue($"{registryPath}{dsnName}", "ServerName", serverValue);
                        Registry.SetValue($"{registryPath}{dsnName}", "DatabaseName", databaseValue);
                    }
                    finally
                    {
                        Console.WriteLine($"  Servidor en ODBC Cambio a {serverValue}");
                        ServerName = ff();
                    }
                }
                else
                {
                    Console.WriteLine("   Valor no pertenece a ningun servidor");
                    ServerName = ff();
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
    Console.ReadKey();
}

catch (Exception ex)
{
    Console.WriteLine($"Error al actualizar la configuración: {ex.Message}");
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
}




