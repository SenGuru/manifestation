import { IStringIndexedObjModel } from "../framework/common/models/i-string-indexed-object,model";

export class HttpHelper {
  public static tenantEntityIdStr = "x-tenant-entity-id";
  public static userRoleTypeIdStr = "x-user-role-id";

  // tslint:disable-next-line:max-line-length
  public static getUpdatedTenantEntityAndRoleHeader(tenantEntityId?: number, roleTypeId?: number): object | null {
    if (tenantEntityId == null || roleTypeId == null)
      return null;
    const result: IStringIndexedObjModel = {};
    result[`${this.tenantEntityIdStr}`] = tenantEntityId.toString();
    result[`${this.userRoleTypeIdStr}`] = roleTypeId.toString();
    return result;
  }
}
