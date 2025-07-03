import { AuditDuoColumnModel } from "../models/app-custom-model";
import { BaseSearchCriteria } from "../framework/common/models/base-search-criteria.model";

export class BaseListComponent<Model, SearchCriteria extends BaseSearchCriteria> {
   public columns: AuditDuoColumnModel[] = [];
   public searchModel: SearchCriteria | undefined;
   public listData: Model[] = [];
   public perPage = 20;
   public totalItems = 0;
   public searchText = '';
   public currentSort = 'up';
}
