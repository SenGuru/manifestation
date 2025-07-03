import { CompanyModel, CompanySummaryModel, SystemSummaryModel } from "src/common/models/app-model";

import { CommonDialogService } from "src/app/shared/services/common-dialog.service";
import { Injectable } from "@angular/core";
import { ObjectUtils } from "src/common/framework/common/utils/object.utils";
import { SESSION_KEYS } from "src/common/constants/app.constants";

@Injectable({
    providedIn: 'root'
})

export class CompanySessionService {

    public currrentCompany: CompanyModel | undefined;
    public constructor(private dialogService: CommonDialogService) {

    }

    public setCurrentCompany(company: CompanyModel): void {
        try {
            if (!ObjectUtils.isObjNullOrUndefined(company)) {
                sessionStorage.setItem(SESSION_KEYS.COMPANY_INFORMATION_SESSION_STORAGE_NAME, JSON.stringify(company));
            }
        } catch (e) {
            this.dialogService.error("Fatal Error: Unable to set current Company Information");
        }
    }

    public getCurrentCompany(): CompanyModel | null {
        try {
            const sessionData = sessionStorage.getItem(SESSION_KEYS.COMPANY_INFORMATION_SESSION_STORAGE_NAME);
            if (sessionData) {
                return JSON.parse(sessionData) as CompanyModel;
            } else {
                return null;
            }
        } catch (e) {
            this.dialogService.error("Fatal Error: Unable to get current Company Information");
            return null;
        }
    }

    public setCurrentCompanySummary(summary: CompanySummaryModel): void {
        try {
            if (!ObjectUtils.isObjNullOrUndefined(summary)) {
                sessionStorage.setItem(SESSION_KEYS.COMPANY_SUMMARY_INFORMATION_SESSION_STORAGE_NAME, JSON.stringify(summary));
            }
        } catch (e) {
            this.dialogService.error("Fatal Error: Unable to set current Company Summary Information");
        }
    }

    public getCurrentCompanySummary(): CompanySummaryModel | null {
        try {
            const sessionData = sessionStorage.getItem(SESSION_KEYS.COMPANY_SUMMARY_INFORMATION_SESSION_STORAGE_NAME) ?? null;
            if (sessionData) {
                return JSON.parse(sessionData) as CompanySummaryModel;
            } else {
                return null;
            }
        } catch (e) {
            this.dialogService.error("Fatal Error: Unable to get current Company Summary Information");
            return null;
        }
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