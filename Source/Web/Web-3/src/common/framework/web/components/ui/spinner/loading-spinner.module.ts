import { ModuleWithProviders, NgModule } from '@angular/core';

import { LoadingSpinnerComponent } from './loading-spinner.component';
import { LoadingSpinnerService } from './loading-spinner.service';

export * from './loading-spinner.service';
export * from './loading-spinner.component';

@NgModule({
  imports: [],
  declarations: [LoadingSpinnerComponent],
  exports: [LoadingSpinnerComponent],
  providers: [LoadingSpinnerService]
})
export class LoadingSpinnerModule {
 public static forRoot(): ModuleWithProviders<LoadingSpinnerModule> {
    return {
      ngModule: LoadingSpinnerModule,
      providers: [LoadingSpinnerService]
    };
  }
}
