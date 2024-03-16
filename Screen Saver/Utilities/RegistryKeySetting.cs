using Microsoft.Win32;
using Screen_Saver.Utilities;
using System;

namespace Screen_Saver
{
    static class RegistryKeySetting
    {
        public static string GetValue(string key)
        {
            RegistryKey programkey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\SARACEN\\DugongScreen");
            try
            {
                string value = programkey.GetValue(key, false).ToString();
                return value == "False" ? null : value;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return "error";
            }
            finally
            {
                programkey.Close();
            }
        }

        public static void SetValue(string key1, string key2)
        {
            RegistryKey programkey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\SARACEN\\DugongScreen");
            try
            {
                programkey.SetValue(key1, key2);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
            finally
            {
                programkey.Close();
            }
        }

        public static void DeleteValue(string key)
        {
            RegistryKey programkey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\SARACEN\\DugongScreen");
            try
            {
                programkey.DeleteValue(key, false);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
            finally
            {
                programkey.Close();
            }
        }
    }
}
