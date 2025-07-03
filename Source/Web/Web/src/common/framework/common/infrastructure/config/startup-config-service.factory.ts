import { ConfigService } from './config.service';

// eslint-disable-next-line @typescript-eslint/ban-types
export function startupConfigServiceFactory(startupService: ConfigService): Function {
    return () => startupService;
}
