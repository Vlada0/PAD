using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    class Broadcast
    {
        private static UdpClient udpClient;
        public static void Send(int port)
        {

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep1 = new IPEndPoint(IPAddress.Loopback, 9050);
            //IPEndPoint iep2 = new IPEndPoint(IPAddress.Loopback, 3000);
            EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
           // byte[] data = Encoding.ASCII.GetBytes(port.ToString());
            PortData portData = new PortData(port);
            var json = JsonConvert.SerializeObject(portData);
            var data = Encoding.UTF8.GetBytes(json);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            sock.SendTo(data, iep1);
           // sock.SendTo(data, iep2);
            Thread thread_broadcast = new Thread(() =>
            {
                for (int i = 1; i < 65535; i++)
                {
                    if (i != port)
                    {
                        iep1.Port = i;
                        sock.SendTo(data, iep1);
                        //Console.WriteLine(iep1);
                    }
                }
                sock.Close();
                Console.WriteLine("End Broadcast");
            });
            thread_broadcast.Start();
            Thread thread = new Thread(() => {
             
                int PORT = 9050;
                udpClient = new UdpClient();
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

                var from = new IPEndPoint(0, 0);
                Task.Run(() =>
                {
                    while (true)
                    {
                        var recvBuffer = udpClient.Receive(ref from);
                        if(Encoding.UTF8.GetString(recvBuffer)== "Give me port!")
                        {
                            udpClient.Send(data, data.Length, from.Address.ToString(), from.Port);
                            Console.WriteLine(Encoding.UTF8.GetString(recvBuffer));
                        }
                        
                    }
                }); 
            });
            thread.Start();
        }
       // public static void 
    }
}
