import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Customer, CustomerSaveResult } from './customer.model';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CustomerService {
  constructor(public http: HttpClient) { }

  get(customerId: number) {
    return this.http.get<Customer>('/api/customer/' + customerId);
  }

  search(term: string): Observable<Customer[]> {
    return this.http.get<Customer[]>('/api/customer/search?term=' + term);
  }

  save(customer: Customer): Observable<CustomerSaveResult> {
    return this.http.post<CustomerSaveResult>('/api/customer/save', customer);
  }
}
