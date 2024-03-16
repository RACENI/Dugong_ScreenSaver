using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Screen_Saver
{
    internal class _InterceptKeys
    {
        #region Delegates

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
                                                      LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
                                                   IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

    internal class InterceptKeys
    {
        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) // 후킹 설정
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)vkCode;

                if (key == System.Windows.Forms.Keys.Escape || key == System.Windows.Forms.Keys.Alt || key == System.Windows.Forms.Keys.RWin || key == System.Windows.Forms.Keys.LWin || key == System.Windows.Forms.Keys.Tab)
                    return (IntPtr)1; // Handled.
            }

            return _InterceptKeys.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static readonly _InterceptKeys.LowLevelKeyboardProc _proc = HookCallback; // 후킹
        private static IntPtr _hookID = IntPtr.Zero; // 후킹
        public IntPtr hookID = _hookID;
        public _InterceptKeys.LowLevelKeyboardProc proc = _proc;

    }

}
