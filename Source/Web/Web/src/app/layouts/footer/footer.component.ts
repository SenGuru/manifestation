import { Component } from '@angular/core';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
    standalone: false
})
export class FooterComponent {

  // set the currenr year
  public year: number = new Date().getFullYear();
  public constructor() {
    // 
  }

}
