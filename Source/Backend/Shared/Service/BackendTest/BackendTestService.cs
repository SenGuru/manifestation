using System.IO;
using System.Threading.Tasks;
using Manifest.Common.Models.ViewModels;
using Manifest.Service.App;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;

namespace Manifest.Service.Admin
{
    public class BackendTestService : BaseAppService
    {
        public BackendTestService(ILog log, ObjectMapperService objectMapperService)
            : base(log, objectMapperService)
        {
        }

        public Task<FileStreamModel> ExportMovieSceneArtistsInJsonLFormat()
        {
            var result = new FileStreamModel
            {
                FileName = "SampleMovieSceneArtistsFile.json",
                ContentType = FileUtils.GetMimeTypeFromFileName("SampleMovieSceneArtistsFile.json")
            };

            if (result.FileData == null)
                result.FileData = new MemoryStream();

            return Task<FileStreamModel>.FromResult(result);
        }
    }
}
