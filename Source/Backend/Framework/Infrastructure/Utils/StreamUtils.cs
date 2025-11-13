using System;
using System.IO;
using System.Text;

namespace Framework.Infrastructure.Utils
{
    public static class StreamUtils
    {
        public static Stream StringToStream(string str)
        {
            if (str == null)
                return new MemoryStream();
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }

        public static Stream ByteArrayToStream(byte[] bytes)
        {
            if (bytes == null)
                return new MemoryStream();
            return new MemoryStream(bytes);
        }

        public static byte[] StreamToByte(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static MemoryStream StreamToStream(Stream stream)
        {
            if (stream == null)
                return new MemoryStream();

            stream.Position = 0;
            var newStream = new MemoryStream();
            stream.CopyTo(newStream);
            newStream.Position = 0;

            return newStream;
        }

        public static bool IsStreamEmpty(Stream fileStream)
        {
            if (fileStream == null)
            {
                return true;
            }

            return fileStream.Length == 0;
        }

        public static string GetImageType(this MemoryStream stream)
        {
            stream.Position = 0;
            byte[] header = new byte[8];
            int bytesRead = stream.Read(header, 0, 8);
            stream.Position = 0;

            if (bytesRead < 2)
            {
                // Not enough bytes to determine type
                return "";
            }

            // Check file signatures
            if (header[0] == 0xFF && header[1] == 0xD8)
                return "jpeg";
            if (bytesRead >= 4 && header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
                return "png";
            if (bytesRead >= 3 && header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46)
                return "gif";
            if (header[0] == 0x42 && header[1] == 0x4D)
                return "bmp";
            if (bytesRead >= 4 &&
                ((header[0] == 0x49 && header[1] == 0x49 && header[2] == 0x2A && header[3] == 0x00) ||
                 (header[0] == 0x4D && header[1] == 0x4D && header[2] == 0x00 && header[3] == 0x2A)))
                return "tiff";

            return "";
        }
    }
}
