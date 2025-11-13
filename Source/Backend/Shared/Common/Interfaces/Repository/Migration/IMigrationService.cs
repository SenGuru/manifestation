namespace Manifest.Common.Interfaces.Repository
{
    public interface IMigrationService
    {
        void MigrateLocalDatastoreConditionally();

        void MigrateLocalDatastoreIfNeeded();
    }
}