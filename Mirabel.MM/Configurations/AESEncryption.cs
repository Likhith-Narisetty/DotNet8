using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Mirabel.MM.Configurations
{
    public class AESEncryption
    {
        public string CryptoEncrypt(string message)
        {
            var Res = "";
            AESEncryptLive aESEncryptLive = new AESEncryptLive();
            string publickey = aESEncryptLive.publickey;
            string privatekey = aESEncryptLive.privatekey;

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = UTF8Encoding.UTF8.GetBytes(publickey);
            aes.Key = UTF8Encoding.UTF8.GetBytes(privatekey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
                var EncryStr = Convert.ToBase64String(dest);
                var decodStr = EncryStr.Replace("+", "-");
                Res = decodStr;
            }
            return Res;
        }

        public string CryptoDecrypt(string encryptedText)
        {

            AESEncryptLive aESEncryptLive = new AESEncryptLive();
            string publickey = aESEncryptLive.publickey;
            string privatekey = aESEncryptLive.privatekey;
            string plaintext = null;
            var Decryvalue = encryptedText.Replace("-", "+");

            using (AesManaged aes = new AesManaged())
            {
                byte[] cipherText = Convert.FromBase64String(Decryvalue);
                byte[] aesIV = UTF8Encoding.UTF8.GetBytes(publickey);
                byte[] aesKey = UTF8Encoding.UTF8.GetBytes(privatekey);
                ICryptoTransform decryptor = aes.CreateDecryptor(aesKey, aesIV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        public static string CryptoEncrypt(string? message, string Key, string IV)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = UTF8Encoding.UTF8.GetBytes(IV);
            aes.Key = UTF8Encoding.UTF8.GetBytes(Key);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(dest);
            }
        }

        public static string CryptoDecrypt(string? encryptedText, string Key, string IV)
        {
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                byte[] cipherText = Convert.FromBase64String(encryptedText);
                byte[] aesIV = UTF8Encoding.UTF8.GetBytes(IV);
                byte[] aesKey = UTF8Encoding.UTF8.GetBytes(Key);
                ICryptoTransform decryptor = aes.CreateDecryptor(aesKey, aesIV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

    }

}