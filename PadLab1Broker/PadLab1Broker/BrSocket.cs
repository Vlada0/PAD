using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    class BrSocket
    {
        private const int Limit = 10;
        private Socket socket;

        public BrSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        public void StartBroker(string ip)
        {
            bool isInvalidPort = true;
            Random rnd = new Random();
            while (isInvalidPort)
            {
                try
                {
                    int value = rnd.Next(1, 65535);
                    socket.Bind(new IPEndPoint(IPAddress.Parse(ip), value));
                    isInvalidPort = false;
                    socket.Listen(Limit);
                    SocketAccept();
                    Broadcast.Send(value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Port is busy");
                }
            }  
        }
       
        private void SocketAccept()
        {
            socket.BeginAccept(AcceptCallBack, null);
        }

        private void AcceptCallBack(IAsyncResult asyncResult)
        {
            ConnectInformation connection = new ConnectInformation();

            try
            {
                connection.Socket = socket.EndAccept(asyncResult);
                connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ReceiveCallBack, connection);
            }
            catch(Exception e)
            {
                Console.WriteLine("Не удалось принять сообщение " + e.Message);
            }
            finally
            {
                SocketAccept();
            }
        }

        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            ConnectInformation connection = asyncResult.AsyncState as ConnectInformation;

            try
            {
                Socket senderSocket = connection.Socket;
                SocketError response;
                int buffer_size = senderSocket.EndReceive(asyncResult, out response);

                if(response == SocketError.Success)
                {
                    byte[] message = new byte[buffer_size];
                    Array.Copy(connection.Buffer, message, message.Length);
                    Handler.Handle(message, connection);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось получить данные "+ e.Message);
            }
            finally
            {
                try
                {
                    connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ReceiveCallBack, connection);
                }
                catch (Exception e)
                {
                    
                    var address = connection.Socket.RemoteEndPoint.ToString();
                    var id = Storage.publisherStorage.GetUserByAddress(address);
                    if (Storage.publisherStorage.Remove(address) > 0)
                    {
                        Console.WriteLine($"Датчик вышел из строя: {id}");
                    }
                    else {
                        Storage.subscriberStorage.Remove(address);
                    }
                    
                    connection.Socket.Close();                   
                }
            }
        }

    }
}
