using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ABM.Business_Logic
{
   
    public static class EncryptionManager
    {
        /// <summary>
        /// Encrypts value with a key, KEY MUST BE 16 CHAR
        /// </summary>
        public static string Encrypt(string value, string encryptionKey)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(encryptionKey); //must be 16 chars
                var rijndael = new RijndaelManaged
                {
                    BlockSize = 128,
                    IV = key,
                    KeySize = 128,
                    Key = key
                };

                var transform = rijndael.CreateEncryptor();
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(value);

                        cs.Write(buffer, 0, buffer.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    ms.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Decrypts value with a key, KEY MUST BE 16 CHAR
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        public static string Decrypt(string value, string encryptionKey)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(encryptionKey); //must be 16 chars
                var rijndael = new RijndaelManaged
                {
                    BlockSize = 128,
                    IV = key,
                    KeySize = 128,
                    Key = key
                };

                var buffer = Convert.FromBase64String(value);
                var transform = rijndael.CreateDecryptor();
                string decrypted;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        cs.Write(buffer, 0, buffer.Length);
                        cs.FlushFinalBlock();
                        decrypted = Encoding.UTF8.GetString(ms.ToArray());
                        cs.Close();
                    }
                    ms.Close();
                }

                return decrypted;
            }
            catch
            {
                return null;
            }
        }
    }

}