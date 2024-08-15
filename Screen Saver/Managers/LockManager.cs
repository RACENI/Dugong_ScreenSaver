using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace Screen_Saver.Managers
{
    internal class LockManager
    {
        public enum LockFlag { COMPLETE , PASSWORD_ERROR }
        public LockFlag LockScreen()
        {
            if (RegistryKeySetting.GetValue("PW") == null)
            {
                return LockFlag.PASSWORD_ERROR;
            }
            else
            {
                DispatcherTimer timer = null;

                /*if (RegistryKeySetting.GetValue("maple") == "is") // 미구현c (게임별로 enum객체 생성)
                {
                    timer = startTimer();
                }*/

                Window LW = new LockingWindow(timer); // 폼 종료시 타이머 멈출 수 있게 객체로 던짐
                LW.ShowDialog();
                return LockFlag.COMPLETE;
            }
        }
    }
}
