using Manifest.Common.Interfaces.Service.App;
using Manifest.Repository.Common;
using Manifest.Service.ObjectMapping;
using Framework.Infrastructure.Interfaces.DbAccess;
using Framework.Infrastructure.Logging;

namespace Manifest.Service.App
{
    public partial class WipService : BaseAppService
    {
#pragma warning disable IDE0052, S4487 // Remove unread private members
        private readonly PermissionService permissionService;
        private readonly ICurrentSessionService currentSessionService;
        private readonly WipRepository wipRepository;
        private readonly ITransactionManager transactionManager;
        private readonly AuditService auditService;
        private readonly CompanyRepository companyRepository;
#pragma warning restore IDE0052, S4487 // Remove unread private members

        public WipService(ILog log, ObjectMapperService objectMapperService, PermissionService permissionService, ICurrentSessionService currentSessionService, WipRepository wipRepository, ITransactionManager transactionManager, AuditService auditService, CompanyRepository companyRepository)
            : base(log, objectMapperService)
        {
            this.permissionService = permissionService;
            this.currentSessionService = currentSessionService;
            this.wipRepository = wipRepository;
            this.transactionManager = transactionManager;
            this.auditService = auditService;
            this.companyRepository = companyRepository;
        }
    }
}