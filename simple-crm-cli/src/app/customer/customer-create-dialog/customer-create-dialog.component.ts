import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Customer } from '../customer.model';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'crm-customer-create-dialog',
  templateUrl: './customer-create-dialog.component.html',
  styleUrls: ['./customer-create-dialog.component.scss']
})
export class CustomerCreateDialogComponent implements OnInit {
  detailForm: FormGroup;

  constructor(private fb: FormBuilder,
    public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Customer | null) {
      this.detailForm = this.fb.group({
        firstName: [''],
        lastName: [''],
        phoneNumber: [''],
        emailAddress: [''],
        preferredContactMethod: ['email']
      });
      if (this.data) {
        this.detailForm.patchValue(this.data);
      }
    }

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
