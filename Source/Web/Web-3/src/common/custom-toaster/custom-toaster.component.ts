import { Toast, ToastPackage, ToastrService } from "ngx-toastr";
import {animate,keyframes,state,style,transition,trigger } from '@angular/animations';
import { Component } from "@angular/core";

@Component({
    selector: 'app-custom-toaster',
    templateUrl: './custom-toaster.component.html',
    styleUrls: ['./custom-toaster.component.css'],
    animations: [
        trigger('flyInOut', [
            state('inactive', style({
                opacity: 0,
            })),
            transition('inactive => active', animate('700ms ease-out', keyframes([
                style({
                    transform: 'translate3d(100%, 0, 0) skewX(-30deg)',
                    opacity: 0,
                }),
                style({
                    transform: 'skewX(20deg)',
                    opacity: 1,
                }),
                style({
                    transform: 'skewX(-5deg)',
                    opacity: 1,
                }),
                style({
                    transform: 'none',
                    opacity: 1,
                }),
            ]))),
            transition('active => removed', animate('700ms ease-out', keyframes([
                style({
                    opacity: 1,
                }),
                style({
                    transform: 'translate3d(100%, 0, 0) skewX(30deg)',
                    opacity: 0,
                }),
            ]))),
        ]),
    ],
    standalone: false
})
export class CustomToasterComponent extends Toast {

 public constructor(protected override toastrService: ToastrService, public override toastPackage: ToastPackage) {
    super(toastrService, toastPackage);
  }
}
