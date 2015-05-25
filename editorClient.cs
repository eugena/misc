using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


/**
 * Клиент приложения поиска вариантов строк по фрагменту.
 */
namespace konturEditorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)  {
                /**
                * Если задана строка, получаем данные
                */
                try {
                    string str = args[0];
                    string ip = "127.0.0.1";
                    UInt16 port = 8100;
                    if (args.Length > 1) {
                        ip = args[1];
                        if (args.Length > 2) {
                            port = Convert.ToUInt16(args[2]);
                        }
                    }
                    GetData(str, ip, port);
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            } else {
                /**
                 * Если строка не задана, предполагаем, что пользователь не знает, что делать 
                 * - выводим справку
                 */
                Console.WriteLine("Программа автоподбора вариантов строк по фрагменту.");
                Console.WriteLine("Фрагмент строки является обязательным параметром.");
                Console.WriteLine("Вторым (необязательным) параметром является ip сервера.");
                Console.WriteLine("Третьим (необязательным) порт сервера.");
            }
        }

        /**
         * Получение данных
         */
        static void GetData(string str, string ip, UInt16 port)
        {
            byte[] bytes = new byte[1024]; // Буфер для входящих данных

            /**
             * Установка точки для сокета, соединение, отправка и получение данных
             */
            IPAddress ipAddr = IPAddress.Parse(ip);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);
            byte[] msg = Encoding.ASCII.GetBytes(string.Format("get {0}", str));
            int bytesSent = sender.Send(msg);
            int bytesRec = sender.Receive(bytes);
            Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
