namespace nino;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

using Nasfaq.JSON;
using Nasfaq.API;

class Program
{
    static async Task Main(string[] args)
    {
        using(NasfaqAPI nasfaq = new NasfaqAPI())
        {
            nasfaq.SocketOnMessage += OnSocketMessage;

            while(true)
            {
                if(!nasfaq.IsSocketOpen())
                {
                    await nasfaq.OpenSocketAsync();
                }
                if(Console.KeyAvailable)  
                {  
                    break;
                }
            }
        }
    }

    static void OnSocketMessage(byte[] data)
    {
        try
        {
            //Console.WriteLine(SocketIO.BytesToString(data));
            IWebsocketData websocketData = SocketIO.BytesToSocketData(data);
            if(websocketData is WSAddMessageGlobal)
            {
                WSAddMessageGlobal messageGlobal = websocketData as WSAddMessageGlobal;
                File.AppendAllText("messagelogs", SocketIO.BytesToString(data) + "\n");
                Console.WriteLine(messageGlobal.username + ": " + messageGlobal.message);
            }
            File.AppendAllText("socketlogs", SocketIO.BytesToString(data) + "\n");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
