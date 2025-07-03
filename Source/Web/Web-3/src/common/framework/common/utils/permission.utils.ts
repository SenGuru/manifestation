import { ObjectUtils } from "./object.utils";
import { STORAGE_KEYS } from "src/common/constants/app.constants";
import { UserPermissionModel } from "src/common/models/app-model";

export class PermissionUtils {

    public static isPermissionExisit(allPermission: UserPermissionModel[], availablePermission: number[]): boolean {
        let result: boolean = false;
        const isPermissionEnable = JSON.parse(localStorage.getItem(STORAGE_KEYS.PERMISSION_ENABLE_DISABLE_OPTION_STORAGE_NAME) ?? '') as boolean ?? false;
        if (isPermissionEnable === true) {
            if (!ObjectUtils.isArrNullOrEmpty(allPermission)) {
                const isPermissionExist = allPermission.some((item) => availablePermission.includes(item.predefinedUserPermissionTypeId));
                result = isPermissionExist;
            }
            else {
                result = false;
            }

        } else {
            result = true;
        }

        return result;
    }

}