using System;
using Framework.Infrastructure.Logging;
using Manifest.Common.Enums;
using Manifest.Service.Cache;
using Manifest.Service.Config;

namespace Manifest.Service.Storage
{
    public class ProfilePictureFileStorage : NewBaseFileStorage
    {
        private readonly string reportStoragePrefix;

        public ProfilePictureFileStorage(ILog log, CompanyCacheService companyCacheService, AppConfigurationFile appConfigurationFile)
            : base(PredefinedFileStorageTypes.ProfilePictureFileStorage, log, companyCacheService, appConfigurationFile)
        {
            reportStoragePrefix = "ProfilePictures";
        }

        public string ProfilePicturePathForArtist(long companyId, long artistId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "artist", artistId.ToString(), fileName);
        }

        public string ProfilePicturePathForTechnician(long companyId, long technicianId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "technician", technicianId.ToString(), fileName);
        }

        public string ProfilePicturePathForVendor(long companyId, long vendorId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "vendor", vendorId.ToString(), fileName);
        }

        public string ProfilePicturePathForEquipment(long companyId, long equipmentId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "equipment", equipmentId.ToString(), fileName);
        }

        public string ProfilePicturePathForProperty(long companyId, long propertyId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "property", propertyId.ToString(), fileName);
        }

        public string ProfilePicturePathForMovie(long companyId, long movieId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "movie", movieId.ToString(), fileName);
        }

        public string ProfilePicturePathForMovieSecondCompany(long companyId, long movieId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "movie-second-company", movieId.ToString(), fileName);
        }

        public string ProfilePicturePathForMovieLocation(long companyId, long movieLocationId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "movie-location", movieLocationId.ToString(), fileName);
        }

        public string ProfilePicturePathForUser(long companyId, long userId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "user", userId.ToString(), fileName);
        }

        public string ProfilePicturePathForCompany(long companyId, string fileName)
        {
            return ProfilePicturePathInternal(companyId.ToString(), "company", companyId.ToString(), fileName);
        }

        public string ProfilePicturePathInternal(string companyId, string type, string id, string fileName)
        {
            return $"/{reportStoragePrefix}/{companyId}/{type}/{DateTime.Now.Year}/{MonthFromNumberAsString(DateTime.Now.Month)}/{id}/{fileName}";
        }
    }
}
