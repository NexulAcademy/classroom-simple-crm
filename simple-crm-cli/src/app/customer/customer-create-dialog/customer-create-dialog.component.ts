import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Customer } from '../customer.model';

@Component({
  selector: 'crm-customer-create-dialog',
  templateUrl: './customer-create-dialog.component.html',
  styleUrls: ['./customer-create-dialog.component.scss']
})
export class CustomerCreateDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Customer | null) { }

  ngOnInit() {
  }

  cancel() {
    this.dialogRef.close();
  }

  save() {
    // TODO: get form data and pass as parameter to close
    const customer = { ...this.data };
    this.dialogRef.close(customer);
  }

}
