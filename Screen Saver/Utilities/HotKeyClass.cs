﻿using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Screen_Saver
{
    class HotKeyClass
    {
        public class HotKeyHandeler : IDisposable
        {
            private readonly Hashtable keyIDs = new Hashtable();
            public IntPtr Handle { get; private set; }
            public event EventHandler<HotKeyEventArgs> HotKeyPressed;
            public AccessModifierKeys ModifierKeys { get; private set; }

            ~HotKeyHandeler()
            {
                Dispose();
            }

            public void Dispose()
            {
                UnegisterHotKey();
            }

            public HotKeyHandeler(IntPtr Handle)
            {
                this.Handle = Handle;
            }

            public HotKeyHandeler(Window window)
            {
                this.Handle = new WindowInteropHelper(window).Handle;
                ComponentDispatcher.ThreadPreprocessMessage += new ThreadMessageEventHandler(ComponentDispatcher_ThreadPreprocessMessage);
            }

            void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
            {
                if (msg.message == WM_HOTKEY)
                {
                    short hkeyid = (short)msg.wParam;
                    if (keyIDs.ContainsKey(hkeyid))
                    {
                        if (HotKeyPressed != null)
                        {
                            int key = (int)(((int)msg.lParam >> 16) & 0xFFFF);
                            AccessModifierKeys modifier = (AccessModifierKeys)((int)msg.lParam & 0xFFFF);

                            HotKeyPressed(this, new HotKeyEventArgs(modifier, KeyInterop.KeyFromVirtualKey(key)));
                        }
                    }
                }
            }

            public void RegisterHotKey(AccessModifierKeys modifiers, Key key)
            {
                // Generates unique ID
                string atomName = modifiers.ToString() + key.ToString() + this.GetType().FullName;
                short hKeyID = GlobalAddAtom(atomName);
                ModifierKeys = modifiers;

                try
                {
                    if (hKeyID != 0)
                    {
                        if (!RegisterHotKey(Handle, hKeyID, (int)modifiers, KeyInterop.VirtualKeyFromKey(key)))
                            throw new ArgumentException("Hotkey combination could not be registered.");
                        else
                            keyIDs.Add(hKeyID, hKeyID);
                    }
                    else
                        throw new ArgumentException("Hotkey ID not generated!");
                }
                catch (Exception e)
                {

                }

            }

            public void UnegisterHotKey()
            {
                foreach (short id in keyIDs.Values)
                {
                    UnregisterHotKey(Handle, id);
                    GlobalDeleteAtom(id);
                }
            }

            #region WINAPI Helpers
            public const int WM_HOTKEY = 0x312;

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

            [DllImport("user32.dll")]
            private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

            [DllImport("kernel32", SetLastError = true)]
            private static extern short GlobalAddAtom(string lpString);

            [DllImport("kernel32", SetLastError = true)]
            private static extern short GlobalDeleteAtom(short nAtom);
            #endregion

        } // 핫키 클래스
        public enum AccessModifierKeys
        {
            /// <summary>
            /// Alt key
            /// </summary>
            Alt = 0x1,

            /// <summary>
            /// Control key
            /// </summary>
            Control = 0x2,

            /// <summary>
            /// Shift key
            /// </summary>
            Shift = 0x4,

            /// <summary>
            /// Window key
            /// </summary>
            Win = 0x8
        } // 핫키 메서드
        public class HotKeyEventArgs : EventArgs
        {
            public HotKeyEventArgs(AccessModifierKeys pModKeys, Key pKey)
            {
                ModifierKeys = pModKeys;
                Key = pKey;
            }
            public Key Key { get; private set; }
            public AccessModifierKeys ModifierKeys { get; private set; }
        } // 핫키 메서드

        public HotKeyHandeler handler = null; // 핫키 메서드
    }
}
