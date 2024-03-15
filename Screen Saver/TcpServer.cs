using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Screen_Saver
{
    internal class TcpServer
    {
        TcpListener tcpListener;
        private bool isRunning = true;

        public void start()
        {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 2000);
            tcpListener.Start();
            //MessageBox.Show("서버 시작 완료");

            // 클라이언트 연결을 수락하는 부분을 백그라운드 스레드로 이동
            Thread acceptThread = new Thread(AcceptClients);
            acceptThread.IsBackground = true;
            acceptThread.Start();
        }

        private void AcceptClients()
        {
            try
            {
                while (isRunning)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    //MessageBox.Show("클라이언트 연결: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

                    // 각 클라이언트와의 통신을 담당하는 스레드 생성
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
            }
            catch (Exception ex) { }
        }

        private void HandleClient(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;

            using (StreamReader reader = new StreamReader(client.GetStream()))
            using (StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true })
            {
                try
                {
                    while (true)
                    {
                        string receiveData = reader.ReadLine();
                        if (receiveData == null)
                            break;

                        switch (receiveData)
                        {
                            case "gd":
                                writer.WriteLine("hi");
                                break;
                            default:
                                writer.WriteLine("Unknown command");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("클라이언트 연결 오류: " + ex.Message);
                }
                finally
                {
                    client.Close(); // 클라이언트 연결 닫기
                }
            }
        }

        // 서버 종료
        public void Stop()
        {
            isRunning = false;
            tcpListener.Stop();
        }
    }
}
