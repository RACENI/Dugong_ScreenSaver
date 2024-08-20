using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Screen_Saver.Managers
{
    internal class WebSocketServerManager
    {
        private WebSocketServer _wss;

        //객체 생성시 서버 초기화
        public WebSocketServerManager(string url)
        {
            _wss = new WebSocketServer(url);
        }

        // WebSocket 서비스를 추가하는 메서드
        public void addService<TBehavior>(string path) where TBehavior : WebSocketBehavior, new()
        {
            _wss.AddWebSocketService<TBehavior>(path);
        }

        // WebSocket 서버를 시작하는 메서드
        public void startServer()
        {
            _wss.Start();
            Console.WriteLine("WebSocket 서버가 시작되었습니다.");
        }

        // WebSocket 서버를 중지하는 메서드
        public void stopServer()
        {
            _wss.Stop();
            Console.WriteLine("WebSocket 서버가 종료되었습니다.");
        }
    }
}
