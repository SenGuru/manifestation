import { ConfigService } from '../config/config.service';
import { HttpHeaders } from '@angular/common/http';

export class UrlConfigService {

    public constructor(private configService: ConfigService) {
    }

    public getUrl(path: string): string {
        return this.configService.serviceConfiguration.baseApiServerUrl + path;
    }

    public headers(): HttpHeaders {
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json');
        return headers;
    }

}
