using Screen_Saver.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Screen_Saver.HotKeyClass;
using System.Windows.Input;
using System.Windows;

namespace Screen_Saver.Managers
{
    internal class HotKeyManager
    {
        //핫키 세팅 
        public void CreatHotKey(Window window, AccessModifierKeys keyA, Key keyB)
        {
            try
            {
                HotKeyClass hot = new HotKeyClass();

                hot.handler = new HotKeyHandeler(window);
                hot.handler.RegisterHotKey(keyA, keyB);
                hot.handler.HotKeyPressed += new EventHandler<HotKeyEventArgs>(handler_HotKeyPressed);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }

        // 핫키 이벤트핸들러
        private void handler_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            LockManager lockManager = new LockManager();
            switch (lockManager.LockScreen())
            {
                case LockManager.LockFlag.PASSWORD_ERROR:
                    MessageBox.Show("비밀번호를 설정해주십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}
