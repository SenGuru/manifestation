using System;
using System.IO;

namespace Framework.Infrastructure.Models.FileInput
{
    public class FileStreamModel : IDisposable
    {
        private bool disposed;

        public FileStreamModel()
        {
        }

        public FileStreamModel(string existingLocalFile)
        {
            FileData = new FileStream(existingLocalFile, FileMode.Open, FileAccess.Read);
            FileName = Path.GetFileName(existingLocalFile);
        }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public Stream FileData { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (FileData != null)
                    {
                        FileData.Close();
                        FileData.Dispose();
                    }
                }

                disposed = true;
            }
        }
    }
}
