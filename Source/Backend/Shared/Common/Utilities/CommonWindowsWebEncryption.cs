/*
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see https://opensource.org/licenses/BSD-3-Clause.
*/
using System;
using System.Security.Cryptography;
using System.Text;

namespace Manifest.Common.Utilities
{
    public static class CommonWindowsWebEncryption
    {
        public static string GetSHA256Hash(string input)
        {
            using (var sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public static bool VerifyHA256Hash(string input, string hash)
        {
            string hashOfInput = GetSHA256Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public static string Encrypt(string toEncrypt, bool useHashing, string key)
        {
            byte[] keyArray;
            var toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                using (var hashmd5 = MD5.Create())
                {
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

#pragma warning disable S5547 // Cipher algorithms should be robust
            using (var tdes = TripleDES.Create())
#pragma warning restore S5547 // Cipher algorithms should be robust
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                var cTransform = tdes.CreateEncryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public static string Decrypt(string cipherString, bool useHashing, string key)
        {
            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                using (var hashmd5 = MD5.Create())
                {
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

#pragma warning disable S5547 // Cipher algorithms should be robust
            using (var tdes = TripleDES.Create())
#pragma warning restore S5547 // Cipher algorithms should be robust
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                var cTransform = tdes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}
