import { TestBed, inject } from '@angular/core/testing';

import { AppIconsService } from './app-icons.service';

describe('AppIconsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AppIconsService]
    });
  });

  it('should be created', inject([AppIconsService], (service: AppIconsService) => {
    expect(service).toBeTruthy();
  }));
});
