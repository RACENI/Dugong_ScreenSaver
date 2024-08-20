using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace Screen_Saver.WebSocket
{
    //WebSocketBehavior클래스를 상속받은 클래스는 WebSocketServer 라이브러리 자체에서 관리함(개발자가 인스턴스 생성 필요 X)
    internal class Dugong : WebSocketBehavior
    {
        private static List<Dugong> _clients = new List<Dugong>();

        protected override void OnOpen()
        {
            // 클라이언트가 연결되면 리스트에 추가
            _clients.Add(this);
            Send("Welcome! You've connected to the server.");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            // 클라이언트로부터 받은 메시지를 다시 클라이언트로 전송
            Send("Dugong: " + e.Data);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            // 클라이언트 연결이 종료되면 리스트에서 제거
            _clients.Remove(this);
        }

        public static void Broadcast(string message)
        {
            // 모든 연결된 클라이언트에게 메시지 전송
            foreach (var client in _clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    client.Send(message);
                }
            }
        }
    }
}
