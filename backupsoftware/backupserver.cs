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
           
            
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipReceive = IPAddress.Parse("your ip here");
            var listener = new TcpListener(ipReceive, 11000);
            listener.Start();


            while (true)
            {
                
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                {

                    var fileNamebuffer = new byte[512];
                    int filenamebytesRead;
                    Console.WriteLine("Client connected. Starting to receive the file");
					 while ((filenamebytesRead = stream.Read(fileNamebuffer, 0, fileNamebuffer.Length)) <= 512)
                    {
						nameOfTheFile = bytesRead;
					}
					
					if(nameOfTheFile != ""){
						
					using (var output = File.Create(nameOfTheFile)){
					 var buffer = new byte[1024];
                    int bytesRead;
				   while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {

                        output.Write(buffer, 0, bytesRead);

                    }
                    listener.Stop();
                    }
                }
			}

               
            }
           
        }
       
       
    }
}
