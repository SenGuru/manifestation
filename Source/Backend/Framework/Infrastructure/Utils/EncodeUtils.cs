/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Utils
{
    public static class EncodeUtils
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static async Task<bool> ConvertBase64ToFile(string base64String, string destinationFilePath)
        {
            if (string.IsNullOrEmpty(base64String))
                return false;

            if (string.IsNullOrEmpty(destinationFilePath))
                return false;

            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);
                await File.WriteAllBytesAsync(destinationFilePath, fileBytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> ConvertBase64ToStream(string base64String, Stream stream)
        {
            if (string.IsNullOrEmpty(base64String))
                return false;

            if (stream == null || !stream.CanWrite)
                return false;

            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
                await stream.FlushAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<string> ConvertFileToBase64(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            if (!File.Exists(filePath))
                return null;

            try
            {
                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                return Convert.ToBase64String(fileBytes);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<string> ConvertFileToBase64(Stream stream)
        {
            if (stream == null || !stream.CanRead)
                return null;

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(fileBytes);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
