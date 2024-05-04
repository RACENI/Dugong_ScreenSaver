using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using static Screen_Saver.HotKeyClass;
using System.Windows.Input;
using System.Diagnostics;
using System.Net;
using Screen_Saver.Utilities;
using System.Drawing;
using System.Runtime.InteropServices;


namespace Screen_Saver.Managers
{
    internal class ScreenSaverManager
    {
        ExceptionLogger ExceptionLogger = new ExceptionLogger();

        #region 타이머
        //여기도 새로 클래스 만들어야되긴 할듯 타이머 클래스
        private DispatcherTimer startTimer()
        {
            DispatcherTimer timer = new DispatcherTimer(); //객체생성
            timer.Interval = TimeSpan.FromMilliseconds(100); //시간간격 설정
            timer.Tick += new EventHandler(maple_timer); //이벤트 추가
            timer.Start(); //타이머 시작. 종료는 timer.Stop();
            return timer;
        }

        //게임 확인 타이머 메서드
        private void maple_timer(object sender, EventArgs e)
        {

            bool mapleon = false;
            Process[] ProcessList = Process.GetProcessesByName("MapleStory");
            if (mapleon == false && ProcessList.Length > 0)
            {
                mapleon = true;
            }
            else if (mapleon == true && ProcessList.Length < 1)
            {
                //Process.Start("shutdown.exe", "-s -t 5");

            }
        }
        #endregion





        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);
        private struct POINT
        {
            public int X;
            public int Y;
        }

        //좌표지정 메서드
        private void CursorPosition()
        {
            POINT cursorPos;
            GetCursorPos(out cursorPos);
            Console.WriteLine("마우스 위치: X={0}, Y={1}", cursorPos.X, cursorPos.Y);
        }
    }
}
