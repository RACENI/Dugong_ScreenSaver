using System;
using System.IO;
using System.Security.Cryptography;

namespace Screen_Saver
{
    class AESEncryptDecrypt
    {
        private RijndaelManaged rijndaelManaged = new RijndaelManaged();
        private byte[] key = Convert.FromBase64String("NYPtX9SaRuneEin4f8tAJ46CDOT/L/JP4HiqTgsL8oA=");
        private byte[] IV = Convert.FromBase64String("epcKQn87rxLE4QNyLwB5bw==");

        public byte[] getKey()
        {
            try
            {
                string key = "NYPtX9SaRuneEin4f8tAJ46CDOT/L/JP4HiqTgsL8oA=";
                return Convert.FromBase64String(key);
            }
            catch(Exception e)
            {
                throw new Exception("AES getKey Error: " + e);
            }
        }

        public void setKey()
        {
           //키값을 생성해줌  
           /*rijndaelManaged.GenerateKey();
           rijndaelManaged.Key;*/
        }

        public byte[] getIV()
        {
            try
            {
                string IV = "epcKQn87rxLE4QNyLwB5bw==";
                return Convert.FromBase64String(IV);
            }
            catch(Exception e)
            {
                throw new Exception("AES getIV Error: " + e);
            }
        }

        public void setIV()
        {
            //IV값 생성
/*            rijndaelManaged.GenerateIV();
            rijndaelManaged.IV;*/
        }

        //AES_256 암호화
        public string encryptAES(string plainText)
        {
            try
            {
                if (plainText == null || plainText.Length <= 0)
                    return null;

                byte[] encrypted;

                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
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
            catch(Exception e)
            {
                throw new Exception("AES encryptAES Error: " + e);
            }
        }

        //AES_256 복호화
        public string decryptAES(string encryptedText)
        {
            if (encryptedText == null || encryptedText.Length <= 0)
                return null;

            byte[] decryptData = Convert.FromBase64String(encryptedText);

            try
            {
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
            catch(Exception e)
            {
                throw new Exception("AES decryptAES Error: " + e); ; ;
            }
        }
    }
}
