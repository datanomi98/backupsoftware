using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace backupsoftware
{
    public partial class Form1 : Form
    { 

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            using (var folder = new FolderBrowserDialog())
            {
                try
                {
                    //open up folderDialog
                    DialogResult result = folder.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                    {
                        string FolderName = Path.GetFileName(folder.SelectedPath);
                        //zip file name will be the name of the selected folder.
                        string zipPath = @"D:\backupfolder\" + FolderName + ".zip";
                        if (File.Exists(zipPath))
                        {
                            richTextBox1.Text += zipPath;
                           
                            File.Delete(zipPath);
                            
                            ZipFile.CreateFromDirectory(folder.SelectedPath.ToString(), zipPath);
                            textBox1.Text = zipPath.ToString();
                           

                        }
                        else
                        {
                            richTextBox1.Text += "creating zip file";
                            textBox1.Text = "";

                            ZipFile.CreateFromDirectory(folder.SelectedPath.ToString(), zipPath);
                            textBox1.Text += zipPath.ToString();
                            richTextBox1.Text = "";

                        }



                    }
                }
                catch (Exception error)
                {
                    //richTextBox1.Text += error.Message.ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse("ip here");
            IPEndPoint ipendpoint = new IPEndPoint(ipaddress, 11000);

            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {


                //connect to the ip address
                string filename = textBox1.Text;
                soc.Connect(ipendpoint);
                string message = System.IO.Path.GetFileNameWithoutExtension(filename);
                var data = new byte[256];
                data = Encoding.ASCII.GetBytes(message);
                soc.Send(data);
                //send the file
                soc.SendFile(filename);

                soc.Shutdown(SocketShutdown.Both);
                soc.Close();
                richTextBox1.Text = "sent \n";

            }
            catch (Exception network)
            {
                richTextBox1.Text += "\n" + network.Message;
            }
        }
        
          
        }
       
           
    }
    
