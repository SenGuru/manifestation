import { CommonDialogService } from "src/app/shared/services/common-dialog.service";
import { Injectable } from "@angular/core";
import { ObjectUtils } from "src/common/framework/common/utils/object.utils";
import { SESSION_KEYS } from "src/common/constants/app.constants";
import { SystemSummaryModel } from "src/common/models/app-model";

@Injectable({
    providedIn: 'root'
})

export class CompanySessionService {

    public constructor(private dialogService: CommonDialogService) {

    }
 
    public setCurrentSystemSummary(summary: SystemSummaryModel): void {
        try {
            if (!ObjectUtils.isObjNullOrUndefined(summary)) {
                sessionStorage.setItem(SESSION_KEYS.SYSTEM_SUMMARY_INFORMATION_SESSION_STORAGE_NAME, JSON.stringify(summary));
            }
        } catch (e) {
            this.dialogService.error("Fatal Error: Unable to set current System Summary Information");
        }
    }

    public getCurrentSystemSummary(): SystemSummaryModel | null {
        try {
            return JSON.parse(sessionStorage.getItem(SESSION_KEYS.SYSTEM_SUMMARY_INFORMATION_SESSION_STORAGE_NAME) ?? '') as SystemSummaryModel;
        } catch (e) {
            this.dialogService.error("Fatal Error: Unable to get current System Summary Information");
            return null;
        }
    }

    public clearAllSessions(): void {
        this.clearCurrentCompanyInformationSession();
        this.clearCurrentCompanySummarySession();
        this.clearCurrentSystemSummarySession();
    }

    public clearCurrentCompanyInformationSession(): void {
        sessionStorage.removeItem(SESSION_KEYS.COMPANY_INFORMATION_SESSION_STORAGE_NAME);
    }

    public clearCurrentCompanySummarySession(): void {
        sessionStorage.removeItem(SESSION_KEYS.COMPANY_SUMMARY_INFORMATION_SESSION_STORAGE_NAME);
    }

    public clearCurrentSystemSummarySession(): void {
        sessionStorage.removeItem(SESSION_KEYS.SYSTEM_SUMMARY_INFORMATION_SESSION_STORAGE_NAME);
    }

}