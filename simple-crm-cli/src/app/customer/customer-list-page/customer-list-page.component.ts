import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatTableDataSource } from '@angular/material';
import { CustomerService } from '../customer.service';
import { SimpleDataSource } from '../../shared/simple-datasource';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$: Observable<Customer[]>;
  dataSource: SimpleDataSource<Customer>;
  displayColumns = ['name', 'phone', 'email', 'status'];

  constructor(private customerService: CustomerService) {}

  ngOnInit() {
    this.customers$ = this.customerService.search('');
    this.dataSource = new SimpleDataSource(this.customers$);
  }
}
