import { TestBed, inject } from '@angular/core/testing';

import { CustomerMockService } from './customer-mock.service';

describe('CustomerMockService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CustomerMockService]
    });
  });

  it('should be created', inject([CustomerMockService], (service: CustomerMockService) => {
    expect(service).toBeTruthy();
  }));
});
