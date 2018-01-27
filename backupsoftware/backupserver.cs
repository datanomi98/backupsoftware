using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace backupserver
{
    class Program
    {

        public static string nameOfTheFile;
        static void Main(string[] args)
        {
            start:
            getFilename();
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipReceive = IPAddress.Parse("your ip here");
            var listener = new TcpListener(ipReceive, 11000);
            listener.Start();


            while (true)
            {
                
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                using (var output = File.Create(nameOfTheFile))
                {

                    var buffer = new byte[1024];
                    int bytesRead;
                    Console.WriteLine("Client connected. Starting to receive the file");
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {

                        output.Write(buffer, 0, bytesRead);

                    }
                    listener.Stop();
                    goto start;
                }

               
            }
           
        }
        static void getFilename()
        {

            var data = new byte[1024];
            IPEndPoint ServerEndPoint = new IPEndPoint(
               IPAddress.Parse("your ip here"), 9050);
            Socket WinSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            WinSocket.Bind(ServerEndPoint);
            Console.WriteLine("Waiting for client");
            IPEndPoint sender = new IPEndPoint(
               IPAddress.Parse("your ip here"), 0);
            EndPoint Remote = (EndPoint)(sender);
            int recv = WinSocket.ReceiveFrom(data, ref Remote);
            string returnFilename = Encoding.ASCII.GetString(data, 0, recv);
            nameOfTheFile = returnFilename;
            WinSocket.Close();
        }
       
    }
}
