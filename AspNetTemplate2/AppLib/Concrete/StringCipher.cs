using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace AspNetTemplate2.AppLib.Concrete
{
    public static partial class StringCipher
    {
        #region internals        

        private static byte[] GetIVFromByteArr(Stream s)
        {
            // this is used for getting IV in decrypting with salt
            // (skip first 4 and read next 16)

            byte[] rawLength = new byte[sizeof(int)];

            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
                throw new SystemException("unable to read stream or stream is invalid");

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];

            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
                throw new SystemException("unable to read stream or stream is invalid");

            return buffer;
        }

        private static byte[] CreateSalt(string password)
        {
            while (password.Length * sizeof(char) < 8) // Min. 8 bytes needed!   
                password = $":{password}";

            byte[] bytes = new byte[password.Length * sizeof(char)];
            System.Buffer.BlockCopy(password.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static RijndaelManaged CreateRijndaelManaged(string password)
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, CreateSalt(password));

            RijndaelManaged aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
            //aesAlg.GenerateKey();
            //aesAlg.GenerateIV();

            return aesAlg;
        }

        /// <summary>
        /// Encrypt using AES with salt.
        /// </summary>
        private static byte[] Encryptor(string plainText, string password)
        {
            RijndaelManaged aesAlg = CreateRijndaelManaged(password);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    swEncrypt.Write(plainText);

                aesAlg.Clear();

                return msEncrypt.ToArray();
            }

        }

        /// <summary>
        /// Encrypt using AES without salt.
        /// </summary>
        private static byte[] SimpleEncryptor(string plainText, string password)
        {
            RijndaelManaged aesAlg = CreateRijndaelManaged(password);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    swEncrypt.Write(plainText);

                return msEncrypt.ToArray();
            }
        }

        /// <summary>
        /// Decrypt AES with salt.
        /// </summary>
        private static string Decryptor(byte[] encryptedBytes, string password)
        {
            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            {
                RijndaelManaged aesAlg = CreateRijndaelManaged(password);

                aesAlg.IV = GetIVFromByteArr(msDecrypt); // iv is extracted from stream to filter out salt

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }

        }

        /// <summary>
        /// Decrypt AES without salt.
        /// </summary>
        private static string SimpleDecryptor(byte[] encryptedBytes, string password)
        {
            RijndaelManaged aesAlg = CreateRijndaelManaged(password);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                return srDecrypt.ReadToEnd();
        }

        #endregion internals


        #region Encrypt        

        /// <summary>
        /// Encrypt as Base64 using AES without salt.
        /// </summary>
        public static string EncryptSimple(string plainText, string password)
        {
            Byte[] encryptedBytes = SimpleEncryptor(plainText, password);
            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Encrypt URL friendly using AES without salt.
        /// </summary>
        public static SecureString EncryptSimpleURL(string plainText, string password)
        {
            Byte[] encryptedBytes = SimpleEncryptor(plainText, password);
            //Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode
            string enc = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(encryptedBytes);
            return new NetworkCredential(string.Empty, enc).SecurePassword;
            //return System.Net.WebUtility.UrlEncode(Encoding.UTF8.GetString(encryptedBytes, 0, encryptedBytes.Length));
            //return System.Web.HttpServerUtility.UrlTokenEncode(encryptedBytes);
        }

        /// <summary>
        /// Encrypt as Base64 using AES with salt.
        /// </summary>
        public static string Encrypt(string plainText, string password)
        {
            byte[] encrypted = Encryptor(plainText, password);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Encrypt URL friendly using AES with salt.
        /// </summary>
        public static string EncryptURL(string plainText, string password)
        {
            byte[] encrypted = Encryptor(plainText, password);
            return System.Net.WebUtility.UrlEncode(Encoding.UTF8.GetString(encrypted, 0, encrypted.Length));
            //return System.Web.HttpServerUtility.UrlTokenEncode(encrypted);
        }

        #endregion Encrypt


        #region Decrypt

        /// <summary>
        /// Decrypt Base64 AES without salt.
        /// </summary>
        public static string DecryptSimple(string encryptedText, string password)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            return SimpleDecryptor(encryptedBytes, password);
        }

        /// <summary>
        /// Decrypt URL friendly AES without salt.
        /// </summary>
        public static SecureString DecryptSimpleURL(string encryptedText, string password)
        {
            //byte[] encryptedBytes = System.Web.HttpServerUtility.UrlTokenDecode(encryptedText);
            //byte[] encryptedBytes = Encoding.UTF8.GetBytes(System.Net.WebUtility.UrlDecode(encryptedText));
            byte[] encryptedBytes = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(encryptedText);
            //Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode
            string dec = SimpleDecryptor(encryptedBytes, password);
            return new NetworkCredential(string.Empty, dec).SecurePassword;
        }

        /// <summary>
        /// Decrypt Base64 AES with salt.
        /// </summary>
        public static string Decrypt(string encryptedText, string password)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            return Decryptor(encryptedBytes, password);
        }

        /// <summary>
        /// Decrypt URL friendly AES with salt.
        /// </summary>
        public static string DecryptURL(string cipherText, string password)
        {
            //byte[] encryptedBytes = System.Web.HttpServerUtility.UrlTokenDecode(cipherText);
            byte[] encryptedBytes = Encoding.UTF8.GetBytes(System.Net.WebUtility.UrlDecode(cipherText));
            return Decryptor(encryptedBytes, password);
        }

        #endregion Decrypt
    }



}
