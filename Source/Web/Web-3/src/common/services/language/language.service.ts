import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({ providedIn: 'root' })
export class LanguageService {

  public languages: string[] = ['en', 'es', 'de', 'it', 'ru'];

  public constructor(public translate: TranslateService) {

    let browserLang: string | undefined;
    /***
     * cookie Language Get
    */
    this.translate.addLangs(this.languages);
    browserLang = translate.getBrowserLang();
    translate.use(browserLang?.match(/en|es|de|it|ru/) ? browserLang : 'en');
  }

  /***
   * Cookie Language set
   */
  public setLanguage(lang: string): void {
    this.translate.use(lang);
  }

}
