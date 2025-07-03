import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { ActivatedRoute } from '@angular/router';
import { MenuItem } from 'src/common/models/app-custom-model';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { RouterUtils } from 'src/common/framework/common/utils/router.utils';
import { SUPER_ADMIN_COMPANY_INFORMATION_MENU } from 'src/app/shared/components/menu/super-admin-menu';

@UntilDestroy()
@Component({
    selector: 'app-super-admin-company-contanier',
    templateUrl: './super-admin-company-contanier.component.html',
    styleUrl: './super-admin-company-contanier.component.scss',
    standalone: false
})
export class SuperAdminCompanyContanierComponent implements OnInit {

  public appMenuItems: MenuItem[] = [];
  public companyId: number = 0;

  public constructor(private route: ActivatedRoute) { }

  public ngOnInit(): void {
    this.route.params.pipe(untilDestroyed(this)).subscribe({
      next: (param) => {
        if (!ObjectUtils.isObjNullOrUndefined(param)) {
          // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
          this.companyId = ObjectUtils.getRouterDataNumber(param['companyId'] as number) ?? 0;
          // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
          const companyMenuItems = JSON.parse(JSON.stringify(SUPER_ADMIN_COMPANY_INFORMATION_MENU));
          this.appMenuItems = RouterUtils.updateUrlWithId(companyMenuItems as MenuItem[], "companyId", this.companyId.toString());
        }
      },
    });
  }

}
