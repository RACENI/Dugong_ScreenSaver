using Screen_Saver.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Screen_Saver.Managers
{
    internal class TrayManager
    {
        private System.Windows.Forms.NotifyIcon notify;
        private readonly Window window;

        public TrayManager(Window window)
        {
            if (notify == null) tray_setting();
            this.window = window;
        }

        //  트레이 유지
        public int TrayOpen()
        {
            try
            {
                notify.Visible = true;   // 트레이아이콘 보이기
                window.Hide();
                return 1;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return 2;
            }
        }


        // 트레이 세팅 //
        private void tray_setting()
        {
            try
            {
                System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
                // 아이콘 설정부분
                notify = new System.Windows.Forms.NotifyIcon();
                notify.Icon = new System.Drawing.Icon(@"icon.ico");   // Resources 아이콘 사용 시 // 외부아이콘 사용 시
                                                                      //new System.Drawing.Icon(@"icon.ico");
                                                                      //notify.Icon = Properties.Resources.TiimeAlram;   // Resources 아이콘 사용 시
                notify.ContextMenu = menu;
                notify.Text = fsetting.cap;

                // 아이콘 더블클릭 이벤트 설정
                notify.DoubleClick += Notify_DoubleClick;

                System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item1);
                item1.Index = 0;
                item1.Text = "프로그램 종료";
                item1.Click += delegate (object click, EventArgs eClick)
                {
                    Application.Current.Shutdown();
                    notify.Dispose();
                };

                System.Windows.Forms.MenuItem item2 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item2);
                item2.Index = 0;
                item2.Text = "화면 잠금";
                item2.Click += delegate (object click, EventArgs eClick)
                {

                    //screenSaverManager.Locking();
                };

                System.Windows.Forms.MenuItem item3 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item3);
                item3.Index = 0;
                item3.Text = "열기";
                item3.Click += delegate (object click, EventArgs eClick)
                {
                    window.Show();
                    window.WindowState = WindowState.Normal;
                    window.Visibility = Visibility.Visible;
                    notify.Visible = false;   // 트레이아이콘 숨기기
                };
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }
        // 트레이 더블클릭 //
        private void Notify_DoubleClick(object sender, EventArgs e)
        {
            window.Show();
            window.WindowState = WindowState.Normal;
            window.Visibility = Visibility.Visible;
            notify.Visible = false; // 트레이아이콘 숨기기
        }
    }
}
