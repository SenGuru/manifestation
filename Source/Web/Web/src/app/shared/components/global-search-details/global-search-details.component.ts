import { ActivatedRoute, Router } from '@angular/router';
import { GlobalSearchFeature, GlobalSearchRecordsCount } from 'src/common/models/app-custom-model';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { Component } from '@angular/core';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';

@UntilDestroy()
@Component({
    selector: 'app-global-search-details',
    templateUrl: './global-search-details.component.html',
    styleUrls: ['./global-search-details.component.css'],
    standalone: false
})
export class GlobalSearchDetailsComponent {
  public searchText: string | undefined;
  public searchFeatures: GlobalSearchFeature[] = [];
  public type: string | undefined;
  public globalsearchDetails: GlobalSearchRecordsCount;

  public constructor(private router: Router,
    private route: ActivatedRoute) {

    this.route.paramMap.pipe(untilDestroyed(this)).subscribe({
      next: (params) => {
        this.searchText = params.get('text') ?? '';
        this.type = params.get('type') ?? '';
      },
      error: (err: unknown) => {
        // eslint-disable-next-line
        console.error(err);
      },
      complete: () => null
    });
    this.globalsearchDetails = new GlobalSearchRecordsCount();
    const navigation = this.router.getCurrentNavigation();
    if (!ObjectUtils.isNullOrUndefined(navigation)) {
      const state = navigation?.extras.state;
      if (!ObjectUtils.isNullOrUndefined(state)) {
        this.globalsearchDetails = state?.['search'] as GlobalSearchRecordsCount;
      }
    }
  }

  public checkTab(type: GlobalSearchFeature): boolean {
    if (this.searchFeatures.includes(type)) {
      return true;
    }
    return false;
  }

  public changeTab(selectedTab: string): void {
    if (this.type === selectedTab) return;

    this.type = selectedTab;
  }

  public backToHomeScreen(): void {
    this.router.navigate(['.']);
  }

}
