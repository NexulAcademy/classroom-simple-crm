import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../customer.service';
import { Customer } from '../customer.model';
import { Observable } from 'rxjs/Observable';
import { FormGroup, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'crm-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent implements OnInit {
  customerId: number;
  customer: Customer;
  detailForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    this.createForm();
   }

  ngOnInit() {
    this.customerId = +this.route.snapshot.params['id'];
    this.customerService.get(this.customerId)
      .subscribe(cust => {
        this.customer = cust;
        this.detailForm.patchValue(cust);
      });
  }

  createForm() {
    this.detailForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
      emailAddress: ['', [Validators.required, Validators.email]],
      preferredContactMethod: ['email']
    });
  }

  save() {
    if (!this.detailForm.valid) { return; }
    const customer = { ...this.customer, ...this.detailForm.value };
    this.customerService.save(customer)
      .subscribe(result => {
        if (!result.success) {
          this.snackBar.open(result.messages.join('. '), 'OOPS');
          return;
        }
        this.snackBar.open('Customer saved.', 'OK');
      });
  }
}
