using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;


/**
 * Сервер приложения поиска вариантов строк по фрагменту.
 * В качестве хранилища используется текстовый файл test.in.
 */
namespace konturEditorServer
{
    class Server {
        TcpListener Listener;

        /**
         * Прием клиентов и передача в новый поток
         */
        public Server(UInt16 Port, string fileName) {
            Listener = new TcpListener(IPAddress.Any, Port); 
            Listener.Start();
            while (true) {
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(
                        delegate(object state) { ProcessRequest(state, fileName); }), 
                        Listener.AcceptTcpClient());
            }
        }

        /**
         * Обработка запроса клиента
         */
        static void ProcessRequest(Object StateInfo, string fileName) {
            TcpClient Client = (TcpClient)StateInfo;
            string str; // фрагмент
            byte[] buffer = new byte[19]; // буфер для хранения принятых от клиента данных (4+15)
            int count; // переменная для хранения количества байт, принятых от клиента
            if ((count = Client.GetStream().Read(buffer, 0, buffer.Length)) > 0) {
                /**
                 * Если данные от клиента получены - получение фрагмента из запроса, 
                 * получение вариантов строк и передача клиенту
                 */
                str = Encoding.ASCII.GetString(buffer, 4, count - 4);  // запрос передается в виде "get <фрагмент>"

                string[] lines = System.IO.File.ReadAllLines(fileName);
                uint n = Convert.ToUInt32(lines[0]);
                Dictionary<string, uint> Data = new Dictionary<string, uint>();
                for (int i = 1; i <= n; i++) {
                    string[] d = lines[i].Split(' ');
                    Data.Add(d[0], Convert.ToUInt16(d[1]));
                }
                var result =
                    (from item in Data
                     where item.Key.StartsWith(str)
                     orderby item.Value descending, item.Key
                     select item.Key).Take(10).ToList();
                byte[] data = Encoding.ASCII.GetBytes(String.Join("\n", result));
                Client.GetStream().Write(data, 0, data.Length);
            }
            Client.Close();
        }

        /**
         * Обработка остановки сервера
         */
        ~Server() {
            if (Listener != null)  {
                Listener.Stop();
            }
        }

        static void Main(string[] args) {
            try {
                UInt16 port = 8100;
                string fileName = "test.in";
                if (args.Length > 0) {
                    port = Convert.ToUInt16(args[0]);
                    if (args.Length > 1) {
                        fileName = args[1];
                    }
                }

                /**
                * Установка количества потоков
                * и создание нового сервера
                */
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                ThreadPool.SetMinThreads(2, 2);
                new Server(port, fileName);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
