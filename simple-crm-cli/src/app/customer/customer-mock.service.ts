import { Injectable } from '@angular/core';
import { CustomerService } from './customer.service';
import { HttpClient } from '@angular/common/http';
import { Customer, CustomerSaveResult } from './customer.model';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';

@Injectable()
export class CustomerMockService extends CustomerService {
  customers: Customer[] = [];
  lastCustomerId: number;

  constructor(public http: HttpClient) {
    super(http);
    console.warn('Warning: You are using the CustomerMockService, not intended for production use.');

    const localCustomers = localStorage.getItem('customers');
    if (localCustomers) {
      this.customers = JSON.parse(localCustomers);
    } else {
      this.customers.push({
        customerId: 1,
        firstName: 'Tory',
        lastName: 'Amos',
        phoneNumber: '314-555-9873',
        emailAddress: 'tory@example.com',
        statusCode: 'Prospect',
        preferredContactMethod: 'email',
        lastContactDate: new Date().toISOString()
      });
    }
    this.lastCustomerId = Math.max(...this.customers.map(x => x.customerId));
  }

  get(customerId: number) {
    console.log('get cust:', customerId);
    const item = this.customers.find(x =>
      x.customerId === customerId);
    return Observable.of(item);
  }

  search(term: string): Observable<Customer[]> {
    const items = this.customers.filter(x =>
      (x.firstName + ' ' + x.lastName).indexOf(term) >= 0
      || x.phoneNumber.indexOf(term) >= 0
      || x.emailAddress.indexOf(term) >= 0);
    return Observable.of(items);
  }

  save(customer: Customer): Observable<CustomerSaveResult> {
    const match = this.customers.find(x => x.customerId === customer.customerId);
    if (match) {
      this.customers = this.customers
        .map(x => x.customerId === customer.customerId ? customer : x);
    } else {
      customer.customerId = ++this.lastCustomerId;
      this.customers = [...this.customers, customer];
    }
    localStorage.setItem('customers', JSON.stringify(this.customers));
    return Observable.of({
      success: true,
      messages: [''],
      item: customer
    });
  }
}
