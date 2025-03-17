using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HomeEnergyApi.Security
{
    public class ValueEncryptor
    {
        private static string key;
        private static string iv;

        public ValueEncryptor(IConfiguration configuration)
        {
            key = configuration["AES:Key"];
            iv = configuration["AES:InitializationVector"];
        }

        public string Encrypt(string text)
        {
            if(key.Length != 32 || iv.Length != 16)
            {
                throw new ArgumentException("Key must be 32 bytes and IV must be 16 bytes long.");
            }

            using var aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Encoding.UTF8.GetBytes(key);
            aesAlgorithm.IV = Encoding.UTF8.GetBytes(iv);

            var encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);
            byte[] encryptedBytes;
            using(MemoryStream msEncrypt = new MemoryStream())
            {
                using(CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                }

                encryptedBytes = msEncrypt.ToArray();
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string cipherText)
        {
            if(key.Length != 32 || iv.Length != 16)
            {
                throw new ArgumentException("Key must be 32 bytes and IV must be 16 bytes long.");
            }

            using var aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Encoding.UTF8.GetBytes(key);
            aesAlgorithm.IV = Encoding.UTF8.GetBytes(iv);

            var decryptor = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, aesAlgorithm.IV);
            string text;

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using(MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using(StreamReader swDecrypt = new StreamReader(csDecrypt))
                    {
                        text = swDecrypt.ReadToEnd();
                    }
                }
            }

            return text;

        }
    }
}
