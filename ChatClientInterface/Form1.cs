using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ChatClientInterface
{
    public partial class ChatWindow : Form
    {
        private Form parent;

        public ChatWindow(string name, string target = null, Form parent = null)
        {
            this.name = name;
            this.parent = parent;
            ip = target;

            start();
        }

        private static Socket _clientSocket;
        private static string ip = null;
        private byte[] buff = new byte[1024];

        private static Thread conn;
        private static Thread recieve;

        private static bool shouldUpdate;
        private static int rec;

        private static string nextText = null;
        private string name;

        private void start()
        {
            InitializeComponent();
            Show();

            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (Environment.MachineName == "MAHPC")
            {
                conn = new Thread(new ThreadStart(LocalLoopConnect));
            }
            else
            {
                conn = new Thread(new ThreadStart(LoopConnect));
            }
            conn.Start();
        }

        private void LoopConnect()
        {
            int tries = 0;
            if (ip == null)
            {
                ip = "192.168.1.30";
            }
            while (!_clientSocket.Connected)
            {
                try
                {

                    tries++;
                    _clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), 25565));
                }
                catch (SocketException)
                {

                }
            }
            //AddText("Connected to Server!");
            recieve = new Thread(new ThreadStart(RecieveDataInf));
            recieve.Start();
            conn.Join();
        }

        public void LocalLoopConnect()
        {
            while (!_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 25565));
                }
                catch (SocketException)
                {

                }
            }
            Console.WriteLine("Connected");
            recieve = new Thread(new ThreadStart(RecieveDataInf));
            recieve.Start();
            conn.Join();
        }


        private void SendRequest()
        {
            try
            {
                if (Input.Text.StartsWith("/"))
                {
                    SendCommand(Input.Text);
                    return;
                }
                string req = "/n|" + name + "|" + Input.Text;

                byte[] data = Encoding.ASCII.GetBytes(req);
                _clientSocket.Send(data);
            }
            catch (SocketException)
            {
                if (!_clientSocket.Connected)
                    AddText("Not connected to any server!");
                else
                    AddText("Unable to send message: SocketException");
            }
        }

        private void SendCommand(string p)
        {
            try
            {

                string req = "/p|" + p;

                byte[] data = Encoding.ASCII.GetBytes(req);
                _clientSocket.Send(data);
            }
            catch (SocketException)
            {
                AddText("No Server Connected");
            }
        }

        private void RecieveData()
        {
            try
            {
                int rec = _clientSocket.Receive(buff);
                byte[] data = new byte[rec];
                Array.Copy(buff, data, rec);
                string mes = Encoding.ASCII.GetString(data);
                AddText(mes);
            }
            catch (SocketException)
            {
                AddText("Server Disconnect!");
                start();
                return;
            }
        }

        private void RecieveDataInf()
        {

            do
            {
                if (!_clientSocket.Connected)
                {
                    LoopConnect();
                }

                if (_clientSocket.Available >= 1)
                {
                    try
                    {
                        rec = _clientSocket.Receive(buff);
                        byte[] data = new byte[rec];
                        Array.Copy(buff, data, rec);
                        string mes = Encoding.ASCII.GetString(data);
                        if (mes.StartsWith("/"))
                        {
                            ProcessCommand(mes);
                        }
                        else
                        {
                            nextText = mes;
                            shouldUpdate = true;
                        }
                        Invalidate();
                    }
                    catch (SocketException)
                    {

                    }
                }
            } while (_clientSocket.Connected);

        }

        private void ProcessCommand(string text)
        {
            try
            {
                if (text == Command.GETNAME)
                {
                    byte[] mes = Encoding.ASCII.GetBytes(name);
                    _clientSocket.Send(mes);
                    return;
                }
                else if (text == Command.GETIP)
                {
                    byte[] mes = Encoding.ASCII.GetBytes(_clientSocket.LocalEndPoint.ToString());
                    _clientSocket.Send(mes);
                }
                else if (text == Command.GETLOGIN)
                {
                    byte[] mes = Encoding.ASCII.GetBytes(name + "|" + _clientSocket.LocalEndPoint.ToString());
                    _clientSocket.Send(mes);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error processing command");
            }

        }

        private void AddText(string text)
        {
            Output.Items.Add(text);
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {
            Output.Items.Clear();
        }

        private void button1_Click(object s, EventArgs e)
        {
            SendRequest();
            Input.Text = "";
        }

        private void Output_Validated(object sender, EventArgs e)
        {
            if (shouldUpdate)
            {
                AddText(nextText);
            }

        }

        private void ChatWindow_Paint(object sender, PaintEventArgs e)
        {
            float size = Output.Font.Size;

            if (shouldUpdate)
            {
                if (nextText.Length * size >= Output.Width)
                {
                    string[] tmp = WrapText(nextText, size);
                    foreach (string s in tmp)
                    {
                        AddText(s);
                    }
                }
                else
                {
                    AddText(nextText);
                }
                shouldUpdate = false;
                Output.Invalidate();
            }
            Update();
        }

        private static string[] WrapText(string org, float size)
        {
            int mid = (int)(org.Length - size * 2);
            List<string> tmp = new List<string>();
            try
            {
                string[] split = new string[] { org.Substring(0, mid), org.Substring(mid) };
                foreach (string s in split)
                {
                    if (s.Length >= Output.Width)
                    {
                        string[] sarray = WrapText(s, size);
                        foreach (string i in sarray)
                        {
                            tmp.Add(i);
                        }
                    }
                    else
                        tmp.Add(s);
                }

            }
            catch (ArgumentOutOfRangeException)
            {

            }

            return tmp.ToArray();
        }

        private void ChatWindow_FormClosed(object sender, FormClosingEventArgs e)
        {
            if (_clientSocket.Connected)
            {
                SendCommand("/stop");
                _clientSocket.Disconnect(false);
                _clientSocket.Dispose();
            }

            if (parent != null)
            {
                parent.Visible = true;
            }

            this.Dispose();
        }

        public void ConfirmDisconnect()
        {
            if (_clientSocket.Connected)
            {
                SendCommand("/stop");
                _clientSocket.Disconnect(false);
                _clientSocket.Dispose();
            }
        }

        private void keyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }

    }

    struct Command
    {
        public static string GETNAME = "/get name";
        public static string GETIP = "/get ip";
        public static string GETLOGIN = "/get login";

    }
}
