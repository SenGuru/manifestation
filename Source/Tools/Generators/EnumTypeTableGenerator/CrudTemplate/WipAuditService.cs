        #region EEntity123
        public async Task EEntity123CreatedAsync(long eentity123Id)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceCreated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EEntity123,  resourceName: "EEntity123", resourceId: eentity123Id, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, companyId: await currentSessionService.GetCurrentCompanyIdAsync());
        }

        public async Task EEntity123UpdatedAsync(long eentity123Id)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUpdated, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EEntity123,  resourceName: "EEntity123", resourceId: eentity123Id, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, companyId: await currentSessionService.GetCurrentCompanyIdAsync());
        }

        public async Task EEntity123DeletedAsync(long eentity123Id)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EEntity123,  resourceName: "EEntity123", resourceId: eentity123Id, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, companyId: await currentSessionService.GetCurrentCompanyIdAsync());
        }

        public async Task EEntity123UnDeletedAsync(long eentity123Id)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceUndeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EEntity123,  resourceName: "EEntity123", resourceId: eentity123Id, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, companyId: await currentSessionService.GetCurrentCompanyIdAsync());
        }

        /*
        public async Task EEntity123EnabledAsync(long eentity123Id)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceEnable, resourceType: PredefinedResourceTypes.EEntity123, resourceName: "EEntity123", resourceId: eentity123Id, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, companyId: await currentSessionService.GetCurrentCompanyIdAsync());
        }

        public async Task EEntity123DisabledAsync(long eentity123Id)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceDisable, resourceType: PredefinedResourceTypes.EEntity123, resourceName: "EEntity123", resourceId: eentity123Id, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, companyId: await currentSessionService.GetCurrentCompanyIdAsync());
        }
        */

        public async Task HardDeleteEEntity123Async(long eentity123Id)
        {
            await auditRepository.InsertAuditAsync(PredefinedAuditOperationTypes.ResourceHardDeleted, objectBeforeUpdate: "", objectAfterUpdate: "", resourceType: PredefinedResourceTypes.EEntity123,  resourceName: "EEntity123", resourceId: eentity123Id, userId: await currentSessionService.GetCurrentUserIdAsync(), userName: currentUserName, ipAddress: currentUserAddress, companyId: await currentSessionService.GetCurrentCompanyIdAsync());
        }
        #endregion
