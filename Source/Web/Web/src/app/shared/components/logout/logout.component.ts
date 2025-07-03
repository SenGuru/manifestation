import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { LoginTypes, LogoutTypes } from 'src/common/models/app-custom-enums';

@Component({
    selector: 'app-logout',
    templateUrl: './logout.component.html',
    styleUrls: ['./logout.component.scss'],
    standalone: false
})
export class LogoutComponent implements OnInit {

  public loginType: LoginTypes | null = null;
  public logoutType: LogoutTypes | null = null;
  public message = "Logout..";
  public statusCode = 'success';
 public constructor(private route: ActivatedRoute,
              private router: Router) { }

public ngOnInit(): void {
    this.route.params.subscribe(param =>{
        if (this.logoutType == LogoutTypes.Manual) {
          this.message = "Successfully Logout";
        } else if (this.logoutType == LogoutTypes.InActivity) {
          this.message = "Your session has expired due to inactivity. Please log back to continue.";
          this.statusCode = "danger"; 
        } else if (this.logoutType == LogoutTypes.Authentication) {
          this.message = "Your session has expired due to bad Api request. Please log back to continue.";
          this.statusCode = "danger"; 
        }
    });
  }

public goToLogin(): void {
    this.router.navigate([`/${this.loginType == LoginTypes.Configurator ? 'admin' : 'user'}/login`]);
  }
}
