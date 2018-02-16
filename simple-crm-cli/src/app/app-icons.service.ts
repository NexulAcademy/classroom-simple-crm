import { Injectable } from '@angular/core';
import { MatIconRegistry } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable()
export class AppIconsService {
  constructor(private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer) {
      iconRegistry.addSvgIcon('online', sanitizer.bypassSecurityTrustResourceUrl('assets/icon-online.svg'));
      iconRegistry.addSvgIcon('money', sanitizer.bypassSecurityTrustResourceUrl('assets/icon-money.svg'));
      iconRegistry.addSvgIcon('users', sanitizer.bypassSecurityTrustResourceUrl('assets/icon-users.svg'));
    }
}
