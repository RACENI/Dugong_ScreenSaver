using Microsoft.Win32;

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
            catch
            {
                return "error";
                throw;
            }
            finally
            {
                programkey.Close();
            }
        }

        public static void SetValue(string key1, string key2)
        {
            RegistryKey programkey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\SARACEN\\DugongScreen");
            programkey.SetValue(key1, key2);
            programkey.Close();
        }

        public static void DeleteValue(string key)
        {
            RegistryKey programkey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\SARACEN\\DugongScreen");
            programkey.DeleteValue(key, false);
            programkey.Close();
        }
    }
}
