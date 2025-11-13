import { ActivatedRoute, Router } from '@angular/router';

import { Component } from '@angular/core';
import { EventService } from 'src/app/layouts/service/event.service';

@Component({
    selector: 'app-vertical',
    templateUrl: './vertical.component.html',
    styleUrls: ['./vertical.component.scss'],
    standalone: false
})
export class VerticalComponent {

  public isCondensed = false;

  public constructor(private router: Router, private eventService: EventService, private activatedRoute: ActivatedRoute) {
  }
  /**
   * On mobile toggle button clicked
   */
  public onToggleMobileMenu(): void {
    const currentSIdebarSize = document.documentElement.getAttribute("data-sidebar-size");
    if (document.documentElement.clientWidth >= 767) {
      if (currentSIdebarSize == null) {
        if ((document.documentElement.getAttribute('data-sidebar-size') == null || document.documentElement.getAttribute('data-sidebar-size') == "lg")) {
          document.documentElement.setAttribute('data-sidebar-size', 'sm');
        } else {
          document.documentElement.setAttribute('data-sidebar-size', 'lg');
        }
      } else if (currentSIdebarSize == "md") {

        if ((document.documentElement.getAttribute('data-sidebar-size') == "md")) {
          document.documentElement.setAttribute('data-sidebar-size', 'sm')
        } else {
          document.documentElement.setAttribute('data-sidebar-size', 'md')
        }
      } else {
        if ((document.documentElement.getAttribute('data-sidebar-size') == "sm")) {
          document.documentElement.setAttribute('data-sidebar-size', 'lg');
        } else {
          document.documentElement.setAttribute('data-sidebar-size', 'sm')
        }
      }
    }

    if (document.documentElement.clientWidth <= 767) {
      document.body.classList.toggle('vertical-sidebar-enable');
    }
    this.isCondensed = !this.isCondensed;
  }

  /**
   * on settings button clicked from topbar
   */
  public onSettingsButtonClicked(): void {
    document.body.classList.toggle('right-bar-enabled');
    const rightBar = document.getElementById('theme-settings-offcanvas');
    if (rightBar != null) {
      rightBar.classList.toggle('show');
      rightBar.setAttribute('style', "visibility: visible;");

    }
  }

  public onResize(event: Event): void {
    if (document.body.getAttribute('layout') == "twocolumn") {
      if ((event.currentTarget as Window).innerWidth <= 767) {
        this.eventService.broadcast('changeLayout', 'vertical');
      } else {
        this.eventService.broadcast('changeLayout', 'twocolumn');
        document.body.classList.remove('twocolumn-panel');
        document.body.classList.remove('vertical-sidebar-enable');
      }
    }
  }

}
