import { TestBed, inject, getTestBed } from '@angular/core/testing';

import { CustomerMockService } from './customer-mock.service';
import { CustomerService } from './customer.service';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';

describe('CustomerMockService', () => {
  let injector: TestBed;
  let service: CustomerMockService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [ CustomerService, CustomerMockService ]
    });
    injector = getTestBed();
    service = injector.get(CustomerMockService);
    httpMock = injector.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
