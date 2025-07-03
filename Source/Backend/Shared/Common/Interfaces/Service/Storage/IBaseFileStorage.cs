using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Storage.Net;
using Storage.Net.Blobs;

namespace Manifest.Common.Interfaces.Service.Storage
{
    public interface IBaseFileStorage
    {
        Task DeleteAsync(IEnumerable<string> fullPaths, CancellationToken cancellationToken = default);

        Task DeleteAsync(string fullPath, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<bool>> ExistsAsync(IEnumerable<string> fullPaths, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(string fullPath, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Blob>> GetBlobsAsync(IEnumerable<string> fullPaths, CancellationToken cancellationToken = default);

        Task<Blob> GetBlobAsync(string fullPath, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Blob>> ListAsync(ListOptions options = null, CancellationToken cancellationToken = default);

        Task<Stream> OpenReadAsync(string fullPath, CancellationToken cancellationToken = default);

        Task<ITransaction> OpenTransactionAsync();

        Task SetBlobsAsync(IEnumerable<Blob> blobs, CancellationToken cancellationToken = default);

        Task SetBlobsAsync(Blob blob, CancellationToken cancellationToken = default);

        Task<byte[]> ReadBytesAsync(string fullPath, CancellationToken cancellationToken = default);

        Task<string> ReadTextAsync(string fullPath, CancellationToken cancellationToken = default);

        Task ReadToFileAsync(string fullPath, string filePath, CancellationToken cancellationToken = default);

        Task ReadToStreamAsync(string fullPath, Stream targetStream, CancellationToken cancellationToken = default);

        Task WriteAsync(string fullPath, byte[] data, bool append = false, CancellationToken cancellationToken = default);

        Task WriteAsync(string fullPath, Stream sourceStream, bool append = false, CancellationToken cancellationToken = default);

        Task WriteFileAsync(string fullPath, string filePath, CancellationToken cancellationToken = default);

        Task WriteTextAsync(string fullPath, string text, CancellationToken cancellationToken = default);
    }
}