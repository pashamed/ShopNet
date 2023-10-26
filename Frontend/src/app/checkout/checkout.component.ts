import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { Address } from '../shared/models/user';
import { BasketService } from '../basket/basket.service';
import { ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CheckoutComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private basketService: BasketService,
    private toast: ToastrService
  ) {}
  ngOnInit(): void {
    this.getAddressFormValues();
    this.getDeliveryMethodValue();
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

  async getDeliveryMethodValue() {
    const basket = await this.basketService.getCurrentBasketValue();
    if (basket && basket.deliveryMethodId) {
      this.checkoutForm
        .get('deliveryForm')
        ?.get('deliveryMethod')
        ?.patchValue(basket.deliveryMethodId.toString());
    }
  }

  checkFormValid() {
    if (
      this.checkoutForm.get('addressForm')?.valid &&
      this.checkoutForm.get('deliveryForm')?.valid
    )
      return true;
    return false;
  }

  async createPaymentIntent() {
    await this.basketService
      .createPaymentIntent()
      .catch((error) => this.toast.error(error.message));
  }
}
