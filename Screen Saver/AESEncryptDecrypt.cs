using System;
using System.IO;
using System.Security.Cryptography;

namespace Screen_Saver
{
    class AESEncryptDecrypt
    {
        private RijndaelManaged rijndaelManaged = new RijndaelManaged();

        public byte[] GetKey()
        {
            try
            {
                //key 생성 및 값 삽입
                if (RegistryKeySetting.GetValue("dgkah") == null)
                {
                    SetKey();
                    byte[] key = rijndaelManaged.Key;
                    return key;
                }
                else
                {
                    return Convert.FromBase64String(RegistryKeySetting.GetValue("dgkah"));
                }
            }
            catch
            {
                throw;
            }
        }

        public void SetKey()
        {
            try
            {
                //key 생성 및 값 삽입
                if (RegistryKeySetting.GetValue("dgkah") == null)
                {
                    rijndaelManaged.GenerateKey();
                    RegistryKeySetting.SetValue("dgkah", Convert.ToBase64String(rijndaelManaged.Key));
                }
                else
                {
                    RegistryKeySetting.DeleteValue("dgkah");
                    RegistryKeySetting.SetValue("dgkah", Convert.ToBase64String(rijndaelManaged.Key));
                }
            }
            catch
            {
                throw;
            }
        }

        public byte[] GetIV()
        {
            try
            {
                //IV 생성 및 값 삽입
                if (RegistryKeySetting.GetValue("ddkl") == null)
                {
                    SetIV();
                    byte[] IV = rijndaelManaged.IV;
                    return IV;
                }
                else
                {
                    return Convert.FromBase64String(RegistryKeySetting.GetValue("ddkl"));
                }
            }
            catch
            {
                throw;
            }
        }

        public void SetIV()
        {
            try
            {
                //IV 생성 및 값 삽입
                if (RegistryKeySetting.GetValue("ddkl") == null)
                {
                    rijndaelManaged.GenerateIV();
                    RegistryKeySetting.SetValue("ddkl", Convert.ToBase64String(rijndaelManaged.IV));
                }
                else
                {
                    RegistryKeySetting.DeleteValue("ddkl");
                    RegistryKeySetting.SetValue("ddkl", Convert.ToBase64String(rijndaelManaged.IV));
                }
            }
            catch
            {
                throw;
            }
        }

        //AES_256 암호화
        public string AESEncrypt(string plainText, byte[] key, byte[] IV)
        {
            try
            {
                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");

                byte[] encrypted;

                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    Console.WriteLine(key);
                    rijAlg.Key = key;
                    rijAlg.IV = IV;

                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {

                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                return Convert.ToBase64String(encrypted);
            }
            catch
            {
                throw;
            }
        }

        //AES_256 복호화
        public string AESDecrypt(byte[] decryptData, byte[] key, byte[] IV)
        {
            try
            {
                if (decryptData == null || decryptData.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (key == null || key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");

                // Declare the string used to hold
                // the decrypted text.
                string plaintext = null;

                // Create an RijndaelManaged object
                // with the specified key and IV.
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = key;
                    rijAlg.IV = IV;

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(decryptData))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;
            }
            catch
            {
                throw;
            }
        }
    }
}
