import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';
import { SimpleDataSource } from '../../shared/simple-datasource';
import { Observable } from 'rxjs/Observable';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$: Observable<Customer[]>;
  dataSource: SimpleDataSource<Customer>;
  displayColumns = ['icon', 'status', 'name', 'phone', 'email', 'lastContactDate', 'action'];

  constructor(
    private customerService: CustomerService,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.search();
  }
  private search() {
    this.customers$ = this.customerService.search('');
    this.dataSource = new SimpleDataSource(this.customers$);
  }

  addCustomer() {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '250px',
      data: null
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.customerService.save(result);
      this.search();
    });
  }

  openDetail(item: Customer) {
    if (item) {
      this.router.navigate([`./customer/${item.customerId}`]);
    }
  }
}
