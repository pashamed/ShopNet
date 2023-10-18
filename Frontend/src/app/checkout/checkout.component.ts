import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { Address } from '../shared/models/user';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CheckoutComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private accountService: AccountService
  ) {}
  ngOnInit(): void {
    this.getAddressFormValues();
  }

  checkoutForm = this.fb.group({
    addressForm: this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required],
      country: ['', Validators.required],
    }),
    deliveryForm: this.fb.group({
      deliveryMethod: ['', Validators.required],
    }),
    paymentForm: this.fb.group({
      nameOnCard: ['', Validators.required],
    }),
  });

  async getAddressFormValues() {
    await this.accountService.getCurrentUserAddress();
    if (this.accountService.user?.address) {
      this.checkoutForm
        .get('addressForm')
        ?.patchValue(this.accountService.user?.address);
    }
  }
}
