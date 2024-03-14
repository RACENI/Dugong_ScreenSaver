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
        StreamReader streamReader1;
        //StreamWriter streamWriter1;
        Thread thread1;
        TcpListener tcpListener1;

        //생성자
       public TcpServer()
        {
            Console.WriteLine("tcp서버 객체가 만들어졌습니다.");
        }
        ~TcpServer()
        {
            tcpListener1.Stop();
            thread1.Abort();
            MessageBox.Show("닫힘");
        }

        //서버 쓰레드 시작
        public void ThreadStart()
        {
            thread1 = new Thread(connect);
            thread1.IsBackground = true; // 폼 종료시 쓰레드 종료
            thread1.Start();
        }


/*        public void sendData(string text)
        {
            streamWriter1.WriteLine(text);
        }*/

        private void connect()
        {
            tcpListener1 = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            tcpListener1.Start();
            MessageBox.Show("커넥트 함수 스타트 완료");
            TcpClient tcpClient1 = tcpListener1.AcceptTcpClient();

            streamReader1 = new StreamReader(tcpClient1.GetStream());

            //streamWriter1 = new StreamWriter(tcpClient1.GetStream());
            //streamWriter1.AutoFlush = true;

            while(tcpClient1.Connected)
            {
                string receiveData1 = streamReader1.ReadLine();
            }
        }
    }
}
