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

            try {
                Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipReceive = IPAddress.Any;
                var listener = new TcpListener(ipReceive, 11000);


                Console.WriteLine("listening: " + ipReceive);
                while (true)
                {
                    listener.Start();
                    using (var client = listener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    {
                        while (true)
                        {
                            Byte[] data = new Byte[256];

                            // String to store the response ASCII representation.
                            String responseData = String.Empty;

                            // Read the first batch of the TcpServer response bytes.
                            int bytes = stream.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            nameOfTheFile = responseData;
                            Console.WriteLine(nameOfTheFile);
                            break;
                        }


                        if (nameOfTheFile != "")
                        {

                            using (var output = File.Create(nameOfTheFile + ".zip"))
                            {
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }


    }
}
